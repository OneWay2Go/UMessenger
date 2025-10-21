# UMessenger

A simple messenger application built with C#.

## Key Features & Benefits

*   **Basic Chat Functionality:** Enables users to exchange messages in real-time.
*   **User Management:** Allows for user registration, login, and profile management.
*   **Chat Management:** Supports creation, deletion, and modification of chat groups.
*   **RESTful API:** Provides a clean and well-documented API for interacting with the messenger.

## Prerequisites & Dependencies

Before you begin, ensure you have met the following requirements:

*   [.NET SDK](https://dotnet.microsoft.com/en-us/download) (Version 8.0 or higher)
*   A code editor (e.g., Visual Studio, VS Code with C# extension, JetBrains Rider)
*   (Optional) A database server (e.g., SQL Server, PostgreSQL, MySQL).  The current implementation is likely configured for a default local database if any persistence layer is used - further investigation of the codebase is necessary.

## Installation & Setup Instructions

Follow these steps to get the project up and running:

1.  **Clone the repository:**

    ```bash
    git clone https://github.com/OneWay2Go/UMessenger.git
    cd UMessenger
    ```

2.  **Restore NuGet Packages:**

    ```bash
    dotnet restore UMessenger.sln
    ```

3.  **Build the project:**

    ```bash
    dotnet build UMessenger.sln
    ```

4.  **Navigate to the API directory:**

    ```bash
    cd src/Messenger.API
    ```

5.  **(Optional) Apply Database Migrations:** This step depends on your database configuration and whether Entity Framework Core or another ORM is used.  Inspect the codebase for any migration commands.

    ```bash
    # Example command (may need adjustment based on actual implementation)
    dotnet ef database update
    ```

6.  **Run the application:**

    ```bash
    dotnet run
    ```

    The API should now be running, usually accessible at `http://localhost:5000` or `https://localhost:5001`.

## Usage Examples & API Documentation

The UMessenger API exposes the following endpoints:

*   **`GET /api/ChatUsers`:** Retrieves a list of chat users. See `ChatUsersController.cs`.
*   **`GET /api/Chats`:** Retrieves a list of chats. See `ChatsController.cs`.
*   **`GET /api/Messages`:** Retrieves a list of messages. See `MessagesController.cs`.
*   **`GET /api/Users`:** Retrieves a list of users. See `UsersController.cs`.

For detailed API documentation, including request parameters and response formats, refer to the Swagger UI (if enabled, usually accessible at `http://localhost:5000/swagger` or `https://localhost:5001/swagger` after running the application).  Inspect the `Program.cs` to confirm Swagger is enabled.

## Configuration Options

The application's configuration is managed through `appsettings.json` and `appsettings.Development.json`. You can modify the following settings:

*   **Connection Strings:**  Located in the `appsettings.json` file.  Configure the database connection string to point to your database server.
*   **Logging:** Configure logging levels and destinations in the `appsettings.json` file.
*   **Other settings:** Other settings might exist, such as authentication configuration or API keys, depending on the complexity of the application.  Review the `appsettings.json` files thoroughly.

## Contributing Guidelines

We welcome contributions to UMessenger! To contribute:

1.  Fork the repository.
2.  Create a new branch for your feature or bug fix.
3.  Implement your changes, ensuring code quality and adding appropriate tests.
4.  Commit your changes with clear and descriptive commit messages.
5.  Submit a pull request to the main branch.

## Acknowledgments

This project uses the .NET SDK and ASP.NET Core framework.
