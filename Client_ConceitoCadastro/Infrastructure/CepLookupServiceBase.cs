using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

// Core/Contracts
public sealed record Address(
    string Cep,
    string Street,
    string Neighborhood,
    string City,
    string State,
    string? Complement = null
);

public interface ICepLookupService
{
    Task<Address?> LookupAsync(string cep, CancellationToken ct = default);
}

// Core/Base: Noralize zip code and valide before send to provider
public abstract class CepLookupServiceBase : ICepLookupService
{
    public async Task<Address?> LookupAsync(string cep, CancellationToken ct = default)
    {
        var normalized = Normalize(cep);
        Validate(normalized);
        return await LookupInternalAsync(normalized, ct);
    }

    protected abstract Task<Address?> LookupInternalAsync(string cep, CancellationToken ct);

    protected static string Normalize(string cep) => new string(cep.Where(char.IsDigit).ToArray());

    protected static void Validate(string cep)
    {
        if (cep.Length != 8) throw new ArgumentException("CEP deve conter 8 dígitos.", nameof(cep));
    }
}
