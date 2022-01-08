using MemesProject.Data;
using MemesProject.Models;
using MemesProject.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MemesProject.Controllers
{
    public class CommentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CommentsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        // GET: CommentsController
        public ActionResult Index()
        {
            return View();
        }

        // GET: CommentsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CommentsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CommentsController/Create
        [Authorize]
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateHub(CommentViewModel commentViewModel)
        {
            var user = await _userManager.GetUserAsync(User);

            commentViewModel.IdUser = user.RealUserName;
            commentViewModel.Date = DateTime.Now;
            commentViewModel.IfBlocked = false;
            commentViewModel.Likes = 0;
            commentViewModel.Dislikes = 0;

            var commentHub = new CommentsHub
            {
                IdMeme = commentViewModel.IdMeme
            };

            var comments = new Comment
            {
                Text = commentViewModel.Text,
                Date = commentViewModel.Date,
                IfBlocked = commentViewModel.IfBlocked,
                Likes = commentViewModel.Likes,
                Dislikes = commentViewModel.Dislikes,
                IdUser = commentViewModel.IdUser,
            };
            //var errors1 = ModelState.Values.SelectMany(v => v.Errors);
            if (ModelState.IsValid)
            {
                _context.CommentsHubs.Add(commentHub);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {

                }
                comments.IdCommentsHub = commentHub.IdCommentHub;
                _context.Comments.Add(comments);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {

                }
            }
            return RedirectToAction("Details", "Memes", new { Id = commentViewModel.IdMeme });
        }
        [HttpGet]
        public async Task<IActionResult> GetCommentHub(long? Id)
        {
            var commentHub = new List<CommentsHub>();
            commentHub = await _context.CommentsHubs.Where(x => x.IdMeme == Id).Include(y=>y.Comments).ToListAsync();
            return PartialView(@"~/Views/Shared/_CommentsHubListPartial.cshtml", commentHub);
        }
        [Authorize]
        [HttpPost]
        public async Task<ActionResult> CreateComment(Comment comment)
        {
            //if (ModelState.IsValid)
            //{
                var user = await _userManager.GetUserAsync(User);

                comment.IdUser = user.RealUserName;
                //comment.IdCommentsHub  = Id.Value;
                comment.Date = DateTime.Now;
                ModelState.ClearValidationState(nameof(Comment));
                if (TryValidateModel(comment, nameof(Comment)))
                {
                    _context.Add(comment);
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction("Details", "Memes", new { Id = comment.IdMeme });
            //}
            //else
            //{
            //    return Json("Błąd, brak wprowadzonych danych");
            //}
        }
        // GET: CommentsController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CommentsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CommentsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CommentsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
