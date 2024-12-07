# To-Do App Documentation

## Description
This is a simple project DBMS project for School.

## Features
This To-Do App supports the following operations:

1. **Create a New Task**
2. **Update a Task**
3. **Edit a Task**
4. **Delete a Task**
5. **View Tasks**

## Database Connection
The app uses the **Microsoft.Data.Sqlite** NuGet package to connect to an SQLite database and execute queries.

### Connection Details
- The connection string is the relative path of the database file.
- The program creates the database file if it does not exist.

## Error Handling
- The program has inbuilt error handling to ensure graceful exits if it fails to connect to the database.
- If any of the operations fail:
  - An error message is displayed.
  - The program does **not** exit but returns to the main menu.
  - Users can retry the operation or exit the program using the `0` option.
