namespace StoreOrder.WebApplication.Authorization
{
    public static class Permissions
    {
        public static class SysAdmin
        {
            public const string ImportLocation = "Permissions.SysAdmin.ImportData";
            public const string ViewLogs = "Permissions.SysAdmin.ViewLogs";
            public const string DeleteLogs = "Permissions.SysAdmin.DeleteLogs";
        }
        public static class Account
        {
            public const string View = "Permissions.Account.View";
            public const string Create = "Permissions.Account.Create";
            public const string Edit = "Permissions.Account.Edit";
            public const string Delete = "Permissions.Account.Delete";
        }

        public static class Users
        {
            public const string View = "Permissions.Users.View";
            public const string Create = "Permissions.Users.Create";
            public const string Edit = "Permissions.Users.Edit";
            public const string Delete = "Permissions.Users.Delete";
        }

        public static class AdminStore
        {
            public const string GetListStores = "Permission.AdminStore.GetListStores";
            public const string GetMenuProduct = "Permission.AdminStore.GetMenuProduct";
            public const string GetListStoreOption = "Permission.AdminStore.Attribute";
            public const string GetListCategoryStore = "Permission.AdminStore.Category";
            public const string GetListProduct = "Permission.AdminStore.GetListProduct";
            public const string CreateProduct = "Permission.AdminStore.CreateProduct";
            public const string UpdateProduct = "Permission.AdminStore.UpdateProduct";
            public const string DeleteProduct = "Permission.AdminStore.DeleteProduct";
        }

        public static class Employee
        {
            public const string GetListTables = "Permission.Employee.GetListTables";
            public const string GetCategoryProductsForEmployee = "Permission.Employee.GetCategoryProductsForEmployee";
            public const string GetListProductsOfStoreForEmployee = "Permission.Employee.GetListProductsOfStoreForEmployee";
            public const string GetListProductsOfStoreWithCategoryForEmployee = "Permission.Employee.GetListProductsOfStoreWithCategoryForEmployee";
            public const string GetListProductsOfStoreForEmployeeV2 = "Permission.Employee.GetListProductsOfStoreForEmployeeV2";
            public const string GetListProductsOfStoreWithCategoryForEmployeeV2 = "Permission.Employee.GetListProductsOfStoreWithCategoryForEmployeeV2";
            public const string CreateOrder = "Permission.Employee.CreateOrder";
            public const string UpdateOrder = "Permission.Employee.UpdateOrder";
            public const string GetOrderProductsWithTable = "Permission.Employee.GetOrderProductsWithTable";
            public const string GetListOrderWithTableOrProductName = "Permission.Employee.GetListOrderWithTableOrProductName";
            public const string GetOrderWithTableViewProductsWithTableId = "Permission.Employee.GetOrderWithTableViewProductsWithTableId";
            public const string ConfirmOrder = "Permission.Employee.ConfirmOrder";
            public const string ConfirmPayOrder = "Permission.Employee.ConfirmPayOrder";
        }

        public static class Upload
        {
            public const string UploadFile = "Permission.Upload.UploadFile";
        }
    }
}
