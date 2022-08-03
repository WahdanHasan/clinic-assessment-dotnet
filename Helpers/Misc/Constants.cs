namespace clinic_assessment_redone.Helpers.Misc
{
    public static class Constants
    {

        /* Program roles */
        public static readonly string ROLE_DOCTOR = "doctor";
        public static readonly string ROLE_PATIENT = "patient";
        public static readonly string ROLE_CA = "clinic admin";

        /* Program permissions */
        public const string CLAIM_TYPE_PERMISSION = "permissions";
        public const string PERMISSION_DOCTOR_BUSY_APT_COUNT = "view-all-doctors-busy-by-date";
        public const string PERMISSION_DOCTOR_BUSY_MIN_HOURS = "view-all-doctors-busy-by-hours";
        public const string PERMISSION_DOCTOR_AVAILABLE_SLOTS = "view-doctor-available-slots";
        public const string PERMISSION_DOCTORS_AVAILABLE_SLOTS = "view-all-doctors-available-slots";
        public const string PERMISSION_APPOINTMENT_CREATE = "appointment-create";
        public const string PERMISSION_APPOINTMENT_CANCEL = "appointment-cancel";
        public const string PERMISSION_APPOINTMENT_DETAILS = "view-appointment-details";
        public const string PERMISSION_PATIENT_APPOINTMENT_HISTORY = "view-patient-appointment-history";

        /* Field formats */
        public static readonly string DATE_FORMAT = "yyyy-MM-dd";
        public static readonly string TIME_FORMAT = "HH:mm";
        public static readonly string DATE_TIME_FORMAT = DATE_FORMAT + TIME_FORMAT;

        /* Exception response messages */
        public static readonly string INVALID_CREDENTIALS = "The username/password is incorrect.";
        public static readonly string INTERNAL_SERVER_ERROR = "An internal server error has occured.";
        public static readonly string NOT_FOUND = "{0} not found.";
        public static readonly string MISSING_FIELD = "Required field {0} is missing/empty.";
        public static readonly string MISSING_PAYLOAD = "The payload is missing.";
        public static readonly string INVALID_FIELD_VALUE = "The field {0} has an invalid value of {1}.";
        public static readonly string MALFORMED_FIELD = "The field {0} is malformed.";
        public static readonly string FIELD_OUTSIDE_RANGE = "The field {0} is outside of the range {1}";
        public static readonly string MULTIPLE_UNIQUE_FIELD_VALUES = "The field {0} contains multiple unique values.";
        public static readonly string TIME_OCCURS_TIME_DATE = "Time of {0} occurs after the time of {1}";


        /* Custom response messages */
        public static readonly string DOCTOR_FULL_SCHEDULE = "The requested doctor is unavailable for the date {0} due to a full schedule";
        public static readonly string OUTSIDE_DOCTOR_SCHEDULE = "The appointment request is outside of the doctor's scheduled hours";
        public static readonly string NO_ACCESS_TOKEN = "No access token present in request.";
        public static readonly string DOCTOR_UNAVAILABLE_DURING_HOURS = "The doctor is unavailable during the requested hours";
        public static readonly string PATIENT_OVERLAPPING_APPOINTMENTS = "The patient has an overlapping appointment for the requested time period";
        public static readonly string JWT_EXPIRED = "The provided JWT has expired.";
        public static readonly string USER_EXISTS = "The user with email {0} already exists";

        /* DB attribute values */
        public static readonly string APPOINTMENT_STATUS_CANCELLED = "cancelled";
        public static readonly string APPOINTMENT_STATUS_VALID = "valid";

        /* Appointment values */
        public static readonly int APPOINTMENT_LOOKAHEAD_DAYS = 7;
        public static readonly int APPOINTMENT_MIN_MINUTES = 15;
        public static readonly int APPOINTMENT_MAX_MINUTES = 120;
        public static readonly int APPOINTMENT_MAX_COUNT = 12;
        public static readonly int APPOINTMENT_MAX_TOTAL_MINUTES = 400;


        /* Misc */
        // Unit is minutes
        public static readonly int JWT_ACCESS_EXPIRY = 20;
        public static readonly int JWT_REFRESH_EXPIRY = 5;
        public static readonly int MINUTES_IN_HOUR = 60;
        public static readonly string TOKEN_PREFIX = "Bearer ";
        public static readonly string TIME_ZONE = "Arabian Standard Time";
        
    }
}
