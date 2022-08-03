using clinic_assessment.data.Models.Db.Entity;
using clinic_assessment.data.Models.Db.Repo.Interfaces;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clinic_assessment.data.Models.Db.Repo
{
    public class PatientRepository : IPatientRepository
    {
        private readonly DataContext _dataContext;
        public PatientRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public Task DeleteById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Patient>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<Dictionary<int, string>> GetFullNamesByIds(List<int> patientIdList)
        {
            Dictionary<int, string> patientFullNames =await _dataContext.Patient
                //.Where(p => patientIdList.Contains(p.Id))
                .Select(p => new { p.Id, fullName = (p.FirstName + " " + p.LastName)})
                .ToDictionaryAsync(p => p.Id, p => p.fullName);

            return patientFullNames;
        }

        public async Task<Patient> GetById(int id)
        {
            return await _dataContext.Patient.Where(p => p.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Patient> Insert(Patient entity)
        {
            await _dataContext.Patient.AddAsync(entity);
            return entity;
        }

        public Task Save()
        {
            return _dataContext.SaveChangesAsync();
        }

        public async Task<List<Patient>> GetAllByIdList(List<int> patientIdsForLookup)
        {

            List<Patient> patientList = await _dataContext.Patient
                .Where(p => patientIdsForLookup.Contains(p.Id))
                .ToListAsync();

            return patientList;
        }
    }
}
