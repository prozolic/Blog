using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Blog.Pages;

public partial class PostComponent : ComponentBase
{
    private enum InitializingState
    {
        Loading,
        BuiltContent,
        SyntaxHighlighted
    }

    [Inject]
    public IJSRuntime? JsRuntime { get; set; }

    private InitializingState initializingState = InitializingState.Loading;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        initializingState = InitializingState.BuiltContent;
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (initializingState == InitializingState.BuiltContent)
        {
            initializingState = InitializingState.SyntaxHighlighted;
            await JsRuntime!.InvokeVoidAsync("Prism.highlightAll");
        }
    }
}