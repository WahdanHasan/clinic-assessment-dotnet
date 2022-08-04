using clinic_assessment.data.Models.Db.Entity;
using clinic_assessment.data.Models.Db.Repo.Interfaces;
using clinic_assessment_redone.Api.Dto.Doctor;
using clinic_assessment_redone.Helpers.Misc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clinic_assessment.data.Models.Db.Repo
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly DataContext _dataContext;
        public DoctorRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public Task DeleteById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Doctor>> GetAll()
        {
            return await _dataContext.Doctor.ToListAsync();
        }

        public async Task<List<BusyDoctorByAptCountResDto>> GetBusyDoctorsByAptCount(DateOnly date)
        {
            var doctorsList = await _dataContext.Doctor
                .Select(d => new BusyDoctorByAptCountResDto
                {
                    firstName = d.FirstName,
                    lastName = d.LastName,
                    appointmentCount = _dataContext.Appointment
                    .Count(a => a.DateCreated == date && a.Doctor.Id == d.Id)
                }).ToListAsync();


            return doctorsList;
        }

        public async Task<List<BusyDoctorsByMinHoursResDto>> GetBusyDoctorsByMinHours(DateOnly date, int minHours)
        {
            var doctorsList = await _dataContext.Doctor
                .Select(d => new BusyDoctorsByMinHoursResDto
                {
                    firstName = d.FirstName,
                    lastName = d.LastName,
                    appointmentHours = _dataContext.Appointment
                    .Where(a => a.Doctor.Id == d.Id)
                    .Sum(a => a.DurationMins) / Consts.MINUTES_IN_HOUR

                }).ToListAsync();

            return doctorsList;
        }

        public async Task<Doctor> GetById(int id)
        {
            return await _dataContext.Doctor.Where(d => d.Id == id).FirstOrDefaultAsync();
        }

        public async Task<DoctorInfoDto> GetInfoById(int doctorId)
        {
            DoctorInfoDto doctorInfo = await _dataContext.Doctor
                .Select(d => new DoctorInfoDto
                {
                    id = d.Id,
                    firstName = d.FirstName,
                    lastName = d.LastName,
                    nationality = d.Nationality,
                    dateOfBirth = d.DateOfBirth,
                    gender = d.Gender,
                    phoneNumber = d.PhoneNumber,
                    phoneExtension = d.PhoneExt,
                    officeRoomNumber = d.OfficeRoomNumber,
                    workStartTimes = d.WorkStartTimes,
                    workEndTimes = d.WorkEndTimes,
                    specialty = d.specialty
                })
                .Where(res => res.id == doctorId)
                .FirstOrDefaultAsync();



            return doctorInfo;
        }

        public async Task<Doctor> Insert(Doctor entity)
        {
            await _dataContext.Doctor.AddAsync(entity);
            return entity;
        }

        public Task Save()
        {
            return _dataContext.SaveChangesAsync();
        }
    }
}
