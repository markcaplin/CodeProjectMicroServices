using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using MicroServicesTest.Models;

namespace MicroServicesTest.Databases
{
	public class StockDatabase : DbContext
	{
		public DbSet<StockPart> Stock { get; set; }
		public DbSet<ProductPart> Product { get; set; }

		/// <summary>
		/// On Configuring
		/// </summary>
		/// <param name="optionsBuilder"></param>
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{

			string stockDatabaseConnectionString = MicroServicesTest.DatabaseConnections.StockDatabaseConnection;
			optionsBuilder.UseSqlServer(stockDatabaseConnectionString);


		}
		/// <summary>
		/// Fluent Api
		/// </summary>
		/// <param name="modelBuilder"></param>
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{

		}


	}
}

