# chinese-auction

A raffle-based online store system designed to manage gifts, donors, purchases, and raffles in one unified platform.  
Built with **Angular (frontend)**, **C#/.NET Core (backend)**, and **SQL Server (database)**.

---

## Overview

This project supports two main user roles:

### Users
- Register and log in with validation.
- Browse and filter gifts.
- Add gifts to cart (draft mode) and finalize purchases.
- View raffle results after the draw.

### Admins (Managers)
- Secure login with predefined credentials.
- Full management of donors, gifts, and purchases.
- Protected controllers and actions using role-based authorization and JWT.
- Generate raffle results and revenue reports.

---

## Modules

### Gift Management
- List, add, update, and delete gifts.
- Track gift popularity and availability.

### Donor Management
- Manage donor information.
- Link gifts to donors for tracking.

### Purchase Management
- Track buyers and their purchases.
- Track gift popularity and purchasing trends.

### Raffle Module
- Conduct raffles for gifts and announce winners.
- Automatically generate raffle results.

---

## Architecture & Technologies

- **Frontend**: Angular  
- **Backend**: .NET Core (C#)  
- **Database**: SQL Server  
- **ORM**: Entity Framework Core (Code First)  
- **Authentication**: JWT & Role-based authorization  
- **Dependency Injection**: Modular and maintainable code  
- **Error Handling**: Centralized logging  

---
