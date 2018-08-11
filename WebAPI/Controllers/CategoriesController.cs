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
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public interface ICategories
    {
        IEnumerable<Categories> GetAll();
        Categories GetByID(string id);
        void Insert(Categories category);
        void Update(string id, Categories category);
        void Delete(string id);
        bool Exists(string id);
    }

    public class CategoriesRepository : ICategories
    {
        private DBModel db = new DBModel();
        public IEnumerable<Categories> GetAll()
        {
            return db.Categories; ;
        }

        public Categories GetByID(string id)
        {
            Categories category = db.Categories.Find(id);
            return category;
        }

        public void Update(string id, Categories category)
        {
            db.Entry(category).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void Insert(Categories category)
        {
            db.Categories.Add(category);
            db.SaveChanges();
        }
        public bool Exists(string id)
        {
            return db.Categories.Count(e => e.Name == id) > 0;
        }

        public void Delete(string id)
        {
            Categories category = db.Categories.Find(id);
            db.Categories.Remove(category);
            db.SaveChanges();
        }
    }
    public class CategoriesController : ApiController
    {
        ICategories  _repository;

        public CategoriesController(ICategories repository)
        {
            _repository = repository;
        }

        public IEnumerable<Categories> GetCategories()
        {
            var details = _repository.GetAll();
            return details;
        }

        [ResponseType(typeof(Categories))]
        public IHttpActionResult GetCategories(string id)
        {
            var details = _repository.GetByID(id);
            if (details == null)
            {
                return NotFound();
            }

            return Ok(details);
        }
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCategories(string id, Categories categories)
        {
            if (id != categories.Name)
            {
                return BadRequest();
            }
            try
            {
                _repository.Update(id, categories);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoriesExists(id))
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
        bool CategoriesExists(string id)
        {
            return _repository.Exists(id);
        }

        [ResponseType(typeof(Categories))]
        public IHttpActionResult PostCategories(Categories categories)
        {
            _repository.Insert(categories);
            return CreatedAtRoute("DefaultApi", new { id = categories.Name }, categories);
        }

        [ResponseType(typeof(Categories))]
        public IHttpActionResult DeleteCategories(string id)
        {
            _repository.Delete(id);
            return Ok();
        }

    }
}