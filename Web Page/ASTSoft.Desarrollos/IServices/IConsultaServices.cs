using Services.Messaging.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IServices
{
    public interface IConsultaServices
    {
        public DetalleArticulo ObtenerArticulo(string articulo);
        public List<AlmacenArticulos> ObtenerDetalleArticulo(string articulo);
        public List<DetalleDatamatrix> ObtenerHistorialDatamatrix(string articulo);
        public List<BodegasArticulos> ObtenerBodegasArticulo(string articulo);
    }
}
