namespace ApiChooser.Interfaces
{
    public interface IConfigurationService
    {
        string BaseUrl { get; }
        string Token { get; }
        IEnumerable<KeyValuePair<string, string?>> Endpoints { get; }
    }
}
