using Microsoft.AspNetCore.Mvc;
using MvcCoreProceduresEF.Models;
using MvcCoreProceduresEF.Repositories;

namespace MvcCoreProceduresEF.Controllers
{
    public class EnfermosController : Controller
    {
        private RepositoryEnfermos repo;

        public EnfermosController (RepositoryEnfermos repo)
        {
            this.repo = repo;
        }

        public IActionResult Index()
        {
            List<Enfermo> enfermos = this.repo.GetEnfermos();
            return View(enfermos);
        }

        public IActionResult IndexView()
        {
            List<Enfermo> enfermos = this.repo.GetEnfermos();
            return View(enfermos);
        }

        public IActionResult Details(int inscripcion)
        {
            Enfermo enfermo = this.repo.FindEnfermo(inscripcion);
            return View(enfermo);
        }

        public IActionResult Delete(int inscripcion)
        {
            this.repo.DeleteEnfermo(inscripcion);
            return RedirectToAction("IndexView");
        }

        public IActionResult Insertar()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Insertar(Enfermo enfermo)
        {
            this.repo.InsertarEnfermo(enfermo.Inscripcion, enfermo.Apellido,
                enfermo.Direccion, enfermo.FechaNacimiento, enfermo.Genero);
            return RedirectToAction("IndexView");
        }
    }
}
