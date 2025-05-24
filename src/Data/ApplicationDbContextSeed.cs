using PNET_semestralka.Data;
using PNET_semestralka.Models;
using Microsoft.AspNetCore.Identity;


public static class ApplicationDbContextSeed
{
	/// <summary>
	/// seed the database with initial data
	/// </summary>
	/// <param name="context">the ApplicationDbContext instance used to interact with the database</param>
	public static void SeedData(ApplicationDbContext context)
    {
		//  if it doesn't exist, automatically creates the database
		context.Database.EnsureCreated();

		// exit if the data already exists
		if (context.Users.Any()) return;



		// Seller
		var seller = new Seller
		{
			UzivatelskeJmeno = "pivnice123",
			Email = "pivo@shop.cz",
			Heslo = "heslo123",
			Products = new List<Product>()
		};

		// Customer
		var customer = new Customer
		{
			UzivatelskeJmeno = "jan.novak",
			Email = "jan@novak.cz",
			Heslo = "heslo123",
			ShippingDetails = new List<SendingAddress>(),
			Orders = new List<Order>()
		};

		// hashing passwords
		var passwordHasher = new PasswordHasher<User>();
		seller.Heslo = passwordHasher.HashPassword(seller, seller.Heslo);
		customer.Heslo = passwordHasher.HashPassword(customer, customer.Heslo);

		// address of customer
		var address = new SendingAddress
        {
            Jmeno = "Jan",
            Prijmeni = "Novák",
            Ulice = "Hlavní",
            PopisneCislo = 25,
            OrientacniCislo = 8,
            Mesto = "Praha",
            Psc = 11000,
            Customer = customer
        };

        // products
        var products = new List<Product>
        {
            new Product
            {
                Nazev = "Birell Pomelo & Grep - sud",
                Cena = 850,
                Mnozstvi = 25,
                Popis = "Nealkoholický sudový Birell s příchutí pomelo a grep",
                Photo = "Birell_Pomelo&Grep_sud.jpg",
                Seller = seller
            },
            new Product
            {
                Nazev = "Gambrinus 10° - přepravka",
                Cena = 289,
                Mnozstvi = 60,
                Popis = "Gambrinus 10° ve vratné přepravce, 20x0.5l",
                Photo = "gambrinus_10_prepravka.jpg",
                Seller = seller
            },
            new Product
            {
                Nazev = "Gambrinus 11° - sud",
                Cena = 299,
                Mnozstvi = 50,
                Popis = "Gambrinus 11° v sudu",
                Photo = "gambrinus_11.jpg",
                Seller = seller
            },
            new Product
            {
                Nazev = "Gambrinus 12° - přepravka",
                Cena = 319,
                Mnozstvi = 45,
                Popis = "Gambrinus 12° ve vratné přepravce, 20x0.5l",
                Photo = "gambrinus_12_prepravka.jpg",
                Seller = seller
            },
            new Product
            {
                Nazev = "Gambrinus - sud",
                Cena = 25,
                Mnozstvi = 120,
                Popis = "Gambrinus světlý ležák",
                Photo = "gambrinus.jpg",
                Seller = seller
            },
            new Product
            {
                Nazev = "Kozel - přepravka",
                Cena = 279,
                Mnozstvi = 70,
                Popis = "Kozel světlý ležák v přepravce, 20x0.5l",
                Photo = "kozel_prepravka.jpg",
                Seller = seller
            },
            new Product
            {
                Nazev = "Kozel - sud 30L",
                Cena = 999,
                Mnozstvi = 30,
                Popis = "Sud piva Kozel 11° vhodný na oslavy",
                Photo = "kozel_sud.jpg",
                Seller = seller
            },
            new Product
            {
                Nazev = "Excelent 11° - láhev",
                Cena = 28,
                Mnozstvi = 85,
                Popis = "Hořké pivo Excelent 11° v přepravce, 20x0.5l",
                Photo = "exelent.jpg",
                Seller = seller
            },
            new Product
            {
                Nazev = "Pilsner Urquell 12° - přepravka",
                Cena = 349,
                Mnozstvi = 55,
                Popis = "Originální Plzeňský ležák ve vratné přepravce",
                Photo = "pilsner_prepravka.jpg",
                Seller = seller
            }
        };

        context.Products.AddRange(products);

		// orders
		var orders = new List<Order>
        {
	        new Order
	        {
		        Jmeno = address.Jmeno,
		        Prijmeni = address.Prijmeni,
		        Ulice = address.Ulice,
		        PopisneCislo = address.PopisneCislo,
		        OrientacniCislo = address.OrientacniCislo,
		        Mesto = address.Mesto,
		        Psc = address.Psc,
		        Customer = customer,
		        Stav = "Košík",
		        OrderItems = new List<OrderItem>
		        {
			        new OrderItem
			        {
				        Product = products[0],
				        Pocet = 4
			        },
			        new OrderItem
			        {
				        Product = products[1],
				        Pocet = 2
			        }
		        }
	        },
	        new Order
	        {
		        Jmeno = address.Jmeno,
		        Prijmeni = address.Prijmeni,
		        Ulice = address.Ulice,
		        PopisneCislo = address.PopisneCislo,
		        OrientacniCislo = address.OrientacniCislo,
		        Mesto = address.Mesto,
		        Psc = address.Psc,
		        Customer = customer,
		        Stav = "Doručena",
		        OrderItems = new List<OrderItem>
		        {
			        new OrderItem
			        {
				        Product = products[2],
				        Pocet = 1
			        },
			        new OrderItem
			        {
				        Product = products[3],
				        Pocet = 3
			        },
			        new OrderItem
			        {
				        Product = products[4],
				        Pocet = 6
			        }
		        }
	        }
        };

		context.Orders.AddRange(orders);

		// save
		context.Users.Add(seller);
        context.Users.Add(customer);
        context.SendingAddress.Add(address);
        context.SaveChanges();
    }

}




