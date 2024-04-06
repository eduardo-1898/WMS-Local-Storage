using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Messaging
{
    public class ContractResponse <T> where T : class
    {
        /// <summary>
        /// Result set of information to return
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// Error handling from the database
        /// </summary>
       // public EnterpriseException Error { get; set; }

        /// <summary>
        /// IF true, this means the response is valid
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        /// The login result message
        /// </summary>
        public string[] ErrorMessages { get; set; }

        /// <summary>
        /// The login result message
        /// </summary>
        public string[] Messages { get; set; }
    }
}
