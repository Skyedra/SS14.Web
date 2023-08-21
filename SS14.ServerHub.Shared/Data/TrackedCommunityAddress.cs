using System.ComponentModel.DataAnnotations.Schema;
using System.Net;
using SS14.ServerHub.Shared.Helpers;

namespace SS14.ServerHub.Shared.Data;

/// <summary>
/// Represents a single IP address range associated with a <see cref="TrackedCommunity"/>.
/// </summary>
public class TrackedCommunityAddress
{
    /// <summary>
    /// The ID of this entity in the database.
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// The address range in question.
    /// </summary>
    //[Column(TypeName = "inet")] // Does not exist in MySQL
    [NotMapped]
    public (IPAddress, int) Address 
    { 
        get
        {
            // If I decide to figure out the admin interface stuff, I will either need to
            // 1) refactor admin interface to use start + long instead of CIDR, or
            // 2) adapt this get/set to return an address tuple in the format that is expected

            throw new NotImplementedException("TODO: Implement address get/set (needed for admin interface only, as far as I know)");

            (IPAddress, int) address = new (IPAddress.None, 0);
            address.Item1 = StartAddressRange;
            address.Item2 = 32; // definitely wrong, should convert CIDR here in future

            return address;
        }
    
        set
        {
            // See notes above
            throw new NotImplementedException("TODO: Implement address get/set (needed for admin interface only, as far as I know)");

            StartAddressRange = value.Item1;
            EndAddressRange = value.Item1; // definitely wrong, implement before removing above exception
        }
    }
    // https://stackoverflow.com/a/596616

    // https://dev.mysql.com/blog-archive/mysql-8-0-storing-ipv6/
    // Need to specifically use varbinary as ipv4 and ipv6 are different lengths
    // (Using binary will result in ipv4 being mangled)
    [Column(TypeName = "varbinary(16)")]
    public IPAddress StartAddressRange;

    [Column(TypeName = "varbinary(16)")]
    public IPAddress EndAddressRange;

    /// <summary>
    /// The ID of the <see cref="TrackedCommunity"/> this address belongs to.
    /// </summary>
    public int TrackedCommunityId { get; set; }
    
    // Navigation properties
    public TrackedCommunity TrackedCommunity { get; set; } = default!;
}