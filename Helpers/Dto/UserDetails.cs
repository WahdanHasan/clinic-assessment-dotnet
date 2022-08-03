namespace clinic_assessment_redone.Helpers.Dto
{
    public class UserDetails
    {
        public string email { get; set; }

        public string password { get; set; }

        public string firstName { get; set; }

        public string lastName { get; set; }

        public string dateOfBirth { get; set; }

        public string nationality { get; set; }

        public string gender { get; set; }

        public string phoneCountryCode { get; set; }

        public string phoneNumber { get; set; }

        public List<string>? roles { get; set; }

    }
}
