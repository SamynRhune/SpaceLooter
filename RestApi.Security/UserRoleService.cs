using ActionCommandGame.Repository;
using ActionCommandGame.Services.Model.Results;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActionCommandGame.RestApi.Security
{
    public class UserRoleService
    {
        private readonly ActionButtonGameDbContext _database;

        public UserRoleService(ActionButtonGameDbContext database)
        {
            _database = database;
        }

        public async Task<List<IdentityUserRole<string>>> Find()
        {
            return await _database.AspNetUserRoles.ToListAsync();
        }

        public async Task<bool> Delete(string userId, string roleId)
        {
            var userrole = _database.AspNetUserRoles.Where(a => a.UserId.Equals(userId) && a.RoleId.Equals(roleId)).FirstOrDefault();

            if (userrole is null)
            {
                return false;
            }

            _database.AspNetUserRoles.Remove(userrole);

            await _database.SaveChangesAsync();
            return true;
        }

        public async Task<IdentityRole> GetRoleFromUser(string userId)
        {
            var userRole = _database.AspNetUserRoles.Where(p => p.UserId == userId).FirstOrDefault();
            if (userRole == null)
            {
                return null;
            }

            var role = _database.AspNetRoles.Where(p => p.Id == userRole.RoleId).FirstOrDefault();
            if (role == null)
            {
                return null;
            }

            return role;
        }

        public async Task<IdentityUserRole<string>> SetRoleFromUser(string userId, string roleName)
        {
            try
            {
                var user = _database.AspNetUsers.FirstOrDefault(p => p.Id.Equals(userId));
                if (user == null)
                {
                    throw new Exception("User not found");
                }

                var role = _database.AspNetRoles.FirstOrDefault(p => p.Name.Equals(roleName));
                if (role == null)
                {
                    throw new Exception("Role not found");
                }

                var userRole = _database.AspNetUserRoles.FirstOrDefault(p => p.UserId == userId);
                if (userRole == null)
                {
                    // If userRole doesn't exist, create it
                    userRole = new IdentityUserRole<string>
                    {
                        UserId = userId,
                        RoleId = role.Id
                    };
                    _database.AspNetUserRoles.Add(userRole);
                }
                else
                {
                    _database.AspNetUserRoles.Remove(userRole);
                    await _database.SaveChangesAsync();

                    IdentityUserRole<string> newRole = new IdentityUserRole<string>{
                        UserId = user.Id,
                        RoleId = role.Id
                    };
                    // Update the role if it already exists
                    userRole.RoleId = role.Id;
                    _database.AspNetUserRoles.Add(userRole);
                }

                await _database.SaveChangesAsync();

                return userRole;
            }
            catch (Exception ex)
            {
                // Log the exception for detailed error information
                // _logger.LogError(ex, "Error setting user role for user {UserId} to {RoleName}", userId, roleName);
                throw new Exception($"An error occurred while setting the user role: {ex.Message}", ex);
            }
        }
    }
}
