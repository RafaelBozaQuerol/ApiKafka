using ApiChooser.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiChooser.Services
{
    public class ConfigurationService : IConfigurationService
    {
        public string? BaseUrl { get; }
        public string? Token { get; }
        public IEnumerable<KeyValuePair<string, string?>> Endpoints { get; }

        public ConfigurationService(IConfiguration configuration)
        {
            Endpoints = configuration.GetSection("EndPoints").AsEnumerable().Where(x => x.Value != null).OrderBy(x => x.Key);
            BaseUrl = configuration.GetSection("base_url").Value;
            Token = configuration.GetSection("token").Value;
        }
    }
}
