using System.Collections.Generic;
using System.Linq;
using FormsModel;
using FormsModel.FormDTOs;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace WorkflowNetAPI.Controllers
{
    [Route("api/[controller]")]
    public class FormsController : Controller
    {
        private readonly FormsContext _context;

        public FormsController(FormsContext context)
        {
            _context = context;
        }
        // GET: api/forms
        [HttpGet]
        public IEnumerable<Form> Get()
        {
            return _context.Forms.ToList();
        }

        // GET api/forms/5
        [HttpGet("{id}")]
        public Form Get(int id)
        {
            
            return _context.Forms.First(i=>i.Id == id);
        }

        // POST api/forms
        [HttpPost]
        
        public void Post([FromBody]Form value)
        {

            _context.Forms.Add(value);
            _context.SaveChanges();
           
        }

        // PUT api/forms/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]Form value)
        {
            value.Id = id;
            _context.Forms.Update(value);
            _context.SaveChanges();
           
        }

        // DELETE api/forms/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _context.Forms.Remove(_context.Forms.First(i=>i.Id == id));
            _context.SaveChanges();
        }
    }
}
