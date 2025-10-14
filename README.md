# AMI Assessment - Weather Application

A full-stack weather application built with ASP.NET Core backend and React frontend. Users can search for weather information by entering a city name and receive current weather data.

**Author:** Selina Liu

## Tech Stack

### Backend
- **ASP.NET Core** (.NET 9)
- **C#** for API development
- **Swagger/OpenAPI** for API documentation
- **xUnit** for unit testing

### Frontend
- **React** (v19)
- **Vite** for build tooling
- **FontAwesome** for icons
- **CSS** for styling

## Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- [Node.js](https://nodejs.org/) (v18 or higher)
- npm (comes with Node.js)

## Setup Instructions

### 1. Clone the Repository

```bash
git clone <repository-url>
cd AMI_Assessment
```

### 2. Backend Setup

The backend is already configured and ready to run. No additional setup is required.

### 3. Frontend Setup

Install frontend dependencies:

```bash
cd frontend
npm install
```

Build the frontend:

```bash
npm run build
```

This creates a production build in the `frontend/dist` directory.

## Running the Application

### Option 1: Run the Full Application

From the `backend` directory:

```bash
cd backend
dotnet run
```

The application will be available at `http://localhost:5262` (or the port specified in the console).

The backend serves the frontend static files, so you only need to run the backend to access the full application.

### Option 2: Development Mode

If you want to develop the frontend separately:

```bash
cd frontend
npm run dev
```

The frontend development server will run at `http://localhost:5173`.

**Note:** In development mode, uncomment the CORS policy in `backend/Program.cs` and run both backend and frontend servers simultaneously.

## Running Tests

Run backend unit tests:

```bash
cd backend.Tests
dotnet test
```

## Project Structure

```
AMI_Assessment/
├── backend/                 # ASP.NET Core API
│   ├── Controllers/         # API controllers
│   ├── Models/              # Data models
│   ├── Services/            # Business logic
│   ├── Utils/               # Utility classes
│   └── Program.cs           # Application entry point
├── backend.Tests/           # Backend unit tests
│   └── Services/            # Service tests
└── frontend/                # React frontend
    ├── src/
    │   ├── Components/      # React components
    │   └── Styles/          # CSS stylesheets
    └── dist/                # Production build (generated)
```

## API Endpoints

- `POST /api/weather` – Get weather data for a specific location
  - **Request body** (JSON):
    ```json
    {
      "city": "string",
      "state": "ST",
      "zip": "17507",
      "unitOfMeasurement": "F"
    }
    ```
  - Response: Weather information including temperature, description, etc.

## Swagger Documentation

When running in development mode, access the Swagger UI at:
`http://localhost:5262/swagger`

## Features

- City-based weather search
- Responsive design
- Loading states and error handling
- Clean and modern UI
