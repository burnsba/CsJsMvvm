using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Jsmvvm
{
    /// <summary>
    /// Kendo mvvm property databind type.
    /// </summary>
    /// <example>
    /// For the following HTML element:
    ///   &lt;input data-bind="value: lastName" />&lt;/label>
    ///   
    /// the DataBindType would be Value
    /// </example>
    public enum DataBindType
    {
        Text,
        Value,
        Checked,
    }
}