namespace StoreOrder.WebApplication.Authorization
{
    public static class Permissions
    {
        public static class SysAdmin
        {
            public const string ImportLocation = "Permissions.SysAdmin.ImportData";
            public const string ViewLogs = "Permissions.SysAdmin.ViewLogs";
            public const string DeleteLogs = "Permissions.SysAdmin.DeleteLogs";
            public const string GetListStores = "Permission.SysAdmin.GetListStores";
            public const string GetListCategoryStore = "Permission.SysAdmin.CategoryStore";
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
            public const string GetMenuProduct = "Permission.AdminStore.GetMenuProduct";
            public const string GetListStoreOption = "Permission.AdminStore.Attribute";
            public const string CreateCategoryStore = "Permission.AdminStore.CreateCategoryStore";
            public const string UpdateCategoryStore = "Permission.AdminStore.UpdateCategoryStore";
            public const string DeleteCategoryStore = "Permission.AdminStore.DeleteCategoryStore";
            public const string GetListProduct = "Permission.AdminStore.GetListProduct";
            public const string CreateProduct = "Permission.AdminStore.CreateProduct";
            public const string UpdateProduct = "Permission.AdminStore.UpdateProduct";
            public const string DeleteProduct = "Permission.AdminStore.DeleteProduct";
            public const string GetListCatProduct = "Permission.AdminStore.GetListCatProduct";
            public const string CreateCatProduct = "Permission.AdminStore.CreateCatProduct";
            public const string UpdateCatProduct = "Permission.AdminStore.UpdateCatProduct";
            public const string DeleteCatProduct = "Permission.AdminStore.DeleteCatProduct";
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
