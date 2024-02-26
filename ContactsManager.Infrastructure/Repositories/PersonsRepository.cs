using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using RepositoryContracts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Microsoft.Extensions.Logging;

namespace Repositories
{
    public class PersonsRepository : IPersonsRepository
    {
        private readonly ApplicationDBContext _db;

        //ilogger
        private readonly ILogger<PersonsRepository> _logger;
        //ilogger

        public PersonsRepository(ApplicationDBContext _dbContext, ILogger<PersonsRepository> logger)
        {
            _db = _dbContext;
            _logger = logger;

        }

        public async Task<Person> AddPerson(Person person)
        {
            _db.Persons.Add(person);
            await _db.SaveChangesAsync();
            return person;
        }

        public async Task<bool> DeletePersonByPersonID(Guid personID)
        {
            _db.Persons.RemoveRange(_db.Persons.Where(temp => temp.Personid == personID));
            int rowsDeleted = await _db.SaveChangesAsync();

            return rowsDeleted > 0;
        }

        public async Task<List<Person>> GetAllPersons()
        {
            //ilogger
            _logger.LogInformation("GetAllPersons of personsrepository");
            //ilogger

            return await _db.Persons.Include("Country").ToListAsync();
        }

        public async Task<List<Person>> GetFilteredPersons(Expression<Func<Person, bool>> predicate)
        {
            //ilogger
            _logger.LogInformation("GetFilteredPersons of personsrepository");
            //ilogger

            return await _db.Persons.Include("Country").Where(predicate).ToListAsync();
        }

        public async Task<Person?> GetPersonByPersonID(Guid personID)
        {
            return await _db.Persons.Include("Country").FirstOrDefaultAsync(temp => temp.Personid == personID);
        }

        public async Task<Person> UpdatePerson(Person person)
        {
            Person matchingPerson = await _db.Persons.FirstAsync(temp => temp.Personid == person.Personid);
            if (matchingPerson == null)
                return person;

            matchingPerson.PersonName = person.PersonName;
            matchingPerson.Email = person.Email;
            matchingPerson.DateOfbirth = person.DateOfbirth;
            matchingPerson.Gender = person.Gender;
            matchingPerson.Address = person.Address;
            matchingPerson.CountryId = person.CountryId;
            matchingPerson.ReceiveNewsLetters = person.ReceiveNewsLetters;
            int countryUpdated = await _db.SaveChangesAsync();
            return matchingPerson;
        }
    }
}
