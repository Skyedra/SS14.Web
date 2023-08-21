using System.Net;
using System.Net.Sockets;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using SS14.ServerHub.Shared.Data;
using SS14.ServerHub.Shared.Helpers;

namespace SS14.ServerHub.Shared;

/// <summary>
/// Shared functionality for pruning old data
/// </summary>
public static class DataPrune
{
    /// <summary>
    /// History earlier than this records will be auto-expunged.
    /// (Prevent filling database up with pointless history)
    /// </summary>
    public const int DAYS_TO_PRUNE = 7;

    public static void PruneServerStatusArchive(HubDbContext dbContext)
    {
        dbContext.Database.ExecuteSql($"DELETE FROM ServerStatusArchive WHERE `Time` < DATE_SUB(NOW(), INTERVAL {DAYS_TO_PRUNE} DAY)");
    }
}