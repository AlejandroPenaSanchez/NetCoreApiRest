using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using ApiRestAlejandro.Models;
using System;

namespace ApiRestAlejandro.Controllers{
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        private readonly Context _context;

        public TestController(Context context)
        {
            _context = context;

            if (_context.TodoItems.Count() == 0)
            {
                _context.TodoItems.Add(new TodoItem { Name = "Item1" });
                _context.SaveChanges();
            }
        }

        [HttpGet]
        public List<TodoItem> GetAll()
        {
            return _context.TodoItems.ToList();
        }

        [HttpGet("{id}", Name = "GetById")]
        public IActionResult GetById(int id)
        {
            var item = _context.TodoItems.Find(id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        [HttpPost]
        public IActionResult Create([FromBody] TodoItem item)
        {
            if (item == null) {
                return BadRequest();
            }
            _context.TodoItems.Add(item);
            _context.SaveChanges();

            return CreatedAtRoute("GetById", new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public IActionResult Change(int id, [FromBody] TodoItem item)
        {
            if (item == null || item.Id != id) {
                return BadRequest();
            }

            var todoItem = _context.TodoItems.Find(id);
            if (todoItem == null) {
                return NotFound();
            }

            todoItem.IsComplete = item.IsComplete;
            todoItem.Name = item.Name;

            _context.TodoItems.Update(todoItem);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var todoItem = _context.TodoItems.Find(id);
            if (todoItem == null) {
                return NotFound();
            }

            _context.TodoItems.Remove(todoItem);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpPatch("{id}")]
        public IActionResult Patch(int id, [FromBody] PatchValue property)
        {
            var todoItem = _context.TodoItems.FirstOrDefault(ti => ti.Id == id);

            if (todoItem == null)
            {
                return NotFound();
            }

            //Posible Values
            var name = "Name";
            var isComplete = "IsComplete";

            if (name.Equals(property.Key, StringComparison.InvariantCultureIgnoreCase))
            {
                todoItem.Name = property.Value;
            }
            else if (isComplete.Equals(property.Key, StringComparison.InvariantCultureIgnoreCase))
            {
                todoItem.IsComplete = Convert.ToBoolean(property.Value);
            }
            else
            {
                return NotFound();
            }
           
            //Este no controla los strings
            //switch (property.Key)
            //{
            //    case "Name":
            //        todoItem.Name = property.Value;
            //        break;

            //    case "IsComplete":
            //        todoItem.IsComplete = Convert.ToBoolean(property.Value);
            //        break;

            //    default:
            //        return NotFound();
            //}


            _context.TodoItems.Update(todoItem);
            _context.SaveChanges();

            return new NoContentResult();
        }
    }
}           