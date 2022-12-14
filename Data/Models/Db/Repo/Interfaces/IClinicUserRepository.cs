using clinic_assessment.data.Models.Db.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clinic_assessment.data.Models.Db.Repo.Interfaces
{
    public interface IClinicUserRepository : IRepository<ClinicUser, int>
    {
        Task<ClinicUser> GetByEmail(string email);

    }
}
