using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace URLShortener.Web.Models
{
    public class IndexViewModel
    {
        [Display(Name = "Short Url")]
        public string shortUrl { get; set; }
        [Display(Name = "Long Url")]
        public string longUrl { get; set; }
    }
}