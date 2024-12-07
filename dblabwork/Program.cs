using Microsoft.Data.Sqlite;

const string ConnectionString = "Data Source=todo.db";

void ToDoApp()
{
    Console.WriteLine("To Do App");
    InitializeDatabase();
    while (true)
    {
        Console.WriteLine("\nChoose an option:");
        Console.WriteLine("1. View tasks");
        Console.WriteLine("2. Add a task");
        Console.WriteLine("3. Update a task");
        Console.WriteLine("4. Edit a task");
        Console.WriteLine("5. Delete a task");
        Console.WriteLine("0. Exit");

        var choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                ViewTasks();
                break;
            case "2":
                AddTask();
                break;
            case "3":
                UpdateTask();
                break;
            case "4":
                EditTask();
                break;
            case "5":
                DeleteTask();
                break;
            case "0":
                Console.WriteLine("Goodbye!");
                return;
            default:
                Console.WriteLine("Invalid option. Please try again.");
                break;
        }
    }
}

static void InitializeDatabase()
{
    try
    {
        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();
            var createTableQuery = @"
CREATE TABLE IF NOT EXISTS ToDo (
Id INTEGER PRIMARY KEY AUTOINCREMENT,
Task TEXT NOT NULL,
IsComplete INTEGER NOT NULL DEFAULT 0
);";
            using var command = connection.CreateCommand();
            command.CommandText = createTableQuery;
            command.ExecuteNonQuery();
            
        }
    }
    catch (Exception e)
    {
        Console.WriteLine("Failed to initialize database.");
        Console.WriteLine(e);
        Environment.Exit(0);
    }
}

static void ViewTasks()
{
    try
    {
        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();
            string query = "SELECT Id, Task, IsComplete FROM ToDo;";
            using (var command = new SqliteCommand(query, connection))
            using (var reader = command.ExecuteReader())
            {
                Console.WriteLine("\nTo-Do List:");
                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    string task = reader.GetString(1);
                    bool isComplete = reader.GetInt32(2) == 1;
                    Console.WriteLine($"{id}. {task} (Completed: {isComplete})");
                }
            }
        }
    }
    catch (Exception e)
    {
        Console.WriteLine("Failed to view tasks.");
        Console.WriteLine(e);
    }
}

static void AddTask()
{
    try
    {
        Console.Write("Enter the new task: ");
        string task = Console.ReadLine();

        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();
            string insertQuery = "INSERT INTO ToDo (Task) VALUES (@task);";
            using (var command = new SqliteCommand(insertQuery, connection))
            {
                command.Parameters.AddWithValue("@task", task);
                command.ExecuteNonQuery();
            }
        }
        ViewTasks();
        Console.WriteLine("Task added successfully!");
    }
    catch (Exception e)
    {
        Console.WriteLine("Failed to add task.");
        Console.WriteLine(e);
    }
}

static void UpdateTask()
{
    try
    {
        ViewTasks();
        Console.Write("Enter the ID of the task to update: ");
        int id = int.Parse(Console.ReadLine());

        Console.Write("Is the task complete? y/n: ");
        string answer = Console.ReadLine();
        
        int isComplete;
        if (answer.ToLower() == "y") isComplete = 1;
        else if (answer.ToLower() == "n")  isComplete = 0;
        else throw new Exception("Invalid answer.");
        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();
            string updateQuery = "UPDATE ToDo SET IsComplete = @isComplete WHERE Id = @id;";
            using (var command = new SqliteCommand(updateQuery, connection))
            {
                command.Parameters.AddWithValue("@isComplete", isComplete);
                command.Parameters.AddWithValue("@id", id);
                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    ViewTasks();
                    Console.WriteLine("Task updated successfully!");
                }
                else
                    Console.WriteLine("Task not found.");
            }
        }
    }
    catch (Exception e)
    {
        Console.WriteLine("Failed to update task.");
        Console.WriteLine(e);
    }
}

static void EditTask()
{
    try
    {
        Console.Write("Enter the ID of the task to edit: ");
        int id = int.Parse(Console.ReadLine());

        Console.Write("Enter the new task name: ");
        string editedTask = Console.ReadLine();

        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();
            string updateQuery = "UPDATE ToDo SET Task = @task WHERE Id = @id;";
            using (var command = new SqliteCommand(updateQuery, connection))
            {
                command.Parameters.AddWithValue("@task", editedTask);
                command.Parameters.AddWithValue("@id", id);
                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    Console.WriteLine("Task edited successfully!");
                    ViewTasks();
                }
                else
                    Console.WriteLine("Task not found.");
            }
        }
    }
    catch (Exception e)
    {
        Console.WriteLine("Failed to update task.");
        Console.WriteLine(e);
    }
}

static void DeleteTask()
{
    try
    {
        Console.Write("Enter the ID of the task to delete: ");
        int id = int.Parse(Console.ReadLine());

        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();
            string deleteQuery = "DELETE FROM ToDo WHERE Id = @id;";
            using (var command = new SqliteCommand(deleteQuery, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    ViewTasks();
                    Console.WriteLine("Task deleted successfully!");
                }
                else
                    Console.WriteLine("Task not found.");
            }
        }
    }
    catch (Exception e)
    {
        Console.WriteLine("Failed to delete task.");
        Console.WriteLine(e);
    }
}

// start app
ToDoApp();