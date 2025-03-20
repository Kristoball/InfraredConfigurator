using System.Collections.Generic;
using InfraredConfigurator.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InfraredConfigurator.Controllers;

public class ServerController : Controller
{
    private readonly ILogger<ServerController> _logger;
    private readonly DatabaseContext _databaseContext;

    public ServerController(ILogger<ServerController> logger, DatabaseContext databaseContext)
    {
        _logger = logger;
        _databaseContext = databaseContext;
    }

    public async Task<IActionResult> Index()
    {
        var servers = await _databaseContext.Servers.ToListAsync();
        return View(servers);
    }

    public IActionResult Edit(int? id)
    {
        if (id.HasValue == false)
        {
            return View(new Server());
        }
        var domain = _databaseContext.Servers.Find(id);
        return View(domain);
    }

    public IActionResult EditSubmit(Server server)
    {
        if (server.Id == 0)
        {
            _databaseContext.Servers.Add(server);
        }
        else
        {
            _databaseContext.Servers.Update(server);
        }
        _databaseContext.SaveChanges();
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Delete(int id)
    {
        var server = await _databaseContext.Servers.SingleAsync(c => c.Id == id);
        return View(server);
    }

    public async Task<IActionResult> ConfirmDelete(int id)
    {
        var server = await _databaseContext.Servers.SingleAsync(c => c.Id == id);
        _databaseContext.Servers.Remove(server);
        await _databaseContext.SaveChangesAsync();
        return RedirectToAction("Index");
    }
}
