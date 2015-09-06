using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToracGolf.ViewModels.Navigation
{
    public class TopNavViewModel
    {

        public TopNavViewModel(bool showNavMenu)
        {
            ShowNavMenu = showNavMenu;
        }

        public bool ShowNavMenu { get; }
    }
}
