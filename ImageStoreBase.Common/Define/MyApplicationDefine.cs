namespace ImageStoreBase.Common.Define
{
    public static class MyApplicationDefine
    {
        #region Role
        public static class Role
        {
            public const string Admin = "Admin";
            public const string User = "User";
            public const string Customer = "Customer";
        }
        #endregion

        #region Command
        public static class Command
        {
            public const string VIEW = "Xem";
            public const string CREATE = "Thêm";
            public const string UPDATE = "Sửa";
            public const string DELETE = "Xoá";
            public const string APPROVE = "Duyệt";
        }
        #endregion

        #region Function
        public static class Function
        {
            public const string DASHBOARD = "Trang chủ";
            public const string CONTENT = "Nội dung";
            public const string STATISTIC = "Thống kê";
            public const string SYSTEM = "Hệ thống";
            public const string SYSTEM_USER = "Người dùng";
            public const string SYSTEM_ROLE = "Nhóm quyền";
            public const string SYSTEM_FUNCTION = "Chức năng";
            public const string SYSTEM_PERMISSION = "Quyền hạn";
        }
        
        #endregion
    }
}
