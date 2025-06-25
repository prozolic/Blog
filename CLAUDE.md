# Blazor WebAssembly Markdown Blog

A blog website built with Blazor WebAssembly and custom Markdown source generators

## Project Overview
- **Framework**: Blazor WebAssembly (.NET 9.0)
- **Markdown Processing**: Custom source generator (Blog.Generator)
- **Markdown Library**: MD2RazorGenerator (v1.2.2) + Custom generator
- **Deployment**: GitHub Pages (PublishSPAforGitHubPages.Build v3.0.0)

## Development Commands

### Build
```bash
dotnet build
```

### Run Development Server
```bash
dotnet run
```

### GitHub Pages Build
```bash
dotnet publish -c Release
```

### Test
```bash
dotnet test
```

## Project Structure
```
Blog/
├── Blog/                          # Main Blazor WebAssembly project
│   ├── Pages/
│   │   ├── Home.razor             # Blog homepage with post listings
│   │   ├── Home.razor.cs          # Homepage code-behind
│   │   ├── Profile.md             # Markdown profile page
│   │   ├── 2025/                  # Year-based organization
│   │   │   ├── 20250624.md
│   │   │   ├── 20250625.md
│   │   │   └── 20250626.md
│   │   └── 2026/
│   │       └── 20260624.md
│   ├── Layout/
│   │   └── MainLayout.razor       # Main layout template
│   ├── wwwroot/                   # Static web assets
│   │   ├── css/app.css
│   │   ├── index.html
│   │   └── manifest.webmanifest
│   ├── Program.cs                 # Application entry point
│   ├── App.razor                  # Root component
│   ├── _Imports.razor             # Global using statements
│   └── Blog.csproj                # Main project file
├── Blog.Generator/                # Custom source generator
│   ├── Generator.cs               # Main source generator logic
│   ├── Internal/Polyfill/
│   │   └── IsExternalInit.cs      # .NET compatibility
│   └── Blog.Generator.csproj      # Generator project file
└── Blog.slnx                      # Solution file
```

## Markdown File Format
All markdown files should include YAML frontmatter:

```markdown
---
title: Page Title
url: /page-url
$namespace: Blog.Pages
---

# Content

Your markdown content here...
```

### Required Frontmatter Properties
- `title`: Page title displayed in navigation and metadata
- `url`: URL path for the page (e.g., `/20250624`)
- `$namespace`: Target namespace for generated Razor components (`Blog.Pages`)

## Source Generator Features

### Custom Blog.Generator
- **Technology**: C# Source Generator (.NET Standard 2.0)
- **Dependencies**: 
  - Markdig (v0.41.3) for Markdown processing
  - VYaml (v1.2.0) for YAML frontmatter parsing
  - Microsoft.CodeAnalysis.CSharp (v4.3.0) for code generation
- **Functionality**: 
  - Automatically converts Markdown files to Razor components
  - Generates `PostProvider` class with categorized post collections
  - Organizes posts by year (`PostProvider._2025`, `PostProvider._2026`)
  - Creates `AllPost` collection for homepage listing

### MD2RazorGenerator Integration
- Provides additional Markdown-to-Razor conversion capabilities
- Works alongside the custom source generator
- Handles complex Markdown syntax and extensions

## Key Components

### PostProvider (Generated)
Auto-generated class that provides access to blog posts:
```csharp
// Usage in components
@foreach(var posts in PostProvider.AllPost)
{
    foreach(var post in posts)
    {
        <article>
            <h2><a href="@post.Date">@post.Title</a></h2>
            <time>@post.Date</time>
        </article>
    }
}
```

### Home Page (Blog/Pages/Home.razor)
- Displays all blog posts in chronological order
- Uses PostProvider to access generated post data
- Includes static profile link

## Dependencies

### Main Project (Blog.csproj)
- `Microsoft.AspNetCore.Components.WebAssembly` (v9.0.6)
- `MD2RazorGenerator` (v1.2.2)
- `PublishSPAforGitHubPages.Build` (v3.0.0)
- Project reference to Blog.Generator (as Analyzer)

### Source Generator (Blog.Generator.csproj)
- `Microsoft.CodeAnalysis.CSharp` (v4.3.0)
- `Markdig` (v0.41.3)
- `VYaml` + `VYaml.Annotations` (v1.2.0)
- Configured as Roslyn analyzer component

## Deployment
- Configured for GitHub Pages deployment
- Uses `PublishSPAforGitHubPages.Build` for static site generation
- Supports service worker for offline capabilities
- PWA-ready with manifest and icons

## Getting Started
1. Clone the repository
2. Run `dotnet restore` to restore packages
3. Run `dotnet build` to build the project and generate components
4. Run `dotnet run` to start the development server
5. Add new blog posts as `.md` files in the `Pages/YYYY/` directories

## Troubleshooting

### Source Generator Issues
If the source generator fails to load dependencies:
1. Clear NuGet caches: `dotnet nuget locals all --clear`
2. Clean and rebuild: `dotnet clean && dotnet build`
3. Check that VYaml assemblies are properly included in the generator package