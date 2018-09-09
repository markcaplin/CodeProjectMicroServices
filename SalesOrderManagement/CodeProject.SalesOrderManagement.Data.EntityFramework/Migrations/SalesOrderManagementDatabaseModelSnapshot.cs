﻿// <auto-generated />
using System;
using CodeProject.SalesOrderManagement.Data.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CodeProject.SalesOrderManagement.Data.EntityFramework.Migrations
{
    [DbContext(typeof(SalesOrderManagementDatabase))]
    partial class SalesOrderManagementDatabaseModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.2-rtm-30932")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CodeProject.SalesOrderManagement.Data.Entities.Customer", b =>
                {
                    b.Property<int>("CustomerId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AccountId");

                    b.Property<string>("AddressLine1");

                    b.Property<string>("AddressLine2");

                    b.Property<double>("AmountOrdered");

                    b.Property<double>("AmountShipped");

                    b.Property<string>("City");

                    b.Property<double>("CreditLimit");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateShipped");

                    b.Property<DateTime>("DateUpdated");

                    b.Property<string>("Name");

                    b.Property<string>("PostalCode");

                    b.Property<string>("Region");

                    b.HasKey("CustomerId");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("CodeProject.SalesOrderManagement.Data.Entities.Product", b =>
                {
                    b.Property<int>("ProductId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AccountId");

                    b.Property<int>("CommittedQuantity");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateUpdated");

                    b.Property<string>("Description");

                    b.Property<int>("OnHandQuantity");

                    b.Property<int>("ProductMasterId");

                    b.Property<string>("ProductNumber");

                    b.Property<double>("UnitPrice");

                    b.HasKey("ProductId");

                    b.HasIndex("ProductNumber");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("CodeProject.SalesOrderManagement.Data.Entities.SalesOrder", b =>
                {
                    b.Property<int>("SalesOrderId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AccountId");

                    b.Property<int>("CustomerId");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateUpdated");

                    b.Property<double>("OrderTotal");

                    b.Property<int>("SalesOrderNumber");

                    b.Property<int>("SalesOrderStatusId");

                    b.Property<string>("ShipToAddressLine1");

                    b.Property<string>("ShipToAddressLine2");

                    b.Property<string>("ShipToCity");

                    b.Property<string>("ShipToPostalCode");

                    b.Property<string>("ShipToRegion");

                    b.HasKey("SalesOrderId");

                    b.HasIndex("CustomerId");

                    b.HasIndex("SalesOrderStatusId");

                    b.ToTable("SalesOrders");
                });

            modelBuilder.Entity("CodeProject.SalesOrderManagement.Data.Entities.SalesOrderDetail", b =>
                {
                    b.Property<int>("SalesOrderDetailId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateUpdated");

                    b.Property<int>("OrderQuantity");

                    b.Property<int>("ProductId");

                    b.Property<int>("SalesOrderId");

                    b.Property<double>("UnitPrice");

                    b.HasKey("SalesOrderDetailId");

                    b.HasIndex("ProductId");

                    b.HasIndex("SalesOrderId");

                    b.ToTable("SalesOrderDetails");
                });

            modelBuilder.Entity("CodeProject.SalesOrderManagement.Data.Entities.SalesOrderStatus", b =>
                {
                    b.Property<int>("SalesOrderStatusId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Description");

                    b.HasKey("SalesOrderStatusId");

                    b.ToTable("SalesOrderStatuses");
                });

            modelBuilder.Entity("CodeProject.SalesOrderManagement.Data.Entities.TransactionQueueInbound", b =>
                {
                    b.Property<int>("TransactionQueueInboundId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DateCreated");

                    b.Property<string>("ExchangeName");

                    b.Property<string>("Payload");

                    b.Property<int>("SenderTransactionQueueId");

                    b.Property<string>("TransactionCode");

                    b.HasKey("TransactionQueueInboundId");

                    b.ToTable("TransactionQueueInbound");
                });

            modelBuilder.Entity("CodeProject.SalesOrderManagement.Data.Entities.TransactionQueueInboundHistory", b =>
                {
                    b.Property<int>("TransactionQueueInboundHistoryId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateCreatedInbound");

                    b.Property<bool>("DuplicateMessage");

                    b.Property<string>("ErrorMessage");

                    b.Property<string>("ExchangeName");

                    b.Property<string>("Payload");

                    b.Property<bool>("ProcessedSuccessfully");

                    b.Property<int>("SenderTransactionQueueId");

                    b.Property<string>("TransactionCode");

                    b.Property<int>("TransactionQueueInboundId");

                    b.HasKey("TransactionQueueInboundHistoryId");

                    b.ToTable("TransactionQueueInboundHistory");
                });

            modelBuilder.Entity("CodeProject.SalesOrderManagement.Data.Entities.TransactionQueueOutbound", b =>
                {
                    b.Property<int>("TransactionQueueOutboundId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateSentToExchange");

                    b.Property<DateTime>("DateToResendToExchange");

                    b.Property<string>("ExchangeName");

                    b.Property<string>("Payload");

                    b.Property<bool>("SentToExchange");

                    b.Property<string>("TransactionCode");

                    b.HasKey("TransactionQueueOutboundId");

                    b.ToTable("TransactionQueueOutbound");
                });

            modelBuilder.Entity("CodeProject.SalesOrderManagement.Data.Entities.SalesOrder", b =>
                {
                    b.HasOne("CodeProject.SalesOrderManagement.Data.Entities.Customer", "Customer")
                        .WithMany()
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("CodeProject.SalesOrderManagement.Data.Entities.SalesOrderStatus", "SalesOrderStatus")
                        .WithMany()
                        .HasForeignKey("SalesOrderStatusId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CodeProject.SalesOrderManagement.Data.Entities.SalesOrderDetail", b =>
                {
                    b.HasOne("CodeProject.SalesOrderManagement.Data.Entities.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("CodeProject.SalesOrderManagement.Data.Entities.SalesOrder", "SalesOrder")
                        .WithMany()
                        .HasForeignKey("SalesOrderId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}