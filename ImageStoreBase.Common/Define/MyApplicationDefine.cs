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
            public const string VIEW = nameof(VIEW);
            public const string CREATE = nameof(CREATE);
            public const string UPDATE = nameof(UPDATE);
            public const string DELETE = nameof(DELETE);
            public const string APPROVE = nameof(APPROVE);
        }
        #endregion

        #region Function
        public static class Function
        {
            public const string DASHBOARD = nameof(DASHBOARD);
            public const string CONTENT = nameof(CONTENT);
            public const string STATISTIC = nameof(STATISTIC);
            public const string SYSTEM = nameof(SYSTEM);
            public const string SYSTEM_USER = nameof(SYSTEM_USER);
            public const string SYSTEM_ROLE = nameof(SYSTEM_ROLE);
            public const string SYSTEM_FUNCTION = nameof(SYSTEM_FUNCTION);
            public const string SYSTEM_PERMISSION = nameof(SYSTEM_PERMISSION);
        }
        #endregion
    }
}
