using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using CodeProject.SalesOrderManagement.Data.Entities;
using CodeProject.Shared.Common.Utilties;
using CodeProject.Shared.Common.Models;

namespace CodeProject.SalesOrderManagement.Data.EntityFramework
{
    public class SalesOrderManagementDatabase : DbContext
	{
		public DbSet<Customer> Customers { get; set; }
		public DbSet<Product> Products { get; set; }
		public DbSet<TransactionQueue> TransactionQueue { get; set; }
		public DbSet<SalesOrder> SalesOrders { get; set; }
		public DbSet<SalesOrderStatus> SalesOrderStatuses { get; set; }
		public DbSet<SalesOrderDetail> SalesOrderDetails { get; set; }

		/// <summary>
		/// On Configuring
		/// </summary>
		/// <param name="optionsBuilder"></param>
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			ConnectionStrings connectionStrings = ConfigurationUtility.GetConnectionStrings();
			string databaseConnectionString = connectionStrings.PrimaryDatabaseConnectionString;
			optionsBuilder.UseSqlServer(databaseConnectionString);
			
		}
		/// <summary>
		/// Fluent Api
		/// </summary>
		/// <param name="modelBuilder"></param>
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			
			modelBuilder.Entity<Product>().HasIndex(u=> u.ProductNumber);

		}

		public SalesOrderManagementDatabase(DbContextOptions<SalesOrderManagementDatabase> options) : base(options)
		{
			
		}

		public SalesOrderManagementDatabase()
		{
			
		}
	}
}
