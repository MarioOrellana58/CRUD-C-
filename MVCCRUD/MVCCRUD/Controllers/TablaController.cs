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


        public ActionResult Editar(int Id)
        {
            TableViewModel model;
            using(var db = new CrudEntities())
            {
                var oRegistro = db.tablas.Find(Id);
                model = new TableViewModel()
                {
                    Id = oRegistro.id,
                    Nombre = oRegistro.nombre,
                    Correo = oRegistro.correo,
                    Fecha_Nacimiento = (DateTime)oRegistro.fecha_nacimiento
                };
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Editar(TableViewModel model)
        {
            try
            {
                if (ModelState.IsValid)//validar los formatos en los data annotations
                {
                    using (var db = new CrudEntities())
                    {
                        var oRegistro = db.tablas.Find(model.Id);
                        
                        oRegistro.nombre = model.Nombre;
                        oRegistro.correo = model.Correo;
                        oRegistro.fecha_nacimiento = model.Fecha_Nacimiento;


                        db.Entry(oRegistro).State = System.Data.Entity.EntityState.Modified;
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

        [HttpGet]
        public ActionResult Eliminar(int Id)
        {            
            using (var db = new CrudEntities())
            {                
                var oRegistro = db.tablas.Find(Id);
                db.tablas.Remove(oRegistro);
                db.SaveChanges();
            }

            return Redirect("~/Tabla");
        }
    }    
}