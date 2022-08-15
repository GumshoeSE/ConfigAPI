using ConfigAPI.Models;

namespace ConfigAPI.Services
{
    public interface IConfigService<T> where T : ConfigBase
    {
        public Task<IResult> AddAsync(T config, string customer, string solution);
        public Task<IResult> GetAsync(string solution, string customer);
        public IResult Remove(string solution, string customer);
    }
}
