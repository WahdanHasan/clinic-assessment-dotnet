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
    public class ClinicUserRepository : IClinicUserRepository
    {
        private readonly DataContext _dataContext;
        public ClinicUserRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public Task DeleteById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ClinicUser>> GetAll()
        {
            return await _dataContext.ClinicUser.ToListAsync();
        }

        public async Task<ClinicUser> GetByEmail(string email)
        {
            return await _dataContext.ClinicUser.Where(b => b.Email == email).FirstOrDefaultAsync();
        }

        public async Task<ClinicUser> GetById(int id)
        {
            return await _dataContext.ClinicUser.Where(cu => cu.Id == id).AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<ClinicUser> Insert(ClinicUser entity)
        {
            await _dataContext.ClinicUser.AddAsync(entity);
            return entity;
        }

        public Task Save()
        {
            return _dataContext.SaveChangesAsync();
        }
    }
}
