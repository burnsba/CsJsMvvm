namespace ModelTranslator
{
    using System;
    using System.Web.Mvc;

    public static class HtmlHelpers
    {
        /// <summary>
        /// Generates a javascript view model class based on the current ViewData.Model type.
        /// </summary>
        /// <param name="helper">Html class.</param>
        /// <param name="rootAppend">Additional text to insert into the root of the javascript object.</param>
        /// <returns>A string containing the javascript view model.</returns>
        public static string BuildJsPrototype(this HtmlHelper helper, string rootAppend)
        {
            var modelType = helper.ViewData.Model.GetType();
            var d = BuildJavaScriptViewModel.Build(modelType, rootAppend);
            return d;
        }

        /// <summary>
        /// Generates a javascript view model class based on the current ViewData.Model type.
        /// </summary>
        /// <param name="helper">Html class.</param>
        /// <param name="targetName">Javascript name of generated view model.</param>
        /// <param name="rootAppend">Additional text to insert into the root of the javascript object.</param>
        /// <returns>A string containing the javascript view model.</returns>
        public static string BuildJsPrototype(this HtmlHelper helper, string targetName, string rootAppend)
        {
            var modelType = helper.ViewData.Model.GetType();
            var d = BuildJavaScriptViewModel.Build(modelType, targetName, rootAppend);
            return d;
        }

        /// <summary>
        /// Generates a javascript view model class based on an argument type.
        /// </summary>
        /// <param name="helper">Html class.</param>
        /// <param name="modelType">Type of object to generate javascript view model for.</param>
        /// <param name="rootAppend">Additional text to insert into the root of the javascript object.</param>
        /// <returns>A string containing the javascript view model.</returns>
        public static string BuildJsPrototype(this HtmlHelper helper, Type modelType, string rootAppend)
        {
            var d = BuildJavaScriptViewModel.Build(modelType, rootAppend);
            return d;
        }

        /// <summary>
        /// Generates a javascript view model class based on an argument type.
        /// </summary>
        /// <param name="helper">Html class.</param>
        /// <param name="modelType">Type of object to generate javascript view model for.</param>
        /// <param name="targetName">Javascript name of generated view model.</param>
        /// <param name="rootAppend">Additional text to insert into the root of the javascript object.</param>
        /// <returns>A string containing the javascript view model.</returns>
        public static string BuildJsPrototype(this HtmlHelper helper, Type modelType, string targetName, string rootAppend)
        {
            var d = BuildJavaScriptViewModel.Build(modelType, targetName, rootAppend);
            return d;
        }
    }
}