using Entities;
using System.Linq.Expressions;

namespace RepositoryContracts
{
    public interface IPersonsRepository
    {
        Task<Person> AddPerson(Person person);

        Task<List<Person>> GetAllPersons();

        Task<Person?> GetPersonByPersonID(Guid personID);

        /// <summary>
        /// returns all person objects based on given expression
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns> all matching person with given conditions</returns>
        Task<List<Person>> GetFilteredPersons(Expression<Func<Person,bool>>predicate); // this is  to receive a lambda expression. passing person type of value and receive the value in boolean type in the parametr of this fuicntion as lambda expression.

        Task<bool> DeletePersonByPersonID(Guid personID);

        Task<Person> UpdatePerson(Person person);


    }
}