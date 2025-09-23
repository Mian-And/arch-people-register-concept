// Core/Application/UseCases/GetZipCode/GetZipCodeHandler.cs
using Client_ConceitoCadastro.Core.Application.Ports;
using Client_ConceitoCadastro.Core.Domain;

namespace Client_ConceitoCadastro.Core.Application.UseCases.GetZipCode;

public sealed class GetZipCodeHandler   // <- public
{
    private readonly ICepLookupService _service;

    public GetZipCodeHandler(ICepLookupService service) => _service = service;

    public Task<Address?> HandleAsync(string cepRaw, CancellationToken ct = default)
        => _service.LookupAsync(new string(cepRaw.Where(char.IsDigit).ToArray()), ct);
}
