using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelTranslator.Attributes
{
    /// <summary>
    /// Attribute used to generate a javascript view model from C# classes. Properties
    /// marked with this attribute will be exported.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ExportToViewModel : Attribute
    {
        /// <summary>
        /// Creates a new instance of <see cref="ExportToViewModel"/>.
        /// </summary>
        public ExportToViewModel()
        {}
    }
}
