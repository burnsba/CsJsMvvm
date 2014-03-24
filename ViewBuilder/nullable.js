<script>
// extends the kendo mvvm binding to support nullable fields
    kendo.data.binders.nullableValue = kendo.data.Binder.extend({
        refresh: function ()
        {
            var value = this.bindings["nullableValue"].get();

            if (!value)
            {
                $(this.element).parent().find(".nullableBaseValue").css("display", "none");
                $(this.element).prop('checked', true);
            } else
            {
                $(this.element).parent().find(".nullableBaseValue").css("display", "inherit");
                $(this.element).prop('checked', false);
            }
        }
    });

    // Called when a checkbox associated with a nullable value changes ('hasValue' property).
    // The viewmodel property is set to null or the newValue depending on the state of the checkbox.
    // obj: HTML node which called change event
    // propertyName: Name of property in view model to update
    // newValue: Default value to set view model property to when non-null.
    function NullableValueChange(obj, propertyName, newValue)
    {
        if (obj === null || obj === undefined || $(obj).get(0) === undefined)
        {
            return;
        }

        if ($(obj).get(0).kendoBindingTarget === null || $(obj).get(0).kendoBindingTarget === undefined)
        {
            return;
        }

        var viewModel = $(obj).get(0).kendoBindingTarget.source;

        if (viewModel === null || viewModel === undefined)
        {
            return;
        }

        if ($(obj).prop('checked'))
        {
            viewModel.set(propertyName, null);
        }
        else
        {
            viewModel.set(propertyName, newValue);
        }
    }
</script>