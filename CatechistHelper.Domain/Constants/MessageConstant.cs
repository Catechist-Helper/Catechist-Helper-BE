namespace CatechistHelper.Domain.Constants
{
    public static class MessageConstant
    {
        #region Template, Suffix, Prefix
        private const string CreateSuccessTemplate = "Tạo mới {0} thành công !!!";
        private const string UpdateSuccessTemplate = "Cập nhật {0} thành công !!!";
        private const string DeleteSuccessTemplate = "Xóa {0} thành công !!!";
        private const string CreateFailTemplate = "Tạo mới {0} thất bại @.@";
        private const string UpdateFailTemplate = "Cập nhật {0} thất bại @.@";
        private const string DeleteFailTemplate = "Xóa {0} thất bại @.@";
        private const string NotFoundTemplate = "{0} không có trong hệ thống";
        private const string InvalidRoleTemplate = "{0} không phải là {1} !!!";
        private const string RequiredSuffix = " không được bỏ trống !!!";
        #endregion

        public static class Account
        {
            #region Account Field
            private const string AccountMessage = "Account";
            private const string Email = "Email";
            private const string Password = "Password";
            private const string Role = "Role";
            #endregion
            public static class Require
            {
                public const string EmailRequired = Email + RequiredSuffix;
                public const string PasswordRequired = Password + RequiredSuffix;
                public const string RoleRequired = Role + RequiredSuffix;
            }
            public static class Success
            {
                public static string CreateAccount = String.Format(CreateSuccessTemplate, AccountMessage);
                public static string UpdateAccount = String.Format(UpdateSuccessTemplate, AccountMessage);
                public static string DeleteAccount = String.Format(DeleteSuccessTemplate, AccountMessage);
            }
            public static class Fail
            {
                public static string CreateAccount = String.Format(CreateFailTemplate, AccountMessage);
                public static string UpdateAccount = String.Format(UpdateFailTemplate, AccountMessage);
                public static string DeleteAccount = String.Format(DeleteFailTemplate, AccountMessage);
                public static string NotFoundAccount = String.Format(NotFoundTemplate, AccountMessage);
                public static string EmailExisted = Email + " đã tồn tại !!!";
            }
        }
        public static class Registration
        {
            #region Registration Field
            private const string RegistrationMessage = "Registration";
            private const string FullName = "Full name";
            private const string Gender = "Gender";
            private const string Email = "Email";
            private const string DateOfBirth = "Date of birth";
            private const string Address = "Adress";
            private const string Phone = "Phone";
            private const string IsTeachingBefore = "Is teaching before";
            private const string YearOfTeaching = "Year of teaching";
            #endregion
            public static class Require
            {
                public const string EmailRequired = Email + RequiredSuffix;
                public const string GenderRequired = Gender + RequiredSuffix;
                public const string FullNameRequired = FullName + RequiredSuffix;
                public const string DateOfBirthRequired = DateOfBirth + RequiredSuffix;
                public const string AddresseRequired = Address + RequiredSuffix;
                public const string PhoneRequired = Phone + RequiredSuffix;
                public const string IsTeachingBeforeeRequired = IsTeachingBefore + RequiredSuffix;
                public const string YearOfTeachingRequired = YearOfTeaching + RequiredSuffix;
            }
            public static class Success
            {
                public static string CreateRegistration = String.Format(CreateSuccessTemplate, RegistrationMessage);
                public static string UpdateRegistration = String.Format(UpdateSuccessTemplate, RegistrationMessage);
                public static string DeleteRegistration = String.Format(UpdateSuccessTemplate, RegistrationMessage);
            }
            public static class Fail
            {
                public static string CreateRegistration = String.Format(CreateFailTemplate, RegistrationMessage);
                public static string UpdateRegistration = String.Format(UpdateFailTemplate, RegistrationMessage);
                public static string DeleteRegistration = String.Format(DeleteFailTemplate, RegistrationMessage);
                public static string NotFoundRegistration = String.Format(NotFoundTemplate, RegistrationMessage);
            }
        }
        public static class Login
        {
            public static class Fail
            {
                public static string PasswordIncorrect = "Password Incorrect";
            }
        }
        public static class Interview
        {
            #region Interview Field
            private const string InterviewMessage = "Interview";
            private const string RegistrationId = "Registration id";
            private const string MeetingTime = "Meeting time";
            #endregion
            public static class Require
            {
                public const string MeetingTimeRequired = MeetingTime + RequiredSuffix;
                public const string RegistrationIdRequired = RegistrationId + RequiredSuffix;
            }
            public static class Success
            {
                public static string CreateInterview = String.Format(CreateSuccessTemplate, InterviewMessage);
                public static string UpdateInterview = String.Format(UpdateSuccessTemplate, InterviewMessage);
            }
            public static class Fail
            {
                public static string CreateInterview = String.Format(CreateFailTemplate, InterviewMessage);
                public static string UpdateInterview = String.Format(UpdateFailTemplate, InterviewMessage);
                public static string DeleteInterview = String.Format(DeleteFailTemplate, InterviewMessage);
            }
        }
        public static class InterviewProcess
        {
            #region InterviewProcess Field
            private const string InterviewProcessMessage = "Interview process";
            #endregion
            public static class Require
            {
            }
            public static class Success
            {
                public static string CreateInterviewProcess = String.Format(CreateSuccessTemplate, InterviewProcessMessage);
                public static string UpdateInterviewProcess = String.Format(UpdateSuccessTemplate, InterviewProcessMessage);
            }
            public static class Fail
            {
                public static string CreateInterviewProcess = String.Format(CreateFailTemplate, InterviewProcessMessage);
                public static string UpdateInterviewProcess = String.Format(UpdateFailTemplate, InterviewProcessMessage);
                public static string DeleteInterviewProcess = String.Format(DeleteFailTemplate, InterviewProcessMessage);
            }
        }
        public static class CertificateOfCandidate
        {
            #region CertificateOfCandidate Field
            private const string CertificateOfCandidateMessage = "Interview process";
            #endregion
            public static class Require
            {
            }
            public static class Success
            {
                public static string CreateCertificateOfCandidate = String.Format(CreateSuccessTemplate, CertificateOfCandidateMessage);
            }
            public static class Fail
            {
                public static string CreateCertificateOfCandidate = String.Format(CreateFailTemplate, CertificateOfCandidateMessage);
                public static string DeleteCertificateOfCandidate = String.Format(DeleteFailTemplate, CertificateOfCandidateMessage);
            }
        }
        public static class PastoralYear
        {
            #region PastoralYear Field
            private const string PastoralYearMessage = "PastoralYear";
            private const string Status = "Status";
            private const string Name = "Name of PastoralYear";
            #endregion
            public static class Require
            {
                public const string NameRequired = Name + RequiredSuffix;
                public const string StatusRequired = Status + RequiredSuffix;
            }
            public static class Success
            {
                public static string CreatePastoralYear = String.Format(CreateSuccessTemplate, PastoralYearMessage);
                public static string UpdatePastoralYear = String.Format(UpdateSuccessTemplate, PastoralYearMessage);
                public static string DeletePastoralYear = String.Format(UpdateSuccessTemplate, PastoralYearMessage);

            }
            public static class Fail
            {
                public static string CreatePastoralYear = String.Format(CreateFailTemplate, PastoralYearMessage);
                public static string UpdatePastoralYear = String.Format(UpdateFailTemplate, PastoralYearMessage);
                public static string DeletePastoralYear = String.Format(DeleteFailTemplate, PastoralYearMessage);
            }
        }
        public static class ChristianName
        {
            #region ChristianName Field
            private const string ChristianNameMessage = "Christian name";
            private const string Name = "Name";
            #endregion
            public static class Require
            {
                public const string NameRequired = Name + RequiredSuffix;
            }
            public static class Success
            {
                public static string CreateChristianName = String.Format(CreateSuccessTemplate, ChristianNameMessage);
                public static string UpdateChristianName = String.Format(UpdateSuccessTemplate, ChristianNameMessage);
                public static string DeleteChristianName = String.Format(UpdateSuccessTemplate, ChristianNameMessage);

            }
            public static class Fail
            {
                public static string CreateChristianName = String.Format(CreateFailTemplate, ChristianNameMessage);
                public static string UpdateChristianName = String.Format(UpdateFailTemplate, ChristianNameMessage);
                public static string DeleteChristianName = String.Format(DeleteFailTemplate, ChristianNameMessage);
            }
        }

        public static class PostCategory
        {
            #region PostCategory Field
            private const string PostCategoryMessage = "PostCategory";
            private const string Name = "PostCategory name";
            private const string Description = "Description";

            #endregion
            public static class Require
            {
                public const string NameRequired = Name + RequiredSuffix;
                public const string DescriptionRequired = Description + RequiredSuffix;
            }
            public static class Success
            {
                public static string CreatePostCategory = String.Format(CreateSuccessTemplate, PostCategoryMessage);
                public static string UpdatePostCategory = String.Format(UpdateSuccessTemplate, PostCategoryMessage);
                public static string DeletePostCategory = String.Format(UpdateSuccessTemplate, PostCategoryMessage);

            }
            public static class Fail
            {
                public static string CreatePostCategory = String.Format(CreateFailTemplate, PostCategoryMessage);
                public static string UpdatePostCategory = String.Format(UpdateFailTemplate, PostCategoryMessage);
                public static string DeletePostCategory = String.Format(DeleteFailTemplate, PostCategoryMessage);
            }
        }
    }
}
