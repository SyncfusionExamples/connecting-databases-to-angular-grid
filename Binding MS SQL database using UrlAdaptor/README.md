# Syncfusion Angular Grid with ASP.NET Core Web API and Microsoft SQL Server

A minimal, high‑level user guide for binding **Microsoft SQL Server** data to a **Syncfusion Angular Grid** using an **ASP.NET Core Web API**. This document focuses on concepts, flow, and run commands only, without implementation details.

---

## Key Features

- SQL Server → ASP.NET Core Web API → Angular architecture
- Syncfusion Angular Grid with paging, sorting, filtering, and searching
- DataManager + UrlAdaptor for remote data binding
- Full CRUD and batch operations
- Server‑side data processing

---

## Prerequisites

- Node.js 18+ and Angular 16+
- .NET SDK 8.0+
- Microsoft SQL Server 2019+
- Syncfusion Angular Grid and DataManager packages

---

## Architecture Overview

The solution follows a simple three‑tier pattern:

- **Database Layer**: SQL Server stores ticket data
- **Service Layer**: ASP.NET Core Web API exposes REST endpoints and processes Grid requests
- **UI Layer**: Angular application renders data using Syncfusion Grid

This separation ensures scalability and maintainability.

---

## Database Setup

SQL Server hosts the application data in a `Tickets` table. The schema includes identifiers, status, priority, assignment details, and date fields required for Grid operations such as filtering, sorting, and paging.

---

## Backend (ASP.NET Core Web API)

The Web API connects Angular and SQL Server.

### Responsibilities

- Manage SQL Server connectivity
- Accept DataManager requests from the Grid
- Apply server‑side searching, filtering, sorting, and paging
- Handle CRUD and batch updates

Configuration such as the connection string and CORS policy is managed through `appsettings.json` and `Program.cs`.

---

## Angular Grid Integration

The Syncfusion Angular Grid acts as the presentation layer.

- Uses **DataManager** with **UrlAdaptor** to communicate with the API
- Sends Grid actions (read, search, filter, sort, page, CRUD) as HTTP requests
- Expects responses in `{ result, count }` format for paging

Enabled Grid features include paging, sorting, filtering, searching, editing, and toolbar actions.

---

## CRUD Operation Flow

1. User performs an action in the Grid
2. DataManager sends the request to the Web API
3. API processes the request and interacts with SQL Server
4. API returns the result
5. Grid updates the UI

This flow is consistent for insert, update, delete, and batch operations.

---

## Running the Application

**Step 1: Build and run the ASP.NET Core Server**

1. Configure the `TicketDb` connection string in `appsettings.json`.
2. From the server project folder, run below command it terminal:

```bash
 dotnet build
 dotnet run
```

**Explanation:**
- The API exposes endpoints at a base similar to `https://localhost:7000/api/tickets` (adjust ports as necessary).
- This endpoint is configured to DataManager url.

**Step 2: Run the Angular Client**

1. From the client folder install dependencies and start the Angular dev server:

```bash
 npm install
 ng serve --open
```

**Step 3: Access the Application**

1. Open a web browser.
2. Navigate to `http://localhost:53605` (or the port shown in the terminal).
3. The Network Support Ticket System is now running and ready to use.

---

## Summary

This minimal guide outlines how to integrate **Syncfusion Angular Grid** with **Microsoft SQL Server** through an **ASP.NET Core Web API**. The approach supports server‑side data operations and provides a clean foundation for enterprise‑ready Angular applications.
