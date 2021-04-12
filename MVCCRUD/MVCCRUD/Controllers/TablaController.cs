using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCCRUD.Models;
using MVCCRUD.Models.ViewModels;

namespace MVCCRUD.Controllers
{
    public class TablaController : Controller
    {
        // GET: Tabla
        public ActionResult Index()
        {
            List<ListTablaViewModel> lst;
            using(var db = new CrudEntities())
            {
                 lst = (from d in db.tablas
                          select new ListTablaViewModel
                          {
                              Id = d.id,
                              Nombre = d.nombre,
                              Fecha_Nacimiento = (DateTime)d.fecha_nacimiento,
                              Correo = d.correo
                          }).ToList();
            }

            return View(lst);
        }

        
        public ActionResult Nuevo()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Nuevo(TableViewModel model)
        {
            try
            {
                if (ModelState.IsValid)//validar los formatos en los data annotations
                {
                    using(var db = new CrudEntities())
                    {
                        var oTabla = new tabla()
                        { 
                            correo = model.Correo,
                            fecha_nacimiento = model.Fecha_Nacimiento,
                            nombre = model.Nombre
                        };

                        db.tablas.Add(oTabla);
                        db.SaveChanges();
                    }

                    return Redirect("~/Tabla");
                }
                
                 return View(model);
                
                
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }    
}