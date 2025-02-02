# Vehicle Management System

This repository contains a system designed to manage vehicles used to serve government officials. The application **increases transparency** and **accountability** in vehicle usage while simplifying daily workflows for drivers, mechanics, operators, dispatchers, and managers.

---

## Table of Contents

- [Vehicle Management System](#vehicle-management-system)
  - [Table of Contents](#table-of-contents)
  - [Overview](#overview)
  - [Key Features](#key-features)
  - [Environment URLs](#environment-urls)
  - [Technology Stack](#technology-stack)
  - [Getting Started](#getting-started)
    - [Prerequisites](#prerequisites)
    - [Setup](#setup)
    - [Tooling](#tooling)
    - [Documentation](#documentation)

---

## Overview

- **Purpose**: Replace paper-based tracking (mileage, fuel, condition checks) with a secure, digital system.
- **Core Benefit**: Minimizes corruption risks (e.g., underreported mileage to pocket leftover fuel), enforces monthly/yearly usage limits, and logs each ride’s checkpoints (doctor review, mechanic handover, refuel, etc.).

---

## Key Features

1. **Doctor Check**: Approves or rejects a driver’s health before they can take a vehicle.
2. **Mechanic Handover & Acceptance**: Tracks initial and final mileage, car condition.
3. **Fuel Logging**: Operator can log fuel amounts, with potential future enhancements for mid-trip refuels.
4. **Dispatcher Review**: Verifies final mileage against expected fuel consumption, flags discrepancies.
5. **Manager Override**: Allows overriding usage limits or suspicious cases if necessary.
6. **Audit Trails**: Every action recorded and stored in the database to reduce data loss and fraud.

---

## Environment URLs

| Environment  | URL                                          | Notes                |
|--------------|----------------------------------------------|----------------------|
| **Production**   | https://prod.example.com                     | Live environment     |
| **Demo**     | https://demo.example.com                     | Demo/testing environment |
| **Integration** | https://integration.example.com               | Used for development branches |

*(Replace these with your actual URLs, or remove if not applicable.)*

---

## Technology Stack

- **Backend**: ASP.NET Core (REST + SignalR)
- **Database**: SQL Server, EF Core ORM
- **Authentication**: ASP.NET Core Identity, JWT tokens
- **Logging**: Serilog for structured logs
- **Background Jobs**: Hangfire (dashboard protected via role-based auth)
- **Documentation**: Swagger/OpenAPI
- **Hosting**: SmarterASP.NET (for production)

---

## Getting Started

### Prerequisites

- .NET 8
- SQL Server (local or remote)
- Git & text editor/IDE (Visual Studio, VS Code, etc.)

### Setup

1. **Clone the Repository**

   ```bash
   git clone https://github.com/YourOrg/GovVehicleSystem.git
   ```
2. **Configure AppSettings**
- **appsettings.json**
  - **Seq Configuration**
    - Ensure the `serverUrl` for **Seq** points to your local or hosted instance. By default, it's http://localhost:5341. Update the port if it's different for your environment. If you don’t have **Seq** installed, see the Tooling section below for instructions.
  - **Sentry Configuration**
    - Replace the `Dsn` value in the Sentry section with your project’s Sentry DSN. Check the **team chat** for the correct URL.

- **appsettings.Development.json**
  - **ConnectionStrings** (Required)
    - Update the `DefaultConnection` string to point to your local or development database.
  - **EmailConfigurations** (Optional)
    - Replace the `From`, `Server`, `Port`, `Username`, and `Password` fields with your email credentials if testing notifications locally.
  - **SmsConfigurations** (Optional)
    - Update the `Token` and `ApiUrl` values if you intend to test SMS notifications in the development environment.

3. **Apply Migrations**
- Open solution using Visual Studio, open **Package Manager Console** and run `Update-Database`.

4. **Run the API**
- The API will typically be accessible at https://localhost:5001 (or your configured port).

5. **Explore Swagger**
- Navigate to https://localhost:5001/swagger to view the automatically generated API documentation.

---

### Tooling
To fully utilize the logging and diagnostics features, you need to install the following tools:
1. **Seq**
- Seq is used for structured logging. To install it locally:
  - Download from https://datalust.co/seq.
  - Configure it to listen on http://localhost:5341 or update the serverUrl in appsettings.json accordingly.
2. **Sentry**
- Used for error tracking and performance monitoring. The `Dsn` field in `appsettings.json` should point to your Sentry project. Contact the team for the correct **DSN URL**.

---

### Documentation
This repository includes additional documentation to help you understand the system’s architecture, domain, and workflows:

- [Architecture](docs/diagrams/Architecture.md)
- [Domain Model](docs/diagrams/Domain-Model.md)
- [Workflow Diagrams](docs/diagrams/Workflow.md)
- [User Guides](docs/User-Guides.md) (Role-based instructions for daily usage)
- [CONTRIBUTING.md](CONTRIBUTING.md) guidelines on branching strategy, coding standards, test coverage, and CI.