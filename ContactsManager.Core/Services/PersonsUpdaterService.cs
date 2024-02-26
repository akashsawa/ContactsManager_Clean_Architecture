using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ServiceContracts;
using Entities;
using ServiceContracts.DTO;
using System.ComponentModel.DataAnnotations;
using Services.Helpers;
using System.Diagnostics;
using ServiceContracts.Enums;
using System.Transactions;
//using Microsoft.EntityFrameworkCore;
using CsvHelper;
using System.Globalization;
using CsvHelper.Configuration;
using OfficeOpenXml;
using RepositoryContracts;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Extensions.Hosting;
using SerilogTimings;
using Exceptions;

namespace Services
{
    public class PersonsUpdaterService : IPersonsUpdaterService
    {
        //ilogger
        private readonly ILogger<PersonsUpdaterService> _logger;
        //ilogger

        //idiagnosticcontext
        private readonly IDiagnosticContext _diagnosticContext;
        //idiagnosticcontext

        //private readonly List<Person> _persons;
        //private readonly ApplicationDBContext _db;
        private readonly IPersonsRepository _personRepository;
        //private readonly ICountriesService
        //    _countriesService;

        //public PersonsService(bool initialize = true)
        //public PersonsService(ApplicationDBContext personDbContext, ICountriesService countriesService)
        public PersonsUpdaterService(IPersonsRepository personRepository, ILogger<PersonsUpdaterService> logger, IDiagnosticContext diagnosticContext)
        {
            //ilogger
            _logger = logger;
            //ilogger

            //idiagnosticcontext
            _diagnosticContext = diagnosticContext;
            //idiagnosticcontext

            _personRepository = personRepository;
            //_countriesService = countriesService;

            //_persons = new List<Person>();
            //_countriesService = new CountriesService();
            //if (initialize)
            //{
            //    //{}
            //    //{}
            //    //{}
            //    //{E1F0C660-B97A-497F-9F7B-3EA313063C3C}

            //    //sampel data
            //    // John Doe, john.doe @email.com,1985 - 07 - 15,Male,123 Main St, TRUE
            //    //Jane Smith, jane.smith @email.com,1990 - 03 - 22,Female,456 Oak Ave, FALSE
            //    //Bob Johnson, bob.johnson @email.com,1978 - 11 - 05,Male,789 Pine Rd, TRUE
            //    //Alice Brown, alice.brown @email.com,1982 - 09 - 18,Female,101 Cedar Ln, FALSE
            //    //Chris Wilson, chris.wilson @email.com,1995 - 04 - 30,Non - Binary,202 Elm Blvd, TRUE
            //    //sample data

            //    _persons.Add(new Person()
            //    {
            //        Personid = Guid.Parse("C8CB6F80-9645-41D8-A6A5-A991EA1ACF06"),

            //        PersonName = "John Doe",
            //        Email = "john.doe @email.com",
            //        DateOfbirth = DateTime.Parse("1985-07-15"),
            //        Address = "123 Main St",
            //        ReceiveNewsLetters = true,
            //        Gender = "Male",
            //        CountryId =
            //        Guid.Parse("9884B371-7E2F-4BC1-BD65-E34301D0AD2D")
            //    });

            //    _persons.Add(new Person()
            //    {
            //        Personid = Guid.Parse("3B86EE5F-5128-4BA4-BA37-63AD8B1C8BF7"),

            //        PersonName = "Jane Smith",
            //        Email = "jane.smith @email.com",
            //        DateOfbirth = DateTime.Parse("1990-03-22"),
            //        Address = "456 Oak Ave",
            //        ReceiveNewsLetters = true,
            //        Gender = "Female",
            //        CountryId =
            //        Guid.Parse("52BC74D6-9094-441F-A51D-C1604F26F376")

            //    });

            //    _persons.Add(new Person()
            //    {
            //        Personid = Guid.Parse("2EFA7A85-7252-4408-A377-CDE1BD1510F3"),

            //        PersonName = "Jane Smith",
            //        Email = "jane.smith @email.com",
            //        DateOfbirth = DateTime.Parse("1990-03-22"),
            //        Address = "456 Oak Ave",
            //        ReceiveNewsLetters = true,
            //        Gender = "Female",
            //        CountryId =
            //        Guid.Parse("9884B371-7E2F-4BC1-BD65-E34301D0AD2D")
            //    });

            //    _persons.Add(new Person()
            //    {
            //        Personid = Guid.Parse("3B86EE5F-5128-4BA4-BA37-63AD8B1C8BF7"),

            //        PersonName = "Bob Johnson",
            //        Email = "bob.johnson @email.com",
            //        DateOfbirth = DateTime.Parse("1987-03-09"),
            //        Address = "789 Pine Rd",
            //        ReceiveNewsLetters = false,
            //        Gender = "male",
            //        CountryId =
            //        Guid.Parse("E45C96A1-3D83-4EF9-99E4-2B6FF51B5399")
            //    });

            //    _persons.Add(new Person()
            //    {
            //        Personid = Guid.Parse("3B86EE5F-5128-4BA4-BA37-63AD8B1C8BF7"),

            //        PersonName = "Alice Brown",
            //        Email = "alice.brown @email.com",
            //        DateOfbirth = DateTime.Parse("1982-09-18"),
            //        Address = "101 Cedar Ln",
            //        ReceiveNewsLetters = true,
            //        Gender = "Female",
            //        CountryId =
            //       Guid.Parse("54DEAD56-5A92-4C41-AF81-F27DC37BC67D")
            //    });
            //}
        }




       

        public async Task<PersonResponse> UpdatePerson(PersonUpdateRequest? personUpdateRequest)
        {
            if (personUpdateRequest == null)
            {
                //throw new ArgumentNullException(nameof(Person));
                throw new InvalidPersonIDException("Given person id doesnt exists !...");
            }
            ValidationHelper.ModelValidation(personUpdateRequest);

            //get matching person object to update
            //Person? matchingPerson = _persons.FirstOrDefault(temp => temp.Personid == personUpdateRequest.PersonId);

            //Person? matchingPerson = await _personRepository.Persons.FirstOrDefaultAsync(temp => temp.Personid == personUpdateRequest.PersonId);

            Person? matchingPerson = await _personRepository.GetPersonByPersonID(personUpdateRequest.PersonId);

            if (matchingPerson == null)
                throw new ArgumentException("Given person id doesnt exist !");

            //update all details
            matchingPerson.PersonName = personUpdateRequest.PersonName;
            matchingPerson.Email = personUpdateRequest.Email;
            matchingPerson.DateOfbirth = personUpdateRequest.DateOfBirth;
            matchingPerson.Gender = personUpdateRequest.Gender.ToString();
            matchingPerson.CountryId = personUpdateRequest.CountryId;
            //await _personRepository.SaveChangesAsync(); //update 

            await _personRepository.UpdatePerson(matchingPerson);
            return matchingPerson.ToPersonResponse();
        }

        
    }
}