using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Diagnostics.Metrics;
using static System.Collections.Specialized.BitVector32;

public class Customer
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public DateTime BirthDate { get; set; }
}

public class NewCountry
{
    public string CountryName { get; set; }
    public string Code { get; set; }
}

public class NewCity
{
    public string CityName { get; set; }
    public int CountryId { get; set; }
}

public class NewSection
{
    public string Name { get; set; }
    public string Description { get; set; }
}

public class NewProduct
{
    public string PName { get; set; }
    public decimal Price { get; set; }
    public int SectionId { get; set; }
}

public class Program
{
    static void Main(string[] args)
    {
        
        string connectionString = "Data Source=localhost;Initial Catalog=MailingList;Integrated Security=True";


        // Task 1

        var newCustomer = new Customer
        {
            FirstName = "Саша",
            LastName = "Осипенко",
            Email = "sanya.osip@gmail.com",
            BirthDate = new DateTime(2004, 27, 11)
        };

        using (var connection = new SqlConnection(connectionString))
        {
            const string sqlCustomer = @"INSERT INTO Customers (FirstName, LastName, Email, BirthDate)
                        VALUES (@FirstName, @LastName, @Email, @BirthDate)";

            connection.Execute(sqlCustomer, newCustomer);
        }


        var newCountry = new NewCountry
        {
            CountryName = "Україна",
            Code = "UA"
        };

        using (var connection = new SqlConnection(connectionString))
        {
            const string sqlCountry = @"INSERT INTO Countries (CountryName, Code)
                        VALUES (@CountryName, @Code)";

            connection.Execute(sqlCountry, newCountry);
        }

        var newCity = new NewCity
        {
            CityName = "Київ",
            CountryId = 1 
        };

        using (var connection = new SqlConnection(connectionString))
        {
            const string sqlCity = @"INSERT INTO Cities (CityName, CountryId)
                        VALUES (@CityName, @CountryId)";

            connection.Execute(sqlCity, newCity);
        }

        var newSection = new NewSection
        {
            Name = "Новини",
            Description = "Останні новини та події"
        };

        using (var connection = new SqlConnection(connectionString))
        {
            const string sqlSection = @"INSERT INTO Sections (Name, Description)
                        VALUES (@Name, @Description)";

            connection.Execute(sqlSection, newSection);
        }


        var newProduct = new NewProduct
        {
            PName = "IPhone 15",
            Price = 1600,
            SectionId = 1
        };

        using (var connection = new SqlConnection(connectionString))
        {
            const string sqlProduct = @"INSERT INTO Products (PName, Price, SectionId)
                        VALUES (@PName, @Price, @SectionId)";

            connection.Execute(sqlProduct, newProduct);
        }

        Console.WriteLine("Дані успішно вставлені!");


        //Task 2

        using (var connection = new SqlConnection(connectionString))
        {
            var customerToUpdate = connection.QuerySingle<Customer>("SELECT * FROM Customers WHERE Id = @Id", new { Id = 1 });
            customerToUpdate.FirstName = "Настя";

            connection.Execute("UPDATE Customers SET FirstName = @FirstName, LastName = @LastName, Email = @Email, BirthDate = @BirthDate WHERE Id = @Id", customerToUpdate);
        }

        using (var connection = new SqlConnection(connectionString))
        {
            var countryToUpdate = connection.QuerySingle<NewCountry>("SELECT * FROM Countries WHERE Id = @Id", new { Id = 1 });

            countryToUpdate.CountryName = "Україна";

            connection.Execute("UPDATE Countries SET Name = @CountryName, Code = @Code WHERE Id = @Id", countryToUpdate);
        }


        using (var connection = new SqlConnection(connectionString))
        {
            var cityToUpdate = connection.QuerySingle<NewCity>("SELECT * FROM Cities WHERE Id = @Id", new { Id = 1 });

            cityToUpdate.CityName = "Odesa";

            connection.Execute("UPDATE Cities SET Name = @CityName, CountryId = @CountryId WHERE Id = @Id", cityToUpdate);
        }

        using (var connection = new SqlConnection(connectionString))
        {
            var sectionToUpdate = connection.QuerySingle<NewSection>("SELECT * FROM Sections WHERE Id = @Id", new { Id = 1 });

            sectionToUpdate.Name = "Новини";

            connection.Execute("UPDATE Sections SET Name = @Name, Description = @Description WHERE Id = @Id", sectionToUpdate);
        }


        using (var connection = new SqlConnection(connectionString))
        {
            var productToUpdate = connection.QuerySingle<NewProduct>("SELECT * FROM Products WHERE Id = @Id", new { Id = 1 });

            productToUpdate.PName = "IPhone 15";
            productToUpdate.Price = 1599;

            connection.Execute("UPDATE Products SET Name = @PName, Price = @Price, SectionId = @SectionId WHERE Id = @Id", productToUpdate);
        }



        //Task3

        using (var connection = new SqlConnection(connectionString))
        {
            connection.Execute("DELETE FROM NewCustomers WHERE Id = @Id", new { Id = 1 });
        }

        using (var connection = new SqlConnection(connectionString))
        {
            connection.Execute("DELETE FROM NewCountries WHERE Id = @Id", new { Id = 1 });
        }

        using (var connection = new SqlConnection(connectionString))
        {
            connection.Execute("DELETE FROM NewCities WHERE Id = @Id", new { Id = 1 });
        }

        using (var connection = new SqlConnection(connectionString))
        {
            connection.Execute("DELETE FROM NewSections WHERE Id = @Id", new { Id = 1 });
        }

        using (var connection = new SqlConnection(connectionString))
        {
            connection.Execute("DELETE FROM NewProducts WHERE Id = @Id", new { Id = 1 });
        }


        //Task4

        using (var connection = new SqlConnection(connectionString))
        {
            var cities = connection.Query<NewCity>("SELECT * FROM NewCities WHERE CountryName = @CountryName", new { CountryName = "Україна" });

            foreach (var city in cities)
            {
                Console.WriteLine($"{city.CityName}");
            }
        }

        using (var connection = new SqlConnection(connectionString))
        {
            var sections = connection.Query<NewSection>("SELECT * FROM NewSections WHERE CustomerId = @Id", new { Id = 1 });

            foreach (var section in sections)
            {
                Console.WriteLine($"{section.Name}");
            }
        }

        using (var connection = new SqlConnection(connectionString))
        {
            var products = connection.Query<NewProduct>("SELECT * FROM NewProducts WHERE SectionId = @Id AND IsSale = 1", new { Id = 1 });

            foreach (var product in products)
            {
                Console.WriteLine($"{product.PName} - {product.Price}");
            }
        }

    }
}
