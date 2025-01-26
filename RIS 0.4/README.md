# RIS – Company Information System

This project was created as a final assignment for the certified professional retraining course "C# Programmer."  
The "RIS – Company Information System" application demonstrates a solution for managing and analyzing personnel data within a military company environment.

---

## Table of Contents

1. [Introduction](#introduction)  
2. [Project Description](#project-description)  
   - [Key Features](#key-features)  
   - [Technology Stack](#technology-stack)  
   - [Security](#security)  
   - [Evaluation](#evaluation)  
3. [User Guide](#user-guide)  
   - [Login](#login)  
   - [Main Window](#main-window)  
   - [Search Records](#search-records)  
   - [Add Records](#add-records)  
   - [Remove Records](#remove-records)  
   - [Record Editing](#record-editing)  
   - [Password Initialization](#password-initialization)  
4. [Internal Functionality](#internal-functionality)  
   - [Class `Vojak`](#class-vojak)  
   - [Class `Vojaci`](#class-vojaci)  
5. [Suggestions for Improvement](#suggestions-for-improvement)  
6. [Conclusion](#conclusion)

---

## Introduction
This document serves as a final report for the project "RIS – Company Information System." The goal of this project was to practically apply the knowledge acquired during the "C# Programmer" course, particularly in designing and developing an information system.

The "RIS" application demonstrates how a system for managing soldiers’ personal data can operate, ranging from basic features (adding a new record) to security measures such as password hashing.

---

## Project Description
"RIS – Company Information System" is built using the .NET Framework and WPF (Windows Presentation Foundation). It enables effective management of soldier records, including searching, adding, editing, and deleting entries.

### Key Features
- **Record Search:** Users can search for soldiers by service ID, first name, or last name.  
- **Adding and Editing Records:** The application lets users add new records or update existing ones, including personal data and important events in a soldier’s career.  
- **Deleting Records:** Users can delete records after confirmation, preventing accidental data loss.

### Technology Stack
- **WPF:** For building the graphical user interface.  
- **C#:** The programming language used for the backend.  
- **.NET Framework:** The core runtime for the application.  
- **Data Binding and DataGrid:** For connecting the UI with the data to enable easy manipulation and display.  
- **OOP:** Object-oriented programming for structured data management.

### Security
- **Password Hashing:** A basic security measure to protect user accounts.  
- **Sensitive Data Protection:** Passwords and other sensitive information are not displayed in the application.

### Evaluation
This project demonstrates a sample information system for personnel management, emphasizing security, accuracy, and an intuitive user interface. Its objective is to streamline personnel record management and enhance administrative processes within the military sector.

---

## User Guide

### Login
When the application starts, a login window appears. A registered user must enter their **username** and **password**. Upon successful login, the main window of the application is displayed.

### Main Window
The main window features a DataGrid showing all soldier records. This table provides a quick overview of essential information. There are three primary buttons for user interaction:

1. [Search Records](#search-records)  
2. [Add Records](#add-records)  
3. [Remove Records](#remove-records)

### Search Records
Clicking **Search Records** opens a new window allowing you to search for soldiers by service ID, first name, or last name. If multiple results are found, each appears in a separate window for editing.

### Add Records
Clicking **Add Records** opens a form for creating a new record.  
- **Service ID** is checked for uniqueness.  
- **Rank, First Name, and Last Name** are required fields.  
- **Username** is auto-generated from the last name (without diacritics) plus the first letter of the first name; an index is added in case of duplicates.  
After saving, an editing window opens where additional information can be entered.

### Remove Records
Clicking **Remove Records** deletes the currently selected entry in the table after user confirmation.

### Record Editing
Double-clicking an entry in the table opens an editing window where soldier details can be modified (e.g., date of birth, address, ID number, service period).

### Password Initialization
On the first login, if the user only provides their username and no password is set yet, the system prompts them to create a new password. This step is required to activate the account.

---

## Internal Functionality

### Class `Vojak`
Represents a soldier’s data, including:
- Personal details (service ID, first name, last name, date of birth, personal ID number, etc.)  
- Service details (start and end dates)  
- Login credentials (username, password, salt for password hashing)

The constructor `Vojak(string serviceId, string rank, string firstName, string lastName)` initializes a new object with the necessary attributes.

### Class `Vojaci`
Manages an `ObservableCollection<Vojak>`:
- **Add(...)** – Adds a new soldier to the collection.  
- **FindByServiceId / FindByFirstName / FindByLastName** – Searches soldiers by various criteria.  
- **WriteToFile / ReadFromFile** – Serializes and deserializes data to/from JSON.  
- **HashPassword / VerifyPassword** – Password hashing and verification.  

The `ZmenaSeznamu` event notifies the UI of any changes in the soldier collection.

---

## Suggestions for Improvement
1. **Database System**  
   Transitioning from JSON to a robust database (e.g., SQL Server, PostgreSQL, MongoDB) would provide better scalability and security.

2. **Enhanced Security**  
   Encrypt data, use SSL/TLS for communication, and implement more advanced authentication mechanisms.

3. **Additional Functionality**  
   - Auto-reminders for end-of-service dates  
   - Automated tasks (data backups, data integrity checks)

4. **User Access Levels**  
   Implement role-based access (admin, manager, user) to control who can view and modify certain data.

5. **UI Improvements**  
   Modernize the interface, add customizable dashboards, interactive charts, and a responsive design.

6. **Mobile Device Support**  
   Develop a mobile or responsive web version to increase flexibility and accessibility.

---

## Conclusion
The "RIS – Company Information System" project served as the final assignment for the "C# Programmer" course, validating the ability to design and implement an information system—from basic features to key security measures (e.g., password hashing, protecting sensitive data).

This demo version proves the feasibility of creating a functional application suitable for a military environment using acquired skills. It also highlights areas for potential future development, such as migrating to a more robust database, adding user roles, and improving scalability and security. Overall, this project provided valuable hands-on experience and serves as a foundation for further professional growth in software engineering.

---

> **Note:** This project is a demo solution and can be further enhanced or expanded according to specific requirements and needs.
