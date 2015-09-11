using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using ToracGolf.MiddleLayer.EFModel;
using ToracGolf.MiddleLayer.EFModel.Tables;

namespace ToracGolf.MiddleLayer.SecurityManager
{

    public static class Security
    {

        public static async Task<UserAccounts> UserLogIn(ToracGolfContext dbContext, string emailAddress, string password)
        {
            return await dbContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.EmailAddress == emailAddress && x.Password == password);
        }

    }

}
