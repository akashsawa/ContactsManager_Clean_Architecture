using ContactsManager.Core.Domain.IdentityEntities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    //for identity
    public class ApplicationDBContext :
        IdentityDbContext<ApplicationUser, ApplicationRole, Guid> //DbContext // dbcontext doesnt offer predefined dbsets but IdentityDbContext offers predefined dbsets like for users, roles.
                                                                     //for identity
    {
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<Person> Persons { get; set; }

        public ApplicationDBContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Country>().ToTable("Countries");
            modelBuilder.Entity<Person>().ToTable("Persons");

            //Seed to Countries
            string countriesJson = System.IO.File.ReadAllText("countries.json");
            List<Country> countries = System.Text.Json.JsonSerializer.Deserialize<List<Country>>(countriesJson);

            foreach (Country country in countries)
                modelBuilder.Entity<Country>().HasData(country);


            //Seed to Persons
            string personsJson = System.IO.File.ReadAllText("persons.json");
            List<Person> persons = System.Text.Json.JsonSerializer.Deserialize<List<Person>>(personsJson);

            foreach (Person person in persons)
                modelBuilder.Entity<Person>().HasData(person);

            modelBuilder.Entity<Person>().Property(temp => temp.TIN).HasColumnName("TaxIdentificationNumbers").HasColumnType("varchar(8)").HasDefaultValue("ABC12345");









            //modelBuilder.Entity<Person>().HasIndex(temp => temp.TIN).IsUnique(); // for adding the unique con.straint on TIN column

            modelBuilder.Entity<Person>().HasCheckConstraint("CHK_TIN", "len([TaxIdentificationNumbers]) = 8");

            //table ef relations: 
            modelBuilder.Entity<Person>(entity =>
            {
                entity.HasOne<Country>(c => c.Country).WithMany(p => p.Persons).HasForeignKey(p => p.CountryId);
            });

        }

        public List<Person> sp_GetAllPersons()
        {
            return Persons.FromSqlRaw("EXECUTE [dbo].[GetAllPersons]").ToList(); // RESULTset will be converted into of type iqueryable class
        }

        public void sp_DeleteAllPersons()
        {

            Database.ExecuteSqlRaw("EXECUTE [dbo].[DELETEALLPERSONS]", null); // RESULTset will be converted into of type iqueryable class
        }

        public int sp_InsertPersons(Person person)
        {
            //@PersonID, @PersonName, @Email, @DateOfBirth, @Gender, @CountryId, @Address, @ReceiveNewsLetters 
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@PersonID", person.Personid),
                new SqlParameter("@PersonName", person.PersonName),
                new SqlParameter("@Email", person.Email),
                new SqlParameter("@DateOfBirth", person.DateOfbirth),
                new SqlParameter("@Gender", person.Gender),
                new SqlParameter("@CountryId", person.CountryId),
                new SqlParameter("@Address", person.Address),
                new SqlParameter("@ReceiveNewsLetters", person.ReceiveNewsLetters),
            };

            return Database.ExecuteSqlRaw("EXECUTE [dbo].[InsertPerson] @PersonID, @PersonName, @Email, @DateOfBirth, @Gender, @CountryId, @Address, @ReceiveNewsLetters  ", parameters); // RESULTset will be converted into of type iqueryable class
        }

    }
}
