namespace CatechistHelper.Domain.Constants
{
    public static class MessageConstant
    {
        #region Template, Suffix, Prefix
        private const string CreateSuccessTemplate = "Tạo mới {0} thành công.";
        private const string UpdateSuccessTemplate = "Cập nhật {0} thành công.";
        private const string DeleteSuccessTemplate = "Xóa {0} thành công.";
        private const string CreateFailTemplate = "Tạo mới {0} thất bại.";
        private const string UpdateFailTemplate = "Cập nhật {0} thất bại.";
        private const string DeleteFailTemplate = "Xóa {0} thất bại.";
        private const string NotFoundTemplate = "{0} không có trong hệ thống.";
        private const string InvalidRoleTemplate = "{0} không phải là {1}.";
        private const string RequiredSuffix = " không được bỏ trống.";  
        #endregion
        public static class Common
        {
            public const string InvalidPhoneNumber = "Không đúng định dạng số điện thoại.";
            public const string InvalidEmail = "Không đúng định dạng email.";
            public const string DeleteFail = "Không thể xóa dữ liệu đang được sử dụng.";
            public const string RestrictedDateManagingCatechism = "Không thể cập nhật khi bắt đầu niên khóa mới.";
            public const string InvalidStartEndTimeError = "Thời gian bắt đầu phải nhỏ hơn thời gian kết thúc.";
            public const string NegativeNumberError = "Không phải là giá trị âm.";
            public const string PositiveNumberError = "Phải lớn hơn 0.";
        }
        public static class Account
        {
            #region Account Field
            private const string AccountMessage = "Tài khoản";
            private const string Email = "Email";
            private const string Password = "Mật khẩu";
            private const string FullName = "Họ và tên";
            private const string Gender = "Giới tính";
            private const string Phone = "Số điện thoại";
            #endregion
            public static class Require
            {
                public const string EmailRequired = Email + RequiredSuffix;
                public const string PasswordRequired = Password + RequiredSuffix;
                public const string FullNameRequired = FullName + RequiredSuffix;
                public const string GenderRequired = Gender + RequiredSuffix;
                public const string PhoneRequired = Phone + RequiredSuffix;
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
                public static string EmailExisted = Email + " đã tồn tại.";
            }
        }
        public static class Registration
        {
            #region Registration Field
            private const string RegistrationMessage = "Đơn đăng kí";
            private const string FullName = "Họ và tên";
            private const string Gender = "Giới tính";
            private const string Email = "Email";
            private const string Address = "Địa chỉ";
            private const string Phone = "Số điện thoại";
            #endregion
            public static class Require
            {
                public const string EmailRequired = Email + RequiredSuffix;
                public const string GenderRequired = Gender + RequiredSuffix;
                public const string FullNameRequired = FullName + RequiredSuffix;
                public const string AddresseRequired = Address + RequiredSuffix;
                public const string PhoneRequired = Phone + RequiredSuffix;
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
                public static string PasswordIncorrect = "Sai mật khẩu.";
                public static string InvalidAccount = "Tài khoản không hợp lệ.";
            }
        }
        public static class Interview
        {
            #region Interview Field
            private const string InterviewMessage = "Phỏng vấn";
            #endregion
            public static class Require
            {
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
                public static string NotFoundInterview = String.Format(NotFoundTemplate, InterviewMessage);
            }
        }
        public static class RegistrationProcess
        {
            #region RegistrationProcess Field
            private const string RegistrationProcessMessage = "Quy trình đơn đăng kí";
            private const string Name = "Tên";
            #endregion
            public static class Require
            {
                public const string NameRequired = Name + RequiredSuffix;
            }
            public static class Success
            {
                public static string CreateRegistrationProcess = String.Format(CreateSuccessTemplate, RegistrationProcessMessage);
                public static string UpdateRegistrationProcess = String.Format(UpdateSuccessTemplate, RegistrationProcessMessage);
            }
            public static class Fail
            {
                public static string CreateRegistrationProcess = String.Format(CreateFailTemplate, RegistrationProcessMessage);
                public static string UpdateRegistrationProcess = String.Format(UpdateFailTemplate, RegistrationProcessMessage);
                public static string DeleteRegistrationProcess = String.Format(DeleteFailTemplate, RegistrationProcessMessage);
                public static string NotFoundRegistrationProcess = String.Format(NotFoundTemplate, RegistrationProcessMessage);
            }
        }
        public static class Certificate
        {
            #region Certificate Field
            private const string CertificateMessage = "Chứng chỉ";
            private const string Name = "Tên chứng chỉ";

            #endregion
            public static class Require
            {
                public const string NameRequired = Name + RequiredSuffix;
            }
            public static class Success
            {
                public static string CreateCertificate = String.Format(CreateSuccessTemplate, CertificateMessage);
            }
            public static class Fail
            {
                public static string CreateCertificate = String.Format(CreateFailTemplate, CertificateMessage);
                public static string DeleteCertificate = String.Format(DeleteFailTemplate, CertificateMessage);
                public static string NotFoundCertificate = String.Format(NotFoundTemplate, CertificateMessage);
                public static string UnsuitableLevel = "Chứng chỉ này không áp dụng cho cấp độ hiện tại.";
            }
        }
        public static class PastoralYear
        {
            #region PastoralYear Field
            private const string PastoralYearMessage = "Niên khóa";
            private const string Name = "Tên niên khóa";
            #endregion
            public static class Require
            {
                public const string NameRequired = Name + RequiredSuffix;
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
                public static string NotFoundPastoralYear = String.Format(NotFoundTemplate, PastoralYearMessage);
                public static string ClassNotFinish = "Không thể hoàn tất niên khóa vì vẫn còn lớp học đang hoạt động. Vui lòng kết thúc tất cả các lớp học trước khi cập nhật trạng thái.";
            }
        }
        public static class ChristianName
        {
            #region ChristianName Field
            private const string ChristianNameMessage = "Tên thánh";
            private const string Name = "Tên";
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
                public static string NotFoundChristianName = String.Format(NotFoundTemplate, ChristianNameMessage);
            }
        }
        public static class PostCategory
        {
            #region PostCategory Field
            private const string PostCategoryMessage = "Danh mục tin";
            private const string Name = "Tên danh mục";
            private const string Description = "Mô tả";

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
                public static string NotFoundPostCategory = String.Format(NotFoundTemplate, PostCategoryMessage);

            }
        }
        public static class Post
        {
            #region Post Field
            private const string PostMessage = "Tin";
            private const string Title = "Tiêu đề";
            private const string Content = "Nội dung";
            private const string Module = "Loại";

            #endregion
            public static class Require
            {
                public const string TitleRequired = Title + RequiredSuffix;
                public const string ContentRequired = Content + RequiredSuffix;
                public const string ModuleRequired = Module + RequiredSuffix;
            }
            public static class Success
            {
                public static string CreatePost = String.Format(CreateSuccessTemplate, PostMessage);
                public static string UpdatePost = String.Format(UpdateSuccessTemplate, PostMessage);
                public static string DeletePost = String.Format(UpdateSuccessTemplate, PostMessage);

            }
            public static class Fail
            {
                public static string CreatePost = String.Format(CreateFailTemplate, PostMessage);
                public static string UpdatePost = String.Format(UpdateFailTemplate, PostMessage);
                public static string DeletePost = String.Format(DeleteFailTemplate, PostMessage);
            }
        }
        public static class Role
        {
            #region Role Field
            private const string RoleMessage = "Vai trò";
            #endregion
            public static class Fail
            {
                public static string NotFoundRole = String.Format(NotFoundTemplate, RoleMessage);
            }
        }
        public static class Major
        {
            #region Major Field
            private const string MajorMessage = "Ngành";
            private const string Name = "Tên ngành";
            #endregion
            public static class Require
            {
                public const string NameRequired = Name + RequiredSuffix;
            }
            public static class Success
            {
                public static string CreateMajor = String.Format(CreateSuccessTemplate, MajorMessage);
                public static string UpdateMajor = String.Format(UpdateSuccessTemplate, MajorMessage);
                public static string DeleteMajor = String.Format(UpdateSuccessTemplate, MajorMessage);

            }
            public static class Fail
            {
                public static string CreateMajor = String.Format(CreateFailTemplate, MajorMessage);
                public static string UpdateMajor = String.Format(UpdateFailTemplate, MajorMessage);
                public static string DeleteMajor = String.Format(DeleteFailTemplate, MajorMessage);
                public static string NotFoundMajor = String.Format(NotFoundTemplate, MajorMessage);
            }
        }
        public static class Catechist
        {
            #region Catechist Field
            private const string CatechistMessage = "Giáo lý viên";
            private const string FullName = "Họ và tên";
            private const string Gender = "Giới tính";
            #endregion
            public static class Require
            {
                public const string FullNameRequired = FullName + RequiredSuffix;
                public const string GenderRequired = Gender + RequiredSuffix;
            }
            public static class Success
            {
                public static string CreateCatechist = String.Format(CreateSuccessTemplate, CatechistMessage);
                public static string UpdateCatechist = String.Format(UpdateSuccessTemplate, CatechistMessage);
                public static string DeleteCatechist = String.Format(DeleteSuccessTemplate, CatechistMessage);
            }
            public static class Fail
            {
                public static string CreateCatechist = String.Format(CreateFailTemplate, CatechistMessage);
                public static string UpdateCatechist = String.Format(UpdateFailTemplate, CatechistMessage);
                public static string DeleteCatechist = String.Format(DeleteFailTemplate, CatechistMessage);
                public static string NotFoundCatechist = String.Format(NotFoundTemplate, CatechistMessage);
                public static string UnqualifiedCatechist = "Giáo lý viên không thuộc khối ngành.";

            }
        }
        public static class SystemConfiguration
        {
            #region System Configuration Field
            private const string SystemConfigurationMessage = "Cấu hình hệ thống";
            private const string Key = "Khóa";
            private const string Value = "Giá trị";
            #endregion
            public static class Require
            {
                public const string KeyRequired = Key + RequiredSuffix;
                public const string ValueRequired = Value + RequiredSuffix;
            }
            public static class Success
            {
                public static string CreateSystemConfiguration = String.Format(CreateSuccessTemplate, SystemConfigurationMessage);
                public static string UpdateSystemConfiguration = String.Format(UpdateSuccessTemplate, SystemConfigurationMessage);
                public static string DeleteSystemConfiguration = String.Format(UpdateSuccessTemplate, SystemConfigurationMessage);
            }
            public static class Fail
            {
                public static string CreateSystemConfiguration = String.Format(CreateFailTemplate, SystemConfigurationMessage);
                public static string UpdateSystemConfiguration = String.Format(UpdateFailTemplate, SystemConfigurationMessage);
                public static string DeleteSystemConfiguration = String.Format(DeleteFailTemplate, SystemConfigurationMessage);
                public static string NotFoundSystemConfiguration = String.Format(NotFoundTemplate, SystemConfigurationMessage);
            }
        }
        public static class Room
        {
            #region Room Field
            private const string RoomMessage = "Phòng";
            private const string Name = "Tên phòng";
            #endregion
            public static class Require
            {
                public const string NameRequired = Name + RequiredSuffix;
            }
            public static class Success
            {
                public static string CreateRoom = String.Format(CreateSuccessTemplate, RoomMessage);
                public static string UpdateRoom = String.Format(UpdateSuccessTemplate, RoomMessage);
                public static string DeleteRoom = String.Format(UpdateSuccessTemplate, RoomMessage);
            }
            public static class Fail
            {
                public static string CreateRoom = String.Format(CreateFailTemplate, RoomMessage);
                public static string UpdateRoom = String.Format(UpdateFailTemplate, RoomMessage);
                public static string DeleteRoom = String.Format(DeleteFailTemplate, RoomMessage);
                public static string NameAlreadyUsed = "Tên đã tồn tại.";
            }
        }
        public static class Level
        {
            #region
            private const string LevelMessage = "Cấp độ";
            private const string Name = "Tên";
            #endregion
            public static class Require
            {
                public const string NameRequired = Name + RequiredSuffix;
            }
            public static class Success
            {
                public static string CreateLevel = String.Format(CreateSuccessTemplate, LevelMessage);
                public static string UpdateLevel = String.Format(UpdateSuccessTemplate, LevelMessage);
                public static string DeleteLevel = String.Format(DeleteSuccessTemplate, LevelMessage);
            }
            public static class Fail
            {
                public static string CreateLevel = String.Format(CreateFailTemplate, LevelMessage);
                public static string UpdateLevel = String.Format(UpdateFailTemplate, LevelMessage);
                public static string DeleteLevel = String.Format(DeleteFailTemplate, LevelMessage);
                public static string NotFoundLevel = String.Format(NotFoundTemplate, LevelMessage);        
            }
        }
        public static class Grade
        {
            #region
            private const string GradeMessage = "Khối";
            private const string Name = "Tên khối";
            #endregion
            public static class Require
            {
                public const string NameRequired = Name + RequiredSuffix;
            }
            public static class Success
            {
                public static string CreateGrade = String.Format(CreateSuccessTemplate, GradeMessage);
                public static string DeleteGrade = String.Format(DeleteSuccessTemplate, GradeMessage);
            }
            public static class Fail
            {
                public static string CreateGrade = String.Format(CreateFailTemplate, GradeMessage);
                public static string DeleteGrade = String.Format(DeleteFailTemplate, GradeMessage);
                public static string NotFoundGrade = String.Format(NotFoundTemplate, GradeMessage);

            }
        }
        public static class Class
        {
            #region Class Field
            private const string ClassMessage = "Lớp học";
            #endregion
            public static class Success
            {
                public static string CreateClass = String.Format(CreateSuccessTemplate, ClassMessage);
                public static string UpdateClass = String.Format(UpdateSuccessTemplate, ClassMessage);
                public static string DeleteClass = String.Format(DeleteSuccessTemplate, ClassMessage);
            }
            public static class Fail
            {
                public static string CreateClass = String.Format(CreateFailTemplate, ClassMessage);
                public static string UpdateClass = String.Format(UpdateFailTemplate, ClassMessage);
                public static string NotFoundClass = String.Format(NotFoundTemplate, ClassMessage);
            }
        }
        public static class CatechistInClass
        {
            #region CatechistInClass Field
            private const string CatechistInClassMessage = "CatechistInClass";
            #endregion
            public static class Success
            {
                public static string CreateCatechistInClass = String.Format(CreateSuccessTemplate, CatechistInClassMessage);
                public static string UpdateCatechistInClass = String.Format(UpdateSuccessTemplate, CatechistInClassMessage);
                public static string DeleteCatechistInClass = String.Format(DeleteSuccessTemplate, CatechistInClassMessage);
            }
            public static class Fail
            {
                public static string CreateCatechistInClass = String.Format(CreateFailTemplate, CatechistInClassMessage);
                public static string UpdateCatechistInClass = String.Format(UpdateFailTemplate, CatechistInClassMessage);
                public static string NotFoundCatechistInClass = String.Format(NotFoundTemplate, CatechistInClassMessage);
            }
        }


        public static class CatechistInSlot
        {
            #region CatechistInClass Field
            private const string CatechistInSlotMessage = "CatechistInSlot";
            #endregion
            public static class Success
            {
                public static string CreateCatechistInSlot = String.Format(CreateSuccessTemplate, CatechistInSlotMessage);
                public static string UpdateCatechistInSlot = String.Format(UpdateSuccessTemplate, CatechistInSlotMessage);
                public static string DeleteCatechistInSlot = String.Format(DeleteSuccessTemplate, CatechistInSlotMessage);
            }
            public static class Fail
            {
                public static string CreateCatechistInSlot = String.Format(CreateFailTemplate, CatechistInSlotMessage);
                public static string UpdateCatechistInSlot = String.Format(UpdateFailTemplate, CatechistInSlotMessage);
                public static string NotFoundCatechistInSlot = String.Format(NotFoundTemplate, CatechistInSlotMessage);
            }
        }

        public static class TrainingList
        {
            #region TrainingList Field
            private const string TrainingListMessage = "Danh sách đào tạo";
            private const string Name = "Tên";
            private const string Description = "Mô tả";
            #endregion
            public static class Require
            {
                public const string NameRequired = Name + RequiredSuffix;
                public const string DescriptionRequired = Description + RequiredSuffix;
            }
            public static class Success
            {
                public static string CreateTrainingList = String.Format(CreateSuccessTemplate, TrainingListMessage);
                public static string UpdateTrainingList = String.Format(UpdateSuccessTemplate, TrainingListMessage);
                public static string DeleteTrainingList = String.Format(DeleteSuccessTemplate, TrainingListMessage);
            }
            public static class Fail
            {
                public static string CreateTrainingList = String.Format(CreateFailTemplate, TrainingListMessage);
                public static string UpdateTrainingList = String.Format(UpdateFailTemplate, TrainingListMessage);
                public static string DeleteTrainingList = String.Format(DeleteFailTemplate, TrainingListMessage);
                public static string NotFoundTrainingList = String.Format(NotFoundTemplate, TrainingListMessage);
                public static string InvalidLevel = "Cấp tiếp theo không hợp lệ. Vui lòng chọn một cấp cao hơn cấp hiện tại.";
                public static string InvalidNextLevel = "Cấp mới chỉ được phép lớn hơn cấp hiện tại 1 cấp.";
                public static string NotFinished = "Khóa đào tạo vẫn đang diễn ra.";
            }
        }
        public static class Event
        {
            #region
            private const string Message = "Sự kiện";
            private const string Name = "Tên sự kiện";
            private const string Description = "Mô tả";
            #endregion
            public static class Require
            {
                public const string NameRequired = Name + RequiredSuffix;
                public const string DescriptionRequired = Description + RequiredSuffix;
            }
            public static class Success
            {
                public static string Create = String.Format(CreateSuccessTemplate, Message);
                public static string Delete = String.Format(DeleteSuccessTemplate, Message);
            }
            public static class Fail
            {
                public static string Create = String.Format(CreateFailTemplate, Message);
                public static string Update = String.Format(UpdateFailTemplate, Message);
                public static string Delete = String.Format(DeleteFailTemplate, Message);
                public static string NotFound = String.Format(NotFoundTemplate, Message);

            }
        }
        public static class ParticipantInEvent
        {
            #region
            private const string Message = "Người tham gia sự kiện";
            private const string FullName = "Họ và tên";
            private const string Email = "Email";
            private const string Phone = "Số điện thoại";
            private const string Gender = "Giới tính";
            private const string Address = "Địa chỉ";
            #endregion
            public static class Require
            {
                public const string FullNameRequired = FullName + RequiredSuffix;
                public const string EmailRequired = Email + RequiredSuffix;
                public const string PhoneRequired = Phone + RequiredSuffix;
                public const string GenderRequired = Gender + RequiredSuffix;
                public const string AddressRequired = Address + RequiredSuffix;
            }
            public static class Success
            {
                public static string Create = String.Format(CreateSuccessTemplate, Message);
                public static string Delete = String.Format(DeleteSuccessTemplate, Message);
            }
            public static class Fail
            {
                public static string Create = String.Format(CreateFailTemplate, Message);
                public static string Update = String.Format(UpdateFailTemplate, Message);
                public static string Delete = String.Format(DeleteFailTemplate, Message);
                public static string NotFound = String.Format(NotFoundTemplate, Message);

            }
        }
        public static class RoleEvent
        {
            #region
            private const string Message = "Vai trò trong sự kiện";
            private const string Name = "Tên vai trò";
            #endregion
            public static class Require
            {
                public const string NameRequired = Name + RequiredSuffix;
            }
            public static class Success
            {
                public static string Create = String.Format(CreateSuccessTemplate, Message);
                public static string Delete = String.Format(DeleteSuccessTemplate, Message);
            }
            public static class Fail
            {
                public static string Create = String.Format(CreateFailTemplate, Message);
                public static string Update = String.Format(UpdateFailTemplate, Message);
                public static string Delete = String.Format(DeleteFailTemplate, Message);
                public static string NotFound = String.Format(NotFoundTemplate, Message);

            }
        }
        public static class BudgetTransaction
        {
            #region
            private const string Message = "Ngân sách sự kiện";
            #endregion
            public static class Success
            {
                public static string Create = String.Format(CreateSuccessTemplate, Message);
                public static string Delete = String.Format(DeleteSuccessTemplate, Message);
            }
            public static class Fail
            {
                public static string Create = String.Format(CreateFailTemplate, Message);
                public static string Update = String.Format(UpdateFailTemplate, Message);
                public static string Delete = String.Format(DeleteFailTemplate, Message);
                public static string NotFound = String.Format(NotFoundTemplate, Message);

            }
        }
        public static class Process
        {
            #region
            private const string Message = "Tiến trình sự kiện";
            private const string Name = "Tên tiến trình";
            private const string Description = "Mô tả";
            #endregion
            public static class Require
            {
                public const string NameRequired = Name + RequiredSuffix;
                public const string DescriptionRequired = Description + RequiredSuffix;
            }
            public static class Success
            {
                public static string Create = String.Format(CreateSuccessTemplate, Message);
                public static string Delete = String.Format(DeleteSuccessTemplate, Message);
            }
            public static class Fail
            {
                public static string Create = String.Format(CreateFailTemplate, Message);
                public static string Update = String.Format(UpdateFailTemplate, Message);
                public static string Delete = String.Format(DeleteFailTemplate, Message);
                public static string NotFound = String.Format(NotFoundTemplate, Message);

            }
        }
        public static class AbsentRequest
        {
            #region
            private const string Message = "Đơn xin nghỉ";
            private const string Name = "Tên đơn";
            private const string Description = "Mô tả";
            #endregion
            public static class Require
            {
                public const string NameRequired = Name + RequiredSuffix;
                public const string DescriptionRequired = Description + RequiredSuffix;
            }
            public static class Success
            {
                public static string Create = String.Format(CreateSuccessTemplate, Message);
                public static string Delete = String.Format(DeleteSuccessTemplate, Message);
            }
            public static class Fail
            {
                public static string Create = String.Format(CreateFailTemplate, Message);
                public static string Update = String.Format(UpdateFailTemplate, Message);
                public static string Delete = String.Format(DeleteFailTemplate, Message);
                public static string NotFound = String.Format(NotFoundTemplate, Message);
                public static string NotApproved = "Đơn xin nghỉ không được chấp thuận.";
                public static string NotValid = "Chỉ được nộp đơn trước {0} ngày.";
            }
        }
        public static class EventCategory
        {
            #region EventCategory Field
            private const string EventCategoryMessage = "Danh mục sự kiện";
            private const string Name = "Tên danh mục sự kiện";

            #endregion
            public static class Require
            {
                public const string NameRequired = Name + RequiredSuffix;
            }
            public static class Success
            {
                public static string CreateEventCategory = String.Format(CreateSuccessTemplate, EventCategoryMessage);
                public static string UpdateEventCategory = String.Format(UpdateSuccessTemplate, EventCategoryMessage);
                public static string DeleteEventCategory = String.Format(UpdateSuccessTemplate, EventCategoryMessage);

            }
            public static class Fail
            {
                public static string CreateEventCategory = String.Format(CreateFailTemplate, EventCategoryMessage);
                public static string UpdateEventCategory = String.Format(UpdateFailTemplate, EventCategoryMessage);
                public static string DeleteEventCategory = String.Format(DeleteFailTemplate, EventCategoryMessage);
                public static string NotFoundEventCategory = String.Format(NotFoundTemplate, EventCategoryMessage);

            }
        }
    }
}
