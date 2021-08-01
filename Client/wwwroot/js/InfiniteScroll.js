/// <summary>
/// Klasa odpowiedzialna za obsługiwanie przewijania strony wraz z wyłowaniem akcji w momencie zjechania na pewną wysokość.
/// </summary>
class InfiniteScroll {
    constructor() {
        this.Init = function (elementId, dotNetHelper) {
            this.element = document.getElementById(elementId);
            this.DotNetHelper = dotNetHelper;
        }

        this.ListenToScroll = function (event, element, DotNetHelper) {
            var bounding = element.getBoundingClientRect();
            if (
                bounding.top >= 0 &&
                bounding.left >= 0 &&
                bounding.right <= (window.innerWidth || element.clientWidth) &&
                bounding.bottom <= (window.innerHeight || element.clientHeight)
            ) {
                DotNetHelper.invokeMethodAsync("LoadMore");
            }
        };
        this.handler = (ev) => { this.ListenToScroll(ev, this.element, this.DotNetHelper) };

        document.addEventListener("scroll", this.handler, true);

        this.RemoveListener = function () {
            document.removeEventListener("scroll", this.handler, true);
        }
    }
}

window.ScrollList = new InfiniteScroll();