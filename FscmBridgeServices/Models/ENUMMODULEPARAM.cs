using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FscmBridgeServices.Models
{

    [Table("ENUMMODULEPARAM")]
    public class ENUMMODULEPARAM
    {
        [Key]
        public int InsSeq { get; set; }
        public string ModuleId { get; set; } = string.Empty;
        public string MKey { get; set; } = string.Empty;
        public string? MValue { get; set; }
        public string? MDesc { get; set; }
     
        public DateTime? InsDate { get; set; }
    }
}