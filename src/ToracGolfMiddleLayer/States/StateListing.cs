using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToracGolf.MiddleLayer.EFModel;
using ToracGolf.MiddleLayer.EFModel.Tables;

namespace ToracGolf.MiddleLayer.States
{
    public static class StateListing
    {

        public static IEnumerable<Ref_State> StateSelect(string connectionString)
        {
            using (var context = new ToracGolfContext(connectionString))
            {
                return context.Ref_State.AsNoTracking().OrderBy(x => x.Description).ToArray();
            }
        }

    }
}
