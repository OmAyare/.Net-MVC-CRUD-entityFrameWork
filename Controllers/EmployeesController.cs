using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CRUD_entityFrameWork.Models;

namespace CRUD_entityFrameWork.Controllers
{
    public class EmployeesController : Controller
    {
        private EmployeeContext db = new EmployeeContext();

        // GET: Employees
        public ActionResult Index()
        {
            var tblEmployees = db.tblEmployees.Include(t => t.tblDepartment);
            return View(tblEmployees.ToList());
        }
         public ActionResult EmployeesByDepartment()
        {
            var departmentTotals = db.tblEmployees.Include(t => t.tblDepartment).GroupBy(x => x.tblDepartment.Name)
                                .Select(y => new DepartmentTotals 
                                { Name= y.Key , Totals = y.Count() }).ToList().OrderByDescending(y => y.Totals);         ;

            return View(departmentTotals);
        }

        // GET: Employees/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblEmployee tblEmployee = db.tblEmployees.Find(id);
            if (tblEmployee == null)
            {
                return HttpNotFound();
            }
            return View(tblEmployee);
        }

        // GET: Employees/Create
        public ActionResult Create()
        {
            ViewBag.DepartmentId = new SelectList(db.tblDepartments, "Id", "Name");
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EmployeeId,Name,Gender,City,DepartmentId")] tblEmployee tblEmployee)
        {
            if (string.IsNullOrEmpty(tblEmployee.Name))
            {
                ModelState.AddModelError("Name", "The Name is Required");
            }

            if (ModelState.IsValid)
            {
                db.tblEmployees.Add(tblEmployee);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DepartmentId = new SelectList(db.tblDepartments, "Id", "Name", tblEmployee.DepartmentId);
            return View(tblEmployee);
        }

        // GET: Employees/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblEmployee tblEmployee = db.tblEmployees.Find(id);
            if (tblEmployee == null)
            {
                return HttpNotFound();
            }
            ViewBag.DepartmentId = new SelectList(db.tblDepartments, "Id", "Name", tblEmployee.DepartmentId);
            return View(tblEmployee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Exclude = "Name")] tblEmployee tblemployee)
        {
            tblEmployee EmployeeFromDb = db.tblEmployees.Single(x => x.EmployeeId == tblemployee.EmployeeId);

            EmployeeFromDb.Gender = tblemployee.Gender;
            EmployeeFromDb.City = tblemployee.City;
            EmployeeFromDb.DepartmentId = tblemployee.DepartmentId;
            if (ModelState.IsValid)
            {
                db.Entry(EmployeeFromDb).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DepartmentId = new SelectList(db.tblDepartments, "Id", "Name", tblemployee.DepartmentId);
            return View(tblemployee);
        }

        // GET: Employees/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblEmployee tblEmployee = db.tblEmployees.Find(id);
            if (tblEmployee == null)
            {
                return HttpNotFound();
            }
            return View(tblEmployee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblEmployee tblEmployee = db.tblEmployees.Find(id);
            db.tblEmployees.Remove(tblEmployee);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
