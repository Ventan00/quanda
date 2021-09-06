# Light and dark mode
...
# Endpoints authorization
...
# Right menu
To add right menu to your page, You need to add ```[CascadingParameter]``` of type ```EventCallback<RightMenuType>```.
Then invoke callback in ```protected async override Task OnInitializedAsync()```
passing the Enum ```RightMenuType``` as an argument.

### Create new shared RightMenuType
1. Add enum to `Quanda.Client\Shared\RightMenu\RightMenuType.cs`.
2. Add new razor component to `Quanda.Client\Shared\RightMenu`. You must also create css file (it can be empty).
3. Add your component to `Quanda.Client\Shared\RightMenu\RightShared.razor` to switch using your enum.
4. Add your component css file to `Quanda.Client.csproj` in _<ItemGroup>_ section under _<!-- Right menu components-->_

### Existing RightMenuTypes
- STANDARD (used in most pages)
- QUESTION (used when adding question)
- NONE (used when right menu should be hidden) 
 