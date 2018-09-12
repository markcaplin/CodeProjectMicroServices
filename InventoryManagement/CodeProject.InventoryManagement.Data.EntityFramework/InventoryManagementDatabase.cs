using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using CodeProject.InventoryManagement.Data.Entities;
using CodeProject.Shared.Common.Utilties;
using CodeProject.Shared.Common.Models;

namespace CodeProject.InventoryManagement.Data.EntityFramework
{
    public class InventoryManagementDatabase : DbContext
	{
		public DbSet<Product> Products { get; set; }
		public DbSet<TransactionQueueInbound> TransactionQueueInbound { get; set; }
		public DbSet<TransactionQueueOutbound> TransactionQueueOutbound { get; set; }
		public DbSet<TransactionQueueInboundHistory> TransactionQueueInboundHistory { get; set; }
		public DbSet<TransactionQueueOutboundHistory> TransactionQueueOutboundHistory { get; set; }
		public DbSet<TransactionQueueSemaphore> TransactionQueueSemaphores { get; set; }

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
			modelBuilder.Entity<TransactionQueueSemaphore>().HasIndex(u => u.SemaphoreKey).IsUnique();

		}

		public InventoryManagementDatabase(DbContextOptions<InventoryManagementDatabase> options) : base(options)
		{
			
		}

		public InventoryManagementDatabase()
		{
			
		}
	}
}
