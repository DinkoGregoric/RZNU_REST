using RZNU.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;

namespace RZNU.Controllers
{
    public class UsersController : Controller
    {
        private ApplicationDbContext _context;

        public UsersController() {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing) {
            _context.Dispose();
        }
       
        // GET: Users
        public ActionResult Index()
        {
            var users = _context.Users.Include(c => c.Books).ToList();
            return View(users);
        }

        // GET: Users
        public ActionResult Details(int id) {
            var users = _context.Users.Single(u => u.Id == id);
            return View(users);
        }
    }
}