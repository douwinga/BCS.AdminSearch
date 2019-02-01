using OrchardCore.Modules.Manifest;

[assembly: Module(
    Name = "BCS Admin Search",
    Author = "Bethany Dev Team",
    Website = "https://www.bethany.org",
    Version = "1.0.0",
    Description = "Adds search functionality to the admin",
    Category = "Content",
    Dependencies = new[] { "OrchardCore.Lucene" }
)]
