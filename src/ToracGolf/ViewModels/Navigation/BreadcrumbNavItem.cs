using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToracGolf.ViewModels.Navigation
{

    public class BreadcrumbNavItem
    {

        public BreadcrumbNavItem(string text, string url)
        {
            Text = text;
            Url = url;
        }

        public string Text { get;  }

        public string Url { get;  }
    }

}
