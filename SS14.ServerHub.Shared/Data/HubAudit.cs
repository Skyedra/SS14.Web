﻿using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using SS14.ServerHub.Shared.Helpers;

namespace SS14.ServerHub.Shared.Data;

/// <summary>
/// Stores an audit log of administrative actions taken with the server hub.
/// </summary>
public sealed class HubAudit : IDisposable
{
    /// <summary>
    /// ID of this database entity.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The user ID of the admin that did the action.
    /// </summary>
    public Guid Actor { get; set; }
    
    /// <summary>
    /// The type of event stored by this log entry.
    /// </summary>
    public HubAuditType Type { get; set; } = default!;
    
    /// <summary>
    /// JSON data for this log entry. Should be (de)serialized based on the <see cref="Type"/>.
    /// </summary>
    [Required] public JsonDocument Data { get; set; } = default!;
    
    /// <summary>
    /// Time this event took place.
    /// </summary>
    public DateTime Time { get; set; }

    public void Dispose() => Data.Dispose();
}

[AttributeUsage(AttributeTargets.Field)]
internal sealed class EntryTypeAttribute : Attribute
{
    public Type Type { get; }

    public EntryTypeAttribute(Type type)
    {
        Type = type;
    }
}  

/// <summary>
/// Types of events stored in the <see cref="HubAudit"/> table.
/// </summary>
[SuppressMessage("ReSharper", "UnusedMember.Global")]
public enum HubAuditType
{
    //
    // 1-10000: community management
    //
    
    /// <summary>
    /// A new, empty tracked community was created by an admin.
    /// </summary>
    [EntryType(typeof(HubAuditCommunityCreated))]
    CommunityCreated = 1,
    
    /// <summary>
    /// The name of a tracked community was changed by an admin.
    /// </summary>
    [EntryType(typeof(HubAuditCommunityChangedName))]
    CommunityChangedName = 2,
    
    /// <summary>
    /// The banned status of a tracked community was changed by an admin.
    /// </summary>
    [EntryType(typeof(HubAuditCommunityChangedBanned))]
    CommunityChangedBanned = 3,
    
    /// <summary>
    /// The notes of a tracked community were changed by an admin.
    /// </summary>
    [EntryType(typeof(HubAuditCommunityChangedNotes))]
    CommunityChangedNotes = 4,
    
    /// <summary>
    /// A tracked community was deleted by an admin.
    /// </summary>
    [EntryType(typeof(HubAuditCommunityDeleted))]
    CommunityDeleted = 5,
    
    // 101-200: community address management
    
    /// <summary>
    /// An address was added to a tracked community by an admin.
    /// </summary>
    [EntryType(typeof(HubAuditCommunityAddressAdd))]
    CommunityAddressAdd = 101,
    
    /// <summary>
    /// An address was removed from a tracked community by an admin.
    /// </summary>
    [EntryType(typeof(HubAuditCommunityAddressDelete))]
    CommunityAddressDelete = 102,

    // 201-300: community address management

    /// <summary>
    /// A domain was added to a tracked community by an admin.
    /// </summary>
    [EntryType(typeof(HubAuditCommunityDomainAdd))]
    CommunityDomainAdd = 201,
    
    /// <summary>
    /// A domain was removed from a tracked community by an admin.
    /// </summary>
    [EntryType(typeof(HubAuditCommunityDomainDelete))]
    CommunityDomainDelete = 202
}

// ReSharper disable NotAccessedPositionalProperty.Global
/// <summary>
/// Simple record that stores name and ID of a community, for grouping in log entries.
/// </summary>
/// <param name="Id">The ID of the <see cref="TrackedCommunity"/>.</param>
/// <param name="Name">The name of the <see cref="TrackedCommunity"/>, at the time the event was recorded.</param>
public sealed record HubAuditCommunity(int Id, string Name)
{
    public static implicit operator HubAuditCommunity(TrackedCommunity community)
    {
        if (community.Id == 0)
            throw new ArgumentException("Invalid ID");
        
        return new HubAuditCommunity(community.Id, community.Name);
    }
}

public sealed record HubAuditCommunityAddress(int Id, string Address)
{
    public static implicit operator HubAuditCommunityAddress(TrackedCommunityAddress address)
    {
        if (address.Id == 0)
            throw new ArgumentException("Invalid ID");

        return new HubAuditCommunityAddress(address.Id, address.Address.FormatCidr());
    }
}

public sealed record HubAuditCommunityDomain(int Id, string DomainName)
{
    public static implicit operator HubAuditCommunityDomain(TrackedCommunityDomain domain)
    {
        if (domain.Id == 0)
            throw new ArgumentException("Invalid ID");

        return new HubAuditCommunityDomain(domain.Id, domain.DomainName);
    }
}
// ReSharper restore NotAccessedPositionalProperty.Global

public abstract record HubAuditEntry
{
    private static readonly Dictionary<HubAuditType, Type> EnumToType;
    private static readonly Dictionary<Type, HubAuditType> TypeToEnum;

    public HubAuditType Type => TypeToEnum[GetType()];
    
    static HubAuditEntry()
    {
        EnumToType = new Dictionary<HubAuditType, Type>();
        TypeToEnum = new Dictionary<Type, HubAuditType>();

        var type = typeof(HubAuditType);
        
        foreach (var value in Enum.GetValues<HubAuditType>())
        {
            var name = value.ToString();
            
            var field = type.GetMember(name)[0];
            var attribute = (EntryTypeAttribute?) Attribute.GetCustomAttribute(field, typeof(EntryTypeAttribute));

            if (attribute == null)
                throw new InvalidOperationException($"{name} is missing {nameof(EntryTypeAttribute)}");

            if (!attribute.Type.IsAssignableTo(typeof(HubAuditEntry)))
                throw new InvalidOperationException($"{attribute.Type} must inherit {nameof(HubAuditEntry)}");
            
            EnumToType.Add(value, attribute.Type);
            TypeToEnum.Add(attribute.Type, value);
        }
    }

    public static HubAuditEntry Deserialize(HubAuditType type, JsonDocument document)
    {
        return (HubAuditEntry)(document.Deserialize(EnumToType[type]) ?? throw new InvalidDataException());
    }
}

// ReSharper disable NotAccessedPositionalProperty.Global
public sealed record HubAuditCommunityCreated(HubAuditCommunity Community) : HubAuditEntry;
public sealed record HubAuditCommunityChangedName(HubAuditCommunity Community, string OldName, string NewName) : HubAuditEntry;
// We really storing a "before" and "after" for a binary property.
public sealed record HubAuditCommunityChangedBanned(HubAuditCommunity Community, bool OldBanned, bool NewBanned) : HubAuditEntry;
public sealed record HubAuditCommunityChangedNotes(HubAuditCommunity Community, string OldNotes, string NewNotes) : HubAuditEntry;
public sealed record HubAuditCommunityDeleted(HubAuditCommunity Community) : HubAuditEntry;
public sealed record HubAuditCommunityAddressAdd(HubAuditCommunity Community, HubAuditCommunityAddress Address) : HubAuditEntry;
public sealed record HubAuditCommunityAddressDelete(HubAuditCommunity Community, HubAuditCommunityAddress Address) : HubAuditEntry;
public sealed record HubAuditCommunityDomainAdd(HubAuditCommunity Community, HubAuditCommunityDomain Domain) : HubAuditEntry;
public sealed record HubAuditCommunityDomainDelete(HubAuditCommunity Community, HubAuditCommunityDomain Domain) : HubAuditEntry;
// ReSharper restore NotAccessedPositionalProperty.Global
