using ConsoleConsumer.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

internal class Program
{
    static async Task Main(string[] args)
    {
        // Configurar la construcción del contenedor de dependencias
        var serviceProvider = new ServiceCollection()
            .AddLogging(builder =>
            {
                builder.AddConsole();
                builder.SetMinimumLevel(LogLevel.Information);
            })
            .AddSingleton<ConsumerService>()
            .AddSingleton<IConfiguration>(provider =>
            {
                var configurationBuilder = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                return configurationBuilder.Build();
            })
            .BuildServiceProvider();

        // Obtener los servicios desde el contenedor de dependencias
        var logger = serviceProvider.GetRequiredService<ILogger<ConsumerService>>();
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
        var _consumerService = new ConsumerService(configuration, logger);

        // Crear un token de cancelación

        using var cts = new CancellationTokenSource();
        var token = cts.Token;

        // Ejecutar el temporizador en una tarea separada
        var timerTask = StartTimerAsync(token);

        Console.WriteLine("Presiona [Enter] para salir del Consumer...");
        Console.ReadLine();

        // Cancelar la tarea del temporizador y esperar a que termine
        cts.Cancel();
        await timerTask;

        async Task StartTimerAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {               
                if (_consumerService != null)
                {
                   await _consumerService.StartAsync(token);
                }
                await Task.Delay(1000, token); // Esperar 1 segundo
            }
        }
    }
}