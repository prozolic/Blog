using Microsoft.AspNetCore.Components;

namespace Blog.Layout;

public partial class MainLayout
{
    [Inject]
    public IConfiguration? Configuration { get; set; }

    [Inject]
    public NavigationManager? NavigationManager { get; set; }

    public string? Name { get; private set; }

    public string? Description { get; private set; }

    public string? GithubUrl { get; private set; }

    public string? BlogSourceUrl { get; private set; }

    public Post? PreviousPost { get; private set; }

    public Post? NextPost { get; private set; }

    protected override Task OnInitializedAsync()
    {
        OnInitialized();

        var section = Configuration?.GetSection("Profile");
        Name = section?["Name"];
        Description = section?["Description"];
        GithubUrl = section?["GithubUrl"];
        BlogSourceUrl = section?["BlogSourceUrl"];

        FindPreviousAndNextPosts();

        return Task.CompletedTask;
    }

    protected override Task OnParametersSetAsync()
    {
        FindPreviousAndNextPosts();
        return base.OnParametersSetAsync();
    }

    private void FindPreviousAndNextPosts()
    {
        var currentUrl = NavigationManager?.ToBaseRelativePath(NavigationManager.Uri);
        if (!string.IsNullOrEmpty(currentUrl) && !currentUrl.Equals("/"))
        {
            currentUrl = "/" + currentUrl;
            FindPreviousAndNextPostsCore(currentUrl);
        }
    }

    private void FindPreviousAndNextPostsCore(string currentUrl)
    {
        var currentPostIndex = 0;
        foreach(var post in PostProvider.AllPosts.AsSpan())
        {
            if (post.Url == currentUrl)
            {
                break;
            }
            currentPostIndex++;
        }

        NextPost = currentPostIndex > 0 ? PostProvider.AllPosts[currentPostIndex - 1] : null;
        PreviousPost = currentPostIndex < PostProvider.AllPosts.Length - 1 ? PostProvider.AllPosts[currentPostIndex + 1] : null;
    }

}