using ApiChooser.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using ApiChooser.Utils;

namespace ApiChooser
{
    public class ApiExecutor
    {
        private readonly IConfigurationService _configurationService;
        private readonly IApiClient _apiClient;

        public ApiExecutor(IConfigurationService configurationService, IApiClient apiClient)
        {
            _configurationService = configurationService;
            _apiClient = apiClient;
        }

        public async Task ExecuteApi(int option)
        {
            //Utils.Utils utils = new();
            if (_configurationService.Endpoints != null)
            {
                var endpoint = _configurationService.Endpoints.ToList()[option - 1];
                var requestUrl = $"{_configurationService.BaseUrl}{endpoint.Value}?token={_configurationService.Token}";

                try
                {
                    string result = await _apiClient.GetAsync(requestUrl);
                    //utils.LogMessage(result, "Variables.txt");
                    Console.Write(result);
                }
                catch (Exception e)
                {
                    throw new ApplicationException($"Error en la solicitud: {e.Message}", e);

                }             

            }
        }
    }
}
