namespace Vinstag.InstagramAPI.Models;

public record EndpointsConfig
{
    public string Id { get; set; }
    public string Endpoint { get; init; }
    public string EndpointHash { get; init; }
    public string NextQueryHash { get; set; } = "";
};