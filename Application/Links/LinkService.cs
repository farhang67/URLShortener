using Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Core.Domain;
using System.Threading.Tasks;
using System.Linq;
using Application.Links.Dto;
using Microsoft.Extensions.Configuration;
using Core.Utils;
using Resource;

namespace Application.Links
{
    public class LinkService : ILinkService
    {

        #region Fields
        private readonly IUnitOfWork _uow;
        private readonly DbSet<Link> _links;
        private readonly IConfiguration _configuration;

        public static readonly string Alphabet = "abcdefghijklmnopqrstuvwxyz0123456789";
        public static readonly int Base = Alphabet.Length;
        #endregion



        #region ctor
        public LinkService(IUnitOfWork uow, IConfiguration configuration)
        {
            _configuration = configuration;
            _uow = uow;
            _links = _uow.Set<Link>();

        }
        #endregion



        #region public methods
        public UrlJsonResult ShortenLink(LinkInput link)
        {

            #region validation
            if (string.IsNullOrEmpty(link.LongURL))
            {
                return new UrlJsonResult() { Result = false, Message = Resource.Messages.RequiredField, Url = string.Empty };
            }
            #endregion
            
            string longUrl = NormalizeUrl(link.LongURL);

            #region if exists in db            
            var existLink = _links.FirstOrDefault(x => x.LongURL == longUrl && x.Deleted!=true);
            if (existLink != null)
            {
                existLink.SubmitCount++;
                _uow.MarkAsChanged(existLink);
                _uow.SaveAllChanges();
                return new UrlJsonResult() { Result = true, Message = Resource.Messages.Successful, Url = existLink.ShortURL };
            }
            #endregion

            #region shorten url
            var insertedLink = _links.Add(new Link()
            {
                FirstClientIP = link.ClientIP,
                FirstClientPlatform = link.ClientPlatform,
                LongURL = longUrl,
                SubmitCount = 1,
                CreateDate = DateTime.Now,
                Deleted = false
            });
            _uow.SaveAllChanges();
            string shortedUrl = Encode(insertedLink.Entity.Id);
            insertedLink.Entity.ShortURL = shortedUrl;
            _uow.MarkAsChanged(insertedLink.Entity);
            _uow.SaveAllChanges();
            return new UrlJsonResult() { Result = true, Message = Resource.Messages.Successful, Url = shortedUrl };            
            #endregion
        }



        public UrlJsonResult ResolveLink(LinkInput link)
        {
            var existLink = _links.FirstOrDefault(x => x.ShortURL == link.ShortURL && x.Deleted!=true);
            if (existLink != null)
            {
                existLink.RequestCount++;
                _uow.MarkAsChanged(existLink);
                _uow.SaveAllChanges();
                return new UrlJsonResult() { Result=true, Message=Messages.Successful, Url = existLink.LongURL };
            }
            else
            {
                return new UrlJsonResult() { Result = false, Message = Messages.NotFound, Url = string.Empty }; ;
            }
        }



        public IEnumerable<LinkOutput> GetAll()
        {
            return _links.Where(x=>x.Deleted!=true).Select(x=>new LinkOutput()
            {
                Id=x.Id,
                CreateDate =x.CreateDate,
                LongUrl=x.LongURL,
                ShortUrl=x.ShortURL,
                RequestCount=x.RequestCount,
                SubmitCount=x.SubmitCount
            }).ToList();
        }



        public bool Delete(int id)
        {
            var link = _links.FirstOrDefault(x=>x.Id==id);
            if(link!=null)
            {
                link.Deleted = true;
                _uow.MarkAsChanged(link);
                _uow.SaveAllChanges();
                return true;
            }
            return false;
        }
        #endregion



        #region private methods
        public static string Encode(int i)
        {
            if (i == 0) return Alphabet[0].ToString();

            var s = string.Empty;

            while (i > 0)
            {
                s += Alphabet[i % Base];
                i = i / Base;
            }

            return string.Join(string.Empty, s.Reverse());
        }



        public static int Decode(string s)
        {
            var i = 0;

            foreach (var c in s)
            {
                i = (i * Base) + Alphabet.IndexOf(c);
            }

            return i;
        }



        private static string NormalizeUrl(string url)
        {
            if (!url.StartsWith("http"))
                url = "http://" + url;
            return url.ToLower();
        }
        #endregion

    }
}
