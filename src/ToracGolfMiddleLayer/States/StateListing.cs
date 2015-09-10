using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToracGolf.MiddleLayer.EFModel;

namespace ToracGolf.MiddleLayer.States
{
    public static class StateListing
    {

        public static IEnumerable<string> StateSelect(string connectionString)
        {
            using (var context = new ToracGolfContext(connectionString))
            {
                //need to pull in the model for each of these, then pull the states...i think just create a dummy model in another app then copy it over.
                //or just look online for a simple code first model
                context.Database.ExecuteSqlCommand("select 1", new object[0]);
            }

            throw new NotImplementedException();
        }

    }
}
