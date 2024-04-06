using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Messaging
{
    public class BaseRequest
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }

        public string Division { get; set; }
        public string ProviderName { get; set; }
        public DateTime InitialDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
