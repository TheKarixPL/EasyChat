using System.ComponentModel.DataAnnotations.Schema;

namespace EasyChat.Models;

public class UserModel
{
    [Column("id")]
    public ulong Id { get; set; }
    
    [Column("name")]
    public string Name { get; set; }

    [Column("email_address")]
    public string EmailAddress { get; set; }

    [Column("account_creation_time")]
    public DateTime AccountCreationTime { get; set; }

    [Column("is_banned")]
    public bool IsBanned { get; set; }

    [Column("ban_reason")]
    public string BanReason { get; set; }

    [Column("settings")]
    public dynamic Settings { get; set; }

    [Column("avatar")]
    public byte[] Avatar { get; set; }

    public Password Password { get; set; }

    [Column("password")]
    public string RawPassword
    {
        get => Password.ToString();
        set => Password = Password.Parse(value);
    }
}