using Models.Entities;

namespace CondefaMovilAPI.Configuration
{
    public class ConfigDB
    {
        public IConfiguration Configuration { get; }

        public ConnectionString ConfigurarBaseDeDatos()
        {
            var connectionString = new ConnectionString(Configuration.GetConnectionString("defaultConnection"));
            return connectionString;
        }
    }
}
