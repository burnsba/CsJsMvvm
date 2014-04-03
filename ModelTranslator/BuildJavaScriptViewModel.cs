using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Collections;
using ModelTranslator.Attributes;
using ModelTranslator.Utilities;

namespace ModelTranslator
{
    /// <summary>
    /// Returns a JavaScript object that represents your .net data model.
    /// It works by using reflection to find all the public properties in the class that
    /// are marked by the <see cref="ExportToViewModel"/> attribute.
    /// </summary>
    /// <remarks>
    /// Partial classes can add custom attributes by using the <see cref="MetadataType"/>
    /// attribute.
    /// 
    /// Based on the project at http://buildjavascriptmodel.codeplex.com/
    /// </remarks>
    public class BuildJavaScriptViewModel
    {
        #region Constants and Fields

        /// <summary>
        /// This is the value that is assigned to each property that represents
        /// a value type in the Datamodel.
        /// </summary>
        //private const string AssignedValue = " ko.observable()";
        private const string AssignedValue = "\"\"";
        private const string ListValue = "[]";
        private const string DateValue = "new Date()";

        #endregion

        #region Public Methods

        /// <summary>
        /// Builds JavaScript data model from modelType.  The generated function will be assigned
        /// to variable that has the same name as the modelType
        /// </summary>
        /// <param name="modelType">.Net class to build JavaScript data model for</param>
        /// <param name="rootAppend">Additional text to insert into the root of the javascript object.</param>
        /// <returns>String contains the generated Json Datamodel</returns>
        public static string Build(Type modelType, string rootAppend)
        {
            return Build(modelType, modelType.Name, rootAppend);
        }

        /// <summary>
        /// Builds JavaScript data model from modelType. 
        /// </summary>
        /// <param name="modelType">.Net class to build JavaScript data model for</param>
        /// <param name="targetName">Name to be assigned to function that returns the data model.</param>
        /// <param name="rootAppend">Additional text to insert into the root of the javascript object.</param>
        /// <returns>String contains the generated Json Datamodel</returns>
        public static string Build(Type modelType, string targetName, string rootAppend)
        {
            if (targetName == null)
            {
                targetName = modelType.Name;
            }
            return InternalFormat(targetName, 1, modelType.Name, modelType, rootAppend);
        }

        #endregion

        #region Methods

        /// <summary>
        ///  Internal, Recursive function that does the actual conversion
        /// </summary>
        /// <param name="targetName">Name to be given to the function that generates the data model</param>
        /// <param name="level">Current Recursion Level</param>
        /// <param name="typeName">Name of the type that data model is being generated for</param>
        /// <param name="modelType">Used to retrieve attributes for partial classes.</param>
        /// <param name="rootAppend">Additional text to insert into the root of the javascript object.</param>
        /// <param name="lastItem">Used to track whether or not a comma should be inserted after an item, after the first level of recursion.</param>
        /// <returns>String contains the generated Json Datamodel</returns>
        private static string InternalFormat(string targetName, int level, string typeName, Type modelType, string rootAppend, bool lastItem = true)
        {
            // to prevent infinite run-away, terminate recursion at 10 levels.
            if (level > 10)
            {
                throw new OverflowException("Excessive Recursion.  Please review data model type.");
            }

            const string eol = "\r\n";
            const string spaces = " ";
            const string dashes = "//---------------------------------------------------------------" + eol;
            string padding = spaces.PadLeft((level + 1) * 4);
            var sb = new StringBuilder();
            if (level == 1)
            {
                sb.Append(eol + dashes);
                sb.Append("//      Begin Auto Generated code -------------------------------" + eol);
                string s = "//      For Model Type - " + typeName + "   " + dashes.Substring(2);
                s = s.Substring(0, dashes.Length) + eol;
                sb.Append(s);
                sb.Append(dashes);

                sb.AppendFormat("var {0} = function(){{ {1} ", targetName, eol);
                sb.Append("    return {" + eol);
            }
            else
            {
                sb.AppendFormat("{0}{1}: {{ " + eol, spaces.PadLeft((level) * 4), targetName);
            }

            List<PropertyInfo> properties = Reflection.GetProperties(modelType);

            int counter = 0;
            int len = properties.Count(x => Attribute.IsDefined(x, typeof(ExportToViewModel)));
            
			
            // go over all the properties found
            foreach (PropertyInfo propertyInfo in properties)
            {
                if (!Attribute.IsDefined(propertyInfo, typeof(ExportToViewModel)))
                {
                    continue;
                }

                counter++;
                
                bool stopRecursion = false;

                PropertyInfo[] p = propertyInfo.PropertyType.GetProperties();

                bool isList = Reflection.IsList(propertyInfo.PropertyType);

                if (isList)
                {
                    stopRecursion = true;
                }

                string fullName = propertyInfo.PropertyType.FullName.ToLower();

                if (Reflection.IsNullable(propertyInfo.PropertyType) && Reflection.IsSimpleGeneric(propertyInfo.PropertyType))
                {
                    // this is a nullable type, but the underlying type is simple,
                    // so avoid recursing an object with one property.
                    Type underlyingType = Reflection.GetSimpleGenericUnderlying(propertyInfo.PropertyType);
                    {
                        p = underlyingType.GetProperties();
                        fullName = underlyingType.FullName.ToLower();
                        stopRecursion = true;
                    }
                }

                // is this a simple builtin type?
                switch (fullName.ToLower())
                {
                    // some basic .NET types have child properties, but these shouldn't be recursed.
                    case "system.string": /* fall through */
                    case "system.datetime":
                    case "system.guid":
                        stopRecursion = true;
                        break;
                    default:
                        break;
                }

                // decide whether to recurse or output.
                if (stopRecursion)
                {
                    if (isList)
                    {
                        StringBuilderHelper(ref sb,
                            padding,
                            propertyInfo.Name,
                            ListValue,
                            counter == len);
                    }
                    else if (fullName == "system.datetime")
                    {
                        StringBuilderHelper(ref sb,
                                padding,
                                propertyInfo.Name,
                                DateValue,
                                counter == len);
                    }
                    else
                    {
                        StringBuilderHelper(ref sb,
                                padding,
                                propertyInfo.Name,
                                AssignedValue,
                                counter == len);
                    }
                }
                else
                {
                    string d = InternalFormat(propertyInfo.Name, level + 1, "", propertyInfo.PropertyType, "", counter == len);
                    sb.Append(d);
                }

            }

            // closing brace is included below, but need to check for rootAppend text
            if (level != 1)
            {
                // closing brace
                sb.Append(padding + "}");

                // can't include this if statement above, or the else branch will be taken when
                // it shouldn't.
                if (!lastItem)
                {
                    sb.Append(",");
                }
            }
            else
            {
                if (!String.IsNullOrEmpty(rootAppend))
                {
                    // comma after all previous items
                    if (properties.Count() > 0)
                    {
                        sb.AppendFormat("{0},", padding);
                        sb.AppendLine();
                    }
                    // add extra text
                    sb.AppendFormat("{2}//--{0}{2}{1}{0}", eol, rootAppend, padding);
                    // last closing brace
                    sb.Append("    }");
                    sb.AppendLine();
                }
                else
                {
                    // last closing brace
                    sb.Append("    }");
                    sb.AppendLine();
                }
                sb.Append("}" + eol);
                sb.Append(dashes);
                sb.Append("//      End Auto Generated code -------------------------------" + eol);
                sb.Append(dashes);
            }
            sb.AppendLine();

            return sb.ToString();
        }

        /// <summary>
        /// String builder function to append object name and default value.
        /// </summary>
        /// <param name="sb">String builder object in use.</param>
        /// <param name="padding">Left side padding before output, as spaces.</param>
        /// <param name="memberName">Name of property to output</param>
        /// <param name="javascriptValue">Value to set property to.</param>
        /// <param name="last">
        /// Whether this is the last item. If not, a
        /// comma will be included after the item.
        /// </param>
        private static void StringBuilderHelper(ref StringBuilder sb, string padding, string memberName, string javascriptValue, bool last = false)
        {
            sb.AppendFormat("{0}{1}: {2}", padding, memberName, javascriptValue);
            if (last == false)
            {
                sb.Append(",");
            }
            sb.AppendLine();
        }

        #endregion
    }
}