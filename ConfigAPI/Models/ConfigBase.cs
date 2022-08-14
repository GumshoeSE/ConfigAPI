namespace ConfigAPI.Models
{
    public abstract class ConfigBase
    {
        public string Solution { get; set; } = string.Empty;
        public string Customer { get; set; } = string.Empty;

    }
}
