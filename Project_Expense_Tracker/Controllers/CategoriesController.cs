using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_Expense_Tracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project_Expense_Tracker.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ExpenseTrackerDbContext db;

        public CategoriesController(ExpenseTrackerDbContext db) { this.db = db;}
        public IActionResult Index()
        {
            return View(db.ExpenseCategories.ToList());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]

        public IActionResult Create(ExpenseCategory ec)
        {
            if (ModelState.IsValid)
            {
                if (db.ExpenseCategories.Any(x => x.Category.ToLower() == ec.Category.ToLower()))
                {
                    ModelState.AddModelError("", "Duplicate Data cannot be inserted");
                    return View(ec);
                }
                db.ExpenseCategories.Add(ec);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ec);
        }

        public IActionResult Edit(int id)
        {
            return View(db.ExpenseCategories.First(x=> x.Id == id));
        }

        [HttpPost]

        public IActionResult Edit(ExpenseCategory ec)
        {
            if (ModelState.IsValid)
            {                
                db.Entry(ec).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ec);
        }

        public IActionResult Delete(int id)
        {
            return View(db.ExpenseCategories.First(x => x.Id == id));
        }

        [HttpPost, ActionName("Delete")]

        public IActionResult ConfirmDelete(int id)
        {
            var ec = new ExpenseCategory { Id = id };
            db.Entry(ec).State = EntityState.Deleted;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
