using Microsoft.EntityFrameworkCore;
using Project_Expense_Tracker.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Project_Expense_Tracker.Models
{
    public class ExpenseCategory
    {
        public ExpenseCategory()
        {
            this.Expenses = new List<Expense>();
        }
        public int Id { get; set; }

        [Required, StringLength(50), Display(Name ="Category Name")]
        public string Category { get; set; }

        //Navigation
        public virtual ICollection<Expense> Expenses { get; set; }
        
    }

    public class Expense
    {
        public int Id { get; set; }

        [Required, StringLength(100), Display(Name = "Expense Description")]
        public string ExpenseDescription { get; set; }

        [Required, Column(TypeName ="Date")
            , Display(Name ="Expense Date")
            , DisplayFormat(DataFormatString ="0: yyyy-MM-dd")
            , FutureDateValidation(ErrorMessage = "Future date cannot be selected")]
        public DateTime DateOfExpense { get; set; }

        [Required, Column(TypeName = "money")]
        public double Amount { get; set; }

        //Foreign Key
        [Required, ForeignKey("ExpenseCategory"),Display(Name ="Category")]
        public int ExpenseCategoryId { get; set; }

        //Navigation
        public virtual ExpenseCategory ExpenseCategory { get; set; }

    }

    public class ExpenseTrackerDbContext:DbContext
    {
        public ExpenseTrackerDbContext(DbContextOptions<ExpenseTrackerDbContext> options) : base(options) { }

        public DbSet<ExpenseCategory> ExpenseCategories { get; set; }
        public DbSet<Expense> Expenses { get; set; }

    }
}
