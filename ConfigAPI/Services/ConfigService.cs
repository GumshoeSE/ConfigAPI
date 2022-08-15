using ConfigAPI.Models;
using System.Text.Json;
using System;

namespace ConfigAPI.Services
{
    public class ConfigService<T> : IConfigService<T> where T : ConfigBase
    {
        private readonly string folder = "Configurations";
        public async Task<IResult> AddAsync(T config, string customer, string solution)
        {
            var fileName = $"{customer}.json";
            var directory = $"{folder}/{solution}";
            var path = $"{directory}/{fileName}";

            if (!Directory.Exists(directory)) 
                Directory.CreateDirectory(directory);

            using var stream = File.Create(path);

            await JsonSerializer.SerializeAsync(stream, config);
            await stream.DisposeAsync();

            return Results.Created($"/api/lss/{customer}", config);
        }

        public async Task<IResult> GetAsync(string solution, string customer)
        {
            var fileName = $"{customer}.json";
            var path = $"{folder}/{solution}/{fileName}";

            if (!File.Exists(path))
                return Results.NotFound("Configuration file not found");
            
            using var stream = File.OpenRead(path);
            var config = await JsonSerializer.DeserializeAsync<T>(stream);
            await stream.DisposeAsync();
            return Results.Ok(config);
            
        }

        public IResult Remove(string solution, string customer)
        {
            var fileName = $"{customer}.json";
            var directory = $"{folder}/{solution}";
            var path = $"{directory}/{fileName}";

            if (!File.Exists(path))
                return Results.NotFound("Configuration file not found");

            File.Delete(path);

            // Delete the directory if empty
            if (!Directory.EnumerateFileSystemEntries(directory).Any())
                Directory.Delete(directory);

            return Results.Ok();
        }
    }
}
