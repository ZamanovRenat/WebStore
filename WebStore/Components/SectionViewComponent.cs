using Microsoft.AspNetCore.Mvc;

namespace WebStore.Components
{
    public class SectionViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
