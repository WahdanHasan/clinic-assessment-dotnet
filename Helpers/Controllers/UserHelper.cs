using clinic_assessment.data;
using clinic_assessment.data.Models.Db.Entity;
using clinic_assessment.data.Models.Db.Repo.Interfaces;
using clinic_assessment.data.Models.Response;
using clinic_assessment_redone.Api.Dto.User;
using clinic_assessment_redone.Helpers.Controllers.Interfaces;
using clinic_assessment_redone.Helpers.Mapper;
using clinic_assessment_redone.Helpers.Misc;
using clinic_assessment_redone.Middleware.ErrorHandler.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace clinic_assessment_redone.Helpers.Controllers
{
    public class UserHelper : IUserHelper
    {
        private readonly IRepository<Role, int> _roleRepository;
        private readonly IRepository<ClinicUser, int> _userRepository;
        private readonly IRepository<Employee, int> _employeeRepository;
        private readonly IRepository<Doctor, int> _doctorRepository;
        private readonly IRepository<Patient, int> _patientRepository;

        public UserHelper(
            IRepository<Role, int> roleRepository, 
            IRepository<ClinicUser, int> userRepository, 
            IRepository<Employee, int> employeeRepository,
            IRepository<Doctor, int> doctorRepository,
            IRepository<Patient, int> patientRepository
            )
        {
            _roleRepository = roleRepository;
            _userRepository = userRepository;
            _employeeRepository = employeeRepository;
            _doctorRepository = doctorRepository;
            _patientRepository = patientRepository;
        }

        public async Task<IEnumerable<ClinicUser>> GetAllUsers()
        {
            return await _userRepository.GetAll();

        }

        public async Task<SuccessResponse> RegisterUser(RegisterUserReqDto req)
        {

            /* Verify the integrity of the payload data */
            if (req == null)
            {
                throw new RestException(StatusCodes.Status400BadRequest, Consts.MISSING_PAYLOAD);
            }

            /* Validate user fields */
            Util.validateUserAttributes(req.user);

            /* Verify that the user does not already exist */
            ClinicUser clinicUser = await ((IClinicUserRepository)_userRepository).GetByEmail(req.user.email);

            if (clinicUser != null)
            {
                throw new RestException(StatusCodes.Status400BadRequest, String.Format(Consts.USER_EXISTS, req.user.email));
            }


            /* Verify the integrity of the roles. If they are valid, add them to the entity */
            if (req.user.roles == null || req.user.roles.Count == 0)
            {
                throw new RestException(StatusCodes.Status400BadRequest, String.Format(Consts.MISSING_FIELD, "roles"));

            }

            // Get all roles and create a list of their names
            List<Role> roles = (await _roleRepository.GetAll()).ToList();

            var roleNames = new List<string>();
            var roleIds = new List<int>();

            foreach (var role in roles)
            {
                roleNames.Add(role.Name);
                roleIds.Add(role.Id);
            }

            // Ensure the roles in the request exist. If they do, create an entity for it
            List<UserRoles> userRoles = new List<UserRoles>();
            bool roleFound;
            for (int i = 0; i < req.user.roles.Count; i++)
            {
                roleFound = false;
                int j;
                for (j = 0; j < roleNames.Count; j++)
                {
                    if (roleNames[j].Equals(req.user.roles[i])){
                        roleFound = true;
                        break;
                    }
                }

                if (roleFound)
                {
                    userRoles.Add(new UserRoles
                    {
                        Role = roles[j]
                    });

                    continue;
                }

                throw new RestException(StatusCodes.Status400BadRequest, String.Format(Consts.INVALID_FIELD_VALUE, "roles", req.user.roles[i]));

            }

            /* Set user roles */
            req.userRoles = userRoles;


            /* Validate the user attributes */
            // TODO: Catch exception
            Util.validateUserAttributes(req.user);

            /* Validate the user roles to ensure multiple unique roles aren't assigned */


            if (Util.containsMultipleUniqueRoles(req.user.roles))
            {
                throw new RestException(StatusCodes.Status400BadRequest, String.Format(Consts.MULTIPLE_UNIQUE_FIELD_VALUES, "roles"));
            }

            /* Validate the employee attributes */
            Util.validateEmployeeAttributes(req.employee);

            /* Encrypt the password */
            req.user.password = Util.hashPassword(req.user.password);

            /* Convert request DTO to entity based on role, then save the entity to DB */
            /* This design assumes that certain roles are unique and a user cannot have more than one */
            if (req.user.roles.Contains(Consts.ROLE_DOCTOR))
            {
                Doctor doctor = UserMapper.RegisterUserReqDtoToDoctorEntity(req);

                await _doctorRepository.Insert(doctor);

                await _doctorRepository.Save();
            }
            else if (req.user.roles.Contains(Consts.ROLE_PATIENT))
            {
                Patient patient = UserMapper.RegisterUserReqDtoToPatientEntity(req);

                await _patientRepository.Insert(patient);

                await _patientRepository.Save();
            }
            else if (req.user.roles.Contains(Consts.ROLE_CA))
            {
                Employee clinicAdmin = UserMapper.RegisterUserReqDtoToEmployeeEntity(req);

                await _employeeRepository.Insert(clinicAdmin);

                await _employeeRepository.Save();
            }

            return new SuccessResponse();
        }


    }
}
