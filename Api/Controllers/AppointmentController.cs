using clinic_assessment_redone.Api.Dto.Appointment;
using clinic_assessment_redone.Helpers.Controllers.Interfaces;
using clinic_assessment_redone.Helpers.Misc;
using clinic_assessment_redone.Middleware.Authorization.Attribute;
using Microsoft.AspNetCore.Mvc;

namespace clinic_assessment_redone.Api.Controllers
{
    [Route("api/appointment")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentHelper _appointmentHelper;

        public AppointmentController(IAppointmentHelper appointmentHelper)
        {
            _appointmentHelper = appointmentHelper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var res = await _appointmentHelper.GetAll();

            return Ok(res);
        }

        [HttpGet("all")]
        public async Task<IActionResult> Get([FromQuery] int patientId)
        {
            var res = await _appointmentHelper.GetAllPatientAppointments(patientId);

            return Ok(res);
        }

        [HttpGet("{appointmentId}/details")]
        [Permissions(Consts.PERMISSION_APPOINTMENT_DETAILS)]
        public async Task<IActionResult> GetAptDetails([FromRoute] int appointmentId)
        {
            var res = await _appointmentHelper.GetAppointmentDetails(appointmentId);

            return Ok(res);
        }

        [HttpPut("{appointmentId}/cancel")]
        [Permissions(Consts.PERMISSION_APPOINTMENT_CANCEL)]
        public async Task<IActionResult> Cancel([FromRoute] int appointmentId)
        {
            await _appointmentHelper.CancelAppointment(appointmentId);

            return Ok();
        }

        [HttpPost("book")]
        [Permissions(Consts.PERMISSION_APPOINTMENT_CREATE)]
        public async Task<IActionResult> Book([FromBody] BookAppointmentReqDto req)
        {
            await _appointmentHelper.BookAppointment(req);

            return Ok();

        }
    }
}
