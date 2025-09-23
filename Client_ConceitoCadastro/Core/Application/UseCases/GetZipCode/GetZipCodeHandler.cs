using Client_ConceitoCadastro.Core.Application.Ports;
using Client_ConceitoCadastro.Core.Domain;
using Core.Domain.ValueObjects;

namespace Client_ConceitoCadastro.Core.Application.UseCases.GetZipCode
{
    internal sealed class GetZipCodeHandler
    {
        private readonly ICepLookupService _service;
        public GetZipCodeHandler(ICepLookupService service) => _service = service;

        public async Task<Address?> HandleAsync(string cepRaw, CancellationToken ct = default)
        {
            if (!BrazilCep.TryCreate(cepRaw, out var cep))
                throw new ArgumentException("CEP inválido.", nameof(cepRaw));

            return await _service.LookupAsync(cep.Value, ct);
        }

    }
}
