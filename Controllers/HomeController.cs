using System.Diagnostics;
using System.Threading.Tasks;
using InfraredConfigurator.Entities;
using InfraredConfigurator.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InfraredConfigurator.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly DatabaseContext _databaseContext;
    private readonly InfraredConfigEditorService _infraredConfigEditorService;

    public HomeController(
        ILogger<HomeController> logger,
        DatabaseContext databaseContext,
        InfraredConfigEditorService infraredConfigEditorService
    )
    {
        _logger = logger;
        _databaseContext = databaseContext;
        _infraredConfigEditorService = infraredConfigEditorService;
    }

    public IActionResult Index()
    {
        var proxyConfigs = _databaseContext
            .ProxyConfigs.Include(x => x.Domain)
            .Include(x => x.Server)
            .ToList();
        return View(proxyConfigs);
    }

    public IActionResult Edit(int? id)
    {
        var domain = _databaseContext.Domains.ToList();
        if (domain.Count != 0)
            ViewBag.Domains = domain;

        var servers = _databaseContext.Servers.ToList();
        if (servers.Count != 0)
            ViewBag.Servers = servers;

        if (id.HasValue == false)
        {
            return View(new ProxyConfig());
        }

        var proxyConfig = _databaseContext
            .ProxyConfigs.Include(x => x.Domain)
            .Include(x => x.Server)
            .Single(c => c.Id == id);
        return View(proxyConfig);
    }

    public async Task<IActionResult> EditSubmit(ProxyConfig proxyConfig)
    {
        if (proxyConfig.DomainId == 0)
        {
            ModelState.AddModelError("DomainId", "Domain is required");
        }

        if (proxyConfig.ServerId == 0)
        {
            ModelState.AddModelError("ServerId", "Server is required");
        }

        if (string.IsNullOrWhiteSpace(proxyConfig.Port))
        {
            ModelState.AddModelError("Port", "Port is required");
        }

        if (string.IsNullOrWhiteSpace(proxyConfig.SubDomain))
        {
            ModelState.AddModelError("SubDomain", "SubDomain is required");
        }

        if (string.IsNullOrWhiteSpace(proxyConfig.DisconnectMessage))
        {
            ModelState.AddModelError("DisconnectMessage", "DisconnectMessage is required");
        }

        if (string.IsNullOrWhiteSpace(proxyConfig.OfflineStatus))
        {
            ModelState.AddModelError("OfflineStatus", "OfflineStatus is required");
        }

        if (string.IsNullOrWhiteSpace(proxyConfig.OnlineStatus))
        {
            ModelState.AddModelError("OnlineStatus", "OnlineStatus is required");
        }

        if (string.IsNullOrWhiteSpace(proxyConfig.Name))
        {
            ModelState.AddModelError("Name", "Name is required");
        }

        if (ModelState.IsValid == false)
        {
            var domain = _databaseContext.Domains.ToList();
            if (domain.Count != 0)
                ViewBag.Domains = domain;

            var servers = _databaseContext.Servers.ToList();
            if (servers.Count != 0)
                ViewBag.Servers = servers;

            return View("Edit", proxyConfig);
        }

        if (proxyConfig.Id == 0)
        {
            _databaseContext.ProxyConfigs.Add(proxyConfig);
        }
        else
        {
            _databaseContext.ProxyConfigs.Update(proxyConfig);
        }
        await _databaseContext.SaveChangesAsync();

        await _infraredConfigEditorService.WriteOutConfig(proxyConfig.Id);

        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Delete(int id)
    {
        var proxyConfig = _databaseContext
            .ProxyConfigs.Include(x => x.Domain)
            .Include(x => x.Server)
            .Single(c => c.Id == id);
        return View(proxyConfig);
    }

    public async Task<IActionResult> ConfirmDelete(int id)
    {
        var proxyConfig = _databaseContext
            .ProxyConfigs.Include(x => x.Domain)
            .Include(x => x.Server)
            .Single(c => c.Id == id);
        _databaseContext.ProxyConfigs.Remove(proxyConfig);
        await _databaseContext.SaveChangesAsync();
        _infraredConfigEditorService.DeleteConfig(id);
        return RedirectToAction("Index");
    }

    public IActionResult ToggleTheme()
    {
        var theme = Request.Cookies["theme"]?.ToLower() == "dark" ? "Light" : "Dark";
        //get the url referrer
        var url = Request.Headers["Referer"].ToString();
        //set the cookie
        Response.Cookies.Append("theme", theme);
        //redirect back to the url
        return Redirect(url);
    }
}
