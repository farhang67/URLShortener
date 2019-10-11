using Application.Links.Dto;
using Core.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Links
{
    public interface ILinkService
    {
        UrlJsonResult ShortenLink(LinkInput link);
        UrlJsonResult ResolveLink(LinkInput link);
        IEnumerable<LinkOutput> GetAll();
        bool Delete(int id);
    }
}
