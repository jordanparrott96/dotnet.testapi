using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    [Route("api/TodoItems")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        private readonly TodoContext _context;

        public TodoItemsController(TodoContext context)
        {
            _context = context;
        }

        // GET: api/TodoItems?id=5
        [HttpGet()]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodoItem(long? id)
        {
            if (id == null) return await _context.ToDoItems.ToListAsync();
            
            var todoItem = await _context.ToDoItems.FindAsync(id);
            if (todoItem == null) return NotFound();

            return new[] { todoItem };
        }

        // PUT: api/TodoItems?id=5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut()]
        public async Task<IActionResult> PutTodoItem(long? id, TodoItem todoItem)
        {
            if (id == null) return BadRequest();

            var matchingTodoItem = await _context.ToDoItems.FindAsync(id);
            if (matchingTodoItem == null) return NotFound();

            matchingTodoItem.Name = todoItem.Name;
            matchingTodoItem.IsComplete = todoItem.IsComplete;

            _context.Entry(matchingTodoItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (TodoItemExists((long) id)) throw;
                return NotFound();
            }

            return NoContent();
        }

        // POST: api/TodoItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TodoItem>> PostTodoItem(TodoItem todoItem)
        {
            _context.ToDoItems.Add(todoItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTodoItem), new { id = todoItem.Id }, todoItem);
        }

        // DELETE: api/TodoItems?id=5
        [HttpDelete()]
        public async Task<IActionResult> DeleteTodoItem(long? id)
        {
            if (id == null) return BadRequest();

            var todoItem = await _context.ToDoItems.FindAsync(id);
            if (todoItem == null) return NotFound();

            _context.ToDoItems.Remove(todoItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TodoItemExists(long id)
        {
            return _context.ToDoItems.Any(e => e.Id == id);
        }
    }
}
