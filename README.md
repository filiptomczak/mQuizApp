Wb application built with ASP.NET Core for creating, editing, and solving quizzes. It features both an administrative panel and a user interface. The project follows a clean layered architecture with patterns such as Repository and Unit of Work.

📌 Features
👤 User Panel
Take quizzes with text, image, or audio questions

See points and results after completing the quiz

🔐 Admin Panel
Create and edit quizzes

Add questions and answers (including file uploads)

Update or delete existing quizzes and their contents

Remove files attached to questions

View leaderboards – top 3 participants per quiz

🧱 Architecture
The project follows a modular and layered architecture:

QuizApp – UI layer (MVC)

Models – Domain entities (Quiz, Question, Answer, etc.)

DataAccess – Repository implementations, AppDbContext, Unit of Work

Services – Business logic and service classes

🛠️ Technologies Used
ASP.NET Core MVC

Entity Framework Core (Code First)

SQL Server

Bootstrap (styling)

Repository + Unit of Work pattern

File upload with physical storage

Partial views + jQuery for dynamic form controls
