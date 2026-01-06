# Appointment Booking System - .NET Core Web API

A comprehensive .NET Core Web API for managing medical appointments with doctor schedules, appointment slots, and user bookings.

## Features

- **User Authentication**: JWT-based authentication with role-based access control
- **Role-Based Authorization**: Admin, Doctor, and User roles
- **Doctor Management**: Admin can add doctors and manage specializations
- **Appointment Slots**: Admin manages available appointment slots for doctors
- **Appointment Booking**: Users can book appointments with available doctors
- **Schedule Management**: Doctors can view their schedules and appointments
- **Global Error Handling**: Centralized exception handling middleware
- **Logging**: Serilog integration for comprehensive logging
- **Swagger UI**: Interactive API documentation

## Technology Stack

- .NET 8.0
- Entity Framework Core
- MySQL Database
- JWT Authentication
- Serilog Logging
- Pomelo.EntityFrameworkCore.MySql

## Prerequisites

- .NET 8.0 SDK or higher
- MySQL 8.0 or higher
- Visual Studio Code or Visual Studio

## Installation & Setup

### 1. Database Setup

Create the database using the provided SQL script:

```bash
mysql -u root -p < Database/script.sql
```

Or execute the SQL commands directly in MySQL Workbench.

### 2. Configure Connection String

Update the connection string in `appsettings.json`:

```json
"ConnectionStrings": {
    "DefaultConnection": "server=localhost;port=3306;database=appointmentbooking;uid=root;pwd=your_password;"
}
```

### 3. JWT Configuration

Update JWT settings in `appsettings.json`:

```json
"JwtSettings": {
    "SecretKey": "your-super-secret-key-that-is-at-least-32-characters-long",
    "Issuer": "AppointmentBooking",
    "Audience": "AppointmentBookingAPI",
    "ExpirationMinutes": 1440
}
```

### 4. Restore Dependencies & Run

```bash
dotnet restore
dotnet run
```

The API will be available at `https://localhost:7069`

## API Endpoints

### Authentication

- **POST** `/api/auth/register` - Register new user
- **POST** `/api/auth/login` - Login user

### Doctors

- **GET** `/api/doctors` - Get all doctors
- **GET** `/api/doctors/{id}` - Get specific doctor
- **POST** `/api/doctors` - Create doctor (Admin only)
- **GET** `/api/doctors/{id}/schedule` - Get doctor schedule (Doctor only)
- **GET** `/api/doctors/{id}/appointments` - Get doctor appointments (Doctor only)

### Appointment Slots

- **GET** `/api/slots/available` - Get available slots
- **GET** `/api/slots/doctor/{doctorId}` - Get doctor slots
- **GET** `/api/slots/{id}` - Get specific slot
- **POST** `/api/slots` - Create slot (Admin only)

### Appointments

- **POST** `/api/appointments` - Book appointment (User only)
- **GET** `/api/appointments/my-appointments` - Get user appointments (User only)
- **GET** `/api/appointments/{id}` - Get specific appointment
- **POST** `/api/appointments/{id}/cancel` - Cancel appointment (User only)

## Database Schema

### Users Table
- Id (Primary Key)
- Email (Unique)
- PasswordHash
- Role (Admin, Doctor, User)
- CreatedAt

### Doctors Table
- Id (Primary Key)
- Name
- Specialization
- Phone
- Email
- UserId (Foreign Key)

### AppointmentSlots Table
- Id (Primary Key)
- DoctorId (Foreign Key)
- StartTime
- EndTime
- MaxCapacity
- CurrentBookings

### Appointments Table
- Id (Primary Key)
- AppointmentSlotId (Foreign Key)
- DoctorId (Foreign Key)
- UserId (Foreign Key)
- PatientName
- PatientPhone
- PatientEmail
- Reason
- Status
- BookedAt

## Project Structure

```
AppointmentBooking/
├── Controllers/
│   ├── AuthController.cs
│   ├── DoctorsController.cs
│   ├── SlotsController.cs
│   └── AppointmentsController.cs
├── Models/
│   ├── User.cs
│   ├── Doctor.cs
│   ├── AppointmentSlot.cs
│   └── Appointment.cs
├── Data/
│   └── AppointmentContext.cs
├── Repositories/
│   └── Repositories.cs
├── Services/
│   ├── AuthService.cs
│   ├── UserService.cs
│   ├── DoctorService.cs
│   ├── AppointmentSlotService.cs
│   └── AppointmentService.cs
├── DTOs/
│   └── Dtos.cs
├── Middleware/
│   └── GlobalExceptionHandlingMiddleware.cs
├── Database/
│   └── script.sql
├── Program.cs
├── appsettings.json
└── README.md
```

## Authentication Flow

1. Register/Login to get JWT token
2. Include token in Authorization header: `Bearer {token}`
3. Token is valid for 24 hours by default
4. Different roles have different access permissions

## Testing with Postman

Import the `Postman_Collection.json` file into Postman to test all API endpoints.

Update the `{{token}}` variable after login with the received JWT token.

## Error Handling

The API includes global error handling middleware that catches all exceptions and returns appropriate HTTP status codes:

- 400 Bad Request - Invalid input
- 401 Unauthorized - Authentication failed
- 404 Not Found - Resource not found
- 500 Internal Server Error - Server error

## Logging

Logs are configured using Serilog and output to the console. You can modify the logging configuration in `Program.cs`.

## Building for Production

```bash
dotnet publish -c Release -o ./publish
```

## Docker Support (Optional)

A Dockerfile can be added for containerization if needed.

## Author

Created as a .NET Core assignment for Appointment Booking System

## License

MIT License
