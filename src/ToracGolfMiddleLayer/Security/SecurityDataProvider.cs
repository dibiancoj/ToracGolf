using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using ToracGolf.MiddleLayer.EFModel;
using ToracGolf.MiddleLayer.EFModel.Tables;
using ToracGolf.MiddleLayer.Season;
using ToracGolf.MiddleLayer.Security;

namespace ToracGolf.MiddleLayer.SecurityManager
{

    public static class SecurityDataProvider
    {

        public static async Task<UserAccounts> UserLogIn(ToracGolfContext dbContext, string emailAddress, string password)
        {
            return await dbContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.EmailAddress == emailAddress && x.Password == password);
        }

        public static async Task<UserAccounts> RegisterUser(ToracGolfContext dbContext, SignUpEnteredDataBase signUpModel)
        {
            //go either get the ref season record or add it
            var refSeasonRecord = await SeasonDataProvider.RefSeasonAddOrGet(dbContext, signUpModel.CurrentSeason);

            //user to add
            var userToAdd = new UserAccounts
            {
                StateId = Convert.ToInt32(signUpModel.HomeState),
                CreatedDate = DateTime.Now,
                CurrentSeasonId = refSeasonRecord.SeasonId,
                EmailAddress = signUpModel.Email,
                FirstName = signUpModel.FirstName,
                LastName = signUpModel.LastName,
                Password = signUpModel.Password
            };

            //go try to add the user
            dbContext.Users.Add(userToAdd);

            //go save the changes
            await dbContext.SaveChangesAsync();

            //add the user season now
            await SeasonDataProvider.UserSeasonAdd(dbContext, userToAdd.UserId, refSeasonRecord);

            //return the user now
            return await dbContext.Users.AsNoTracking().FirstAsync(x => x.UserId == userToAdd.UserId);
        }

        public static async Task<string> Password(ToracGolfContext dbContext, int userId)
        {
            return (await dbContext.Users.AsNoTracking().FirstAsync(x => x.UserId == userId)).Password;
        }

        public static async Task ChangePassword(ToracGolfContext dbContext, int userId, string password)
        {
            var user = await dbContext.Users.FirstAsync(x => x.UserId == userId);

            user.Password = password;

            await dbContext.SaveChangesAsync();
        }

    }

}
