using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Links;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class HomeController : Controller
    {

        #region fieds
        private readonly ILinkService _linkService;
        #endregion



        #region ctor
        public HomeController(ILinkService linkService)
        {
            _linkService = linkService;
        }
        #endregion



        #region public methods
        public IActionResult Index()
        {
            return View();
        }



        public IActionResult LinksStats()
        {
            var model = _linkService.GetAll();
            return View(model);

        }



        public IActionResult EditLinks()
        {
            var model = _linkService.GetAll();
            return View(model);
        }



        public IActionResult DeleteLink(int id)
        {
            _linkService.Delete(id);
            return RedirectToAction("EditLinks");
        }
        #endregion

    }
}