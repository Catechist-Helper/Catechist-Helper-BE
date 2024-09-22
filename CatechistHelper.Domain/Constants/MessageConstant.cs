using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            #region User Field
            private const string UserMessage = "người dùng";
            private const string CenterMessage = "Trung tâm";
            private const string UserName = "Tên đăng nhập";
            private const string Password = "Mật khẩu";
            private const string FullName = "Tên người dùng";
            private const string Email = "email";
            private const string Description = "Mô tả";
            private const string UserStatus = "Trạng thái";
            private const string Role = "Chức vụ";
            #endregion
            public static class Require
            {
                public const string UserNameRequired = UserName + RequiredSuffix;
                public const string PasswordRequired = Password + RequiredSuffix;
                public const string FullNameRequired = FullName + RequiredSuffix;
                public const string EmailRequired = Email + RequiredSuffix;
                public const string DescriptionRequired = Description + RequiredSuffix;
                public const string StatusRequired = UserStatus + RequiredSuffix;
                public const string RoleRequired = Role + RequiredSuffix;
            }
            public static class Success
            {
                public static string CreateUser = String.Format(CreateSuccessTemplate, UserMessage);
                public static string UpdateUser = String.Format(UpdateSuccessTemplate, UserMessage);
                public static string DeleteUser = String.Format(DeleteSuccessTemplate, UserMessage);
            }
            public static class Fail
            {
                public static string CreateUser = String.Format(CreateFailTemplate, UserMessage);
                public static string UpdateUser = String.Format(UpdateFailTemplate, UserMessage);
                public static string DeleteUser = String.Format(DeleteFailTemplate, UserMessage);
                public static string NotFoundCenter = String.Format(NotFoundTemplate, CenterMessage);
                public static string NotFoundUser = String.Format(NotFoundTemplate, UserMessage);
                public static string UserNameExisted = UserName + " đã tồn tại !!!";
            }
        }
    }
}
