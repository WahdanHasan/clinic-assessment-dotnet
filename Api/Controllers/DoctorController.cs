using clinic_assessment_redone.Helpers.Controllers.Interfaces;
using clinic_assessment_redone.Helpers.Misc;
using clinic_assessment_redone.Middleware.Authorization.Attribute;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace clinic_assessment_redone.Api.Controllers
{
    [Route("api/doctor")]
    [ApiController]
    public class DoctorController : ControllerBase
    {

        private readonly IDoctorHelper _doctorHelper;

        public DoctorController(IDoctorHelper doctorHelper)
        {
            _doctorHelper = doctorHelper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var res = await _doctorHelper.GetAllDoctors();

            return Ok(res);
        }

        [HttpGet("{doctorId}")]
        public async Task<IActionResult> Get([FromRoute] int doctorId)
        {
            var res = await _doctorHelper.GetDoctor(doctorId);

            return Ok(res);
        }

        [HttpGet("busy/{date}")]
        [Permissions(Constants.PERMISSION_DOCTOR_BUSY_APT_COUNT)]
        public async Task<IActionResult> Get([FromRoute] string date)
        {
            var res = await _doctorHelper.GetAllBusyByAptCount(date);

            return Ok(res);
        }

        [HttpGet("busy/{date}/{minimum-hours}")]
        [Permissions(Constants.PERMISSION_DOCTOR_BUSY_MIN_HOURS)]
        public async Task<IActionResult> Get([FromRoute] string date, int minHours)
        {
            var res = await _doctorHelper.GetAllBusyByMinHours(date, minHours);

            return Ok(res);
        }

        [HttpGet("{id}/slots")]
        [Permissions(Constants.PERMISSION_DOCTOR_AVAILABLE_SLOTS)]
        public async Task<IActionResult> Get([FromRoute(Name = "id")] int doctorId, 
                                             [FromQuery] string date)
        {
            var res = await _doctorHelper.GetAvailableSlots(date, doctorId);

            return Ok(res);
        }

        [HttpGet("all/slots")]
        [Permissions(Constants.PERMISSION_DOCTOR_AVAILABLE_SLOTS)]
        public async Task<IActionResult> GetAllSlots([FromQuery] string date)
        {
            var res = await _doctorHelper.GetAllAvailableSlots(date);

            return Ok(res);
        }
    }
}
