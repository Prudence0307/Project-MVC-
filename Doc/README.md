# RaceDay MVC (ASP.NET Core MVC + SQL Server)

A starter web application built from the supplied RaceDay API endpoint plan and SQL schema.

## Included
- ASP.NET Core MVC / Razor views
- SQL Server through Entity Framework Core
- Cookie authentication and role checks for Participant and Organiser
- Registration with password hashing and login/logout
- Public event listing, filtering and details
- Organiser event create/edit/delete with ownership checks
- Category creation and details / route information
- Participant enrolment, capacity checks, duplicate prevention and cancellation
- Public category leaderboard
- Responsive RaceDay landing page and UI styling

## Requirements
- Visual Studio 2022 with the ASP.NET and web development workload
- .NET 8 SDK
- SQL Server LocalDB (usually installed with Visual Studio) or SQL Server

## Run it
1. Extract the ZIP and open `RaceDayMVC.csproj` in Visual Studio.
2. Check `appsettings.json` and update `ConnectionStrings:RaceDayDB` if your SQL Server instance differs.
3. Create the database using one of these approaches:
   - **Recommended:** open Package Manager Console and run `Add-Migration InitialCreate`, then `Update-Database`.
   - Or run your supplied `RaceDay_Schema.sql` in SSMS to create tables. If using the SQL script, remove the `Drop table` section or run it only against a disposable development database. Do not run EF migrations against an already-created schema without aligning migration history.
4. Press F5 / Ctrl+F5 to run.
5. Register a Participant or Organiser account from the site.

## Important database note
The supplied SQL script seeds sample users with placeholder password hashes (`HASHED_PWD_1`, etc.). Those are not valid passwords for this app's password verifier. For normal login testing, register accounts through the app. The SQL seed script also drops tables before recreating them, so it can erase data.

## Current scope and next steps
The attached API plan describes REST endpoints. This starter implements the core browser MVC workflows, not the complete `/api/...` REST surface yet. Weather forecast integration, route create/edit, organiser enrolment management, result capture/edit, full profile/history screens and a REST API layer remain to be implemented. Add a weather provider API key before enabling live forecasts.

For production: add email verification/password reset, stronger rate limiting, audit logging, secure secrets management, automated tests, and review cookie/CSRF/security settings.
