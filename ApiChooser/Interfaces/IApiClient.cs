using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiChooser.Interfaces
{
    public interface IApiClient
    {
        Task<string> GetAsync(string url);
    }
}
