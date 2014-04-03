using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelTranslator.Attributes
{
    /// <summary>
    /// A way to mark a property for export as its own view model definition.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class RequiresOwnExport : Attribute
    {
        /// <summary>
        /// Creates a new instance of <see cref="RequiresOwnExport"/>.
        /// </summary>
        /// <param name="export">Whether or not to export the property as its
        /// own definition.</param>
        public RequiresOwnExport()
        {}
    }
}
