# Light and dark mode
...
# Endpoints authorization
...
# Right menu
The Right menu is hidden by default.
To add right menu to your page, You need to inject ```RightMenuStateService```.
Then set RightMenuType of injected service to desired menu in ```protected async override Task OnParametersSetAsync()```.

### Create new shared RightMenuType
1. Add enum to `Quanda.Client\Shared\RightMenu\RightMenuType.cs`.
2. Add new razor component to `Quanda.Client\Shared\RightMenu`. You must also create css file (it can be empty).
3. Add your component to `Quanda.Client\Shared\RightMenu\RightShared.razor` to switch using your enum.
4. Add your component css file to `Quanda.Client.csproj` in _<ItemGroup>_ section under `<!-- Right menu components-->`

### Existing RightMenuTypes
- STANDARD (used in most pages)
- QUESTION (used when adding question)
- NONE (used when right menu should be hidden) 
 
