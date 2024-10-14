namespace CatechistHelper.Domain.Constants
{
    public static class ApiEndPointConstant
    {
        static ApiEndPointConstant()
        {
        }

        public const string RootEndPoint = "/api";
        public const string ApiVersion = "/v1";
        public const string ApiEndpoint = RootEndPoint + ApiVersion;

        public const string ByIdRoute = "/{id}";
        public static class Authentication
        {
            /// <summary>"api/v1/login"</summary>
            public const string LoginEndPoint = ApiEndpoint + "/login";
            // <summary>"api/v1/google/login"</summary>
            public const string RegisterEndPoint = ApiEndpoint + "/register";
        }
        public static class Account
        {
            /// <summary>"api/v1/accounts"</summary>
            public const string AccountsEndPoint = ApiEndpoint + "/accounts";
            // <summary>"api/v1/accounts/{id}"</summary>
            public const string AccountEndPoint = AccountsEndPoint + ByIdRoute;
        }
        public static class Candidate
        {
            /// <summary>"api/v1/candidates"</summary>
            public const string CandidatesEndPoint = ApiEndpoint + "/candidates";
            // <summary>"api/v1/candidates/{id}"</summary>
            public const string CandidateEndPoint = CandidatesEndPoint + ByIdRoute;
            // <summary>"api/v1/candidates/{id}/certificates"</summary>
            public const string CertificatesOfCandidateEndPoint = CandidateEndPoint + "/certificates";
        }
        public static class Certificate
        {
            /// <summary>"api/v1/candidates"</summary>
            public const string CertificatesEndPoint = ApiEndpoint + "/certificates";
            // <summary>"api/v1/candidates/{id}"</summary>
            public const string CertificateEndPoint = CertificatesEndPoint + ByIdRoute;
        }
        public static class Registration
        {
            /// <summary>"api/v1/registrations"</summary>
            public const string RegistrationsEndPoint = ApiEndpoint + "/registrations";
            // <summary>"api/v1/registrations/{id}"</summary>
            public const string RegistrationEndPoint = RegistrationsEndPoint + ByIdRoute;
            // <summary>"api/v1/registrations/{id}/interviews"</summary>
            public const string InterviewsOfRegistrationEndPoint = RegistrationEndPoint + "/interviews";
            // <summary>"api/v1/registrations/{id}/interview-processes"</summary>
            public const string InterviewProcessesOfRegistrationEndPoint = RegistrationEndPoint + "/interview-processes";
        }
        public static class Interview
        {
            /// <summary>"api/v1/interviews"</summary>
            public const string InterviewsEndPoint = ApiEndpoint + "/interviews";
            // <summary>"api/v1/interviews/{id}"</summary>
            public const string InterviewEndPoint = InterviewsEndPoint + ByIdRoute;
        }
        public static class InterviewProcess
        {
            /// <summary>"api/v1/interview-processes"</summary>
            public const string InterviewProcessesEndPoint = ApiEndpoint + "/interview-processes";
            // <summary>"api/v1/interview-processes/{id}"</summary>
            public const string InterviewProcessEndPoint = InterviewProcessesEndPoint + ByIdRoute;
        }
        public static class Catechist
        {
            public const string CatechistsEndpoint = ApiEndpoint + "/catechists";
            public const string CatechistEndpoint = CatechistsEndpoint + ByIdRoute;
        }
        public static class PastoralYear
        {
            /// <summary>"api/v1/pastoral-years"</summary>
            public const string PastoralYearsEndpoint = ApiEndpoint + "/pastoral-years";
            /// <summary>"api/v1/pastoral-years/{id}"</summary>
            public const string PastoralYearEndpoint = PastoralYearsEndpoint + ByIdRoute;
        }
        public static class ChristianName
        {
            /// <summary>"api/v1/christian-names"</summary>
            public const string ChristianNamesEndpoint = ApiEndpoint + "/christian-names";
            /// <summary>"api/v1/christian-names/{id}"</summary>
            public const string ChristianNameEndpoint = ChristianNamesEndpoint + ByIdRoute;
        }
        public static class PostCategory
        {
            /// <summary>"api/v1/post-categories"</summary>
            public const string PostCategoriesEndpoint = ApiEndpoint + "/post-categories";
            /// <summary>"api/v1/post-categories/{id}"</summary>
            public const string PostCategoryEndpoint = PostCategoriesEndpoint + ByIdRoute;
        }

        public static class Post
        {
            /// <summary>"api/v1/posts"</summary>
            public const string PostsEndpoint = ApiEndpoint + "/posts";
            /// <summary>"api/v1/posts/{id}"</summary>
            public const string PostEndpoint = PostsEndpoint + ByIdRoute;
        }
        public static class Major
        {
            /// <summary>"api/v1/majors"</summary>
            public const string MajorsEndpoint = ApiEndpoint + "/majors";
            /// <summary>"api/v1/majors/{id}"</summary>
            public const string MajorEndpoint = MajorsEndpoint + ByIdRoute;
        }
        public static class SystemConfiguration
        {
            /// <summary>"api/v1/system-configurations"</summary>
            public const string SystemConfigurationsEndpoint = ApiEndpoint + "/system-configurations";
            /// <summary>"api/v1/system-configurations/{id}"</summary>
            public const string SystemConfigurationEndpoint = SystemConfigurationsEndpoint + ByIdRoute;
        }
        public static class Room
        {
            /// <summary>"api/v1/rooms"</summary>
            public const string RoomsEndpoint = ApiEndpoint + "/rooms";
            /// <summary>"api/v1/rooms/{id}"</summary>
            public const string RoomEndpoint = RoomsEndpoint + ByIdRoute;
        }

        public static class Timetable
        {
            public const string TimetableEndpoint = ApiEndpoint + "/timetable";
            public const string SlotsEndpoint = ApiEndpoint + "/slots";
            public const string ClassesEndpoint = ApiEndpoint + "/classes";
            public const string ExportEndpoint = ClassesEndpoint + "/export" + ByIdRoute;
        }
    }
}
