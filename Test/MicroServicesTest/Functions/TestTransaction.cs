using System;
using System.Collections.Generic;
using System.Text;
using MicroServicesTest.Models;
using MicroServicesTest.Databases;
using System.Data;
using System.Linq;
using System.Data.SqlClient;


using System.Transactions;
using Microsoft.EntityFrameworkCore;

namespace MicroServicesTest.Functions
{
    public class TestTransaction
    {
		public void RunTestTransaction()
		{
		
			/*SqlConnection connection = new SqlConnection();
			connection.ConnectionString = MicroServicesTest.DatabaseConnections.StockDatabaseConnection;
			connection.Open();

			SqlTransaction transaction = connection.BeginTransaction(System.Data.IsolationLevel.Serializable, "test");

			string sqlStatement = "SELECT * FROM STOCK WITH (UPDLOCK) WHERE PRODUCTNUMBER = '" + "CAPLIN" + "'";
			
			SqlCommand sqlStockCommand = new SqlCommand();
			sqlStockCommand.Connection = connection;
			sqlStockCommand.CommandText = sqlStatement;
			sqlStockCommand.Transaction = transaction;

			using (SqlDataReader reader = sqlStockCommand.ExecuteReader())
			{
				
				if (reader.Read())
				{
					int quantityOnHand = Convert.ToInt32(reader["QuantityOnHand"]);
				}
				
				reader.Close();
			}

			Console.WriteLine("Locking data SERIALIZABLE before commit");
			Console.ReadKey();
			transaction.Commit();

			connection.Close();*/

			try
			{
				
				using (StockDatabase stockDatabase = new StockDatabase())
				{
					using (var transaction = stockDatabase.Database.BeginTransaction(System.Data.IsolationLevel.Serializable))
					{
					
						string sql = "SELECT * FROM STOCK WITH (UPDLOCK) WHERE PRODUCTNUMBER = '" + "CAPLIN" + "'";
						StockPart stockPart = stockDatabase.Stock.FromSql(sql).FirstOrDefault();
						stockPart.QuantityOnHand = stockPart.QuantityOnHand - 10;

						stockDatabase.SaveChanges();

						Console.WriteLine("Locking data SERIALIZABLE with EF Core " + stockPart.QuantityOnHand);
						Console.ReadKey();

						transaction.Commit();
					}
				}
					
			}
			catch (Exception ex)
			{
				string errorMessage = ex.Message;
			}

			/*try
			{
				using (var scope = new TransactionScope(transactionScopeOption, transactionOptions))
				{
					StockDatabase stockDatabase = new StockDatabase();

					StockPart stockPart = stockDatabase.Stock.Where(x => x.ProductNumber == "CAPLIN").FirstOrDefault();
					Console.WriteLine("Locking data");
					Console.ReadKey();
					stockPart.QuantityOnHand = stockPart.QuantityOnHand - 10;
					stockDatabase.SaveChanges();
					scope.Complete();


					/*
					StockPart stock = new StockPart();
					stock.ProductNumber = "CAPLIN";
					stock.QuantityOnHand = 1000;
					stock.QuantityOnOrder = 2000;

					ProductPart product = new ProductPart();
					product.ProductNumber = "CAPLIN";
					product.Description = "This is a test";
					product.UnitPrice = 10.00M;

					//StockDatabase productDatabase = new StockDatabase();
					//StockDatabase stockDatabase = new StockDatabase();

					stockDatabase.Product.Add(product);
					stockDatabase.SaveChanges();

					stockDatabase.Stock.Add(stock);
					stockDatabase.SaveChanges();
					



				
			}
			catch (Exception ex)
			{
				string errorMessage = ex.Message;
			}*/

		}

	}

}
