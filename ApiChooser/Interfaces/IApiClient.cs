namespace ApiChooser.Interfaces
{
    public interface IApiClient
    {
        Task<string> GetAsync(string url);
    }
}
