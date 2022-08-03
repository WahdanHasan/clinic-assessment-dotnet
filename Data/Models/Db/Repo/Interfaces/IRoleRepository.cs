using clinic_assessment.data.Models.Db.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clinic_assessment.data.Models.Db.Repo.Interfaces
{
    public interface IRoleRepository : IRepository<Role, int>
    {
        Task<List<Role>> GetUserRoles(int userId);
    }
}
