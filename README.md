# Survey Basket ASP API (.NET9)


<img width="1435" alt="untitled-Survey Basket App Mind Map" src="https://github.com/user-attachments/assets/9d177f04-7727-4942-9c5f-8c35ab715102" />

## 📋 Overview

The Survey Basket API is a comprehensive and user-friendly platform for managing surveys, polls, and user responses. It facilitates creating, retrieving, and analyzing polls and questions while ensuring scalability—ideal for developers looking to integrate survey capabilities into their applications.
## 🛠 Tech Stack

- **Backend Framework:** ASP.NET Core 9.0
- **ORM:** Entity Framework Core
- **Database:** SQL Server
-  **LINQ:** Used for querying and manipulating data efficiently.
- ✅ **SOLID Principles:** some of SOLID Principles  such as the single responsibility principle , Dependency Inversion Principle
-
### Design Patterns & Architecture Pattern:

- ✅ **Service Layer Pattern**  
  The service layer encapsulates the core business logic, invoked by controllers (API) for performing operations. This separation improves maintainability, reusability, and testability, while allowing controllers to focus solely on handling HTTP requests and responses.

- ✅ **Dependency Injection (DI)**  
  DI is leveraged to manage system dependencies, promoting loose coupling between components, improving maintainability, and allowing easier extensibility of the system.
- **Authentication & Authorization:** ASP.NET Core Identity
- **Logging:** Serilog (used for logging errors and exceptions)

- **SOLID Principles:** Incorporates principles such as Single Responsibility and Dependency Inversion
- **Architectural Principles:** Dependency Injection (DI)
## ✨ Features

- **🗳️ Poll Management:** Create, update, and delete polls.
- **❓ Question Management:** Add, edit, and organize questions in polls.
- **📝 User Responses:** Record and retrieve user votes and responses.
- **📊 Results Analysis:** Aggregate and display poll results.
- **🔔 Real-Time Notifications:** Sends notifications when new polls are added.

## 🚀 Key Highlights

- **🔒 User & Role Management:**  
  Utilizes JWT for robust authentication and authorization, ensuring smooth and secure access control.

- **📈 Polls & Surveys:**  
  Empowers users to effortlessly create, manage, and participate in polls, streamlining data collection and engagement.

- **📝 Audit Logging:**  
  Implemented audit logging using Serilog to track changes on resources, ensuring transparency and accountability in user actions.


- **⚠️  Exception Handling:**  
  Provides unified error management to handle exceptions gracefully, significantly enhancing the user experience.

- **🛠️ Structured Error Handling:**  
  Adopts the result pattern to deliver clear, actionable feedback for error management.

- **🔄 Mapping: Mapster :**  
  Employs efficient object mapping between models to improve data handling and reduce boilerplate code.

- **✅ Fluent Validation:**  
  Ensures data integrity by rigorously validating inputs, resulting in user-friendly error messages.

- **🔑 Account Management:**  
  Offers robust features for user account control, including functionalities for password changes and resets.

- **⏱️ Rate Limiting:**  
  Manages request rates to prevent misuse, ensuring equitable resource access for all users.

- **🔧 Background Jobs:**  
  Leverages Hangfire to manage background tasks such as sending confirmation emails and processing password resets seamlessly.

- **💓 Health Checks:**  
  Integrates system health checks to monitor performance and maintain high uptime and reliability.

- **🗃️ Caching:**  
- **Hybrid Caching (.NET9):** (.NET9):Optimized performance with caching for frequently accessed data, significantly improving response times.

- **🚦CORS:**   
- **(Cross-Origin Resource Sharing):** a security feature implemented by web browsers to prevent web pages from making requests to a different domain than the one that served the web page. 

- **✉️ Email Confirmation:**  
 Managed user email confirmations, password changes, and resets seamlessly to enhance security.

- **📌 API Versioning:**  
  Supports multiple API versions to maintain backward compatibility and ease the transition as the project evolves.

- **🗳️ Data Seeding:**  
Automatically seeds essential data, including admin roles and users, to ensure the system starts with pre-configured data, simplifying the setup and initial use.

## 🔧 Getting Started

To set up the project locally:

 **Clone the Repository:**
   ```bash
   git clone  https://github.com/MohammedAwadEleash/ASP.NETWebAPISurveyBasketApp  .

 ```
## 📖 API Documentation (Swagger UI):

![Swagger UI - Google Chrome 3_8_2025 6_45_46 PM](https://github.com/user-attachments/assets/81d3505e-41f1-43b0-8351-c1fcc91f7a35)
![Swagger UI - Google Chrome 3_8_2025 6_46_03 PM](https://github.com/user-attachments/assets/30418e3b-813a-41bc-87c6-3d2de739dcce)
![Swagger UI - Google Chrome 3_8_2025 6_46_10 PM](https://github.com/user-attachments/assets/7557dab7-e76d-46c3-a001-861e9b6d54b9)
![Swagger UI - Google Chrome 3_8_2025 6_46_20 PM](https://github.com/user-attachments/assets/76b74d22-ae3d-4ca0-950b-1ccafe543098)
![Swagger UI - Google Chrome 3_8_2025 6_46_26 PM](https://github.com/user-attachments/assets/3d165f38-a08b-4d2b-98a1-fd9f17942dd6)
![Swagger UI - Google Chrome 3_8_2025 6_46_34 PM](https://github.com/user-attachments/assets/88fbed00-ca64-40b1-adba-e447ddc59b40)
![Swagger UI - Google Chrome 3_8_2025 7_29_09 PM](https://github.com/user-attachments/assets/feb56861-abb6-4966-8cab-fc0cdd30ceec)
![Swagger UI - Google Chrome 3_8_2025 7_20_36 PM](https://github.com/user-attachments/assets/36030588-9489-478e-b151-16b32b643b2d)
![Swagger UI - Google Chrome 3_8_2025 7_08_04 PM](https://github.com/user-attachments/assets/9d273c52-58ca-4c47-a483-b5a316b73cec)


