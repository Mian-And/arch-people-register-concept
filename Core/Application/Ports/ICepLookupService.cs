using System;
using System.Threading;
using System.Threading.Tasks;

public interface ICepLookupService
{
    Task<Address?> LookupAsync(string cep, CancellationToken ct = default);
}