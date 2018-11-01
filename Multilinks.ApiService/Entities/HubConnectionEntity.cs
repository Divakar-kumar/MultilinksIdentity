using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Multilinks.ApiService.Entities
{
    public class HubConnectionEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long Id { get; set; }
        public string ConnectionID { get; set; }
        public Guid EndpointId { get; set; }
    }
}
