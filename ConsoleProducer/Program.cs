// See https://aka.ms/new-console-template for more information

using ConsoleProducer.Services;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using ConsoleProducer.Models;
using System.Text.Json;

internal class Program
{
    static async Task Main(string[] args)
    {
        // Configurar la construcción del contenedor de dependencias
        var serviceProvider = new ServiceCollection()            
            .AddSingleton<ProducerService>()
            .AddSingleton<IConfiguration>(provider =>
            {
                var configurationBuilder = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                return configurationBuilder.Build();
            })
            .BuildServiceProvider();

        // Obtener el servicio desde el contenedor de dependencias
        ProducerService? _producerService = serviceProvider.GetService<ProducerService>();

        // Crear un token de cancelación

        using var cts = new CancellationTokenSource();
        var token = cts.Token;

        // Ejecutar el temporizador en una tarea separada
        var timerTask = StartTimerAsync(token);

        Console.WriteLine("Presiona [Enter] para salir del Producer...");
        Console.ReadLine();

        // Cancelar la tarea del temporizador y esperar a que termine
        cts.Cancel();
        await timerTask;

        async Task StartTimerAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                // Ejecutar el método cada segundo
                string hash = GenerateSha256Hash(DateTime.Now.ToString("o"));
                var message = new Message
                {
                    Hash = hash,
                    CreatedDate = DateTime.Now
                };
                string json_message = JsonSerializer.Serialize(message);

                if (_producerService != null)
                {
                    await _producerService.ProduceAsync("KafkaMesssages", json_message);
                }

                Console.WriteLine("SHA256 Hash de la hora actual: " + hash);

                await Task.Delay(1000, token); // Esperar 1 segundo
            }
        }

        string GenerateSha256Hash(string input)
        {
            using var sha256 = SHA256.Create();
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            byte[] hashBytes = sha256.ComputeHash(inputBytes);

            // Convertir el hash a una cadena hexadecimal
            StringBuilder sb = new();
            foreach (var b in hashBytes)
            {
                sb.Append(b.ToString("x2"));
            }

            return sb.ToString();
        }
    }
}