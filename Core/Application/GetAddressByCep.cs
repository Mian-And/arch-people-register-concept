using Client_ConceitoCadastro.Core.Application.Ports;
using Client_ConceitoCadastro.Core.Domain;
using System.Threading;
using System.Threading.Tasks;

namespace Client_ConceitoCadastro;

// Caso de uso para buscar um endereço a partir do CEP
public sealed class GetAddressByCep
{
    private readonly ICepLookupService _cepLookupService;

    // O DI vai injetar a implementação correta (ViaCepLookupService, CorreiosSoapCepLookupService, etc.)
    public GetAddressByCep(ICepLookupService cepLookupService)
    {
        _cepLookupService = cepLookupService;
    }

    // Método principal do caso de uso
    public Task<Address?> ExecuteAsync(string cep, CancellationToken ct = default)
    {
        return _cepLookupService.LookupAsync(cep, ct);
    }
}
