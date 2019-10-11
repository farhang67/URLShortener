using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Application.Links.Dto
{
    public class LinkOutput
    {
        public int Id { get; set; }
        [Display(Name ="Long Url")]
        public string LongUrl { get; set; }
        public string ShortUrl { get; set; }
        public int SubmitCount { get; set; }
        public int RequestCount { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
