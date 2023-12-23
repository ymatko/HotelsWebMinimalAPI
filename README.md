# HotelsWebMinimalAPI

**Description:**
This is a minimalistic ASP.NET Core web API for managing hotel information. It provides endpoints for handling hotel data, including CRUD operations, user authentication, and token-based authorization using JSON Web Tokens (JWT). The API integrates Swagger/OpenAPI for easy documentation and testing.

**Key Features:**
- User authentication and authorization using JWT.
- CRUD operations for managing hotel data.
- Integration with Swagger/OpenAPI for API documentation.

**Getting Started:**
1. Clone this repository to your local machine.
2. Update the connection string in the `appsettings.json` file to point to your desired database.
3. Run the application.

**Dependencies:**
- ASP.NET Core
- Entity Framework Core
- Swagger/OpenAPI
- JWT for authentication and authorization

**Installation:**
No additional installation steps are required. Simply clone the repository and run the application.

**Configuration:**
Update the database connection string and JWT settings in the `appsettings.json` file to match your environment.

**Usage:**
- Access Swagger UI at `/swagger` to explore and test API endpoints.
- Use the `/login` endpoint to obtain a JWT token for authentication.

**API Endpoints:**
- `/hotels`: GET all hotels (authenticated)
- `/hotels/{id}`: GET a specific hotel by ID (authenticated)
- `/hotels`: POST a new hotel (authenticated)
- `/hotels`: PUT update an existing hotel (authenticated)
- `/hotels/{id}`: DELETE a hotel by ID (authenticated)
- `/hotels/search/name/{query}`: GET hotels by name search (authenticated)
- `/hotels/search/location/{coordinate}`: GET hotels by location coordinates (authenticated)

**Security:**
This API uses JWT for user authentication and authorization. Make sure to secure your JWT secret key and configure the appropriate settings in the `appsettings.json` file.

**Development:**
- The application automatically creates the database schema on startup in development mode.
- Swagger UI is available in development mode for easy testing and documentation.

>This project is licensed under the MIT License - see the [LICENSE](https://github.com/ymatko/HotelsWebMinimalAPI/blob/main/LICENSE.txt) file for details.
