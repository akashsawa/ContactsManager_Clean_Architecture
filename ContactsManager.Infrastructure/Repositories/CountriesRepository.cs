using Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;

namespace Repositories
{
    public class CountriesRepository : ICountriesRepository
    {
        private readonly ApplicationDBContext _dbContext;

        public CountriesRepository(ApplicationDBContext _db)
        {
            _dbContext = _db;
        }

        public async Task<Country> AddCountry(Country country)
        {
            _dbContext.Countries.Add(country);
            await _dbContext.SaveChangesAsync();
            return country;
        }

        public async Task<List<Country>> GetAllCountries()
        {
            return await _dbContext.Countries.ToListAsync();

        }

        public async Task<Country> GetCountryByCountryID(Guid countryID)
        {
            return await _dbContext?.Countries.FirstOrDefaultAsync(temp => temp.CountryID == countryID);
        }

        public async Task<Country> GetCountryByCountryName(string countryName)
        {
            return await _dbContext?.Countries.FirstOrDefaultAsync(temp => temp.CountryName == countryName);
        }
    }
}