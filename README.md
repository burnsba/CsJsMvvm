CsJsMvvm
========

This is a set of tools designed to auto-generate code for use with a C# MVC site. Knockout JS or similar technology will be used to bind client partial views and the auto-generated JavaScript view models. These tools are not meant to create a fully working site "out of the box," but rather to expedite the process by automatically performing many of the routine (and boring) steps and creating a usable skeleton to build upon.

A brief overview of how these tools might be used:

- Generate a model using Entity Framework.
- Use the TagModel script to automatically duplicate the partial classes created by entity framework and populate all properties with one or more default attributes.
- Manually update any attributes (remove primary keys, list editor type for strings which are rich text, etc).
- In some project -- let's call it Core -- define which Types will be used in the client code.
- After the model has been tagged and useable Types have been listed in Core, use the BuildJavascriptDataModel class to automatically generate JavaScript which will define the view model classes.
- Use the MakeViews script to automatically generate partial views for the listed classes in Core and add an HTML attribute to data bind each property to a property of the same name.

### Notable Features

- If Core lists a class which should have a JavaScript viewmodel but it should not have a director editor -- for instance, it is a child property of another class and does not need it's own partial editor but it does need a JavaScript view model -- the class can be listed as a "hidden" type. Doing so will cause it to be included as a child property of the parent class. Additionally, when building the partial editor views the data bind attribute will reference the full path of the property (Example: Education class with property of type Degree; partial view would have an editor with data bind attribute "value: Education.Degree").  

### Notes

BuildJavascriptDataModel is based on the project at http://buildjavascriptmodel.codeplex.com/
