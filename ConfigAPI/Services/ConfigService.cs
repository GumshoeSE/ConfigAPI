using ConfigAPI.Models;
using System.Text.Json;
using System;

namespace ConfigAPI.Services
{
    public class ConfigService<T> : IConfigService<T> where T : ConfigBase
    {
        private readonly string folder = "Configurations";
        public async Task AddAsync(T config, string customer, string solution)
        {
            var fileName = $"{customer}.json";
            var directory = $"{folder}/{solution}";
            var path = $"{directory}/{fileName}";

            if (!Directory.Exists(directory)) 
                Directory.CreateDirectory(directory);

            using var stream = File.Create(path);

            await JsonSerializer.SerializeAsync(stream, config);
            await stream.DisposeAsync();
        }

        public async Task<T?> GetAsync(string solution, string customer)
        {
            T? config = null;
            var fileName = $"{customer}.json";
            var path = $"{folder}/{solution}/{fileName}";

            if (File.Exists(path))
            {
                using var stream = File.OpenRead(path);
                config = await JsonSerializer.DeserializeAsync<T>(stream);
                await stream.DisposeAsync();
            }

            return config;
        }

        public Task Remove(string solution, string customer)
        {
            var fileName = $"{customer}.json";
            var directory = $"{folder}/{solution}";
            var path = $"{directory}/{fileName}";

            File.Delete(path);

            // Delete the directory if empty
            if (!Directory.EnumerateFileSystemEntries(directory).Any())
                Directory.Delete(directory);
            
            return Task.CompletedTask;
        }
    }
}
