using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Messaging
{
    public class  BaseResponse
    {
        public BaseResponse()
        {
            IsValid = true;
        }
        public bool IsValid { get; set; }

        public string[] ErrorMessages { get; set; }
    }
}
