namespace ProductLibrary;

public class Product
{
	private const int IdFieldWidth = -8;
	private const int NameFieldWidth = -20;
	private const int QtyFieldWidth = 8;
	private const int PriceFieldWidth = 8;
	private const int TotalFieldWidth = 10;
	
	public int Id { get; set; }
	public string? Name { get; set; }
	public int Quantity { get; set; }
	public double Price { get; set; }
	// ReSharper disable once MemberCanBePrivate.Global
	public double Total() => Price * Quantity;
	
	public Product(){}

	public Product(int id, string? name, int qty, double price)
	{
		Id = id;
		Name = name;
		Quantity = qty;
		Price = price;
	}

	public string Info => $"{Id, IdFieldWidth} {Name, NameFieldWidth} {Quantity, QtyFieldWidth} {Price.ToString("F2") + "$", PriceFieldWidth} {Total().ToString("F2") + "$", TotalFieldWidth}";

	public string Header => $"{"ID", IdFieldWidth} {"Name", NameFieldWidth} {"Quantity", QtyFieldWidth} {"Price", PriceFieldWidth} {"Total", TotalFieldWidth}";
}