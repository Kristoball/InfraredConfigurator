using System.Text.Json;
using System.Threading.Tasks;
using InfraredConfigurator.Entities;
using Microsoft.EntityFrameworkCore;

namespace InfraredConfigurator.Services;

public class InfraredConfigEditorService
{
    private readonly DatabaseContext _databaseContext;
    private string _basePath { get; init; }

    public InfraredConfigEditorService(DatabaseContext databaseContext, string basePath)
    {
        _databaseContext = databaseContext;
        _basePath = basePath;
    }

    public async Task WriteOutConfig(int configId)
    {
        // Write out the config to the file system as json
        // the filename should be "auto_geterated_proxy_{id}.json"
        var config = await _databaseContext
            .ProxyConfigs.Include(x => x.Server)
            .Include(x => x.Domain)
            .SingleAsync(x => x.Id == configId);
        var json = JsonSerializer.Serialize(config.ToJsonModel());
        var filename = $"auto_generated_proxy_{config.Id}.json";

        //check if the directory exists
        if (!Directory.Exists(_basePath))
        {
            Directory.CreateDirectory(_basePath);
        }

        await File.WriteAllTextAsync(_basePath + filename, json);
    }

    public void DeleteConfig(int configId)
    {
        // Delete the config file from the file system
        var filename = $"auto_generated_proxy_{configId}.json";
        var path = _basePath + filename;
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }
}
