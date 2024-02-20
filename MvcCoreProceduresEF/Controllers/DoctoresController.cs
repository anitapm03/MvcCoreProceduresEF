using Microsoft.AspNetCore.Mvc;
using MvcCoreProceduresEF.Models;
using MvcCoreProceduresEF.Repositories;

namespace MvcCoreProceduresEF.Controllers
{
    public class DoctoresController : Controller
    {
        private RepositoryDoctores repo;

        public DoctoresController(RepositoryDoctores repo)
        {
            this.repo = repo;
        }

        public IActionResult IncrementarSalarios()
        {
            List<string> especialidades = this.repo.GetEspecialidades();
            ViewData["ESPECIALIDADES"] = especialidades;
            return View();
        }
        [HttpPost]
        public IActionResult IncrementarSalarios
            (string especialidad, int incremento)
        {
            List<string> especialidades = this.repo.GetEspecialidades();
            ViewData["ESPECIALIDADES"] = especialidades;
            List<Doctor> afectados = this.repo.Incrementar(especialidad, incremento);
            return View(afectados);
        }
    }
}
