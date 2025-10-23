using Client_ConceitoCadastro.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client_ConceitoCadastro.Core.Application.Ports
{
    public interface IDeliveryRepository
    {
        Task AddAsync(Delivery delivery, CancellationToken ct = default);
        Task SaveChangesAsync(CancellationToken ct = default);
    }

}
