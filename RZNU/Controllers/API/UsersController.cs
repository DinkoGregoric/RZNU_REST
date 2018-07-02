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
    /// Enables the interaction between your application and my users.
    /// </summary>
    [Authorize]
    public class UsersController : ApiController
    {
        private ApplicationDbContext db;
        /// <summary>
        /// Empty User constructor.
        /// </summary>
        public UsersController() {
            db = new ApplicationDbContext();
        }

        // GET: api/Users
        /// <summary>
        /// Retrieves a list of all users.
        /// </summary>
        /// <returns>Returns 200 OK.</returns>
        public IQueryable<User> GetUsers()
        {
            return db.Users;
        }

        // GET: api/Users/5
        /// <summary>
        /// Retrieves a single user.
        /// </summary>
        /// <param name="id">Specifies the user to retrieve.</param>
        /// <returns>Returns 200 OK if a user exists, otherwise returns 404 Not Found.</returns>
        [ResponseType(typeof(User))]
        public IHttpActionResult GetUser(int id)
        {
            User user = db.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        // PUT: api/Users/5
        /// <summary>
        /// Edits a specified user.
        /// </summary>
        /// <param name="id">Id of the user that's being edited.</param>
        /// <param name="user">The edited user.</param>
        /// <returns>Returns 200 OK. If the request is not valid returns 400 Bad Request. If the user does not exist, returns 404 Not Found.</returns>
        [ResponseType(typeof(void))]
        public IHttpActionResult PutUser(int id, User user)
        {
            user.Id = id;
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != user.Id)
            {
                return BadRequest();
            }

            db.Entry(user).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(user);
        }

        // POST: api/Users
        /// <summary>
        /// Creates a user.
        /// </summary>
        /// <param name="user">The user to be created.</param>
        /// <returns>Returns 201 Created.</returns>
        [ResponseType(typeof(User))]
        public IHttpActionResult PostUser(User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Users.Add(user);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = user.Id }, user);
        }

        // DELETE: api/Users/5

        /// <summary>
        /// Deletes the specified user.
        /// </summary>
        /// <param name="id">Id of the user.</param>
        /// <returns>Returns 204 No Content.</returns>
        
        [ResponseType(typeof(User))]
        public IHttpActionResult DeleteUser(int id)
        {
            User user = db.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            db.Users.Remove(user);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }





        // GET: api/Users/{id}/Books
        /// <summary>
        /// Gets all of the books of a specified user.
        /// </summary>
        /// <param name="id">Id of the user.</param>
        /// <returns>Returns 200 OK.</returns>
        [Route("api/users/{id}/books")]
        public IQueryable<Book> GetUserBooks(int id) {
            return db.Users.Single(c => c.Id == id).Books.AsQueryable();
        }

        // GET: api/Users/{id}/Books/{book_id}
        /// <summary>
        /// Returns the specified book that belongs to the specified user.
        /// </summary>
        /// <param name="id">User Id.</param>
        /// <param name="book_id">Book Id</param>
        /// <returns>Returns 200 OK. If the book does not exist - returns 404 Not Found.</returns>
        [Route("api/users/{id}/books/{book_id}")]
        public IHttpActionResult GetUserBook(int id, int book_id) {
            Book book = null;
            try {
                book = db.Users.Single(c => c.Id == id).Books.Single(b => b.Id == book_id);
            } catch (Exception e) {

                Console.WriteLine(e);
            }
            

            if (book == null) {
                return NotFound();
            }

            return Ok(book);
        }

        /// <summary>
        /// Creates a book and connects it to a user.
        /// </summary>
        /// <param name="id">User Id.</param>
        /// <param name="book">The book object.</param>
        /// <returns></returns>
        [Route("api/users/{id}/books")]
        public IHttpActionResult PostUserBook(int id, Book book) {
            
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var user = db.Users.Single(u => u.Id == id);
            if(user == null) {
                return NotFound();
            }
            db.Books.Add(book);
            user.Books.Add(book);
            db.SaveChanges();

            return CreatedAtRoute("NestedApi", new { first_id = id, second_id = book.Id }, book);
        }
        /// <summary>
        /// Deletes the connection between a user and his/her book.
        /// </summary>
        /// <param name="id">User Id.</param>
        /// <param name="book_id">Book Id.</param>
        /// <returns>Returns 204 No Content. If the user or the book don't exist, returns 404 Not Found.</returns>
        [Route("api/users/{id}/books/{book_id}")]
        public IHttpActionResult DeleteUserBookRelationship(int id, int book_id) {
            var user = db.Users.Single(u => u.Id == id);
            var book = db.Books.Single(b => b.Id == book_id);
            if (user == null || book == null) {
                return NotFound();
            }
            
            user.Books.Remove(book);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }
        /// <summary>
        /// Creates a connection between a book and a user.
        /// </summary>
        /// <param name="id">User Id.</param>
        /// <param name="book_id">Book Id.</param>
        /// <returns>Returns 201 Created on success. On failure returns 404 Not Found (if no user or no book).</returns>
        [Route("api/users/{id}/books/{book_id}")]
        public IHttpActionResult PutUserBook(int id, int book_id) {

            var user = db.Users.Single(u => u.Id == id);
            var book = db.Books.Single(b => b.Id == book_id);
            if (user == null || book == null) {
                return NotFound();
            }
            if (!user.Books.Contains(book)) {
                user.Books.Add(book);
            }

            db.SaveChanges();

            return CreatedAtRoute("NestedApi", new { first_id = id, second_id = book.Id }, new { user_id = user.Id, book_id = book.Id});
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

        private bool UserExists(int id)
        {
            return db.Users.Count(e => e.Id == id) > 0;
        }
    }
}