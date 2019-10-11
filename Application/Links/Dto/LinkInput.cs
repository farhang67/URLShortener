using Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Links.Dto
{
    public class LinkInput
    {
        public string LongURL { get; set; }
        public string ShortURL { get; set; }
        public string ClientIP { get; set; }
        public ClientPlatform ClientPlatform { get; set; }
    }
}
