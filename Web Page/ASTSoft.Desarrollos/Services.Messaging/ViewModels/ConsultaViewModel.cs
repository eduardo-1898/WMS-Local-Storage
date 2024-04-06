using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Services.Messaging.ViewModels
{
	public class ConsultaViewModel
	{
        public DetalleArticulo detalleArticulo { get; set; }
        public List<AlmacenArticulos> almacenArticulos { get; set; }
        public List<BodegasArticulos> bodegasArticulos { get; set; }
        public List<DetalleDatamatrix> detalleDatamatrix { get; set;  }
    }

    public class DetalleArticulo {
        public string Articulo { get; set; } = string.Empty;
        public string ArticuloBusqueda { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public string Lote { get; set; } = string.Empty;
        public string Bodega { get; set; } = string.Empty;
        public string FechaVencimiento { get; set; } = string.Empty;
        public string Localizacion { get; set; } = string.Empty;
        public string Consecutivo { get; set; } = string.Empty;
        public string Documento { get; set; } = string.Empty;
        public string codigo1 { get; set; } = string.Empty;
        public string codigo2 { get; set; } = string.Empty;
        public string cabys { get; set; } = string.Empty;
        public float costo { get; set; }
        public float margen { get; set; }
        public float precioVenta { get; set; } 
        public float iva { get; set; } 
        public int cantidad { get; set; }
        public string codArt { get; set; }

    }

    public class BodegasArticulos {
        public string Bodega { get; set; }
        public string Localizacion { get; set; }
        public string Disponible { get; set; }
        public string Reservado { get; set; }
    }

    public class AlmacenArticulos { 
        public string Propiedades { get; set; }
        public DateTime Fecha { get; set; }
        public int Disponible { get; set; }
    }

    public class DetalleDatamatrix
    {
        public string tipo_mov { get; set; }
        public string doc { get; set; }
        public string bod_org { get; set; }
        public string loc_org { get; set; }
        public string bod_dest { get; set; }
        public string loc_dest { get; set; }
        public DateTime Fecha { get; set; }
        public string Usuario { get; set; }
    }

}
