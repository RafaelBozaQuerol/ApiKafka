using ApiChooser.Interfaces;
using ApiChooser.Services;
using ApiChooser;
using Microsoft.Extensions.Configuration;
using ApiChooser.Utils;

internal class Program
{

    static async Task Main(string[] args)
    {
        Utils utils = new();

        Console.WriteLine(" -- Presione Ctrl + C para salir en cualquier momento. -- ");

        IConfigurationBuilder builder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
        IConfigurationRoot configuration = builder.Build();
        IConfigurationService configurationService = new ConfigurationService(configuration);

        HttpClient httpClient = new HttpClient
        {
            Timeout = TimeSpan.FromMinutes(5),
            DefaultRequestHeaders = { ExpectContinue = false }
        };

        IApiClient apiClient = new ApiClient(httpClient);
        ApiExecutor apiExecutor = new ApiExecutor(configurationService, apiClient);

        List<int> options = Enumerable.Range(1, configurationService.Endpoints.Count()).ToList();

        while (true)
        {
            if(options.Count > 0) 
            {                
                string message = utils.MensajeApisDisponibles(configurationService.Endpoints.Count());
                Console.WriteLine(message);
                string? line = Console.ReadLine();
                if (line != null && line.Length == 1 && int.TryParse(line, out int selectedOption) && options.Contains(selectedOption))
                {
                    Console.WriteLine("Opción válida");
                    await apiExecutor.ExecuteApi(selectedOption);
                    break;
                }
                else
                {
                    Console.WriteLine("El valor seleccionado no es una opción válida");
                }
            }
            else
            {
                Console.WriteLine("No hay ninguna api configurada, revise su fichero de configuración (appsettings.json)");
                break;
            }

        }        
    }
}