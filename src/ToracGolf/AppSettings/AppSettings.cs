using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToracGolf.Settings
{

    public class AppSettings
    {

        public AppSettingsData Data { get; set; }

        public int DefaultListingRecordsPerPage { get; set; }

        public string CourseImageSavePath { get; set; }

        public string CourseImageVirtualUrl { get; set; }
    }

    public class AppSettingsData
    {
        public AppSetingsConnectionStrings DefaultConnection { get; set; }
    }

    public class AppSetingsConnectionStrings
    {
        public string ConnectionString { get; set; }
    }

}
