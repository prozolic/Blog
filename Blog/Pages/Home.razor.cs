using Microsoft.AspNetCore.Components;

namespace Blog.Pages;

public partial class Home
{
    [Inject]
    public IConfiguration? Configuration { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        var section = Configuration?.GetSection("Profile");
        var name = section?["Name"];
        var description = section?["Description"];
    }

}