# GreenLife Organic Store Management System

## Project Overview

GreenLife Organic Store Management System is a desktop-based application developed using C# (.NET Windows Forms) and SQL Server. The system is designed to manage the operations of an organic retail store by allowing administrators to manage products, customers, and orders, while customers can browse products, place orders, and track their order status.

The application provides a simple and efficient way to handle product inventory, customer data, order processing, and report generation in a single integrated system.

# Technologies Used

- **Programming Language:** C#
- **Framework:** .NET Windows Forms
- **IDE:** Microsoft Visual Studio
- **Database:** Microsoft SQL Server
- **Data Access:** ADO.NET
- **Version Control:** Git & GitHub

# System Features

## Admin Features

Admin login, manage product details (add, update, delete), manage customer information, manage orders and update order status, generate sales and stock reports, export reports to CSV, view dashboard statistics, and receive low stock notifications.


## Customer Features

Customer registration and login, search products by name or category, add products to cart, place orders, track order status, view order history, and manage personal profile details.

# System Architecture

### Presentation Layer

Contains all Windows Forms that interact with the user interface.

### Business Logic Layer

Contains classes that represent the core entities of the system.

### Data Access Layer

Responsible for communicating with the SQL Server database.

# Object-Oriented Programming Concepts Used

### Classes

Classes represent the entities of the system.

### Properties

Properties store the data of each object.

### Methods

Methods perform operations related to each class.

### Constructors

Constructors initialize objects when forms or classes are created.

### Encapsulation

Data and related operations are grouped inside the same class to maintain modular and organized code.

# Search Functionality

The system implements **search and filtering algorithms** to allow customers to find products efficiently.

# How to Run the Project

Follow these steps to run the system:

1. Clone the repository from GitHub

2. Open the project in **Visual Studio**

3. Restore NuGet packages if required

4. Create the database in **SQL Server**

5. Run the SQL scripts to create tables:

6. Update the **connection string** in the DbConnection class.

7. Build and run the project.
