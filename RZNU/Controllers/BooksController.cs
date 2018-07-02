using RZNU.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RZNU.Controllers
{
    public class BooksController : Controller
    {
        private ApplicationDbContext _context;

        public BooksController() {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing) {
            _context.Dispose();
        }

        // GET: Books
        public ActionResult Index() {
            var books = _context.Books.ToList();
            return View(books);
        }
    }
}