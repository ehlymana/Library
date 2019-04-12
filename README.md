# Library

*Library* is a C# Windows Forms app for managing a library.

## The Code

**BooksLibrary** contains the classes necessary for the application to work correctly.

- The *Book*, *Author* and *Genre* classes are helpers for the database entries.
- The *Validation* class is a helper for the data validation.
- The *PostgreSQL* class is a helper for database connection.

**LibraryForms** contains the Windows Forms necessary for the application to work correctly.

- The *BooksDashboard* form is the main form of the app.
- The *AddNewBooks* form is the form for adding new books into the database.
- The *SearchBooks* form is the form for filtering database entries.

## Application Features

- Adding books to the library (with proper validation);
- Editing books (directly in the table or separately);
- Deleting books (directly from the table);
- Filtering books by genres/authors;
- Importing/exporting JSON data.

## Additional Information

The application uses a custom-made **PostgreSQL** connection. The connection string can be changed depending on your own preferences.

You can use the release version of the app, which is located in the following directory:

```
Library\LibraryForms\bin\Release\LibraryForms.exe
```

2019. © *Krupalija Ehlimana*