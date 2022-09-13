using GraphQLStack.Domain;
using Microsoft.EntityFrameworkCore;

namespace GraphQLStack
{
    public class Context : DbContext
    {
        public DbSet<Rocket>? Rockets { get; set; }
        public DbSet<Engine>? Engines { get; set; }

        public DbSet<Employee>? Employees { get; set; }

        public DbSet<AppUser> Users { get; set; }

        public Context(DbContextOptions options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Rocket>().HasKey(x => x.Id);
            modelBuilder.Entity<Engine>().HasKey(x => x.Id);
            modelBuilder.Entity<Employee>().HasKey(x => x.Id);

            modelBuilder.Entity<AppUser>().HasKey(x => x.Id);


            modelBuilder.Entity<Rocket>().HasData(new Rocket { Id = 1, Name = "Falcon 9" });
            modelBuilder.Entity<Rocket>().HasData(new Rocket { Id = 2, Name = "Eagle 10" });

            modelBuilder.Entity<AppUser>().HasData(new AppUser { Id = 2, Email = "test", Password="test" });

            modelBuilder.Entity<Engine>().HasData(
                new Engine { Id = 1, Name = "Merlin 1D+", Isp = 310, RocketId = 1, Thrust = 420, Type = EngineType.SeaLevel },
                new Engine { Id = 2, Name = "Merlin 1D+", Isp = 348, RocketId = 1, Thrust = 225, Type = EngineType.Vacuum }
                );

            modelBuilder.Entity<Employee>().HasData(
                    new Employee { Id = 1, FirstName = "Boss", LastName="Boss" ,Address="KA", SupervisorId=0},
                    new Employee { Id = 2, FirstName = "Emp1", LastName = "Test1", Address = "KR" , SupervisorId=1},
                    new Employee { Id = 3, FirstName = "Emp2", LastName = "Test2", Address = "KA" , SupervisorId=1 },
                    new Employee { Id = 4, FirstName = "Emp3", LastName = "Test3", Address = "KA", SupervisorId = 2 },
                    new Employee { Id = 5, FirstName = "Emp4", LastName = "Test4", Address = "KA", SupervisorId = 2 }
                );
        }
    }
}