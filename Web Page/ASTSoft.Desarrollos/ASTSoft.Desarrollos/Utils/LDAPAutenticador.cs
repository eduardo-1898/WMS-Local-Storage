using System;
using System.Collections;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Threading.Tasks;

namespace ASTSoft.Desarrollos.Utils
{
    public class LDAPAutenticador
    {
        private string _path;
        private string _filterAttribute;
        public string info;
        private ArrayList listaPropiedades = new ArrayList();

        public LDAPAutenticador(string path)
        {
            _path = path;
        }

        public bool autenticado(string dominio, string usuario, string pass)
        {
            bool autenticado = true;

            string acceso = dominio + @"\" + usuario;
            System.DirectoryServices.DirectoryEntry entry = new System.DirectoryServices.DirectoryEntry(_path, usuario, pass);
            entry.AuthenticationType = AuthenticationTypes.Secure;

            try
            {
                object obj = entry.NativeObject;
                DirectorySearcher search = new DirectorySearcher(entry);

                search.Filter = "(SAMAccountName=" + usuario + ")";
                string[] requiredProperties = new string[] { "cn", "givenname", "sn" };
                foreach (String property in requiredProperties)
                    search.PropertiesToLoad.Add(property);

                SearchResult result = search.FindOne();

                if (null == result)
                {
                    autenticado = false;
                    return autenticado;
                }
                else
                {

                    foreach (String property in requiredProperties)
                        foreach (Object myCollection in result.Properties[property])
                            listaPropiedades.Add(myCollection.ToString());
                }

                //Update the new path to the user in the directory.
                _path = result.Path;
                _filterAttribute = (string)result.Properties["cn"][0];
            }
            catch (Exception)
            {
                return false;
            }

            return autenticado;
        }

    }
}