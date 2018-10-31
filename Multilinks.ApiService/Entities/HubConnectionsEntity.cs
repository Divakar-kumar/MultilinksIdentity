using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Multilinks.ApiService.Entities
{
    public class HubConnectionsEntity
    {
        [Key]
        public string ConnectionID { get; set; }
        public Guid EndpointId { get; set; }
        public bool Connected { get; set; }
    }
}
