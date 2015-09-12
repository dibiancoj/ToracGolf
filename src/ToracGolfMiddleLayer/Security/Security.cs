using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using ToracGolf.MiddleLayer.EFModel;
using ToracGolf.MiddleLayer.EFModel.Tables;
using ToracGolf.MiddleLayer.Security;

namespace ToracGolf.MiddleLayer.SecurityManager
{

    public static class Security
    {

        public static async Task<UserAccounts> UserLogIn(ToracGolfContext dbContext, string emailAddress, string password)
        {
            return await dbContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.EmailAddress == emailAddress && x.Password == password);
        }

        public static async Task<UserAccounts> RegisterUser(ToracGolfContext dbContext, SignUpEnteredDataBase signUpModel)
        {
            //let's first try to find the season with the same name
            var seasonToAdd = await dbContext.Ref_Season.AsNoTracking().FirstOrDefaultAsync(x => x.SeasonText == signUpModel.CurrentSeason);

            //didn't find a season with this name
            if (seasonToAdd == null)
            {
                seasonToAdd = new Ref_Season { SeasonText = signUpModel.CurrentSeason, CreatedDate = DateTime.Now };

                dbContext.Ref_Season.Add(seasonToAdd);

                //we need to save it for foreign key 
                await dbContext.SaveChangesAsync();
            }

            //user to add
            var userToAdd = new UserAccounts
            {
                StateId = Convert.ToInt32(signUpModel.HomeState),
                CreatedDate = DateTime.Now,
                CurrentSeasonId = seasonToAdd.SeasonId,
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
            dbContext.UserSeason.Add(new UserSeason
            {
                UserId = userToAdd.UserId,
                SeasonId = seasonToAdd.SeasonId,
                CreatedDate = DateTime.Now
            });

            //go save the changes
            await dbContext.SaveChangesAsync();

            //return the user now
            return await dbContext.Users.AsNoTracking().FirstAsync(x => x.UserId == userToAdd.UserId);
        }

    }

}
