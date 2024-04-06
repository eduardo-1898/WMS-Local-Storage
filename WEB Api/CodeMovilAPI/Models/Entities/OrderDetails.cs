using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities
{
    public class OrderDetails
    {
        public int Id {  get; set; }
        public string? Pedido { get; set; }
        public string? Articulo { get; set; }
        public int Linea { get; set; }
        public string? Bod { get; set; }
        public string? Loc { get; set; }
        public string? Traza { get; set; }
        public string? Tipo { get; set; }
        public int CantidadPedido { get; set; }
        public int CantidadEscaneado { get; set; }
        public string? Producto { get; set; }
        public int Renglon { get; set; }
        public int renglon2 { get; set; }
        public int Cantidad { get; set; }
        public string? bocon { get; set; }
        public string? trazable { get; set; }
        public int cantidadAlmacenes { get; set; }
    }
}
