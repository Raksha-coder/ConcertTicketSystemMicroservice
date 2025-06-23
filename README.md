# 🎟️ Event Management System – Microservices Architecture

A full-featured **Event Management System** built using **.NET Web API** following a modular **Microservices Architecture**. This system manages events, tickets, and venue capacity with service isolation and clean architecture principles.

---

## 🧩 Modules

### 1. 🎤 Event Management Service
- Create, update, and manage event listings.
- Define event name, venue, date, and maximum capacity.
- Exposes RESTful APIs for other services.

### 2. 🎫 Ticket Reservation & Sales Service
- Reserve and confirm tickets for available events.
- Validates availability and manages booking lifecycle.
- Simulated payment step included.

### 3. 🏟️ Venue Capacity Service
- Monitors and restricts event booking beyond allowed capacity.
- Tracks available vs booked seats in real time.

### 4. 🌐 API Gateway (YARP)
- Centralized gateway using **YARP**.
- Routes requests to appropriate services.
- Handles JWT token validation for secure communication.

---

## 🔒 Authentication & Security
- **JWT Token Authentication** is implemented across services.
- Token generation and validation handled via shared Auth Service.
- Token-based service-to-service communication ensures secure APIs.

---

## 🛠️ Tech Stack

| Layer              | Technology                       |
|-------------------|----------------------------------|
| Backend Framework | .NET Core Web API                |
| Gateway           | YARP (.NET Reverse Proxy)        |
| Authentication    | JWT Token-Based Auth             |
| Database          | SQL Server (per service basis)   |
| Architecture      | Microservices + Clean Architecture |

---

## 📁 Folder Structure
/src
/EventService
/TicketService
/VenueService
/ApiGateway
/Shared.Authentication

---

# Rate Limiting :
Implemented Rate Limiting and send an error response through response
![image](https://github.com/user-attachments/assets/1f0bf291-94e0-4ea4-9426-c5540cfaa123)


# Implemented Global Exception Handling:
![image](https://github.com/user-attachments/assets/69f34067-1f6a-4490-9d53-e0702dd1d9b3)

# Tested Global Exception:
![image](https://github.com/user-attachments/assets/8c46169a-37f7-4756-aa36-dc5d80926bcb)

