using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

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

    public Post? CurrentPost { get; private set; }

    public Post? PreviousPost { get; private set; }

    public Post? NextPost { get; private set; }

    public string SearchKeyword { get; set; } = string.Empty;

    protected override Task OnInitializedAsync()
    {
        var section = Configuration?.GetSection("Profile");
        Name = section?["Name"];
        Description = section?["Description"];
        GithubUrl = section?["GithubUrl"];
        BlogSourceUrl = section?["BlogSourceUrl"];

        FindPreviousAndNextPosts();

        return base.OnInitializedAsync();
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
            FindPreviousAndNextPostsCore(currentUrl);
        }
        else
        {
            CurrentPost = null;
            NextPost = null;
            PreviousPost = null;
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

        var result = currentPostIndex < PostProvider.AllPosts.Length;

        CurrentPost = result && PostProvider.AllPosts.Length > 0 ? PostProvider.AllPosts[currentPostIndex] : null;
        NextPost = result && currentPostIndex > 0 ? PostProvider.AllPosts[currentPostIndex - 1] : null;
        PreviousPost = result && currentPostIndex < PostProvider.AllPosts.Length - 1 ? PostProvider.AllPosts[currentPostIndex + 1] : null;
    }

    private void OnSearchKeyDown(KeyboardEventArgs e)
    {
        if (e.Key == "Enter")
        {
            OnSearch();
        }
    }

    private void OnSearch()
    {
        if (!string.IsNullOrWhiteSpace(SearchKeyword))
        {
            var parameter = new Dictionary<string, object?>
            {
                { "q", Uri.EscapeDataString(SearchKeyword) }
            };

            var url = NavigationManager?.GetUriWithQueryParameters("search", parameter.AsReadOnly()) ?? "/";
            NavigationManager?.NavigateTo(url!);
        }
    }

}