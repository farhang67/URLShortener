using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Application.Links;
using Application.Links.Dto;
using Core.Attributes;
using Core.Enums;
using Core.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using URLShortener.Models;

namespace URLShortener.Controllers
{
    public class HomeController : Controller
    {

        #region Fields
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILinkService _linkService;
        #endregion



        #region ctor
        public HomeController(IHttpContextAccessor httpContextAccessor, ILinkService linkService)
        {
            _httpContextAccessor = httpContextAccessor;
            _linkService = linkService;
        }
        #endregion



        #region public methods
        public IActionResult Index()
        {
            return View();
        }



        [HttpPost]
        [AjaxOnly]
        public IActionResult ShortenLink(string url)
        {
            var link = new LinkInput()
            {
                LongURL = url,
                ClientPlatform = ClientPlatform.Website,
                ClientIP = _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString()
            };

            var result = _linkService.ShortenLink(link);
            string baseUrl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}/r/";
            return PartialView("_ShortenLinkResult", baseUrl + result.Url);
        }



        [HttpGet]
        [Route("r/{shortUrl}")]
        public IActionResult r(string shortUrl)
        {
            var link = new LinkInput()
            {
                ShortURL = shortUrl,
                ClientIP = _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString(),
                ClientPlatform = ClientPlatform.Website
            };

            UrlJsonResult resolveResult = _linkService.ResolveLink(link);

            if (resolveResult.Result)
            {                
                return Redirect(resolveResult.Url);
            }
            else
                return RedirectToAction("UrlNotFound");
        }



        public IActionResult UrlNotFound()
        {
            return View();
        }
        #endregion


    }
}
