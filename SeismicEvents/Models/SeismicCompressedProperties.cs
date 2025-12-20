using MessagePack;
using System.ComponentModel.DataAnnotations.Schema;

namespace SeismicEventsFireEvents.Models
{
    [MessagePackObject]
    public class SeismicCompressedProperties
    {
        [Key(0)] public int Id { get; set; }
        [Key(1)] public string FlynnRegion { get; set; }
        [Key(2)] public int ChunkId { get; set; }
        [Key(3)] public int EventCount { get; set; }
        [Key(4)] public string CompressionType { get; set; } = "gzip";
        [Key(5)] public double? MinDepth { get; set; }
        [Key(6)] public double? MaxDepth { get; set; }
        [Key(7)] public double? MinMagnitude { get; set; }
        [Key(8)] public double? MaxMagnitude { get; set; }
        [Key(9)] public double? MinLongitude { get; set; }
        [Key(10)] public double? MaxLongitude { get; set; }
        [Key(11)] public double? MinLatitude { get; set; }
        [Key(12)] public double? MaxLatitude { get; set; }
        [Key(13)] public DateTime RegistrationDate { get; set; }
        [Key(14)] public DateTime? FirstEventDate { get; set; }
        [Key(15)] public DateTime? LastEventDate { get; set; }
        [Column(TypeName = "varbinary(max)")]
        [Key(16)] public byte[] CompressedEventProperties { get; set; }
    }

}
