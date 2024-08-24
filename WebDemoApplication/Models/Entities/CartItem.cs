namespace WebDemoApplication.Models.Entities;

public class CartItem
{
    public int Id { get; set; }
    public Guid CartNumber { get; set; }
    public Guid BallId { get; set; }
    public int Quantity { get; set; }
    public Ball Ball { get; set; }
}
