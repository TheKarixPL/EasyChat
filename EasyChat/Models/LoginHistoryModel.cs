using System.ComponentModel.DataAnnotations.Schema;
using System.Net;

namespace EasyChat.Models;

public class LoginHistoryModel
{
    [Column("id")]
    public long Id { get; set; }

    [Column("ip")]
    public IPAddress Ip { get; set; }

    [Column("time")]
    public DateTime Time { get; set; }
}