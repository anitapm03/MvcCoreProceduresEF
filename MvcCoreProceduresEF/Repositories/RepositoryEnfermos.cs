using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

using MvcCoreProceduresEF.Data;
using MvcCoreProceduresEF.Models;
using System.Data.Common;

namespace MvcCoreProceduresEF.Repositories
{
    #region procedimientos 
    /*
        CREATE PROCEDURE SP_TODOS_ENFERMOS
        AS
	        SELECT * FROM ENFERMO
        GO

        CREATE PROCEDURE SP_FIND_ENFERMO
        (@INSCRIPCION INT)
        AS
	        SELECT * FROM ENFERMO
	        WHERE INSCRIPCION = @INSCRIPCION
        GO

        CREATE PROCEDURE SP_DELETE_ENFERMO
        (@INSCRIPCION INT)
        AS
	        DELETE FROM ENFERMO 
	        WHERE INSCRIPCION = @INSCRIPCION
        GO

    ALTER PROCEDURE SP_INSERTAR_ENFERMO
    ( @APELLIDO NVARCHAR(30), 
    @DIRECCION NVARCHAR(50), @FECHA DATETIME,
    @S NVARCHAR(10))
    AS
	    DECLARE @MAXINS INT
	    SELECT @MAXINS = MAX(INSCRIPCION) +1 FROM ENFERMO
	    INSERT INTO ENFERMO VALUES(
	    @MAXINS, @APELLIDO, @DIRECCION, @FECHA, @S, 7778)
    GO
         **/
    #endregion
    public class RepositoryEnfermos
    {
        private HospitalContext context;

        public RepositoryEnfermos(HospitalContext context)
        {
            this.context = context;
        }

        public List<Enfermo> GetEnfermos()
        {
            //para consultas de seleccion con los procedimientos 
            //almacenados debemos mapear manualmente los datos 
            //que recibimos
            using (DbCommand com =
                this.context.Database.GetDbConnection().CreateCommand())
            {
                string sql = "SP_TODOS_ENFERMOS";
                com.CommandType = System.Data.CommandType.StoredProcedure;
                com.CommandText = sql;
                //abrimos la conexion a traves de comando
                com.Connection.Open();
                //ejecutamos el reader
                DbDataReader reader = com.ExecuteReader();
                List<Enfermo> enfermos = new List<Enfermo>();
                while (reader.Read())
                {
                    Enfermo enfermo = new Enfermo
                    {
                        Inscripcion = int.Parse(reader["INSCRIPCION"].ToString()),
                        Apellido = reader["APELLIDO"].ToString(),
                        Direccion = reader["DIRECCION"].ToString(),
                        FechaNacimiento = DateTime.Parse(reader["FECHA_NAC"].ToString()),
                        Genero = reader["S"].ToString()
                    };
                    enfermos.Add(enfermo);
                }
                reader.Close();
                com.Connection.Close();
                return enfermos;
            }
        }
        public Enfermo FindEnfermo(int inscripcion)
        {
            //para llamar a procedimientos con parametros 
            //la llamada se realiza incluyendo los param y
            //tbn el nombre del procedure
            string sql = "SP_FIND_ENFERMO @INSCRIPCION";
            //PARA DECLARAR PARAMETROS SE USA LA CLASE SQLPARAMETER
            //DEBEMOS TENER CUIDADO CON EL NAMESPACE 
            // EL NAMESPACE es Microsoft.Data
            SqlParameter pamIns =
                new SqlParameter("@INSCRIPCION", inscripcion);
            //al ser un proc select puedo usar el metodo
            //FromSqlRaw para extraer los datos
            //si mi consulta coincide con un model, puedo usar
            //Linq para mapear los datos
            //cuando tenemos un procedure select, las peticiones
            //se dividen en dos. no puedo hacer Linq y despues un
            //foreach, debemos extraer los datos en dos acciones
            
            var consulta = this.context.Enfermos.FromSqlRaw(sql, pamIns);

            //extraer las entidades de la consulta(ejecutar)
            //para ejecutar necesitamos AsEnumerable()
            Enfermo enfermo = consulta.AsEnumerable().FirstOrDefault();
            return enfermo;

        }

        public void DeleteEnfermo(int inscripcion)
        {
            string sql = "SP_DELETE_ENFERMO @INSCRIPCION";

            SqlParameter pamins = new SqlParameter("@INSCRIPCION", inscripcion);

            //ejecutar consultas de accion se realiza mediante el metodo
            //ExecuteSqlRaw que se accede desde database
            this.context.Database.ExecuteSqlRaw(sql, pamins);


        }

        public void InsertarEnfermo(int inscripcion, string apellido,
            string direccion, DateTime fecha, string s)
        {
            string sql = "SP_INSERTAR_ENFERMO @APELLIDO, @DIRECCION, @FECHA, @S";
            SqlParameter pamape = new SqlParameter("@APELLIDO", apellido);
            SqlParameter pamdir = new SqlParameter("@DIRECCION", direccion);
            SqlParameter pamfecha = new SqlParameter("@FECHA", fecha);
            SqlParameter pams = new SqlParameter("@S", s);

            this.context.Database.ExecuteSqlRaw
                (sql, pamape, pamdir, pamfecha, pams);
        }
    }
}
