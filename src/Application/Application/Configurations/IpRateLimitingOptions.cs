namespace Application.Application.Configurations;

public class IpRateLimitingOptions
{
    public bool EnableEndpointRateLimiting { get; set; }
    public bool StackBlockedRequests { get; set; }
    public string RealIpHeader { get; set; }
    public string ClientIdHeader { get; set; }
    public int HttpStatusCode { get; set; }
    public List<GeneralRuleModel> GeneralRules { get; set; }
}

public class GeneralRuleModel
{
    public string Endpoint { get; set; }
    public string Period { get; set; }
    public int Limit { get; set; }
}