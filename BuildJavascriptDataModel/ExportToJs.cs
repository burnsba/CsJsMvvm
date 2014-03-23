using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BuildJavascriptDataModel
{
    /// <summary>
    /// Attribute used to generate a javascript view model from C# classes. Properties
    /// marked with this attribute will be exported.
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public class ExportToJs : Attribute
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public ExportToJs()
        { }
    }
}
