using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_Expense_Tracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project_Expense_Tracker.Controllers
{
    public class ExpensesController : Controller
    {
        private readonly ExpenseTrackerDbContext db;

        public ExpensesController(ExpenseTrackerDbContext db) { this.db = db; }
        public IActionResult Index(DateTime? from, DateTime? to)
        {
            if (from.HasValue && to.HasValue)
            {
                var data = db.Expenses
                    .Include(x => x.ExpenseCategory)
                    .Where(x => x.DateOfExpense.Date >= from.Value.Date && x.DateOfExpense.Date <= to.Value.Date)
                    .ToList();
                return View(data);
            }
            else
            {
                var data = db.Expenses.Include(x => x.ExpenseCategory).ToList();
                return View(data);
            }
        }

        public ActionResult Create()
        {
            ViewBag.Categories = db.ExpenseCategories.ToList();
            return View();
        }

        [HttpPost]

        public ActionResult Create(Expense ex)
        {
            if (ModelState.IsValid)
            {
                db.Expenses.Add(ex);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Categories = db.ExpenseCategories.ToList();
            return View(ex);
        }
        public ActionResult Edit(int id)
        {
            ViewBag.Categories = db.ExpenseCategories.ToList();
            return View(db.Expenses.First(x => x.Id == id));
        }

        [HttpPost]

        public ActionResult Edit(Expense ex)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ex).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Categories = db.ExpenseCategories.ToList();
            return View(ex);
        }
        public ActionResult Delete(int id)
        {
            return View(db.Expenses.Include(x => x.ExpenseCategory).First(x => x.Id == id));
        }

        [HttpPost, ActionName("Delete")]

        public ActionResult ConfirmDelete(int id)
        {
            var ex = new Expense { Id = id };
            db.Entry(ex).State = EntityState.Deleted;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
