using MemesProject.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace MemesProject.ViewComponents
{
    public class CategoryListViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _db;

        public CategoryListViewComponent(ApplicationDbContext db)
        {
            _db = db;
        }


        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categoryList = new List<SelectListItem>();
            var category =  await _db.Categories.ToListAsync();

            foreach(var item in category)
            {
                categoryList.Add(new SelectListItem(item.CategoryName, item.CategoryName));
            }
            
            ViewBag.categories = categoryList;
            return View();


        }
    }
}
