using ConfigAPI.Models;

namespace ConfigAPI.Services
{
    public interface IConfigService<T> where T : ConfigBase
    {
        public Task AddAsync(T config, string customer, string solution);
        public Task<T?> GetAsync(string solution, string customer);
        public Task Remove(string solution, string customer);
    }
}
