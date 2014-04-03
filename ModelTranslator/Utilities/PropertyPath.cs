using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ModelTranslator.Utilities
{
    public static class PropertyPath
    {
        /// <summary>
        /// Converts a labmda expression containing a path to a property into a string.
        /// </summary>
        /// <typeparam name="T">Type of object containing property.</typeparam>
        /// <typeparam name="TProperty">Type of property.</typeparam>
        /// <param name="obj">Source object to generate path to property.</param>
        /// <param name="sourceProperty">Expression pointing to property.</param>
        /// <param name="seperator">Path seperator. Defaults to period.</param>
        /// <returns>Path from source object to target property as a string.</returns>
        /// <example>
        ///   this.GetPropertyPath(x => x.Experience_GovernmentCustomers.ModifiedDate.Year, "/");
        /// returns
        ///   "Experience_GovernmentCustomers/ModifiedDate/Year"
        /// </example>
        public static string GetPropertyPath<T, TProperty>(
            this T obj,
            Expression<Func<T, TProperty>> sourceProperty,
            string seperator)
        {
            if (String.IsNullOrEmpty(seperator))
            {
                seperator = ".";
            }

            MemberExpression sourceMember = sourceProperty.Body as MemberExpression;
            if (sourceMember == null)
            {
                throw new ArgumentException(string.Format(
                    "Expression '{0}' refers to a method, not a property.",
                    sourceProperty.ToString()));
            }

            PropertyInfo sourcePropertyInfo = sourceMember.Member as PropertyInfo;
            if (sourcePropertyInfo == null)
            {
                throw new ArgumentException(string.Format(
                    "Expression '{0}' refers to a field, not a property.",
                    sourceProperty.ToString()));
            }

            string output = sourceProperty.Body.ToString().Substring(sourceProperty.Body.ToString().IndexOf(".") + 1);

            output = output.Replace(".", seperator);

            return output;
        }

        /// <summary>
        /// Attempts to find a path which resolves to a child property for use in the view.
        /// </summary>
        /// <param name="modelType">Type of object to find path for.</param>
        /// <param name="pathSeperator">Property path seperator.</param>
        /// <param name="result">The path to the property to use in the view.</param>
        /// <returns>True if a property path was found, false otherwise.</returns>
        public static bool TryGetViewDisplayPath(Type modelType, string pathSeperator, out string result)
        {
            bool foundPath = false;
            result = String.Empty;

            // check the underlying type. If it has display information, stop recursion.
            if (Reflection.HasMethod(modelType, "PropertyDisplayPath"))
            {
                dynamic o = modelType.Assembly.CreateInstance(modelType.FullName);
                try
                {
                    result = o.PropertyDisplayPath(pathSeperator);
                    foundPath = true;
                }
                catch
                {
                }
            }

            return foundPath;
        }
    }
}
