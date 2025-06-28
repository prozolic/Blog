using Microsoft.AspNetCore.Components;

namespace Blog.Layout;

public partial class MainLayout
{
    [Inject]
    public IConfiguration? Configuration { get; set; }

    public string? Name { get; private set; }

    public string? Description { get; private set; }

    public string? GithubUrl { get; private set; }

    public string? BlogSourceUrl { get; private set; }

    protected override Task OnInitializedAsync()
    {
        OnInitialized();

        var section = Configuration?.GetSection("Profile");
        Name = section?["Name"];
        Description = section?["Description"];
        GithubUrl = section?["GithubUrl"];
        BlogSourceUrl = section?["BlogSourceUrl"];

        return Task.CompletedTask;
    }

    public IEnumerable<(string Title, string Date, string Url)> GetLatestPosts(int count)
    {
        var recentPosts = new List<(string Title, string Date, string Url)>();
        foreach (var posts in PostProvider.AllPost)
        {
            foreach (var post in posts)
            {
                recentPosts.Add((post.Title ?? "", post.Date ?? "", post.Url ?? ""));
            }
        }
        return recentPosts.OrderByDescending(p => p.Date).Take(3);
    }

}