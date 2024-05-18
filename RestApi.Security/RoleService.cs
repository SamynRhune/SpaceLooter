using ActionCommandGame.Repository;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActionCommandGame.RestApi.Security
{
    
    public class RoleService
    {
        private readonly ActionButtonGameDbContext _database;

        public RoleService(ActionButtonGameDbContext database)
        {
            _database = database;
        }

        public async Task<List<IdentityRole>> GetAllRoles()
        {
            var roles = _database.AspNetRoles.ToList();
            return roles;
        }
    }
}
