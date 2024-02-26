using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Entities;
using ServiceContracts.DTO;
using ServiceContracts;
using Services;
using ServiceContracts.Enums;
using Xunit.Abstractions;
//using Microsoft.EntityFrameworkCore;
using EntityFrameworkCoreMock;
using AutoFixture;
using FluentAssertions;
using RepositoryContracts;
using Moq;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;
using FluentAssertions.Execution;
using System.Linq.Expressions;
using Serilog.Extensions.Hosting;
using Castle.Core.Logging;
using Microsoft.Extensions.Logging;

namespace CrudTests
{
    public class PersonServiceTest
    {
        //private readonly IPersonsService _personsService;
        private readonly IPersonsGetterService _personsGetterService;
        private readonly IPersonsAdderService _personsAdderService;
        private readonly IPersonsUpdaterService _personsUpdaterService;
        private readonly IPersonsDeleterService _personsDeleterService;
        private readonly IPersonsSorterService _personsSorterService;

        //private readonly ICountriesService _countriesService;
        private readonly ITestOutputHelper _testoutputHelper;
        private readonly IFixture _fixture;

        private readonly IPersonsRepository _personRepository;
        private readonly Mock<IPersonsRepository> _personRepositoryMock;

        public PersonServiceTest(ITestOutputHelper testoutputHelper) //ITestOutputHelper is for able to see the output for every test case just like console.writeline()
        {
            // mocking repository
            _personRepositoryMock = new Mock<IPersonsRepository>();
            _personRepository = _personRepositoryMock.Object;
            // mocking repository

            //_personsService = new PersonsService(false);

            //_countriesService = new CountriesService(new ApplicationDBContext(new DbContextOptionsBuilder<ApplicationDBContext>().Options));
            //_personsService = new PersonsService(new ApplicationDBContext(new DbContextOptionsBuilder<ApplicationDBContext>().Options), _countriesService);


            //_testoutputHelper = testoutputHelper;

            //mocking dbcontext

            //var countriesInitrialData = new List<Country>() { };
            //var personInitialData = new List<Person>();
            //DbContextMock<ApplicationDBContext> dbContextMock = new DbContextMock<ApplicationDBContext>(new DbContextOptionsBuilder<ApplicationDBContext>().Options);


            //ApplicationDBContext dbContext = dbContextMock.Object;
            //dbContextMock.CreateDbSetMock(temp => temp.Countries, countriesInitrialData);
            //dbContextMock.CreateDbSetMock(temp => temp.Persons, personInitialData);

            //_countriesService = new CountriesService(dbContext);

            //_countriesService = new CountriesService(null);

            //_personsService = new PersonsService(dbContext, _countriesService);

            //mocking diagnosticcontext
            var diagnosticContextMock = new Mock<DiagnosticContext>();

            //var loggerMock = new Mock<ILogger<PersonsService>>();
            var loggerMockGetter = new Mock<ILogger<PersonsGetterService>>();
            var loggerMockAdder = new Mock<ILogger<PersonsAdderService>>();
            var loggerMockUpdater = new Mock<ILogger<PersonsUpdaterService>>();
            var loggerMockDeleter = new Mock<ILogger<PersonsDeleterService>>();
            var loggerMockSorter = new Mock<ILogger<PersonsSorterService>>();
            //var loggerMock = new Mock<ILogger<PersonsService>>();
            //var loggerMock = new Mock<ILogger<PersonsService>>();
            //var loggerMock = new Mock<ILogger<PersonsService>>();
            //var loggerMock = new Mock<ILogger<PersonsService>>();

            //mocking diagnosticcontext

            _personsGetterService = new PersonsGetterService(_personRepository, loggerMockGetter.Object, diagnosticContextMock.Object);
            _personsAdderService = new PersonsAdderService(_personRepository, loggerMockAdder.Object, diagnosticContextMock.Object);
            _personsUpdaterService = new PersonsUpdaterService(_personRepository, loggerMockUpdater.Object, diagnosticContextMock.Object);
            _personsDeleterService = new PersonsDeleterService(_personRepository, loggerMockDeleter.Object, diagnosticContextMock.Object);
            _personsSorterService = new PersonsSorterService(_personRepository, loggerMockSorter.Object, diagnosticContextMock.Object);

            _testoutputHelper = testoutputHelper;


            _fixture = new Fixture();


        }

        #region Addperson
        //when we supply null value as personaddrequest it should return argumentnullexcpetion

        [Fact]
        public async Task AddPerson_NullPerson_ToBeArgumentNullException()
        {
            PersonAddRequest? personAddRequest = null;

            //await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            //{
            //    await _personsService.AddPerson(personAddRequest);
            //});

            //fluent assertions
            Func<Task> action = (async () =>
            {
                await _personsAdderService.AddPerson(personAddRequest);
            });
            await action.Should().ThrowAsync<ArgumentNullException>();
            //fluent assertions

        }

        //when we supply null value as personname it should throw argumentexcpetion
        [Fact]
        public async Task AddPerson_PersonNameIsNull_TobeArgumentException()
        {
            //PersonAddRequest? personAddRequest = new PersonAddRequest()
            //{
            //    PersonName = null
            //};

            //autofixture
            PersonAddRequest? personAddRequest = _fixture.Build<PersonAddRequest>().With(temp => temp.PersonName, null as string).Create();

            //mocking repository
            Person person = personAddRequest.ToPerson();

            _personRepositoryMock.Setup(temp => temp.AddPerson(It.IsAny<Person>())).ReturnsAsync(person);
            //mocking repository

            //await Assert.ThrowsAsync<ArgumentException>(async () =>
            //{
            //    await _personsService.AddPerson(personAddRequest);
            //});

            //fluent assertions
            Func<Task> action = (async () =>
            {
                await _personsAdderService.AddPerson(personAddRequest);
            });
            await action.Should().ThrowAsync<ArgumentException>();
            //fluent assertions
        }


        //when we supply proper person details  it should insert the person into person list and it should returnan object of personresponse, which inclused with the newly generated perosn id.
        [Fact]
        public async Task AddPerson_FullPersonDetails_ToBeSuccessful()
        {
            //PersonAddRequest? personAddRequest = new PersonAddRequest()
            //{
            //    PersonName = "Akash",
            //    Email = "akash@gmail.com",
            //    Address = "Bhandup",
            //    CountryId = Guid.NewGuid(),
            //    Gender = GenderOptions.Male,
            //    DateOfBirth = DateTime.Parse("1998-07-22"),
            //    ReceiveNewsLetters = true
            //};

            //instead o fhtis we use autofixture

            //autofixture
            //PersonAddRequest? personAddazaRequest = _fixture.Create<PersonAddRequest>();

            PersonAddRequest? personAddRequest = _fixture.Build<PersonAddRequest>().With(temp => temp.Email, "someone@example.com")
                .Create(); // build doesnt finaliza objwect immediately, it lets us customize the values.
            ///


            //mocking particular repository method
            Person person = personAddRequest.ToPerson();
            PersonResponse person_response_expected = person.ToPersonResponse();
            //if we supply any argument value to the addperson method , it should return the same return value. 
            _personRepositoryMock.Setup(temp => temp.AddPerson(It.IsAny<Person>())).ReturnsAsync(person); // 

            //mocking particular repository method

            //act
            PersonResponse person_respnse_from_add = await _personsAdderService.AddPerson(personAddRequest);

            person_response_expected.PersonId = person_respnse_from_add.PersonId;

            //List<PersonResponse> persons_list = await _personsService.GetAllPersons();

            //Assert.True(person_respnse_from_add.PersonId != Guid.Empty);

            //fluent assertions
            person_respnse_from_add.PersonId.Should().NotBe(Guid.Empty);
            //fluent assertions

            //Assert.Contains(person_respnse_from_add, persons_list);

            //fluent assertions
            //persons_list.Should().Contain(person_respnse_from_add);
            person_respnse_from_add.Should().Be(person_response_expected);
            //fluent assertions
        }

        #endregion

        #region Getperosnbyperosnid
        //if we supply null as perosnid , it should return null as perosnresponse.

        [Fact]
        public async void GetpersonByPersonId_NullPersonId_ToBeNull()
        {
            Guid? personId = null;
            PersonResponse? person_response_from_get = await _personsGetterService.GetpersonByPersonId(personId);

            //Assert.Null(person_response_from_get);

            //fluent assertions
            person_response_from_get.Should().BeNull();
            //fluent assertions

        }

        //if we supply valid as perosnid , it should return valid persondetails as as perosnresponse object.

        [Fact]
        public async void GetpersonByPersonId_WithPersonId_ToBeSuccessfull()
        {
            //CountryAddRequest country_request = new CountryAddRequest()
            //{
            //    CountryName = "China"
            //};

            //autofixture
            //CountryAddRequest country_request = _fixture.Create<CountryAddRequest>();

            //CountryResponse country_response = await _countriesService.AddCountry(country_request);

            //autofixture
            //PersonAddRequest person_request = _fixture.Build<PersonAddRequest>().With(temp => temp.Email, "email@example.com").Create();

            //mocking repossitory
            Person person = _fixture.Build<Person>().With(temp => temp.Email, "email@example.com").With(temp => temp.Country, null as Country).Create();


            PersonResponse person_response_expected = person.ToPersonResponse();
            //mocking repossitory


            //PersonAddRequest person_request = new PersonAddRequest()
            //{
            //    PersonName = "Perosn name...",
            //    Email = "email@gmail.com",
            //    Address = "address...",
            //    CountryId = country_response.CountryID,
            //    DateOfBirth = DateTime.Parse("2000-01-02"),
            //    Gender = GenderOptions.Female,
            //    ReceiveNewsLetters = false
            //};
            //PersonResponse person_response_form_add = await _personsService.AddPerson(person_request);

            //PersonResponse? person_response_from_get = await _personsService.GetpersonByPersonId(person_response_form_add.PersonId);

            //mocking repository
            _personRepositoryMock.Setup(temp => temp.GetPersonByPersonID(It.IsAny<Guid>())).ReturnsAsync(person);

            PersonResponse? person_response_from_get = await _personsGetterService.GetpersonByPersonId(person.Personid);
            //mocking repository

            //Assert.Equal(person_response_from_get, person_response_form_add);

            //fluent assertions
            person_response_from_get.Should().Be(person_response_from_get);
            //fluent assertions

        }
        #endregion

        #region getallpersons

        //the getallpersons() should return an empty list by default

        [Fact]
        public async Task GetAllPerosns_EmptyList_ToBeEmptyList()
        {
            //act
            //mocking repository
            var persons = new List<Person>();

            _personRepositoryMock.Setup(temp => temp.GetAllPersons()).ReturnsAsync(persons);

            //mocking repository

            List<PersonResponse> persons_from_get = await _personsGetterService.GetAllPersons();

            //assert
            //Assert.Empty(persons_from_get);

            //fluent persons_from_get
            persons_from_get.Should().BeEmpty();
            //fluent assertions
        }

        [Fact]
        public async Task GetAllPerosns_AddFewPersons_WithFewPersonsToBeSuccessfull()
        {
            //BEST PRACTICE: WE HAVE TO TEST SINGLE METHOD IN SINGLE UNIT TEST.

            //act
            //CountryAddRequest country_request1 = new CountryAddRequest() { CountryName = "USA" };

            //CountryAddRequest country_request1 = _fixture.Create<CountryAddRequest>();

            ////CountryAddRequest country_request2 = new CountryAddRequest() { CountryName = "India" };

            //CountryAddRequest country_request2 = _fixture.Create<CountryAddRequest>();

            //CountryResponse country_response_1 = await _countriesService.AddCountry(country_request1);
            //CountryResponse country_response_2 = await _countriesService.AddCountry(country_request2);


            //PersonAddRequest persons_request_1 = new PersonAddRequest() { PersonName = "Smith", Email = "smith@gmail.com", Gender = GenderOptions.Male, Address = "Mumbai", CountryId = country_response_1.CountryID, DateOfBirth = DateTime.Parse("2000-01-02") }; 


            //mocking repository
            List<Person> persons = new List<Person>()
            {
                _fixture.Build<Person>().With(temp => temp.Email, "someone_1@gmail.com").With(temp => temp.Country, null as Country).Create(),

                    _fixture.Build<Person>().With(temp => temp.Email, "someone_2@gmail.com").With(temp => temp.Country, null as Country).Create(),

                    _fixture.Build<Person>().With(temp => temp.Email, "someone_3@gmail.com").With(temp => temp.Country, null as Country).Create()
            };

            //mocking repository

            //PersonAddRequest persons_request_2 = new PersonAddRequest() { PersonName = "john", Email = "john@gmail.com", Gender = GenderOptions.Male, Address = "tokyo", CountryId = country_response_1.CountryID, DateOfBirth = DateTime.Parse("1996-09-12") };

            //PersonAddRequest persons_request_2 = _fixture.Build<PersonAddRequest>().With(temp => temp.Email, "someone_2@gmail.com").Create();

            //assert
            //List<PersonAddRequest> perosn_requests = new List<PersonAddRequest>() { persons_request_1, persons_request_2 };

            //List<PersonResponse> person_response_list_from_add = new List<PersonResponse>();

            List<PersonResponse> person_response_list_expected = persons.Select(temp => temp.ToPersonResponse()).ToList();

            //foreach (PersonAddRequest person_request in perosn_requests)
            //{
            //    PersonResponse person_response = await _personsService.AddPerson(person_request);
            //    person_response_list_from_add.Add(person_response);
            //}

            //itestoutputhelper
            _testoutputHelper.WriteLine("expected: ");
            //foreach (PersonResponse person_response_from_add in person_response_list_from_add)
            //{
            //    _testoutputHelper.WriteLine(person_response_from_add.ToString());
            //}

            //mocking repository
            foreach (PersonResponse person_response_from_add in person_response_list_expected)
            {
                _testoutputHelper.WriteLine(person_response_from_add.ToString());
            }

            _personRepositoryMock.Setup(temp => temp.GetAllPersons()).ReturnsAsync(persons);
            //mocking repository

            //

            List<PersonResponse> persons_list_from_get = await _personsGetterService.GetAllPersons();
            foreach (PersonResponse person_response_from_add in persons_list_from_get)
            {
                Assert.Contains(person_response_from_add, persons_list_from_get);
            }

            //fluent persons_from_get
            persons_list_from_get.Should().BeEquivalentTo(person_response_list_expected);
            //fluent assertions

            //itestoutputhelper
            _testoutputHelper.WriteLine("actual: ");
            foreach (PersonResponse person_response_from_get in persons_list_from_get)
            {
                _testoutputHelper.WriteLine(person_response_from_get.ToString());
            }
            //

        }

        #endregion

        #region GetFiletredpersons

        //if the search text is empty and search by is personname it should return all persons. 

        [Fact]
        public async Task GetFilteredPersons_EmptySearchText()
        {
            //act
            //CountryAddRequest country_request1 = new CountryAddRequest() { CountryName = "USA" };
            //CountryAddRequest country_request2 = new CountryAddRequest() { CountryName = "India" };

            //CountryAddRequest country_request1 = _fixture.Create<CountryAddRequest>();

            //CountryAddRequest country_request2 = _fixture.Create<CountryAddRequest>();

            //PersonAddRequest persons_request_1 = _fixture.Build<PersonAddRequest>().With(temp => temp.Email, "someone_1@gmail.com").Create();

            //PersonAddRequest persons_request_2 = _fixture.Build<PersonAddRequest>().With(temp => temp.Email, "someone_2@gmail.com").Create();

            //CountryResponse country_response_1 = await _countriesService.AddCountry(country_request1);
            //CountryResponse country_response_2 = await _countriesService.AddCountry(country_request2);


            //PersonAddRequest persons_request_1 = new PersonAddRequest() { PersonName = "Smith", Email = "smith@gmail.com", Gender = GenderOptions.Male, Address = "Mumbai", CountryId = country_response_1.CountryID, DateOfBirth = DateTime.Parse("2000-01-02") };

            //PersonAddRequest persons_request_2 = new PersonAddRequest() { PersonName = "john", Email = "john@gmail.com", Gender = GenderOptions.Male, Address = "tokyo", CountryId = country_response_1.CountryID, DateOfBirth = DateTime.Parse("1996-09-12") };


            //mocking repository
            List<Person> persons = new List<Person>()
            {
                _fixture.Build<Person>().With(temp => temp.Email, "someone_1@gmail.com").With(temp => temp.Country, null as Country).Create(),

                    _fixture.Build<Person>().With(temp => temp.Email, "someone_2@gmail.com").With(temp => temp.Country, null as Country).Create(),

                    _fixture.Build<Person>().With(temp => temp.Email, "someone_3@gmail.com").With(temp => temp.Country, null as Country).Create()
            };

            //mocking repository

            //assert
            //List<PersonAddRequest> perosn_requests = new List<PersonAddRequest>() { persons_request_1, persons_request_2 };

            //List<PersonResponse> person_response_list_from_add = new List<PersonResponse>();

            //foreach (PersonAddRequest person_request in perosn_requests)
            //{
            //    PersonResponse person_response = await _personsService.AddPerson(person_request);
            //    person_response_list_from_add.Add(person_response);
            //}

            //itestoutputhelper

            //mocking repository
            List<PersonResponse> person_response_list_expected = persons.Select(temp => temp.ToPersonResponse()).ToList();

            _testoutputHelper.WriteLine("expected: ");

            foreach (PersonResponse person_response_from_add in person_response_list_expected)
            {
                _testoutputHelper.WriteLine(person_response_from_add.ToString());
            }
            //mocking repository

            //_testoutputHelper.WriteLine("expected: ");
            //foreach (PersonResponse person_response_from_add in person_response_list_from_add)
            //{
            //    _testoutputHelper.WriteLine(person_response_from_add.ToString());
            //}
            //

            //mocking repository
            _personRepositoryMock.Setup(temp => temp.GetFilteredPersons(It.IsAny<Expression<Func<Person, bool>>>())).ReturnsAsync(persons);
            //mocking repository

            List<PersonResponse> persons_list_from_search = await _personsGetterService.GetFilteredpersons(nameof(Person.PersonName), "");
            foreach (PersonResponse person_response_from_add in persons_list_from_search)
            {
                //Assert.Contains(person_response_from_add, persons_list_from_search);
            }

            //fluent assertions
            //persons_list_from_search.Should().BeEquivalentTo(person_response_list_from_add);

            //mocking repository
            persons_list_from_search.Should().BeEquivalentTo(person_response_list_expected);
            //mocking repository

            //fluent assertions

            //itestoutputhelper
            _testoutputHelper.WriteLine("actual: ");
            foreach (PersonResponse person_response_from_get in persons_list_from_search)
            {
                _testoutputHelper.WriteLine(person_response_from_get.ToString());
            }
            //

        }

        //first we will add few eprsons and then we will search based on eprosn name with some search string , it should return the matching perosns.
        [Fact]
        public async Task GetFilteredPersons_SearchByPersonsName_ToBeSuccessfull()
        {
            //commented all 

            ////act
            ////CountryAddRequest country_request1 = new CountryAddRequest() { CountryName = "USA" };
            ////CountryAddRequest country_request2 = new CountryAddRequest() { CountryName = "India" };

            ////CountryAddRequest country_request1 = _fixture.Create<CountryAddRequest>();

            ////CountryAddRequest country_request2 = _fixture.Create<CountryAddRequest>();





            ////CountryResponse country_response_1 = await _countriesService.AddCountry(country_request1);
            ////CountryResponse country_response_2 = await _countriesService.AddCountry(country_request2);

            ////PersonAddRequest persons_request_1 = _fixture.Build<PersonAddRequest>().With(temp => temp.PersonName, "Rahman").With(temp => temp.Email, "someone_1@gmail.com").With(temp => temp.CountryId, country_response_1.CountryID).Create();

            ////PersonAddRequest persons_request_2 = _fixture.Build<PersonAddRequest>().With(temp => temp.PersonName, "Mary").With(temp => temp.Email, "someone_2@gmail.com").With(temp => temp.CountryId, country_response_2.CountryID).Create();

            ////PersonAddRequest persons_request_1 = new PersonAddRequest() { PersonName = "Smith", Email = "smith@gmail.com", Gender = GenderOptions.Male, Address = "Mumbai", CountryId = country_response_1.CountryID, DateOfBirth = DateTime.Parse("2000-01-02") };

            ////PersonAddRequest persons_request_2 = new PersonAddRequest() { PersonName = "john", Email = "john@gmail.com", Gender = GenderOptions.Male, Address = "tokyo", CountryId = country_response_1.CountryID, DateOfBirth = DateTime.Parse("1996-09-12") };

            ////mocking repository
            //List<Person> persons = new List<Person>()
            //{
            //    _fixture.Build<Person>().With(temp => temp.Email, "someone_1@gmail.com").With(temp => temp.Country, null as Country).Create(),

            //        _fixture.Build<Person>().With(temp => temp.Email, "someone_2@gmail.com").With(temp => temp.Country, null as Country).Create(),

            //        _fixture.Build<Person>().With(temp => temp.Email, "someone_3@gmail.com").With(temp => temp.Country, null as Country).Create()
            //};

            ////mocking repository

            ////assert
            ////List<PersonAddRequest> perosn_requests = new List<PersonAddRequest>() { persons_request_1, persons_request_2 };

            //List<PersonResponse> person_response_list_from_add = new List<PersonResponse>();


            //_testoutputHelper.WriteLine("ACTUAL: ");

            ////mocking repository
            //foreach (Person person_request in persons)
            //{
            //    PersonResponse person_response = await _personsService.AddPerson(person_request);
            //    person_response_list_from_add.Add(person_response);
            //}
            ////mocking repository

            ////foreach (PersonAddRequest person_request in perosn_requests)
            ////{
            ////    PersonResponse person_response = await _personsService.AddPerson(person_request);
            ////    person_response_list_from_add.Add(person_response);
            ////}

            ////itestoutputhelper
            //_testoutputHelper.WriteLine("expected: ");
            //foreach (PersonResponse person_response_from_add in person_response_list_from_add)
            //{
            //    _testoutputHelper.WriteLine(person_response_from_add.ToString());
            //}
            ////

            //List<PersonResponse> persons_list_from_search = await _personsService.GetFilteredpersons(nameof(Person.PersonName), "ma");
            ////foreach (PersonResponse person_response_from_add in persons_list_from_search)
            ////{
            ////    if (person_response_from_add.PersonName != null)
            ////    {
            ////        if (person_response_from_add.PersonName.Contains("ma", StringComparison.OrdinalIgnoreCase))
            ////        {
            ////            //Assert.Contains(person_response_from_add, persons_list_from_search);
            ////        }
            ////    }
            ////}



            //////itestoutputhelper
            ////_testoutputHelper.WriteLine("actual: ");
            ////foreach (PersonResponse person_response_from_get in persons_list_from_search)
            ////{
            ////    _testoutputHelper.WriteLine(person_response_from_get.ToString());
            ////}
            ////

            ////fluent assertions
            //persons_list_from_search.Should().OnlyContain(temp => temp.PersonName.Contains("ma", StringComparison.OrdinalIgnoreCase));
            ////fluent assertions

            //commented all


            //mocking repository
            List<Person> persons = new List<Person>()
            {
                _fixture.Build<Person>().With(temp => temp.Email, "someone_1@gmail.com").With(temp => temp.Country, null as Country).Create(),

                    _fixture.Build<Person>().With(temp => temp.Email, "someone_2@gmail.com").With(temp => temp.Country, null as Country).Create(),

                    _fixture.Build<Person>().With(temp => temp.Email, "someone_3@gmail.com").With(temp => temp.Country, null as Country).Create()
            };

            //mocking repository
            List<PersonResponse> person_response_list_expected = persons.Select(temp => temp.ToPersonResponse()).ToList();
            //mocking repository

            _testoutputHelper.WriteLine("expected: ");

            foreach (PersonResponse person_response_from_add in person_response_list_expected)
            {
                _testoutputHelper.WriteLine(person_response_from_add.ToString());
            }
            //mocking repository



            //mocking repository
            _personRepositoryMock.Setup(temp => temp.GetFilteredPersons(It.IsAny<Expression<Func<Person, bool>>>())).ReturnsAsync(persons);
            //mocking repository

            List<PersonResponse> persons_list_from_search = await _personsGetterService.GetFilteredpersons(nameof(Person.PersonName), "sa");



            //mocking repository
            persons_list_from_search.Should().BeEquivalentTo(person_response_list_expected);
            //mocking repository


            _testoutputHelper.WriteLine("actual: ");
            foreach (PersonResponse person_response_from_get in persons_list_from_search)
            {
                _testoutputHelper.WriteLine(person_response_from_get.ToString());
            }
            //

        }


        #endregion

        //when we sort based on personname is desc, it shoudl return perosns in descending order .
        #region GetSortedPersons
        [Fact]
        public async Task GetSortedPersons_ToBeSuccessfull()
        {
            //act
            //CountryAddRequest country_request1 = new CountryAddRequest() { CountryName = "USA" };
            //CountryAddRequest country_request2 = new CountryAddRequest() { CountryName = "India" };
            //CountryResponse country_response_1 = await _countriesService.AddCountry(country_request1);
            //CountryResponse country_response_2 = await _countriesService.AddCountry(country_request2);

            //CountryAddRequest country_request1 = _fixture.Create<CountryAddRequest>();

            //CountryAddRequest country_request2 = _fixture.Create<CountryAddRequest>();





            //CountryResponse country_response_1 = await _countriesService.AddCountry(country_request1);
            //CountryResponse country_response_2 = await _countriesService.AddCountry(country_request2);

            //PersonAddRequest persons_request_1 = _fixture.Build<PersonAddRequest>().With(temp => temp.PersonName, "Smith").With(temp => temp.Email, "someone_1@gmail.com").With(temp => temp.CountryId, country_response_1.CountryID).Create();

            //PersonAddRequest persons_request_2 = _fixture.Build<PersonAddRequest>().With(temp => temp.PersonName, "Gian").With(temp => temp.Email, "someone_2@gmail.com").With(temp => temp.CountryId, country_response_2.CountryID).Create();



            //PersonAddRequest persons_request_1 = new PersonAddRequest() { PersonName = "Akash", Email = "akash@gmail.com", Gender = GenderOptions.Female, Address = "Mumbai", CountryId = country_response_1.CountryID, DateOfBirth = DateTime.Parse("2000-01-02") };

            //PersonAddRequest persons_request_2 = new PersonAddRequest() { PersonName = "Sagar", Email = "sagar@gmail.com", Gender = GenderOptions.Male, Address = "tokyo", CountryId = country_response_1.CountryID, DateOfBirth = DateTime.Parse("1996-09-12") };



            //assert
            //List<PersonAddRequest> perosn_requests = new List<PersonAddRequest>() { persons_request_1, persons_request_2 };

            //mocking repository
            List<Person> persons = new List<Person>()
            {
                _fixture.Build<Person>().With(temp => temp.Email, "someone_1@gmail.com").With(temp => temp.Country, null as Country).Create(),

                    _fixture.Build<Person>().With(temp => temp.Email, "someone_2@gmail.com").With(temp => temp.Country, null as Country).Create(),

                    _fixture.Build<Person>().With(temp => temp.Email, "someone_3@gmail.com").With(temp => temp.Country, null as Country).Create()
            };

            //mocking repository

            //mocking repository
            List<PersonResponse> person_response_list_expected = persons.Select(temp => temp.ToPersonResponse()).ToList();

            _personRepositoryMock.Setup(temp => temp.GetAllPersons()).ReturnsAsync(persons);


            //mocking repository

            //mocking repository
            _testoutputHelper.WriteLine("expected: ");

            foreach (PersonResponse person_response_from_add in person_response_list_expected)
            {
                _testoutputHelper.WriteLine(person_response_from_add.ToString());
            }
            //mocking repository


            //List<PersonResponse> person_response_list_from_add = new List<PersonResponse>();

            //foreach (PersonAddRequest person_request in perosn_requests)
            //{
            //    PersonResponse person_response = await _personsService.AddPerson(person_request);
            //    person_response_list_from_add.Add(person_response);
            //}

            //itestoutputhelper
            //_testoutputHelper.WriteLine("expected: ");
            //foreach (PersonResponse person_response_from_add in person_response_list_from_add)
            //{
            //    _testoutputHelper.WriteLine(person_response_from_add.ToString());
            //}
            //

            List<PersonResponse> allPersons = await _personsGetterService.GetAllPersons();
            List<PersonResponse> persons_list_from_sort = await _personsSorterService.GetSortedPersons(allPersons, nameof(Person.PersonName), SortOrderOptions.DESC);

            //itestoutputhelper
            _testoutputHelper.WriteLine("actual: ");
            foreach (PersonResponse person_response_from_get in persons_list_from_sort)
            {
                _testoutputHelper.WriteLine(person_response_from_get.ToString());
            }
            //
            //person_response_list_from_add = person_response_list_from_add.OrderBy(temp => temp.PersonName, StringComparer.OrdinalIgnoreCase).ToList();

            //assert
            //for (int i = 0; i < person_response_list_from_add.Count; i++)
            //{
            //    Assert.Equal(person_response_list_from_add[i], persons_list_from_sort[i]);
            //}

            //fluent assertions
            //persons_list_from_sort.Should().BeEquivalentTo(person_response_list_from_add); // this checks whether all elements of one list is present in other list

            persons_list_from_sort.Should().BeInDescendingOrder(order => order.PersonName);
            //fluent assertions
        }
        #endregion

        #region UpdatePerson
        /// <summary>
        /// when we supply nulll as personupdaterequest it should throw arguiment null excpetion.
        /// </summary>
        [Fact]
        public async Task UpdatePerson_NullPersonToBeArgumentNullException()
        {
            PersonUpdateRequest personUpdateRequest = new PersonUpdateRequest();
            personUpdateRequest = null;
            //await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            //{
            //    await _personsService.UpdatePerson(personUpdateRequest);
            //});

            //fluent assertions
            Func<Task> action = async () =>
            {
                await _personsUpdaterService.UpdatePerson(personUpdateRequest);
            };

            await action.Should().ThrowAsync<ArgumentNullException>();
            //fluent assertions


        }

        [Fact]
        public async Task UpdatePerson_InvalidPersonIDToBeArgumentException()
        {
            //PersonUpdateRequest personUpdateRequest = new PersonUpdateRequest() { PersonId = Guid.NewGuid() };

            PersonUpdateRequest personUpdateRequest = _fixture.Build<PersonUpdateRequest>().Create();

            //await Assert.ThrowsAsync<ArgumentException>(async () =>
            //{
            //    await _personsService.UpdatePerson(personUpdateRequest);
            //});

            //fluent assertions
            Func<Task> action = async () =>
            {
                await _personsUpdaterService.UpdatePerson(personUpdateRequest);
            };

            await action.Should().ThrowAsync<ArgumentException>();
            //fluent assertions
        }

        [Fact]
        public async Task UpdatePerson_PersonNameIsNullToBeArgumentException()
        {
            //CountryAddRequest country_add_request = new CountryAddRequest() { CountryName = "UK" };

            //CountryAddRequest country_add_request = _fixture.Create<CountryAddRequest>();



            //CountryResponse country_response_from_add = await _countriesService.AddCountry(country_add_request);
            //PersonAddRequest person_add_request = new PersonAddRequest() { PersonName = "John", CountryId = country_response_from_add.CountryID, Email = "john@gmail.com", Gender = GenderOptions.Male };

            //PersonAddRequest person_add_request = _fixture.Build<PersonAddRequest>().With(temp => temp.PersonName, "Smith").With(temp => temp.Email, "someone_1@gmail.com").With(temp => temp.CountryId, country_response_from_add.CountryID).Create();

            //mocking repository
            Person person = _fixture.Build<Person>().With(temp => temp.PersonName, null as string).With(temp => temp.Email, "someone_1@gmail.com").With(temp => temp.Country, null as Country).With(temp => temp.Gender, "Male").Create();


            //PersonResponse person_response_from_add = await _personsService.AddPerson(person_add_request);

            PersonResponse person_response_from_add = person.ToPersonResponse();

            PersonUpdateRequest personUpdateRequest = person_response_from_add.ToPersonUpdateRequest();
            //personUpdateRequest.PersonName = null;
            //mocking repository

            //await Assert.ThrowsAsync<ArgumentException>(async () =>
            //{
            //    await _personsService.UpdatePerson(personUpdateRequest);
            //});

            //fluent assertions
            Func<Task> action = async () =>
            {
                await _personsUpdaterService.UpdatePerson(personUpdateRequest);
            };

            await action.Should().ThrowAsync<ArgumentException>();
            //fluent assertions
        }

        /// <summary>
        /// add new perosn and to update the same
        /// </summary>
        [Fact]
        public async void UpdatePerson_PersonFullDetailsToBeSuccessfull()
        {
            //CountryAddRequest country_add_request = new CountryAddRequest() { CountryName = "UK" };

            //CountryAddRequest country_add_request = _fixture.Create<CountryAddRequest>();


            //CountryResponse country_response_from_add = await _countriesService.AddCountry(country_add_request);
            //PersonAddRequest person_add_request = new PersonAddRequest() { PersonName = "John", CountryId = country_response_from_add.CountryID, Address = "ABC ROAD", Email = "abc@gmail.com", DateOfBirth = DateTime.Parse("2000-05-09"), Gender = GenderOptions.Male, ReceiveNewsLetters = true };

            Person person = _fixture.Build<Person>().With(temp => temp.PersonName, "Smith").With(temp => temp.Email, "someone_1@gmail.com").With(temp => temp.Country, null as Country).With(temp => temp.Gender, "Male").Create();

            //PersonResponse person_response_from_add = await _personsService.AddPerson(person_add_request);

            PersonResponse person_response_expected = person.ToPersonResponse();

            PersonUpdateRequest personUpdateRequest = person_response_expected.ToPersonUpdateRequest();

            _personRepositoryMock.Setup(temp => temp.UpdatePerson(It.IsAny<Person>())).ReturnsAsync(person);

            _personRepositoryMock.Setup(temp => temp.GetPersonByPersonID(It.IsAny<Guid>())).ReturnsAsync(person);

            //personUpdateRequest.PersonName = "MAX";
            //personUpdateRequest.Email = "max@gmail.com";


            PersonResponse person_response_from_update = await _personsUpdaterService.UpdatePerson(personUpdateRequest);

            //PersonResponse person_response_from_get = await _personsService.GetpersonByPersonId(person_response_from_update.PersonId);

            //Assert.Equal(person_response_from_get, person_response_from_update);

            //fluent assertions
            //person_response_from_update.Should().Be(person_response_from_get);

            person_response_from_update.Should().Be(person_response_expected);
            //fluent assertions
        }

        #endregion

        #region Deleteperson
        [Fact]
        public async Task DeletePerson_ValidPersonID_ToBeSuccessfull()
        {
            //CountryAddRequest country_add_request = new CountryAddRequest() { CountryName = "USA" };

            //CountryAddRequest country_add_request = _fixture.Create<CountryAddRequest>();

            //CountryResponse country_respinse_form_add = await _countriesService.AddCountry(country_add_request);
            //PersonAddRequest person_add_request = new PersonAddRequest() { PersonName = "John", CountryId = country_respinse_form_add.CountryID, Address = "ABC ROAD", Email = "abc@gmail.com", DateOfBirth = DateTime.Parse("2000-05-09"), Gender = GenderOptions.Male, ReceiveNewsLetters = true };

            //PersonAddRequest person_add_request = _fixture.Build<PersonAddRequest>().With(temp => temp.PersonName, "Smith").With(temp => temp.Email, "someone_1@gmail.com").With(temp => temp.CountryId, country_respinse_form_add.CountryID).Create();

            Person person = _fixture.Build<Person>().With(temp => temp.PersonName, "Smith").With(temp => temp.Email, "someone_1@gmail.com").With(temp => temp.Country, null as Country).Create();

            //PersonResponse person_response_from_add = await _personsService.AddPerson(person);

            //PersonResponse person_response_from_add = await _personsService.AddPerson(person_add_request);

            _personRepositoryMock.Setup(temp => temp.DeletePersonByPersonID(It.IsAny<Guid>())).ReturnsAsync(true);

            _personRepositoryMock.Setup(temp => temp.GetPersonByPersonID(It.IsAny<Guid>())).ReturnsAsync(person);

            //bool isDeleted = await _personsService.DeletePerson(person_response_from_add.PersonId);
            bool isDeleted = await _personsDeleterService.DeletePerson(person.Personid);

            //Assert.True(isDeleted);

            //fluent assertions
            isDeleted.Should().BeTrue();
            //fluent assertions
        }

        [Fact]
        public async Task DeletePerson_InValidPersonID()
        {
            bool isDeleted = await _personsDeleterService.DeletePerson(Guid.NewGuid());
            //Assert.False(isDeleted);

            //fluent assertions
            isDeleted.Should().BeFalse();
            //fluent assertions
        }
        #endregion
    }
}
