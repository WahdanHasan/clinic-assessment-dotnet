using clinic_assessment_redone.Data.Models.Db.Repo.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clinic_assessment.data.Models.Db.Repo.Interfaces
{
    public interface IRepository<T1, T2> where T1 : class
    {
        Task<IEnumerable<T1>> GetAll();

        Task<T1> GetById(T2 id);

        Task<T1> Insert(T1 entity);

        Task DeleteById(T2 id);

        Task Save();
    }
}
