using System.Diagnostics;
using System.Threading.Tasks;
using InfraredConfigurator.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InfraredConfigurator.Controllers;

public class DomainController : Controller
{
    private readonly ILogger<DomainController> _logger;
    private readonly DatabaseContext _databaseContext;

    public DomainController(ILogger<DomainController> logger, DatabaseContext databaseContext)
    {
        _logger = logger;
        _databaseContext = databaseContext;
    }

    public async Task<IActionResult> Index()
    {
        var domains = await _databaseContext.Domains.ToListAsync();
        return View(domains);
    }

    public IActionResult Edit(int? id)
    {
        if (id.HasValue == false)
        {
            return View(new Domain());
        }
        var domain = _databaseContext.Domains.Find(id);
        return View(domain);
    }

    public IActionResult EditSubmit(Domain domain)
    {
        if (domain.Id == 0)
        {
            _databaseContext.Domains.Add(domain);
        }
        else
        {
            _databaseContext.Domains.Update(domain);
        }
        _databaseContext.SaveChanges();
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Delete(int id)
    {
        var domain = await _databaseContext.Domains.SingleAsync(c => c.Id == id);
        return View(domain);
    }

    public async Task<IActionResult> ConfirmDelete(int id)
    {
        var domain = await _databaseContext.Domains.SingleAsync(c => c.Id == id);
        _databaseContext.Domains.Remove(domain);
        await _databaseContext.SaveChangesAsync();
        return RedirectToAction("Index");
    }
}
