using System.ComponentModel.DataAnnotations.Schema;

namespace EasyChat.Models;

public class AttachmentModel
{
    [Column("id")]
    public long Id { get; set; }

    [Column("name")]
    public string Name { get; set; }

    [Column("content_type")]
    public string ContentType { get; set; }

    [Column("content")]
    public byte[] Content { get; set; }

    [Column("key")]
    public string Key { get; set; }
}