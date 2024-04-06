using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Messaging.ViewModels
{
    public class ModelSelect
    {
        public string descripcion { get; set; }
        public string id { get; set; }
        public List<SelectListItem> ListComentarios { get; set; }
        public List<SelectListItem> ListRazonNumeroEquivocado { get; set; }

    }
}
