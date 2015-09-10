using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace ToracGolf.MiddleLayer.EFModel
{
    public class ToracGolfContext : DbContext
    {
        public ToracGolfContext(string connectionString) : base(connectionString)
        {
        }

    }
}
