using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace Jsmvvm
{
    /// <summary>
    /// Used to build a simple table row with strong link to model properties.
    /// </summary>
    /// <typeparam name="T">Model type to use.</typeparam>
    public class TableRow<T>
    {
        #region Fields

        /// <summary>
        /// Collection of table cells in the row.
        /// </summary>
        private List<TableCell<T>> _tableCells = new List<TableCell<T>>();

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of <see cref="TableRow"/>.
        /// </summary>
        public TableRow()
        { }

        /// <summary>
        /// Creates a new instance of <see cref="TableRow"/> and copies all values from source.
        /// </summary>
        /// <param name="src">Item to copy.</param>
        public TableRow(TableRow<T> src)
        {
            src._tableCells.ForEach(x => this._tableCells.Add(x));
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds a new <see cref="TableCell"/> to the <see cref="TableRow"/>.
        /// </summary>
        /// <param name="exp">Expression describing table cell.</param>
        /// <returns>The updated table row.</returns>
        public TableRow<T> Add(Expression<Func<TableCell<T>, TableCell<T>>> exp)
        {
            MethodCallExpression method = exp.Body as MethodCallExpression;
            if (method == null)
            {
                throw new ArgumentException(string.Format(
                    "Expression '{0}' is not a method.",
                    exp.ToString()));
            }

            TableCell<T> td = new TableCell<T>();

            Func<TableCell<T>, TableCell<T>> c = exp.Compile();
            td = c(td);

            _tableCells.Add(td);

            return new TableRow<T>(this);
        }

        /// <summary>
        /// Converts the <see cref="TableRow"/> to a string for use in client page.
        /// </summary>
        /// <returns>String containing table row as html.</returns>
        public override string ToString()
        {
            string output = "<tr>";

            _tableCells.ForEach(x => output += x.ToString());

            output += "</tr>";

            return output;
        }

        #endregion
    }
}