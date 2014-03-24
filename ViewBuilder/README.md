## CsJsMvvm.ViewBuilder

Set of Visual Studio T4 Text Templates to automatically create view editors for use with a JavaScript mvvm platform.

- Automatically split camel case into display name
- By default, no properties are included. Properties must be white listed with [ExportToJs] attribute or [ScaffoldColumnAttribute(true)]. ScaffoldColumnAttribute(false) is higher priority than ExportToJs.
- Uses System.ComponentModel.DataAnnotations attributes
- Use [DataTypeAttribute] (example: [DataTypeAttribute("richtextc")] ) to choose view editor template
- Use [Display] attribute Name property to set label name; if type is nullable, use Description property to set checkbox label. (Example: 'public Nullable<System.DateTime> EndDate' with attribute [Display(Name = "End Date", Description = "Currently Ongoing")])

###### A few notes on the nullableValue binding:

- Assumes a checkbox is data bound to the property which should be nullable.
- Assumes the checkbox is a sibling of a DOM element which contains the "base" value (that is, a node data bound to the property), and that	this element has a class of "nullableBaseValue".
- Assumes both the above elements are contained in a parent class.
- Assumes when the checkbox is checked, the base value is null.	This is probably backwards from what would be assumed, but it is primarily used for an 'active' flag, in which case the date is not used.
- See 'nullable.js' for the required JavaScript

Example of how to use the nullableValue binding

        <div class="editor-label">
            <label for="CurrentlyOngoing">Currently ongoing:</label>

            <input type="checkbox" class="k-checkbox" name="CurrentlyOngoing"
                onchange="NullableValueChange(this, 'EndDate', new Date());"
                data-bind="nullableValue: EndDate"/>

            <div class="nullableBaseValue">
                <div class="editor-label">
                    <label for="EndDate">End Date</label>
                </div>

                @(Html.Kendo().DatePicker()
                    .Name("resume-editor-category-"+category+"-end-date")
                    .Start(CalendarView.Year)
                    .Depth(CalendarView.Month)
                    .HtmlAttributes(new { style = "width:150px", data_bind="value: EndDate" })
                )
            </div>
        </div>
