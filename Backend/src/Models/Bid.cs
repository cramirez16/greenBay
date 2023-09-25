using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Src.Models
{
    public class Bid
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public decimal BidAmount { get; set; }
        [Required]
        public int BiderId { get; set; }

        [Required]
        public int ItemId { get; set; }

        // navigation properties
        [ForeignKey("BiderId")]
        [Required]
        public required User Bider { get; set; }

        [ForeignKey("ItemId")]
        [JsonIgnore]
        [Required]
        public required Item Item { get; set; }

    }
}