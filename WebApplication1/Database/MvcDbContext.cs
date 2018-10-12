namespace WebApplication1.Database
{
    using System.Data.Entity;
    using WebApplication1.Models;

    public class MvcDbContext : DbContext
    {
        // Your context has been configured to use a 'MvcDbContext' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'WebApplication1.Database.MvcDbContext' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'MvcDbContext' 
        // connection string in the application configuration file.
        public MvcDbContext()
            : base("name=MvcDbContext")
        {
        }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        // public virtual DbSet<MyEntity> MyEntities { get; set; }
        public DbSet<UserModel> Users { get; set; }
        public DbSet<RoleModel> Roles { get; set; }

       // public System.Data.Entity.DbSet<WebApplication1.Models.RegisterViewModel> RegisterViewModels { get; set; }
    }

    //public class MyEntity
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}
}