using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelTranslator.Attributes
{
    /// <summary>
    /// Describes how to display a list in the view.
    /// </summary>
    public enum ListHint
    {
        /// <summary>
        /// Item is not a list.
        /// </summary>
        Unused,

        /// <summary>
        /// Display each item of the list on it's own line.
        /// </summary>
        OnePerLine,

        /// <summary>
        /// Display all the items of the list on one line and use a seperator between each object.
        /// </summary>
        UseSeperator
    }
}
