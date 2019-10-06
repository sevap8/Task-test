using SampleSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task
{
   [Title("LINQ to Sql Module")]
    [Prefix("Linq")]
    class LinqToSqlSamples : SampleHarness
    {
       
        NorthwindDataContext db = new NorthwindDataContext();

        [Category("LINQ to Sql")]
        [Title("Homework - Task 1")]
        [Description("Give a list of all customers whose total turnover(the sum of all orders) exceeds a certain value X. + LINQ to Sql")]

        public void Linq1()
        {
            decimal x = 10000;
            var customerList =
                from c in db.Customers
                where c.Orders.Sum(p => p.Order_Details.Sum(l =>
                l.UnitPrice * (decimal)(1 - l.Discount) * l.Quantity)) > x
                select c.CustomerID + " "+ c.Orders.Sum(p => p.Order_Details.Sum(l =>
                l.UnitPrice * (decimal)(1 - l.Discount) * l.Quantity)) + " "+ c.CompanyName;

            Console.WriteLine($"{Environment.NewLine}Customers with total orders > {x} :");
            foreach (var i in customerList)
            {
                Console.WriteLine(i);
            }

            x = 20000;
            Console.WriteLine($"{Environment.NewLine}Customers with total orders > {x} :");
            foreach (var i in customerList)
            {
                Console.WriteLine(i);
            }

            x = 300000;
            Console.WriteLine($"{Environment.NewLine}Customers with total orders > {x} :");
            foreach (var i in customerList)
            {
                Console.WriteLine(i);
            }
        }

        [Category("LINQ to Sql")]
        [Title("Homework - Task 3")]
        [Description("Find all customers who have orders exceeding X in total. + LINQ to Sql")]

        public void Linq2()
        {
            decimal x = 3000;
            var customerList =
                from c in db.Customers
                where c.Orders.Any(p => p.Order_Details.Sum(l =>
                l.UnitPrice * (decimal)(1 - l.Discount) * l.Quantity) > x)
                select c.CompanyName + " "+ c.Orders.Sum(p => p.Order_Details.Sum(l =>
                l.UnitPrice * (decimal)(1 - l.Discount) * l.Quantity));

            Console.WriteLine($"{Environment.NewLine}Customers with any order > {x} :");
            foreach (var i in customerList)
            {
                Console.WriteLine(i);
            }
        }

        [Category("LINQ to Sql")]
        [Title("Homework - Task 6")]
        [Description("Indicate all customers who have a non-digital postal code or a region is not filled out or an operator code is not indicated on the phone (for simplicity we consider this to be equivalent to “no parentheses at the beginning” + LINQ to Sql")]

        public void Linq3()
        {
            var customerList =
                from customer in db.Customers
                where System.Data.Linq.SqlClient.SqlMethods.Like(customer.PostalCode.Trim(), "%[^0-9]%")
                || customer.Region == null
                || customer.Phone[0] != '('
                select customer.CompanyName + " " + customer.Phone + " " + customer.Region + " " + customer.PostalCode;

            Console.WriteLine($"{Environment.NewLine}Customers with wrong filled fields: ");
            foreach (var i in customerList)
            {
                Console.WriteLine(i);
            }
        }
    }
}
