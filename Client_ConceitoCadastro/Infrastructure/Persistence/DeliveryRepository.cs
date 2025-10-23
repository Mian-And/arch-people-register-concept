using Client_ConceitoCadastro.Core.Application.Ports;
using Client_ConceitoCadastro.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client_ConceitoCadastro.Infrastructure.Persistence
{
    public class DeliveryRepository : IDeliveryRepository
    {
        private readonly AppDbContext _db;
        public DeliveryRepository(AppDbContext db) => _db = db;

        public Task AddAsync(Delivery delivery, CancellationToken ct = default)
            => _db.Deliveries.AddAsync(delivery, ct).AsTask();

        public Task SaveChangesAsync(CancellationToken ct = default)
            => _db.SaveChangesAsync(ct);
    }

}
