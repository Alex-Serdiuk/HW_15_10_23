namespace react.Models;

public class Task
{
    public int Id { get; set; } // ідентифікатор справи
    public string Title { get; set; } // назва справи
    public string? Description { get; set; } // опис справи
    public DateTime DueDate { get; set; } // термін виконання справи
    public bool Completed { get; set; } // чи виконана справа
}
