using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeProject.Portal.Models
{
  public class AppSettings
  {
    public string AccountManagementWebApiUrl { get; set; }
    public string InventoryManagementWebApiUrl { get; set; }
    public string PurchaseOrderManagementWebApiUrl { get; set; }
    public string SalesOrderManagementWebApiUrl { get; set; }
    public string Environment { get; set; }
  }
}
