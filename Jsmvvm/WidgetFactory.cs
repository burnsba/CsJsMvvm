using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;

namespace Jsmvvm
{
    // This files contains methods and classes for generating code for use with kendo observable items.

    /// <summary>
    /// Base container class.
    /// </summary>
    public static class WidgetFactory
    {
        #region Methods

        /// <summary>
        /// Creates a new instance of <see cref="TableBuilder"/>.
        /// </summary>
        /// <typeparam name="T">Model type to use.</typeparam>
        /// <returns>A TableBuilder.</returns>
        public static TableBuilder<T> Table<T>()
        {
            return new TableBuilder<T>();
        }

        #endregion
    }

    
}