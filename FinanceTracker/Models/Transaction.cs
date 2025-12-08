namespace AiFinanceTracker.Models;

public class Transaction
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime Date { get; set; } = DateTime.Today;
    public bool IsIncome { get; set; }
}
