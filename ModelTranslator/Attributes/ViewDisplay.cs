using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelTranslator.Attributes
{
    /// <summary>
    /// Gives information about how the item or items should be displayed in the view.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class)]
    public class ViewDisplay : Attribute
    {
        #region Fields

        public const string DefaultListSeperator = ",";

        private string _selfDisplayName = String.Empty;
        private bool _hasSelfDisplayName = false;

        private string _singularDisplayName = String.Empty;
        private bool _hasSingularDisplayName = false;

        private ListHint _listHint = ListHint.Unused;
        private string _listSeperator = DefaultListSeperator;

        private string _optionDisplayName = String.Empty;
        private bool _hasOptionDisplayName = false;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of <see cref="ViewDisplay"/>.
        /// </summary>
        /// <param name="displayName">Value to be used to identify the property in the view.</param>
        public ViewDisplay(string displayName)
        {
            _selfDisplayName = displayName;
            _hasSelfDisplayName = true;
        }

        /// <summary>
        /// Creates a new instance of <see cref="ViewDisplay"/>.
        /// </summary>
        /// <param name="displayName">Value to be used to identify the property in the view.</param>
        /// <param name="listHint">How items in a list should be displayed.</param>
        /// <param name="listSeperator">Seperator between items in a list.</param>
        public ViewDisplay(string displayName, ListHint listHint, string listSeperator = DefaultListSeperator)
        {
            _selfDisplayName = displayName;
            _hasSelfDisplayName = true;

            _listHint = listHint;
            _listSeperator = listSeperator;
        }

        /// <summary>
        /// Creates a new instance of <see cref="ViewDisplay"/>.
        /// </summary>
        /// <param name="collectionDisplayName">Value to be used to identify the property in the view.</param>
        /// <param name="itemDisplayName">Value to be used to identify an item in the collection in the view.</param>
        public ViewDisplay(string collectionDisplayName, string itemDisplayName)
        {
            _selfDisplayName = collectionDisplayName;
            _hasSelfDisplayName = true;

            _singularDisplayName = itemDisplayName;
            _hasSingularDisplayName = true;
        }

        /// <summary>
        /// Creates a new instance of <see cref="ViewDisplay"/>.
        /// </summary>
        /// <param name="collectionDisplayName">Value to be used to identify the property in the view.</param>
        /// <param name="itemDisplayName"></param>
        /// <param name="listHint">How items in a list should be displayed.</param>
        /// <param name="listSeperator">Seperator between items in a list.</param>
        public ViewDisplay(string collectionDisplayName, string itemDisplayName, ListHint listHint, string listSeperator = DefaultListSeperator)
        {
            _selfDisplayName = collectionDisplayName;
            _hasSelfDisplayName = true;

            _singularDisplayName = itemDisplayName;
            _hasSingularDisplayName = true;

            _listHint = listHint;
            _listSeperator = listSeperator;
        }

        /// <summary>
        /// Creates a new instance of <see cref="ViewDisplay"/>.
        /// </summary>
        /// <param name="displayName">Value to be used to identify the property in the view.</param>
        /// <param name="optionDisplayName">Value to be used on the view option item (checkbox) in the view.</param>
        /// <param name="t">Ignored type parameter used to generate unique signature for use with nullable types. Use static NullableType.</param>
        public ViewDisplay(string displayName, string optionDisplayName, NullableType t)
        {
            _selfDisplayName = displayName;
            _hasSelfDisplayName = true;

            _optionDisplayName = optionDisplayName;
            _hasOptionDisplayName = true;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the display value for the property.
        /// </summary>
        public string DisplayName
        {
            get
            {
                return _selfDisplayName;
            }
        }

        /// <summary>
        /// Gets a value indicating whether or not the display name property is used.
        /// </summary>
        public bool HasDisplayName
        {
            get
            {
                return _hasSelfDisplayName;
            }
        }

        /// <summary>
        /// Gets the display value for a single item. To be used if the property is a collection.
        /// </summary>
        public string SingularDisplayName
        {
            get
            {
                return _singularDisplayName;
            }
        }

        /// <summary>
        /// Gets a value indicating whether or not the singular display name property is used.
        /// </summary>
        public bool HasSingularDisplayName
        {
            get
            {
                return _hasSingularDisplayName;
            }
        }

        public string OptionDisplayName
        {
            get
            {
                return _optionDisplayName;
            }
        }

        public bool HasOptionDisplayName
        {
            get
            {
                return _hasOptionDisplayName;
            }
        }

        /// <summary>
        /// Gets a value indicating how a list should be displayed.
        /// </summary>
        public ListHint ListDisplayHint
        {
            get
            {
                return _listHint;
            }
        }

        /// <summary>
        /// Gets a value indicating how items in the list should be seperated.
        /// </summary>
        public string ListSeperator
        {
            get
            {
                return _listSeperator;
            }
        }

        #endregion
    }
}
