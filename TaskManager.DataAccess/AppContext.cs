namespace TaskManager.DataAccess
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using TaskManager.Models;

    public class AppContext : DbContext
    {
        public AppContext()
            : base("name=AppContext")
        {
           // Database.SetInitializer(new DataInitializer());
        }

        public virtual DbSet<Job> Jobs { get; set; }
    }
}