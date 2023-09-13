namespace EAnalytics.Common.Configurations;

public class DataBaseConfiguration
{
    public int MaxRetryCount { get; set; }
    public int CommandTimeOut { get; set; }
    public bool EnableDetailedErrors { get; set; }
    public bool EnableSesitiveDataLogging { get; set; }
}
