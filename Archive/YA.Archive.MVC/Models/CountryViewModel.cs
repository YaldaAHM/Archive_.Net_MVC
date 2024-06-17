using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace YA.Archive.MVC2.Models
{
    public class CountryViewModel
    {
            public List<string> Countries { get; set; }
            public MultiSelectList CountryList { get; private set; }

            public CountryViewModel()
            {
                
            }

            
        }
}