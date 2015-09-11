using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using ToracGolf.MiddleLayer.EFModel.Tables;

namespace ToracGolf.MiddleLayer.EFModel
{
    public class ToracGolfContext : DbContext
    {
        public ToracGolfContext(string connectionString) : base(connectionString)
        {
        }

        public virtual DbSet<UserAccounts> Users { get; set; }
        public virtual DbSet<Ref_State> Ref_State { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }

    }
}
