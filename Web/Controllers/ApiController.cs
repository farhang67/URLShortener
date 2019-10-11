using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Links;
using Application.Links.Dto;
using Core.Enums;
using Core.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class ApiController : Controller
    {

        #region fields
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILinkService _linkService;
        #endregion



        #region ctor
        public ApiController(IHttpContextAccessor httpContextAccessor, ILinkService linkService)
        {
            _httpContextAccessor = httpContextAccessor;
            _linkService = linkService;
        }
        #endregion



        #region public methods
        [HttpPost]
        public UrlJsonResult ShortenLink(string url)
        {
            var link = new LinkInput()
            {
                LongURL = url,
                ClientPlatform = ClientPlatform.Webservice,
                ClientIP = _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString()
            };

            return _linkService.ShortenLink(link);
        }
        #endregion



    }
}