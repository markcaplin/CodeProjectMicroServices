using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using CodeProject.LoggingManagement.Data.Entities;
using CodeProject.Shared.Common.Utilties;
using CodeProject.Shared.Common.Models;

namespace CodeProject.LoggingManagement.Data.EntityFramework
{
    public class LoggingManagementDatabase : DbContext
	{
		public DbSet<MessagesSent> MessagesSent { get; set; }
		public DbSet<MessagesReceived> MessagesReceived { get; set; }
		public DbSet<AcknowledgementsQueue> AcknowledgementsQueue { get; set; }


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
			
		}

		public LoggingManagementDatabase(DbContextOptions<LoggingManagementDatabase> options) : base(options)
		{
			
		}

		public LoggingManagementDatabase()
		{
			
		}
	}
}
