using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client_ConceitoCadastro.Core.Domain.Entities
{
    // Core/Domain/Entities/Delivery.cs
    public class Delivery
    {
        public Guid Id { get; set; }
        // recipient + message
        public string RecipientName { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        // Address
        public Address Address { get; set; } = default!;
        public string HouseNumber { get; set; } = string.Empty;
        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    }

}
