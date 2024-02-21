using Microsoft.EntityFrameworkCore;
using MvcCoreProceduresEF.Data;
using MvcCoreProceduresEF.Models;

namespace MvcCoreProceduresEF.Repositories
{
    public class RepositoryViewEmpleados
    {
        private HospitalContext context;

        public RepositoryViewEmpleados(HospitalContext context)
        {
            this.context = context;
        }

        //realizamos la peticion a la vista de forma asincrona
        //tenemos un metodo llamado ToListAsync dentro de EF
        //que devuelve las listas de var consulta de forma asincrona
        public async Task<List<ViewEmpleado>> GetEmpleadosAsync()
        {
            var consulta = from datos in this.context.ViewEmpleados
                           select datos;

            return await consulta.ToListAsync();
        }
    }
}
