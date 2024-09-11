using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiChooser.Interfaces
{
    public interface IConfigurationService
    {
        string BaseUrl { get; }
        string Token { get; }
        IEnumerable<KeyValuePair<string, string?>> Endpoints { get; }
    }
}
