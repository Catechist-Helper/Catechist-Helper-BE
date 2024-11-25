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

        public const string ImageEndpoint = "/image";
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
            // <summary>"api/v1/registrations/{id}/registration-processes"</summary>
            public const string RegistrationProcessesOfRegistrationEndPoint = RegistrationEndPoint + "/registration-processes";
        }
        public static class Interview
        {
            /// <summary>"api/v1/interviews"</summary>
            public const string InterviewsEndPoint = ApiEndpoint + "/interviews";
            // <summary>"api/v1/interviews/{id}"</summary>
            public const string InterviewEndPoint = InterviewsEndPoint + ByIdRoute;
        }
        public static class RegistrationProcess
        {
            /// <summary>"api/v1/registration-processes"</summary>
            public const string RegistrationProcessesEndPoint = ApiEndpoint + "/registration-processes";
            // <summary>"api/v1/registration-processes/{id}"</summary>
            public const string RegistrationProcessEndPoint = RegistrationProcessesEndPoint + ByIdRoute;
        }
        public static class Catechist
        {
            public const string CatechistsEndpoint = ApiEndpoint + "/catechists";
            public const string CatechistEndpoint = CatechistsEndpoint + ByIdRoute;
            public const string UpdateImageEndpoint = CatechistsEndpoint + ImageEndpoint + ByIdRoute;
            public const string CertificateOfCatechistsEndpoint = CatechistEndpoint + "/certificates";
            public const string ClassesEndpoint = CatechistsEndpoint + ByIdRoute + "/classes";
            public const string GradesEndpoint = CatechistsEndpoint + ByIdRoute + "/grades";
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
            /// <summary>"api/v1/majors/{id}/levels/"</summary>
            public const string LevelOfMajorsEndpoint = MajorEndpoint + "/levels";
            /// <summary>"api/v1/majors/{id}/levels/{id}"</summary>
            public const string LevelOfMajorEndpoint = MajorsEndpoint + "/{majorId}" + "/levels" + "/{levelId}";
            /// <summary>"api/v1/majors/{id}/levels/{id}"</summary>
            public const string CatechistInMajorsEndpoint = MajorEndpoint + "/catechists";
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
            public const string YearsEndpoint = ApiEndpoint + "/pastoral-years";
            public const string TimetableEndpoint = ApiEndpoint + "/timetable";
            public const string SlotsEndpoint = ApiEndpoint + "/slots";
            public const string ClassesEndpoint = ApiEndpoint + "/classes";
            public const string ExportEndpoint = ClassesEndpoint + "/export" + ByIdRoute;
            public const string ExportYearEndpoint = YearsEndpoint + "/export";
            public const string ExportCatechistEndpoint = ApiEndpoint + "/catechists/export";
        }
        public static class Level
        {
            /// <summary>"api/v1/rooms"</summary>
            public const string LevelsEndpoint = ApiEndpoint + "/levels";
            /// <summary>"api/v1/rooms/{id}"</summary>
            public const string LevelEndpoint = LevelsEndpoint + ByIdRoute;
            /// <summary>"api/v1/majors/{id}/levels/"</summary>
            public const string MajorOfLevelsEndpoint = LevelEndpoint + "/majors";
        }
        public static class CertificateOfCatechist
        {
            /// <summary>"api/v1/certificate-of-catechists"</summary>
            public const string CertificateOfCatechistsEndpoint = ApiEndpoint + "/certificate-of-catechists";
            /// <summary>"api/v1/certificate-of-catechists"</summary>
            public const string CertificateOfCatechistEndpoint = CertificateOfCatechistsEndpoint + ByIdRoute;
        }
        public static class Grade
        {
            /// <summary>"api/v1/grades"</summary>
            public const string GradesEndpoint = ApiEndpoint + "/grades";
            /// <summary>"api/v1/grades/{id}"</summary>
            public const string GradeEndpoint = GradesEndpoint + ByIdRoute;
            /// <summary>"api/v1/grades/{id}/catechists"</summary>
            public const string CatechistsInGradeEndpoint = GradeEndpoint + "/catechists";
            /// <summary>"api/v1/grades/{id}/classes"</summary>
            public const string ClassesByGradeEndpoint = GradeEndpoint + "/classes";
        }
        public static class Class
        {
            /// <summary>"api/v1/classes"</summary>
            public const string ClassesEndpoint = ApiEndpoint + "/classes";
            /// <summary>"api/v1/classes/{id}"</summary>
            public const string ClassEndpoint = ClassesEndpoint + ByIdRoute;
            /// <summary>"api/v1/classes/{id}/catechists"</summary>
            public const string CatechistInClassesEndpoint = ClassEndpoint + "/catechists";
            /// <summary>"api/v1/classes/{id}/slots"</summary>
            public const string SlotsOfClassEndpoint = ClassEndpoint + "/slots";

            public const string RoomOfClassEndpoint = ClassEndpoint + "/room";
        }
        public static class CatechistInGrade
        {
            /// <summary>"api/v1/catechist-in-grades"</summary>
            public const string CatechistInGradesEndpoint = ApiEndpoint + "/catechist-in-grades";
        }
        public static class CatechistInClass
        {
            /// <summary>"api/v1/catechist-in-classes"</summary>
            public const string CatechistInClassesEndpoint = ApiEndpoint + "/catechist-in-classes";
            public const string CatechistInClassEndpoint = CatechistInClassesEndpoint + ByIdRoute;

        }
        public static class CatechistInTraining
        {
            /// <summary>"api/v1/catechist-in-trainings"</summary>
            public const string CatechistInTrainingsEndpoint = ApiEndpoint + "/catechist-in-trainings";
            /// <summary>"api/v1/catechist-in-trainings/{trainingListId}"</summary>
            public const string CatechistInTrainingEndpoint = CatechistInTrainingsEndpoint + "/{trainingListId}";

        }
        public static class TrainingList
        {
            /// <summary>"api/v1/training-lists"</summary>
            public const string TrainingListsEndpoint = ApiEndpoint + "/training-lists";
            /// <summary>"api/v1/training-list/{id}"</summary>
            public const string TrainingListEndpoint = TrainingListsEndpoint + ByIdRoute;
            public const string CatechistsInTrainingEndpoint = TrainingListEndpoint + "/catechists";
        }
        public static class Event
        {
            /// <summary>"api/v1/events"</summary>
            public const string EventsEndpoint = ApiEndpoint + "/events";
            /// <summary>"api/v1/events/{id}"</summary>
            public const string EventEndpoint = EventsEndpoint + ByIdRoute;
            /// <summary>"api/v1/events/{id}/members"</summary>
            public const string MemberInEventEndpoint = EventEndpoint + "/members";
            /// <summary>"api/v1/events/{id}/budget-transactions"</summary>
            public const string BudgetTransactionInEventEndpoint = EventEndpoint + "/budget-transactions";
            /// <summary>"api/v1/events/{id}/processes"</summary>
            public const string ProcessInEventEndpoint = EventEndpoint + "/processes";
            /// <summary>"api/v1/events/{id}/participants"</summary>
            public const string ParticipantInEventEndpoint = EventEndpoint + "/participants";

            public const string ParticipantInEventEndpointExport = ParticipantInEventEndpoint + "/export";
        }
        public static class ParticipantInEvent
        {
            /// <summary>"api/v1/participent-in-events"</summary>
            public const string ParticipantInEventsEndpoint = ApiEndpoint + "/participent-in-events";
            /// <summary>"api/v1/participent-in-events/{id}"</summary>
            public const string ParticipantInEventEndpoint = ParticipantInEventsEndpoint + ByIdRoute;
        }
        public static class RoleEvent
        {
            /// <summary>"api/v1/role-events"</summary>
            public const string RoleEventsEndpoint = ApiEndpoint + "/role-events";
            /// <summary>"api/v1/role-events/{id}"</summary>
            public const string RoleEventEndpoint = RoleEventsEndpoint + ByIdRoute;
        }
        public static class Member
        {
            /// <summary>"api/v1/members"</summary>
            public const string MembersEndpoint = ApiEndpoint + "/members";
            /// <summary>"api/v1/members/{eventId}"</summary>
            public const string MemberEndpoint = MembersEndpoint + "/{eventId}";
        }
        public static class BudgetTransaction
        {
            /// <summary>"api/v1/budget-transactions"</summary>
            public const string BudgetTransactionsEndpoint = ApiEndpoint + "/budget-transactions";
            /// <summary>"api/v1/budget-transactions/{id}"</summary>
            public const string BudgetTransactionEndpoint = BudgetTransactionsEndpoint + ByIdRoute;
        }
        public static class Process
        {
            /// <summary>"api/v1/processes"</summary>
            public const string ProcessesEndpoint = ApiEndpoint + "/processes";
            /// <summary>"api/v1/processes/{id}"</summary>
            public const string ProcessEndpoint = ProcessesEndpoint + ByIdRoute;
            /// <summary>"api/v1/processes/{id}/members"</summary>
            public const string MemberOfProcessEndpoint = ProcessEndpoint + "/members";
        }
        public static class MemberOfProcess
        {
            /// <summary>"api/v1/members"</summary>
            public const string MemberOfProcessesEndpoint = ApiEndpoint + "/members";
            /// <summary>"api/v1/members/{eventId}"</summary>
            public const string MemberOfProcessEndpoint = MemberOfProcessesEndpoint + "/{processId}";
        }
        public static class AbsenceRequest
        {
            public const string Endpoint = ApiEndpoint + "/absences";
            public const string Submit = Endpoint + "/submit";
            public const string AbsenceProcess = Endpoint + "/process";
            public const string AssignCatechist = Endpoint + "/assign";
        }
        public static class RecruiterInInterview
        {
            /// <summary>"api/v1/members"</summary>
            public const string RecruiterInInterviewsEndpoint = ApiEndpoint + "/recruiter-in-interviews";
            /// <summary>"api/v1/members/{eventId}"</summary>
            public const string RecruiterInInterviewEndpoint = RecruiterInInterviewsEndpoint + "/{interviewId}";
        }
        public static class EventCategory
        {
            /// <summary>"api/v1/event-categories"</summary>
            public const string EventCategoriesEndpoint = ApiEndpoint + "/event-categories";
            /// <summary>"api/v1/event-categories/{id}"</summary>
            public const string EventCategoryEndpoint = EventCategoriesEndpoint + ByIdRoute;
        }
    }
}
