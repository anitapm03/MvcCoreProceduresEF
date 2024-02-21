using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MvcCoreProceduresEF.Data;
using MvcCoreProceduresEF.Models;

namespace MvcCoreProceduresEF.Repositories
{
    #region procedures
    /*CREATE PROCEDURE SP_TRABAJADORES_OFICIO
        (@OFICIO NVARCHAR(50), 
        @PERSONAS INT OUT,
        @MEDIA INT OUT,
        @SUMA INT OUT)
        AS
	        SELECT * FROM V_TRABAJADORES
	        WHERE OFICIO = @OFICIO

	        SELECT @PERSONAS = COUNT(IDTRABAJADOR),
	        @MEDIA = AVG(SALARIO), @SUMA = SUM(SALARIO)
	        FROM V_TRABAJADORES WHERE OFICIO = @OFICIO
        GO*/
    #endregion
    public class RepositoryTrabajadores
    {

        private HospitalContext context;

        public RepositoryTrabajadores(HospitalContext context)
        {
            this.context = context;
        }

        public async Task<List<Trabajador>> GetTrabajadoresAsync()
        {
            var consulta = from datos in this.context.Trabajadores
                           select datos;
            return await consulta.ToListAsync();
        }

        public async Task<List<string>> GetOficiosAsync()
        {
            var consulta = (from datos in this.context.Trabajadores
                           select datos.Oficio).Distinct();
            return await consulta.ToListAsync();
        }

        public async Task<TrabajadoresModel>
            GetTrabajadoresOficioAsync(string oficio)
        {
            //la unica diferencia es que debemos incluir la palabra
            //out en cada param de salida
            // SP_NOMBRE @P1, @P2 OUT
            string sql = "SP_TRABAJADORES_OFICIO @OFICIO, @PERSONAS OUT," +
                " @MEDIA OUT, @SUMA OUT";
            SqlParameter pamOficio = new SqlParameter("@OFICIO", oficio);
            //los param de salida siempre llevaran un valor por defecto
            SqlParameter pamPersonas = new SqlParameter("@PERSONAS", -1);
            SqlParameter pamMedia = new SqlParameter("@MEDIA", -1);
            SqlParameter pamSuma = new SqlParameter("@SUMA", -1);

            //indicamos la direccion de los parametros out
            pamPersonas.Direction = System.Data.ParameterDirection.Output;
            pamMedia.Direction = System.Data.ParameterDirection.Output;
            pamSuma.Direction = System.Data.ParameterDirection.Output;

            //ejecutamos la consulta de seleccion
            var consulta = this.context.Trabajadores.FromSqlRaw
                (sql, pamOficio, pamPersonas, pamMedia, pamSuma);
            //creamos nuestro model para recuperar los datos
            TrabajadoresModel model = new TrabajadoresModel();

            //los parametros se recuperan despues de extraer los
            //datos del select (cuando se cierra el reader)
            model.Trabajadores = await consulta.ToListAsync();
            model.Personas = int.Parse(pamPersonas.Value.ToString());
            model.MediaSalarial = int.Parse(pamMedia.Value.ToString());
            model.SumaSalarial = int.Parse(pamSuma.Value.ToString());
            return model;
        }
    }
}
