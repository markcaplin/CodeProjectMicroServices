using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CodeProject.Portal.Pages
{

  public class IndexModel : PageModel
  {

    private Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;

    public string _wwwroot { get; private set; }
    public string _currentRoutePath { get; private set; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="hostingEnvironment"></param>
    public IndexModel(Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment)
    {
      _hostingEnvironment = hostingEnvironment;
    }

    /// <summary>
    /// On Get
    /// </summary>
    public void OnGet(string currentRoutePath)
    {
      _wwwroot = _hostingEnvironment.WebRootPath;

      _currentRoutePath = "/";

      if (currentRoutePath != null)
      {
        _currentRoutePath = currentRoutePath;
      }

    }

  }
}
