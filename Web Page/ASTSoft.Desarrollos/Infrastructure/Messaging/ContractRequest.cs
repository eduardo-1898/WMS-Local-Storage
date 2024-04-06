using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Messaging
{
    public class ContractRequest <T> where T: class
    {
        /// <summary>
        /// Gets or sets the information to send in the request if type BaseDTO
        /// </summary>
        public T Data { get; set; }

        public int? Page { get; set; }

        public int? PageSize { get; set; }

        public string Search { get; set; }

        public string OrderBy { get; set; }

        public bool OrderDesc { get; set; }
    }
}
