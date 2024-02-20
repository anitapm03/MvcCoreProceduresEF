using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcCoreProceduresEF.Models
{
    [Table("DOCTOR")]
    public class Doctor
    {
        [Key]
        [Column("HOSPITAL_COD")]
        public int idHospital { get; set; }

        [Column("DOCTOR_NO")]
        public int idDoctor { get; set; }

        [Column("APELLIDO")]
        public string Apellido { get; set; }

        [Column("ESPECIALIDAD")]
        public string Especialidad { get; set; }

        [Column("SALARIO")]
        public int Salario { get; set; }
    }
}
