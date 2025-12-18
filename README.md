# StudentManagementSystem (WinForms)

This is a simple Student Management System UI (Add / Update / Delete / Show / Search) **WITHOUT database**.
It uses an in-memory repository now, and your friend can replace it later with SQL (ADO.NET) by implementing `IStudentRepository`.

## Requirements
- Visual Studio 2022 (recommended) with .NET Desktop Development
OR
- .NET SDK 8 + `dotnet` CLI (on Windows)

## Run
1) Open `StudentManagementSystem.sln` in Visual Studio
2) Run (F5)

## Notes for database integration
- Implement `IStudentRepository` in a new class (e.g., `SqlStudentRepository`)
- Replace `new InMemoryStudentRepository(...)` in `Program.cs` with your SQL repository
- UI code won't change.

## Assets
Images are in `Assets/` and are copied to the build output automatically.
