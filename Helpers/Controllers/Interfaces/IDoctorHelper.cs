using clinic_assessment.data.Models.Db.Entity;
using clinic_assessment_redone.Api.Dto.Doctor;

namespace clinic_assessment_redone.Helpers.Controllers.Interfaces
{
    public interface IDoctorHelper
    {
        Task<AllDoctorsResDto> GetAllDoctors();

        Task<AllBusyDoctorsByAptCountResDto> GetAllBusyByAptCount(string date);
        Task<AllBusyDoctorsByMinHoursResDto> GetAllBusyByMinHours(string date, int minHours);
        Task<DoctorAvailableSlotResDto> GetAvailableSlots(string date, int doctorId);
        Task<List<DoctorAvailableSlotResDto>> GetAllAvailableSlots(string date);
        Task<DoctorInfoDto> GetDoctor(int doctorId);
    }
}
