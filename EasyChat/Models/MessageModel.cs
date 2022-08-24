﻿using System.ComponentModel.DataAnnotations.Schema;

namespace EasyChat.Models;

public class MessageModel
{
    [Column("id")]
    public ulong Id { get; set; }

    [Column("content")]
    public string Content { get; set; }

    [Column("time")]
    public DateTime Time { get; set; }

    [Column("source_id")]
    public ulong SourceId { get; set; }

    [Column("target_id")]
    public ulong TargetId { get; set; }
}