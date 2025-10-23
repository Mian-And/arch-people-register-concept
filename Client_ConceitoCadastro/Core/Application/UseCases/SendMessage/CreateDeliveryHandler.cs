using Client_ConceitoCadastro.Core.Application.Ports;
using Client_ConceitoCadastro.Core.Domain;
using Client_ConceitoCadastro.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client_ConceitoCadastro.Core.Application.UseCases.SendMessage
{
    public sealed record CreateDeliveryCommand(
       string RecipientName, string Message,
       string Cep, string Street, string Neighborhood, string City, string State,
       string HouseNumber, string? Complement);

    public sealed class CreateDeliveryHandler
    {
        private readonly IDeliveryRepository _repo;
        public CreateDeliveryHandler(IDeliveryRepository repo) => _repo = repo;

        public async Task<Guid> HandleAsync(CreateDeliveryCommand cmd, CancellationToken ct = default)
        {
            // CEP normalizado (apenas dígitos)
            var normalizedCep = new string(cmd.Cep.Where(char.IsDigit).ToArray());

            var entity = new Delivery
            {
                Id = Guid.NewGuid(),
                RecipientName = cmd.RecipientName,
                Message = cmd.Message,
                // >>> CEP e demais campos de endereço ficam dentro do Address (owned type)
                Address = new Address(
                ZipCode: normalizedCep,
                Street: cmd.Street,
                Neighborhood: cmd.Neighborhood,
                City: cmd.City,
                State: cmd.State,
                Complement: cmd.Complement), // ver observação abaixo
                HouseNumber = cmd.HouseNumber
            };
            await _repo.AddAsync(entity, ct);
            await _repo.SaveChangesAsync(ct);
            return entity.Id;
        }
    }

}
