using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MvcCoreProceduresEF.Data;
using MvcCoreProceduresEF.Models;

namespace MvcCoreProceduresEF.Repositories
{
    public class RepositoryDoctores
    {
        private EnfermosContext context;

        public RepositoryDoctores (EnfermosContext context)
        {
            this.context = context;
        }   

        public List<string> GetEspecialidades()
        {
            string sql = "SP_GET_DOCS";

            var consulta = this.context.Doctores.FromSqlRaw(sql);

            List<Doctor> doctors = consulta.AsEnumerable().ToList();
            List<string> esp = new List<string>();
            foreach (var doctor in doctors)
            {
                esp.Add(doctor.Especialidad);
            }

            return esp;
        }

        public List<Doctor> Incrementar
            (string especialidad, int incremento)
        {
            string sql = "SP_INCREMENTAR_SALARIOS @ESPECIALIDAD, @INCREMENTO";
            SqlParameter pamesp = new SqlParameter("@ESPECIALIDAD", especialidad);
            SqlParameter paminc = new SqlParameter("@INCREMENTO", incremento);

            var consulta = 
                this.context.Doctores.FromSqlRaw(sql, pamesp, paminc);

            List<Doctor> doctores = consulta.AsEnumerable().ToList();
            return doctores;
        }
    }
}
