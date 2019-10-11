using Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.Domain
{
    public class Link
    {
        [Key]
        public int Id { get; set; }
        public string LongURL { get; set; }
        public string  ShortURL { get; set; }
        public string FirstClientIP { get; set; }
        public ClientPlatform FirstClientPlatform { get; set; }
        public int SubmitCount { get; set; }
        public int RequestCount { get; set; }
        public DateTime CreateDate { get; set; }
        public bool Deleted { get; set; }
    }
}
