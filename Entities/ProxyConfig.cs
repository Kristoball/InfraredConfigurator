namespace InfraredConfigurator.Entities;

public class ProxyConfig
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public Server? Server { get; set; }
    public int ServerId { get; set; }
    public string Port { get; set; } = string.Empty;
    public bool SendProxyProtocol = true;
    public string DomainString => $"{SubDomain}.{Domain?.DomainString}";
    public string SubDomain { get; set; } = string.Empty;
    public Domain? Domain { get; set; }
    public int DomainId { get; set; }
    public string DisconnectMessage { get; set; } = string.Empty;
    public string OfflineStatus { get; set; } =
        "We are sorry {{username}}, the server is currently offline!";
    public string OnlineStatus { get; set; } = "Come join us {{username}}!";

    public ProxyConfigJsonModel ToJsonModel()
    {
        return new ProxyConfigJsonModel
        {
            domainName = DomainString,
            proxyTo = $"{Server?.IpAddress}:{Port}",
            sendProxyProtocol = SendProxyProtocol,
            disconnectMessage = DisconnectMessage,
            offlineStatus = new ServerStatus { motd = OfflineStatus },
            onlineStatus = new ServerStatus { motd = OnlineStatus },
        };
    }
}

public class ProxyConfigJsonModel
{
    public string domainName { get; set; } = string.Empty;
    public string proxyTo { get; set; } = string.Empty;
    public bool sendProxyProtocol { get; set; } = true;
    public string disconnectMessage { get; set; } = string.Empty;
    public ServerStatus offlineStatus { get; set; } = new();
    public ServerStatus onlineStatus { get; set; } = new();
}

public class ServerStatus
{
    public string motd { get; set; } = string.Empty;
}
