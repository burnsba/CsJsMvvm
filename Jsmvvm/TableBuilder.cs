using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;

namespace Jsmvvm
{
    /// <summary>
    /// Used to build a simple table object with strong link to model properties. Basically, a simple kendo grid
    /// that will support client view model binding.
    /// </summary>
    /// <typeparam name="T">Model type to use.</typeparam>
    public class TableBuilder<T>
    {
        /// <summary>
        /// Builds a table row.
        /// </summary>
        /// <param name="exp">Configuration of row.</param>
        /// <returns>Unescaped html string containing table row.</returns>
        public HtmlString SimpleTr(Expression<Func<TableRow<T>, TableRow<T>>> exp)
        {
            MethodCallExpression method = exp.Body as MethodCallExpression;
            if (method == null)
            {
                throw new ArgumentException(string.Format(
                    "Expression '{0}' is not a method.",
                    exp.ToString()));
            }

            Func<TableRow<T>, TableRow<T>> compiledExpression = exp.Compile();

            TableRow<T> tableRow = new TableRow<T>();

            tableRow = compiledExpression(tableRow);

            HtmlString htmlString = new HtmlString(tableRow.ToString());
            return htmlString;
        }

        /// <summary>
        /// Adds jquery code to turn an object into an editable object.
        /// </summary>
        /// <typeparam name="Tprop">Type of model.</typeparam>
        /// <param name="class">Name of class of editable element.</param>
        /// <param name="target">JavaScript function to run when the object is edited.</param>
        /// <param name="modelProperty">Path to property to bind to in the view model.</param>
        /// <returns>Unescaped html string containing jquery javascript to turn an object into an editable object.</returns>
        public HtmlString EditableTd<Tprop>(string @class, string target, Expression<Func<T, Tprop>> modelProperty)
        {
            MemberExpression member = modelProperty.Body as MemberExpression;
            if (member == null)
            {
                throw new ArgumentException(string.Format(
                    "Expression '{0}' refers to a method, not a property.",
                    modelProperty.ToString()));
            }

            PropertyInfo propInfo = member.Member as PropertyInfo;
            if (propInfo == null)
            {
                throw new ArgumentException(string.Format(
                    "Expression '{0}' refers to a field, not a property.",
                    modelProperty.ToString()));
            }

            string output = "";

            // double brackets escape in String.Format
            output += String.Format("$(\".{0}\").editable(\"https://\", {{", @class);
            output += String.Format(" target: {0},", target);
            output += String.Format(" viewModelProperty: '{0}'", propInfo.Name);
            output += String.Format("}});");
       
            return new HtmlString(output);
        }
    }
}