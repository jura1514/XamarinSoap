using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App3
{
    public class ToDoItemManager
    {
        ISoapService soapService;

        public ToDoItemManager(ISoapService service)
        {
            soapService = service;
        }

        public Task<string> Authenticate(string userName, string password)
        {
            return soapService.Authenticate(userName, password);
        }
    }
}
