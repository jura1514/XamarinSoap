using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App3
{
    public interface ISoapService
    {
        Task<string> Authenticate(string userName, string password);
    }
}
