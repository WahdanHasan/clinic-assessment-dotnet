using clinic_assessment.data.Models.Db.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clinic_assessment.data.Models.Db.Repo.Interfaces
{
    public interface IPatientRepository : IRepository<Patient, int>
    {
        Task<Dictionary<int, string>> GetFullNamesByIds(List<int> patientIdList);
        Task<List<Patient>> GetAllByIdList(List<int> patientIdsForLookup);
    }
}
