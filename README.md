/src
 ├── Domain
 │   ├── Entities
 │   │   ├── ApplicationUser.cs
 │   │   ├── ApplicationRole.cs
 │   ├── Validators
 │   │   ├── ApplicationUserValidator.cs
 │   │   ├── ApplicationRoleValidator.cs
 ├── Application (Use Cases)
 ├── Infrastructure (Persistence, EF Core, etc.)
 ├── Presentation (Controllers, APIs)

dotnet ef migrations add FixSchemas --project ./Infrastructure --startup-project ./Presentation --context ApplicationDbContext

dotnet ef database update --project ./Infrastructure --startup-project ./Presentation --context ApplicationDbContext