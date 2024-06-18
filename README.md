Archive and Management System

A comprehensive web application for managing user access, files, and activities within organizations. The system includes robust user management, access control, file handling functionalities, detailed activity logging, and reporting features.
Table of Contents

    Introduction
    Features
    Technologies Used
    Architecture and Design
    Screenshots
    Activity Logging
    Reports
    

Introduction

The Archive and Management System is designed to facilitate efficient management of files and folders within organizations. It provides robust user authentication and authorization mechanisms, ensuring secure access control based on user roles, claims, and groups. The system also logs user activities for better traceability and includes powerful reporting capabilities with Stimulsoft.
Features

    User management with roles and permissions
    Secure file upload and download functionalities
    Access control using claims and groups
    Activity logging (file removal, file editing, user and date tracking)
    Comprehensive reporting with Stimulsoft

Technologies Used

    .NET Framework
    C#
    Kendo UI for ASP.NET MVC
    CSS
    AJAX
    JavaScript
    jQuery
    Stimulsoft for reporting

Architecture and Design

The project follows an N-layer architecture, separating concerns into presentation, business logic, and data access layers. Design patterns implemented include:

    Factory Pattern
    Singleton Pattern
    Dependency Injection
    Repository Pattern
    Unit of Work Pattern

Screenshots

![Archive1](https://github.com/YaldaAHM/Archive_.Net_MVC/assets/169922419/32bada08-d04d-47c6-a7b9-b350aa65405c)


Activity Logging

The system logs all user activities related to file management, including:

    File removal
    File editing
    User performing the action
    Date and time of the action

This information is stored in the database and can be accessed through the admin panel.
Reports

The system integrates Stimulsoft for generating various reports, including:

    User activity reports
    File management reports
    Access control reports

These reports provide detailed insights into system usage and user activities.
