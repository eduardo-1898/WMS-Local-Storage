using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Messaging.ViewModels.DataTable
{
    public class DataTableResponse<T>
    {
        public int Draw { get; set; }

        public long RecordsTotal { get; set; }

        public long RecordsFiltered { get; set; }

        public IEnumerable Data { get; set; }

        public string Error { get; set; }

        public DataTableResponse()
        {
            Data = new List<T>();
        }
    }
}
