using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;

namespace Jsmvvm
{
    /// <summary>
    /// Used to build a simple table cell with strong link to model properties.
    /// </summary>
    /// <typeparam name="T">Model type to use.</typeparam>
    public class TableCell<T>
    {
        #region Fields

        /// <summary>
        /// Contents of cell.
        /// </summary>
        private string _innerHtml = String.Empty;

        /// <summary>
        /// Flag for whether or not to include class information.
        /// </summary>
        private bool _useClass = false;

        /// <summary>
        /// Class information for cell.
        /// </summary>
        private string _class = String.Empty;

        /// <summary>
        /// Flag for whether or not to bind to a property in the view model.
        /// </summary>
        private bool _useDataBind = false;

        /// <summary>
        /// Type of data bind target property.
        /// </summary>
        private DataBindType _dataBindType;

        /// <summary>
        /// Path to property being bound in the view model.
        /// </summary>
        private string _dataBindTarget;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of <see cref="TableCell"/>.
        /// </summary>
        public TableCell()
        { }

        /// <summary>
        /// Creates a new instance of <see cref="TableCell"/> and copies all values from source.
        /// </summary>
        /// <param name="src">Item to copy.</param>
        private TableCell(TableCell<T> src)
        {
            _innerHtml = src._innerHtml;
            _useClass = src._useClass;
            _class = src._class;
            _useDataBind = src._useDataBind;
            _dataBindType = src._dataBindType;
            _dataBindTarget = src._dataBindTarget;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Sets the contents of the cell.
        /// </summary>
        /// <param name="value">Contents of the cell.</param>
        /// <returns>The updated table cell.</returns>
        public TableCell<T> InnerHtml(string value)
        {
            _innerHtml = value;

            return new TableCell<T>(this);
        }

        /// <summary>
        /// Sets the class attribute of the cell.
        /// </summary>
        /// <param name="value">CSS class names.</param>
        /// <returns>The updated table cell.</returns>
        public TableCell<T> Class(string value)
        {
            _useClass = true;
            _class = value;

            return new TableCell<T>(this);
        }

        /// <summary>
        /// Sets the data bind attribute of the table cell. Does not support path to child properties.
        /// </summary>
        /// <typeparam name="Tprop">Type of object.</typeparam>
        /// <param name="dataBindType">Type of data bind target property.</param>
        /// <param name="modelProperty">Path to property to bind to in view model.</param>
        /// <returns>The updated table cell.</returns>
        public TableCell<T> DataBind<Tprop>(DataBindType dataBindType, Expression<Func<T, Tprop>> modelProperty)
        {
            _useDataBind = true;
            _dataBindType = dataBindType;

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

            _dataBindTarget = propInfo.Name;

            return new TableCell<T>(this);
        }

        /// <summary>
        /// Converts the <see cref="TableCell"/> to a string for use in client page.
        /// </summary>
        /// <returns>String containing table cell as html.</returns>
        public override string ToString()
        {
            string dataBindText = String.Empty;
            string classText = String.Empty;

            if (_useClass)
            {
                classText = String.Format(" class='{0}'", _class);
            }

            if (_useDataBind)
            {
                dataBindText = String.Format(" data-bind='{0}: {1}'", _dataBindType.ToString().ToLower(), _dataBindTarget);
            }

            string openTag = String.Format("<td{0}{1}>", classText, dataBindText);

            return String.Format("{0}{1}</td>", openTag, _innerHtml);
        }

        #endregion
    }
}