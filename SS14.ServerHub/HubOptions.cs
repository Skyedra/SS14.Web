namespace SS14.ServerHub;

public sealed class HubOptions
{
    public const string Position = "Hub";
        
    /// <summary>
    /// How long until drop a server off hub list.  
    /// 
    /// Default for server is to ping every 2 minutes, and for hub to drop for 3, which is tight but 
    /// theoretically okay.  However, multiple servers seem to be missing the two minute window 
    /// consistently, and being up to ~72sec late.  Because of that, just going to set this to 15 minutes.
    /// </summary>
    /// <value></value>
    public float AdvertisementExpireMinutes { get; set; } = 7;
        
    /// <summary>
    /// When a server advertises itself with the hub, we check whether we can reach the address.
    /// This is the timeout for that test.
    /// </summary>
    public float AdvertisementStatusTestTimeoutSeconds { get; set; } = 5;

    /// <summary>
    /// When fetching <code>/status</code> from advertised servers, maximum size of response bodies in kilobytes.
    /// </summary>
    public int MaxStatusResponseSize = 2;
    
    /// <summary>
    /// When fetching <code>/info</code> from advertised servers, maximum size of response bodies in kilobytes.
    /// </summary>
    public int MaxInfoResponseSize = 10;
}