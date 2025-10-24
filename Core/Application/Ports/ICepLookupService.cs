using Client_ConceitoCadastro.Core.Domain;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Client_ConceitoCadastro.Core.Application.Ports;

public interface ICepLookupService
{
    Task<Address?> LookupAsync(string normalizedCep, CancellationToken ct = default);
}