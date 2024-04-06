using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Messaging.ViewModels.DataTable
{
    public class DataTableModelRequest<T> where T : class
    {

        public int Draw { get; set; }

        public IList<DataTableModelColumns> Columns { get; set; }

        public int Start { get; set; }

        public int Length { get; set; }

        public DataTableModelSearch Search { get; set; }

        public IList<DataTableModelOrder> Order { get; set; }

        public DataTableModelRequest()
        {
            Columns = new List<DataTableModelColumns>();
            Order = new List<DataTableModelOrder>();

        }

        public T Filter { get; set; }
    }
}
