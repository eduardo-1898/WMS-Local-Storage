using Services.Messaging.ViewModels.AST_DocumentosEmpaque;
using Services.Messaging.ViewModels.AST_DocumentosEmpaqueD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IServices
{
    public interface IAST_DocumentosEmpaqueServices
    {
        public AST_DocumentosEmpaqueView createDocument(string usuario);
        public List<AST_DocumentosEmpaqueView> getDocuments();
        public AST_DocumentosEmpaqueView getDocument(long id);
        public List<AST_DocumentosEmpaqueDView> getPedidos(long idDocumento);
        public bool addNewOrder(AST_DocumentosEmpaqueDView document);
        public bool deleteOrder(long id);

    }
}
