using Microsoft.AspNetCore.Components;
using System.Collections.Immutable;

namespace Blog.Pages;

public partial class Home
{
    private DisplayMode currentDisplayMode = DisplayMode.Default;

    [Inject]
    public NavigationManager? NavigationManager { get; set; }

    [Parameter]
    public string? CategoryName { get; set; }

    [SupplyParameterFromQuery(Name = "q")]
    [Parameter]
    public string? RawQuery { get; set; }

    public string? UnescapeQuery { get; set; }

    public ImmutableArray<Post> Posts { get; private set; }

    public string SearchResultText
    {
        get
        {
            return currentDisplayMode switch
            {
                DisplayMode.Category => $"「{CategoryName}」に関する記事一覧（{Posts.Length}件）",
                DisplayMode.Search => $"「{UnescapeQuery}」の検索結果（{Posts.Length}件）",
                _ => $"記事一覧（{Posts.Length}件）",
            };
        }
    }

    protected override Task OnParametersSetAsync()
    {
        // clear previous state
        UnescapeQuery = null;

        if (!string.IsNullOrEmpty(CategoryName))
        {
            Posts = PostProvider.AllPosts.Where(p => p.Categories.Contains(CategoryName!)).ToImmutableArray();
            currentDisplayMode = DisplayMode.Category;
        }
        else if (!string.IsNullOrEmpty(RawQuery))
        {
            UnescapeQuery = Uri.UnescapeDataString(RawQuery);
            Posts = PostProvider.AllPosts.Where(p => p.Title!.Contains(UnescapeQuery) || p.HeadText!.Contains(UnescapeQuery)).ToImmutableArray();
            currentDisplayMode = DisplayMode.Search;
        }
        else
        {
            Posts = PostProvider.AllPosts;
            currentDisplayMode = DisplayMode.Default;
        }
        return base.OnParametersSetAsync();
    }


    private enum DisplayMode
    {
        Default = 0,
        Category = 1,
        Search = 2,
    }
}