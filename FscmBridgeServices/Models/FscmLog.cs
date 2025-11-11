using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.ComponentModel.DataAnnotations;

namespace FscmBridgeServices.Models
{
    [Table("FscmLogs")]
    public class FscmLog
    {
        [Key]
        public int Id { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }

        public string VALUE { get; set; }
        public string RequestJson { get; set; }
        public string ResponseJson { get; set; }
        public string ap_regno{get;set;}
        public string UuidResponse { get; set; } = string.Empty;
        public string CreatedBy { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; 

    }
}