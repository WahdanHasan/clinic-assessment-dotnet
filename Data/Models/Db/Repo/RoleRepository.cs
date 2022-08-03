using clinic_assessment.data.Models.Db.Entity;
using clinic_assessment.data.Models.Db.Repo.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clinic_assessment.data.Models.Db.Repo
{
    public class RoleRepository : IRoleRepository
    {
        private readonly DataContext _dataContext;
        public RoleRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public Task DeleteById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Role>> GetAll()
        {
            return await _dataContext.Role.ToListAsync();
        }

        public Task<Role> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Role>> GetUserRoles(int userId)
        {

            List<Role> userRoles = await _dataContext.Role
                .Where(r => 
                    r.RoleUsers.Any(ru => ru.User.Id == userId)
                )
                .ToListAsync();

            return userRoles;
        }

        public async Task<Role> Insert(Role entity)
        {
            throw new NotImplementedException();
        }

        public Task Save()
        {
            throw new NotImplementedException();
        }
    }
}
