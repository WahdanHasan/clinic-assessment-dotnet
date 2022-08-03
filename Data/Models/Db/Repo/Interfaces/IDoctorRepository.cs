using clinic_assessment.data.Models.Db.Entity;
using clinic_assessment_redone.Api.Dto.Doctor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clinic_assessment.data.Models.Db.Repo.Interfaces
{
    public interface IDoctorRepository : IRepository<Doctor, int>
    {
        Task<List<BusyDoctorByAptCountResDto>> GetBusyDoctorsByAptCount(DateOnly date);
        Task<List<BusyDoctorsByMinHoursResDto>> GetBusyDoctorsByMinHours(DateOnly date, int minHours);
        Task<DoctorInfoDto> GetInfoById(int doctorId);
    }
}
