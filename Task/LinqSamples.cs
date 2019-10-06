// Copyright © Microsoft Corporation.  All Rights Reserved.
// This code released under the terms of the 
// Microsoft Public License (MS-PL, http://opensource.org/licenses/ms-pl.html.)
//
//Copyright (C) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using SampleSupport;
using Task.Data;

// Version Mad01

namespace SampleQueries
{
	[Title("LINQ Module")]
	[Prefix("Linq")]
	public class LinqSamples : SampleHarness
	{

		private DataSource dataSource = new DataSource();

		[Category("Restriction Operators")]
		[Title("Where - Task 1")]
		[Description("This sample uses the where clause to find all elements of an array with a value less than 5.")]

        public void LinqExample1()
		{
			int[] numbers = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };

			var lowNums =
				from num in numbers
				where num < 5
				select num;

			Console.WriteLine("Numbers < 5:");
			foreach (var x in lowNums)
			{
				Console.WriteLine(x);
			}
		}

		[Category("Restriction Operators")]
		[Title("Where - Task 2")]
		[Description("This sample return return all presented in market products")]

		public void LinqExample2()
		{
			var products =
				from p in dataSource.Products
				where p.UnitsInStock > 0
				select p;

			foreach (var p in products)
			{
				ObjectDumper.Write(p);
			}
		}

        [Category("Homework")]
        [Title("Homework - Task 1")]
        [Description("Give a list of all customers whose total turnover (the sum of all orders) exceeds a certain value X.")]

        public void Linq1()
        {
            int[] x = { 10000, 20000, 30000 };

            foreach (var item in x)
            {
                var customerList = dataSource.Customers.Where(q => q.Orders.Sum(w => w.Total) > item)
                    .Select(e => new { e.CustomerID, Total = e.Orders.Sum(r => r.Total), e.CompanyName });

                foreach (var p in customerList)
                {
                    ObjectDumper.Write(p);
                }

                ObjectDumper.Write("-------------------------");
            }            
        }

        [Category("Homework")]
        [Title("Homework - Task 2")]
        [Description("For each customer, make a list of suppliers located in the same country and the same city.")]

        public void Linq2()
        {
            
            var customerList1 = dataSource.Customers.Join(dataSource.Suppliers, c => new { c.Country, c.City, }, s => new { s.Country, s.City },
                (c, s) => new { c.CustomerID, c.Country, c.City, s.SupplierName });

            var customerList2 = dataSource.Customers.Join(dataSource.Suppliers, c => new { c.Country, c.City, }, s => new { s.Country, s.City },
                (c, s) => new { c.CustomerID, c.Country, c.City, s.SupplierName }).GroupBy(q => q.SupplierName);


            foreach (var p in customerList1)
            {
                ObjectDumper.Write(p);
            }

            ObjectDumper.Write("-------------------------");

            foreach (var p in customerList2)
            {
                ObjectDumper.Write(p);
            }
        }

        [Category("Homework")]
        [Title("Homework - Task 3")]
        [Description("Find all customers who have orders exceeding X in total.")]

        public void Linq3()
        {
            decimal x = 3000;

            var customerList = dataSource.Customers.Where(q => q.Orders.Any(w => w.Total > x))
                .Select(e => new{ e.CustomerID, Total = e.Orders.Max(r => r.Total), e.CompanyName });

            foreach (var p in customerList)
            {
                ObjectDumper.Write(p);
            }
        }

        [Category("Homework")]
        [Title("Homework - Task 4")]
        [Description("Give out a list of customers indicating the month from which year they became customers (accept the month and year of the very first order as such).")]

        public void Linq4()
        {
            var customerList = dataSource.Customers.Where(q => q.Orders.Count() > 0)
                .Select(w => new { w.CustomerID, DateTime = w.Orders.Min(e => e.OrderDate), w.CompanyName });

            foreach (var p in customerList)
            {
                ObjectDumper.Write(p);
            }
        }

        [Category("Homework")]
        [Title("Homework - Task 5")]
        [Description("Do the previous task, but issue a list sorted by year, month, customer turnover (from maximum to minimum) and the name of the client.")]

        public void Linq5()
        {
            var customerList = dataSource.Customers.Where(q => q.Orders.Count() > 0)
                .Select(w => new { DateTime = w.Orders.Min(e => e.OrderDate), CashTurnover = w.Orders.Sum(b => b.Total), w.CompanyName })
                .OrderBy(y => y.DateTime.Month).OrderBy(u => u.DateTime.Year).OrderBy(r => r.CompanyName).OrderByDescending(t => t.CashTurnover);

            foreach (var p in customerList)
            {
                ObjectDumper.Write(p);
            }
        }

        [Category("Homework")]
        [Title("Homework - Task 6")]
        [Description("Indicate all customers who have a non-digital postal code or a region is not filled out or an operator code is not indicated on the phone (for simplicity we consider this to be equivalent to “no parentheses at the beginning”).")]

        public void Linq6()
        {
            var customerList = dataSource.Customers
                .Where(c => string.IsNullOrEmpty(c.PostalCode) || c.PostalCode.Any(char.IsLetter) || string.IsNullOrEmpty(c.Region) || !c.Phone.StartsWith("("))
                .Select(c => new { c.CustomerID, c.PostalCode, c.Phone, c.Region, c.CompanyName });
                
            foreach (var p in customerList)
            {
                ObjectDumper.Write(p);
            }
        }

        [Category("Homework")]
        [Title("Homework - Task 7")]
        [Description("Group all products by category, inside - by stock status, inside the last group sort by price.")]

        public void Linq7()
        {
            var productsList = dataSource.Products.GroupBy(p => p.Category)
                .Select(s => new { Categor = s.Key, Status = s.GroupBy(p => p.UnitsInStock > 0)
                .Select(c => new { Depot = c.Key, Price = c.OrderBy(p => p.UnitPrice) })
                });

            foreach (var p in productsList)
            {
                ObjectDumper.Write("#########");
                ObjectDumper.Write(p.Categor);
                ObjectDumper.Write("#########");
                foreach (var c in p.Status)
                {
                    ObjectDumper.Write(c.Depot);
                    foreach (var a in c.Price)
                        ObjectDumper.Write("price =  " + a.UnitPrice + " product: " + a.ProductName);
                }
                ObjectDumper.Write(String.Empty);
            }
        }

        [Category("Homework")]
        [Title("Homework - Task 8")]
        [Description("Group the products into groups of “cheap”, “average price”, “expensive”.")]

        public void Linq8()
        {
            decimal min = 10;
            decimal max = 100;

            var products = dataSource.Products.OrderBy(q => q.UnitPrice).
                GroupBy(w => w.UnitPrice < min ? "Cheap": w.UnitPrice < max ? "AveragePrice”" : "Expensive");

            foreach (var p in products)
            {
                ObjectDumper.Write("#########");
                ObjectDumper.Write(p.Key);
                ObjectDumper.Write("#########");
                foreach (var item in p)
                {
                    ObjectDumper.Write("Productc " + item.ProductName + "Price " + item.UnitPrice);
                }
            }
        }

        [Category("Homework")]
        [Title("Homework - Task 9")]
        [Description("Calculate the average profitability of each city (average order amount for all customers from a given city) and average intensity (average number of orders per client from each city).")]

        public void Linq9()
        {
            var client = dataSource.Customers.GroupBy(x => x.City);
            var averagePerformance = client
                .Select(q => new {q.Key, Income = q.Average(w => w.Orders.Sum(e => e.Total)), Frequency = q.Average(y => y.Orders.Count())
            });

            foreach (var s in averagePerformance)
            {
                ObjectDumper.Write(s);
            }
        }
        

        [Category("Homework")]
        [Title("Homework - Task 10")]
        [Description("Make the average annual statistics of customer activity by month (excluding the year), statistics by year, by year and month (i.e. when one month in different years has its own value).")]

        public void Linq10()
        { 
            var ordersDate = dataSource.Customers.SelectMany(w => w.Orders).OrderBy(w => w.OrderDate.Month).GroupBy(e =>e.OrderDate.Month)
                .Select(x => new { x.Key, MonthsOrders = x.Count() }).OrderBy(x => x.Key);
            var statistics = dataSource.Customers.SelectMany(q => q.Orders).OrderBy(w => w.OrderDate.Year).GroupBy(e => e.OrderDate.Year)
                .Select(r => new { r.Key, Statistics = r.ToList()
                .Select(x => x.Total).Sum(), Count = r.ToList()
                .Select(x => x.Total).Count() });

            foreach (var c in statistics)
            {
                ObjectDumper.Write("#########");
                ObjectDumper.Write(c, 1);
                ObjectDumper.Write("#########");

                foreach (var m in ordersDate)
                {
                    ObjectDumper.Write(m, 1);
                }       
            }
        }
    }
}

