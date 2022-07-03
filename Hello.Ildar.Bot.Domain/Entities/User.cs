namespace Hello.Ildar.Bot.Domain.Entities;

/// <summary>
/// Пользователь.
/// </summary>
public class User : EntityBase
{
    /// <summary>
    /// Unique identifier for this chat. This number may have more
    /// than 32 significant bits and some programming languages may have
    /// difficulty/silent defects in interpreting it. But it has
    /// at most 52 significant bits, so a signed 64-bit integer
    /// or double-precision float type are safe for storing this identifier.
    /// </summary>
    public int ChatId { get; set; }

    /// <summary>
    /// Type of chat, can be either “private”, “group”, “supergroup” or “channel”
    /// </summary>
    public int ChatType { get; set; }

    /// <summary>
    /// Optional. Title, for supergroups, channels and group chats
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// Optional. Username, for private chats, supergroups and channels if available
    /// </summary>
    public string? Username { get; set; }

    /// <summary>
    /// Optional. First name of the other party in a private chat
    /// </summary>
    public string? FirstName { get; set; }

    /// <summary>
    /// Optional. Last name of the other party in a private chat
    /// </summary>
    public string? LastName { get; set; }
}