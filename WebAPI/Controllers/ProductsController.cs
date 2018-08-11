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
    public interface IProducts
    {
        IEnumerable<Products> GetAll();
        Products GetByID(int id);
        void Insert(Products product);
        void Update(int id, Products product);
        void Delete(int id);
        bool Exists(int id);
    }

    public class ProductsRepository : IProducts
    {
        private DBModel db = new DBModel();
        public IEnumerable<Products> GetAll()
        {
             return db.Products;;
        }

        public Products GetByID(int id)
        {
           Products products = db.Products.Find(id);
           return products;
        }

        public void Update (int id, Products product)
        {
            db.Entry(product).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void Insert(Products product)
        {
            db.Products.Add(product);
            db.SaveChanges();
        }
        public bool Exists(int id)
        {
            return db.Products.Count(e => e.ID == id) > 0;
        }

        public void Delete(int id)
        {
            Products products = db.Products.Find(id);
            db.Products.Remove(products);
            db.SaveChanges();
        }
    }
    public class ProductsController : ApiController
    {
        IProducts _repository;

        public ProductsController(IProducts repository)
        {
            _repository = repository;
        }

        public IEnumerable<Products> GetProducts()
        {
            var details = _repository.GetAll();
            return details;
        }

        [ResponseType(typeof(Products))]
        public IHttpActionResult GetProducts(int id)
        {
            var details = _repository.GetByID(id);
            if (details == null)
            {
                return NotFound();
            }

            return Ok(details);
        }
        [ResponseType(typeof(void))]
        public IHttpActionResult PutProducts(int id, Products products)
        {
            if (id != products.ID)
            {
                return BadRequest();
            }
            try 
            {
                _repository.Update(id, products);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductsExists(id))
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
        bool ProductsExists(int id)
        {
            return _repository.Exists(id);
        }

         [ResponseType(typeof(Products))]
        public IHttpActionResult PostProducts(Products products)
        {
            _repository.Insert(products);
            return CreatedAtRoute("DefaultApi", new { id = products.ID }, products);
        }

        [ResponseType(typeof(Products))]
         public IHttpActionResult DeleteProducts(int id)
         {
             _repository.Delete(id);
             return Ok();
         }

    }
}
