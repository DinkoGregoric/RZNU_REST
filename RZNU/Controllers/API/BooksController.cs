using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using RZNU.Models;

namespace RZNU.Controllers.API
{
    /// <summary>
    /// Enables the interaction between your application and my books.
    /// </summary>
    [Authorize]
    public class BooksController : ApiController
    {
        private ApplicationDbContext db;

        /// <summary>
        /// Books empty constructor.
        /// </summary>
        public BooksController() {
            db = new ApplicationDbContext();
        }

        // GET: api/Books
        /// <summary>
        /// Gets all books in application.
        /// </summary>
        /// <returns>Returns 200 OK.</returns>
        public IQueryable<Book> GetBooks()
        {
            return db.Books;
        }

        // GET: api/Books/5


        /// <summary>
        /// Gets the id-specified book.
        /// </summary>
        /// <param name="id"> Defines which book to get.</param>
        /// <returns>Returns 200 OK.</returns>
        
        [ResponseType(typeof(Book))]
        public IHttpActionResult GetBook(int id)
        {
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return NotFound();
            }

            return Ok(book);
        }

        // PUT: api/Books/5

        /// <summary>
        /// Edits the id-specified book.
        /// </summary>
        /// <param name="id">Defines the book to edit.</param>
        /// <param name="book">Defines the changes to the book.</param>
        /// <returns>Returns 204 No Content.</returns>
        
        [ResponseType(typeof(void))]
        public IHttpActionResult PutBook(int id, Book book)
        {
            book.Id = id;
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != book.Id)
            {
                return BadRequest();
            }

            db.Entry(book).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Books


        /// <summary>
        /// Creates a book.
        /// </summary>
        /// <param name="book">The book which is being created.</param>
        /// <returns>Returns 201 Created.</returns>
        
        [ResponseType(typeof(Book))]
        public IHttpActionResult PostBook(Book book)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Books.Add(book);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = book.Id }, book);
        }

        // DELETE: api/Books/5
        /// <summary>
        /// Deletes the id-specified book.
        /// </summary>
        /// <param name="id">Defines the book to delete.</param>
        /// <returns>Returns 204 No Content.</returns>
        [ResponseType(typeof(Book))]
        public IHttpActionResult DeleteBook(int id)
        {
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return NotFound();
            }

            db.Books.Remove(book);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Overrides the original <i>Dispose</i> method.
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BookExists(int id)
        {
            return db.Books.Count(e => e.Id == id) > 0;
        }
    }
}