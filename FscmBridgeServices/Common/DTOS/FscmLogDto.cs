using System;

namespace FscmBridgeServices.Common.DTOS
{
    public class FscmLogDto
    {
        public int Id { get; set; }
        public string? Type { get; set; }
        public string? Name { get; set; }
        public string? VALUE { get; set; }
        public string? RequestJson { get; set; }
        public string? ResponseJson { get; set; }
        public string? ApRegno { get; set; }
        public string? UuidResponse { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
