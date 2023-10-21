using HW_15_10_23.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Task = HW_15_10_23.Models.Task;

namespace HW_15_10_23.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly SiteDbContext _context; // контекст бази даних
    private readonly ILogger<TasksController> _logger;

    public TasksController(SiteDbContext context, ILogger<TasksController> logger)
    {
        _context = context;
        _logger = logger;
    }

    //public IActionResult Index()
    //{
    //    return View();
    //}

    // GET: api/Tasks
    // Переглянути список справ на обрану дату
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Task>>> GetTasks([FromQuery] DateTime date)
    {
        var tasks = new List<Models.Task>();
        if (date == null)
        {
            tasks = await _context.Tasks.Where(t => t.DueDate.Date == date.Date).ToListAsync();
        }
        else 
        {
            // фільтруємо справи за датою
            tasks = await _context.Tasks.Where(t => t.DueDate.Date == date.Date).ToListAsync();
        }
       
        return Ok(tasks);
    }

    // GET: api/Tasks/5
    // Переглянути деталі справи за ідентифікатором
    [HttpGet("{id}")]
    public async Task<ActionResult<Task>> GetTask(int id)
    {
        // знаходимо справу за ідентифікатором
        var task = await _context.Tasks.FindAsync(id);

        if (task == null)
        {
            return NotFound(); // якщо справа не знайдена, повертаємо 404
        }

        return Ok(task); // якщо справа знайдена, повертаємо її
    }

    // PUT: api/Tasks/5
    // Редагувати справу за ідентифікатором
    [HttpPut("{id}")]
    public async Task<IActionResult> PutTask(int id, Task task)
    {
        if (id != task.Id)
        {
            return BadRequest(); // якщо ідентифікатори не співпадають, повертаємо 400
        }

        _context.Entry(task).State = EntityState.Modified; // оновлюємо стан справи

        try
        {
            await _context.SaveChangesAsync(); // зберігаємо зміни в базі даних
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!TaskExists(id))
            {
                return NotFound(); // якщо справа не знайдена, повертаємо 404
            }
            else
            {
                throw;
            }
        }

        return NoContent(); // повертаємо 204
    }

    // Перевіряє, чи існує справа з певним ідентифікатором
    private bool TaskExists(int id)
    {
        // Звертаємося до контексту бази даних
        return _context.Tasks.Any(e => e.Id == id);
    }

    // POST: api/Tasks
    // Додати нову справу
    [HttpPost]
    public async Task<ActionResult<Task>> PostTask([FromBody] Task task)
    {
        _logger.LogInformation("Add todo item");
        _context.Tasks.Add(task); // додаємо справу до контексту
        await _context.SaveChangesAsync(); // зберігаємо зміни в базі даних

        //return CreatedAtAction("GetTask", new { id = task.Id }, task); // повертаємо 201 і справу
        return Ok(task);
    }

    // DELETE: api/Tasks/5
    // Видалити справу за ідентифікатором
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTask(int id)
    {
        var task = await _context.Tasks.FindAsync(id); // знаходимо справу за ідентифікатором
        if (task == null)
        {
            return NotFound(); // якщо справа не знайдена, повертаємо 404
        }

        _context.Tasks.Remove(task); // видаляємо справу з контексту
        await _context.SaveChangesAsync(); // зберігаємо зміни в базі даних

        return NoContent(); // повертаємо 204
    }

    // Відмітити справу як виконану (або навпаки не виконану)
    [HttpPatch("{id}/completed")]
    public async Task<IActionResult> PatchTaskCompleted(int id, [FromBody] bool completed)
    {
        var task = await _context.Tasks.FindAsync(id); // знаходимо справу за ідентифікатором
        if (task == null)
        {
            return NotFound(); // якщо справа не знайдена, повертаємо 404
        }

        task.Completed = completed; // змінюємо статус виконання справи
        _context.Entry(task).State = EntityState.Modified; // оновлюємо стан справи

        try
        {
            await _context.SaveChangesAsync(); // зберігаємо зміни в базі даних
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!TaskExists(id))
            {
                return NotFound(); // якщо справа не знайдена, повертаємо 404
            }
            else
            {
                throw;
            }
        }

        return NoContent(); // повертаємо 204
    }

}
