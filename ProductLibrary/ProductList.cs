namespace ProductLibrary;

public class ProductList
{
	public List<Product> Products = new();

	public event EventHandler<Product>? Created;
	public event EventHandler<Product>? Readed;
	public event EventHandler<Product>? Updated;
	public event EventHandler<Product>? Deleted;

	public bool Create(Product prd)
	{
		if (!IsValidProduct(prd)) return false;
		
		if (IsValidProductId(prd.Id) &&
		    IsValidProductName(prd.Name) &&
		    IsValidProductQty(prd.Quantity) &&
		    IsValidProductPrice(prd.Price))
		{
			Products.Add(prd);
			Created?.Invoke(this, prd);
			return true;
		}

		return false;
	}

	public bool Read(Product prd)
	{
		if (!IsValidProduct(prd)) return false;

		Readed?.Invoke(this, prd);
		return true;
	}

	public bool Update(int id, string? newName = null, int? newQty = null, double? newPrice = null)
	{
		var product = Products.FirstOrDefault(p => p.Id == id);
		if (product == null) return false;
		
		if (newName != null && IsValidProductName(newName)) product.Name = newName;
		if (newQty.HasValue && IsValidProductQty(newQty.Value)) product.Quantity = newQty.Value;
		if (newPrice.HasValue && IsValidProductPrice(newPrice.Value)) product.Price = newPrice.Value;
		
		Updated?.Invoke(this, product);
		return true;
	}

	public bool Delete(int id)
	{
		var product = Products.FirstOrDefault(p => p.Id == id);
		if (product == null) return false;

		Products.Remove(product);
		Deleted?.Invoke(this, product);
		return true;
	}

	public bool Initialize(string fileName)
	{
		try
		{
			using (StreamReader sr = new StreamReader(fileName))
			{
				string? line;
				int lineNumber = 0;

				Console.WriteLine($"Data read from {fileName}");

				while ((line = sr.ReadLine()) != null)
				{
					lineNumber++;
					string[] fields = line.Split('/');

					if (fields.Length == 4)
					{
						try
						{
							int id = 0;
							string name = fields[1];
							int qty = 0;
							double price = 0.0;

							if (!int.TryParse(fields[0], out id) || !IsValidProductId(id))
							{
								Console.WriteLine(
									$"Line {lineNumber}: Invalid ID format or value '{fields[0]}'. Skipping.");
								continue;
							}

							if (!int.TryParse(fields[2], out qty) || !IsValidProductQty(qty))
							{
								Console.WriteLine(
									$"Line {lineNumber}: Invalid Quantity format or value '{fields[2]}'. Skipping.");
								continue;
							}

							if (!double.TryParse(fields[3], out price) || !IsValidProductPrice(price))
							{
								Console.WriteLine(
									$"Line {lineNumber}: Invalid Price format or value '{fields[3]}'. Skipping.");
								continue;
							}

							Product prd = new Product(id, name, qty, price);

							if (!Create(prd))
							{
								// This block catches any general list validation errors inside Create
								Console.WriteLine(
									$"Line {lineNumber}: Product validation failed within Create method. Not added.");
							}
						}
						catch (FormatException)
						{
							Console.WriteLine($"Line {lineNumber}: Could not convert data due to format error!");
						}
					}
					else
					{
						Console.WriteLine($"There are missing data in file {fileName}");
					}
				}
			}
		}
		catch (FileNotFoundException)
		{
			Console.WriteLine($"File Name {fileName} could not found!");
			return false;
		}
		catch (Exception ex)
		{
			Console.WriteLine($"An unexpected error occurred during file reading: {ex.Message}");
		}

		return true;
	}

	private bool IsValidProduct(Product? prd)
	{
		return prd != null;
	}
	
	private bool IsValidProductId(int id)
	{
		return id >= 0;
	}

	private bool IsValidProductName(string? name)
	{
		return !string.IsNullOrWhiteSpace(name);
	}

	private bool IsValidProductQty(int qty)
	{
		return qty >= 0;
	}

	private bool IsValidProductPrice(double price)
	{
		return price >= 0.0;
	}
}