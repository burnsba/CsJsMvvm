using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ModelTranslator;

namespace ModelTranslator.Attributes
{
    /// <summary>
    /// Attribute used to generate an output object to be shown to the user. Properties
    /// marked with this attribute will be exported.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ExportToView : Attribute
    {
        /// <summary>
        /// Creates a new instance of <see cref="ExportToView"/>.
        /// </summary>
        public ExportToView()
        {}
    }
}
