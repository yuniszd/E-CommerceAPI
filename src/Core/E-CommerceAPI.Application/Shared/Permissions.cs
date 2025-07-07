namespace E_CommerceAPI.Application.Shared;

public static class Permissions
{
    public static class Products
    {
        public const string GetMy = "Permissions.Products.GetMy";
        public const string Create = "Permissions.Products.Create";
        public const string Update = "Permissions.Products.Update";
        public const string Delete = "Permissions.Products.Delete";
        public const string DeleteProductImage = "Permissions.Products.DeleteProductImage";
        public const string AddProductImage = "Permissions.Products.AddProductImage";

        public static List<string> All => new()
        {
            GetMy,
            Create,
            Update,
            Delete,
            DeleteProductImage,
            AddProductImage
        };
    }

    public static class Categories
    {
        public const string MainCreate = "Permissions.Categories.MainCreate";
        public const string SubCreate = "Permissions.Categories.SubCreate";
        public const string GetAll = "Permissions.Categories.GetAll";
        public const string Update = "Permissions.Categories.Update";
        public const string Delete = "Permissions.Categories.Delete";

        public static List<string> All => new()
        {
            MainCreate,
            SubCreate,
            GetAll,
            Update,
            Delete
        };
    }

    public static class Orders
    {
        public const string GetAll = "Permissions.Orders.GetAll";
        public const string Create = "Permissions.Orders.Create";
        public const string Update = "Permissions.Orders.Update";
        public const string Delete = "Permissions.Orders.Delete";
        public const string GetMy = "Permissions.Orders.GetMy";
        public const string GetDetails = "Permissions.Orders.GetDetails";
        public const string GetMySales = "Permissions.Orders.GetMySales";

        public static List<string> All => new()
        {
            GetAll,
            Create,
            Update,
            Delete,
            GetMy,
            GetDetails,
            GetMySales
        };
    }

    public static class Role
    {
        public const string GetAllPermissions = "Permissions.Users.GetAllPermissions";
        public const string Create = "Permissions.Users.Create";
        public const string Update = "Permissions.Users.Update";
        public const string Delete = "Permissions.Users.Delete";


        public static List<string> All => new()
        {
            GetAllPermissions,
            Create,
            Update,
            Delete
        };
    }

    public static class Favourites
    {
        public const string View = "Permissions.Favourites.View";
        public const string Create = "Permissions.Favourites.Create";
        public const string Delete = "Permissions.Favourites.Delete";

        public static List<string> All => new()
        {
            View,
            Create,
            Delete
        };
    }

    public static class Accounts
    {
        public const string AddRole = "Permissions.Favourites.AddRole";
        public const string Create = "Permissions.Favourites.Create";
        public const string Delete = "Permissions.Favourites.Delete";

        public static List<string> All => new()
        {
            AddRole,
            Create,
            Delete
        };
    }

    public static class Users
    {
        public const string PasswordReset = "Permissions.Favourites.PasswordReset";
        public const string Create = "Permissions.Favourites.Create";

        public static List<string> All => new()
        {
            PasswordReset,
            Create
        };
    }
}