using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ModelTranslator.Utilities
{
    public static class Reflection
    {
        /// <summary>
        /// Returns a list of properties populated on a type. Will return all
        /// properties and include attribute information. Checks for attributes
        /// added to properties in partial classes.
        /// </summary>
        /// <param name="modelType">Type of object to discover properties.</param>
        /// <returns>List of properties for the type.</returns>
        public static List<PropertyInfo> GetProperties(Type modelType)
        {
            // need to check for indirect attributes added to partial classes,
            // for instance, partial classes generated with Entity Framework.
            // http://stackoverflow.com/questions/1910532/attribute-isdefined-doesnt-see-attributes-applied-with-metadatatype-class
            MetadataTypeAttribute[] metadataTypes =
                    modelType
                    .GetCustomAttributes(typeof(MetadataTypeAttribute), true)
                    .OfType<MetadataTypeAttribute>()
                    .ToArray();
            MetadataTypeAttribute metadata = metadataTypes.FirstOrDefault();

            List<PropertyInfo> properties = new List<PropertyInfo>();
            if (metadata != null)
            {
                properties = metadata.MetadataClassType.GetProperties().ToList();
            }

            // if this is not a partial class, retrieve attributes the direct way
            if (metadataTypes.Count() == 0)
            {
                properties = modelType.GetProperties().ToList();
            }

            return properties;
        }

        /// <summary>
        /// Gets a list of attributes of the specified type on a property.
        /// </summary>
        /// <typeparam name="T">Type of attribute to find.</typeparam>
        /// <param name="modelProperty">Property to check.</param>
        /// <returns>List of attributes found on the given property type.</returns>
        public static List<T> GetPropertyAttributes<T>(PropertyInfo modelProperty) where T : Attribute
        {
            List<T> attributes = modelProperty
                            .GetCustomAttributes(typeof(T), false)
                            .OfType<T>()
                            .ToList();

            return attributes ?? new List<T>();
        }

        /// <summary>
        /// Gets a list of attributes of the specified type on a property.
        /// </summary>
        /// <typeparam name="T">Type of attribute to find.</typeparam>
        /// <param name="modelType">Object to check.</param>
        /// <returns>List of attributes found on the given property type.</returns>
        public static List<T> GetAttributes<T>(Type modelType) where T : Attribute
        {
            List<T> attributes = modelType
                                .GetCustomAttributes(typeof(T), false)
                                .OfType<T>()
                                .ToList();

            return attributes ?? new List<T>();
        }

        /// <summary>
        /// Returns the underlying type of a generic object. Assumes <see cref="IsSimpleGeneric"/> is true.
        /// </summary>
        /// <param name="modelType">Type of object to get underlying type.</param>
        /// <returns>The underlying type of the generic object.</returns>
        public static Type GetSimpleGenericUnderlying(Type modelType)
        {
            return modelType.GenericTypeArguments[0];
        }

        /// <summary>
        /// Checks whether or not a type contains some kind of collection of elements.
        /// </summary>
        /// <param name="modelType">Type of object to check.</param>
        /// <returns>True if object is some kind of collection of elements.</returns>
        public static bool IsList(Type modelType)
        {
            // Notes on the list checking:
            //     if(propertyInfo.PropertyType is IEnumerable)
            // was always returning false. Had to find another method of checking
            // for lists; not sure how accurate this will be ...
            bool isList = false;
            if (typeof(IEnumerable).IsAssignableFrom(modelType) ||
                typeof(ICollection).IsAssignableFrom(modelType) ||
                typeof(IDictionary).IsAssignableFrom(modelType) ||
                modelType.IsArray)
            {
                isList = true;
            }

            // strings are enumerable, grrr
            if (modelType.FullName.ToLower() == "system.string")
            {
                isList = false;
            }

            return isList;
        }

        /// <summary>
        /// Checks whether or not an object is a nullable type.
        /// </summary>
        /// <param name="modelType">Type of object to check.</param>
        /// <returns>True if object is nullable, false otherwise.</returns>
        public static bool IsNullable(Type modelType)
        {
            return modelType.IsGenericType &&
                        modelType.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        /// <summary>
        /// Checks wether or not an object is a generic type with only one
        /// underlying type.
        /// </summary>
        /// <param name="modelType">Type of object to check.</param>
        /// <returns>True if object is generic and has only one underlying type, false otherwise.</returns>
        public static bool IsSimpleGeneric(Type modelType)
        {
            return modelType.GenericTypeArguments != null &&
                        modelType.GenericTypeArguments.Length == 1 &&
                        modelType.GenericTypeArguments[0] != null;
        }

        /// <summary>
        /// Checks whether an object has a public method by the given name.
        /// </summary>
        /// <param name="modelType">Type of object to check.</param>
        /// <param name="methodName">Method to find on object.</param>
        /// <returns>True if the method exists, false otherwise.</returns>
        public static bool HasMethod(Type modelType, string methodName)
        {
            MethodInfo[] underlyingMethodInfos = modelType.GetMethods(BindingFlags.Public | BindingFlags.Instance);
            if (underlyingMethodInfos.Any(x => x.Name.Contains(methodName)))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Gets properties that are a collection for a given type.
        /// </summary>
        /// <param name="modelType">Type of object to check.</param>
        /// <returns>A list of property names which are collections.</returns>
        public static List<Type> GetCollectionProperties(Type modelType)
        {
            List<Type> results = new List<Type>();

            List<PropertyInfo> properties = GetProperties(modelType);

            foreach (PropertyInfo property in properties)
            {
                if (IsList(property.PropertyType))
                {
                    results.Add(GetSimpleGenericUnderlying(property.PropertyType));
                }
            }

            results = results.OrderBy(x => x.Name).ToList();

            return results;
        }

        /// <summary>
        /// Checks a type and returns a list of properties containing the given attribute. Any properties
        /// found are recursively checked. A flat list is returned containing the list of found types. 
        /// Collection types are ignored; instead the base type is used.
        /// </summary>
        /// <param name="modelType">Type of object to check.</param>
        /// <param name="attr">Attribute to find in target class.</param>
        /// <returns>A flat list of all properties found with the attribute.</returns>
        public static List<Type> GetPropertyTypesByAttributeTree<Ttarget>
            (Type modelType)
            where Ttarget : Attribute
        {
            List<Type> results = new List<Type>();
            List<Type> alreadyChecked = new List<Type>();

            GetPropertyTypesByAttributeTreeRecursive<Ttarget, Ttarget>(modelType, results, alreadyChecked, 0);

            results = results.OrderBy(x => x.Name).ToList();

            return results;
        }

        /// <summary>
        /// Checks a type and returns a list of properties containing the given attribute. Any properties
        /// found are recursively checked. A flat list is returned containing the list of found types. Collection types are
        /// ignored; instead the base type is used.
        /// </summary>
        /// <param name="modelType">Type of object to check.</param>
        /// <param name="attr">Attribute to find in target class.</param>
        /// <param name="traverseBy">Properties marked with this attribute will be traversed
        /// but not added to the list of properties found.</param>
        /// <returns>A flat list of all properties found with the attribute.</returns>
        public static List<Type> GetPropertyTypesByAttributeTree<Ttarget, Ttraverse>
            (Type modelType)
            where Ttarget : Attribute
            where Ttraverse : Attribute
        {
            List<Type> results = new List<Type>();
            List<Type> alreadyChecked = new List<Type>();

            GetPropertyTypesByAttributeTreeRecursive<Ttarget, Ttraverse>(modelType, results, alreadyChecked, 0);

            results = results.OrderBy(x => x.Name).ToList();

            return results;
        }

        private static void GetPropertyTypesByAttributeTreeRecursive<Ttarget, Ttraverse>
            (Type modelType, List<Type> results, List<Type> alreadyChecked, int level)
            where Ttarget : Attribute
            where Ttraverse : Attribute
        {
            if (results == null)
            {
                throw new Exception("Parameter results is null");
            }

            if (level > 20)
            {
                throw new Exception("Excessive recursion");
            }

            Type currentType = modelType;

            if (IsSimpleGeneric(modelType))
            {
                currentType = GetSimpleGenericUnderlying(modelType);
            }

            List<PropertyInfo> properties = GetProperties(currentType);

            foreach (PropertyInfo property in properties)
            {
                Type propertyCurrentType = property.PropertyType;

                if (IsSimpleGeneric(property.PropertyType))
                {
                    propertyCurrentType = GetSimpleGenericUnderlying(property.PropertyType);
                }

                if (alreadyChecked.Contains(propertyCurrentType))
                {
                    continue;
                }

                alreadyChecked.Add(propertyCurrentType);

                List<Ttarget> targetAttributes = GetPropertyAttributes<Ttarget>(property);
                List<Ttraverse> traverseAttributes = GetPropertyAttributes<Ttraverse>(property);

                if (targetAttributes.Any() && !results.Contains(propertyCurrentType))
                {
                    results.Add(propertyCurrentType);
                    GetPropertyTypesByAttributeTreeRecursive<Ttarget, Ttraverse>(propertyCurrentType, results, alreadyChecked, level + 1);
                }
                else if (traverseAttributes.Any())
                {
                    GetPropertyTypesByAttributeTreeRecursive<Ttarget, Ttraverse>(propertyCurrentType, results, alreadyChecked, level + 1);
                }
            }
        }
    }
}
