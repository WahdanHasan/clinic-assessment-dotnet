using clinic_assessment.data.Models.Db.Entity;
using clinic_assessment.data.Models.Db.Repo.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clinic_assessment.data.Models.Db.Repo
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly DataContext _dataContext;
        public EmployeeRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public Task DeleteById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Employee>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Employee> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Employee> Insert(Employee entity)
        {
            await _dataContext.Employee.AddAsync(entity);
            return entity;
        }

        public Task Save()
        {
            return _dataContext.SaveChangesAsync();
        }
    }
}
