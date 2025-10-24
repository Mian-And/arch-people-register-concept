using Client_ConceitoCadastro.Core.Application.Ports;
using Client_ConceitoCadastro.Core.Domain;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Client_ConceitoCadastro.Infrastructure.ZipCode
{

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
}