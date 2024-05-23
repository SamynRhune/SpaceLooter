using ActionCommandGame.Repository;
using ActionCommandGame.Services.Model.Requests;
using ActionCommandGame.Services.Model.Results;
using Microsoft.EntityFrameworkCore;


namespace ActionCommandGame.RestApi.Service
{
    public class AccountService
    {
        private readonly ActionButtonGameDbContext _database;
        

        public AccountService(ActionButtonGameDbContext database)
        {
            _database = database;
        }

        public async Task<bool> Delete(string id)
        {
            var user = await _database.AspNetUsers
               .FirstOrDefaultAsync(a => a.Id.Equals(id));

            if (user is null)
            {
                return false;
            }

            _database.AspNetUsers.Remove(user);

            await _database.SaveChangesAsync();
            return true;
        }

        public async Task<AccountResult> GetAccount(int id)
        {
            //apart omdat entity er moeite mee heeft
            var player = await _database.Players
         .SingleOrDefaultAsync(p => p.Id == id);

            if (player == null)
            {
                return null;
            }
            var account = await _database.AspNetUsers.SingleOrDefaultAsync(a => a.Id.Equals(player.IdentityPlayerId));
            if (account == null)
            {
                return null;
            }

            AccountResult result = new AccountResult
            {
                Email = account.Email,
                UserName = account.UserName,
                PhoneNumber = account.PhoneNumber
            };


            return result;
        }

        public async Task<AccountResult> UpdateAccount(int id, AccountRequest request)
        {
            var db_player = await _database.Players.Where(pi => pi.Id == id).FirstOrDefaultAsync();

            if (db_player is null)
            {
                return null;
            }

            var account = await _database.AspNetUsers.SingleOrDefaultAsync(a => a.Id.Equals(db_player.IdentityPlayerId));
            if (account == null)
            {
                return null;
            }

            /*AccountResult result = new AccountResult
            {
                Email = account.Email,
                UserName = account.UserName,
                PhoneNumber = account.PhoneNumber
            };*/

            account.UserName = request.UserName;
            account.Email = request.Email;
            account.PhoneNumber = request.PhoneNumber;
            await _database.SaveChangesAsync();

            db_player.Name = request.UserName;

            await _database.SaveChangesAsync();

            return await GetAccount(id);
        }
    }
}
