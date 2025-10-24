// Infra/Providers/ViaCepLookupService.cs
using Client_ConceitoCadastro.Core.Domain;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Client_ConceitoCadastro.Infrastructure.ZipCode
{
    public sealed class ViaCepLookupService : CepLookupServiceBase
    {
        private readonly HttpClient _http;
        public ViaCepLookupService(HttpClient http) => _http = http;

        protected override async Task<Address?> LookupInternalAsync(string cep, CancellationToken ct)
        {
            // Ex.: https://viacep.com.br/ws/01001000/json/
            using var resp = await _http.GetAsync($"https://viacep.com.br/ws/{cep}/json/", ct);
            resp.EnsureSuccessStatusCode();

            var dto = await resp.Content.ReadFromJsonAsync<ViaCepDto>(cancellationToken: ct);
            if (dto is null || dto.Erro) return null;

            return new Address(
                ZipCode: dto.Cep?.Replace("-", "") ?? cep,
                Street: dto.Logradouro ?? "",
                Neighborhood: dto.Bairro ?? "",
                City: dto.Localidade ?? "",
                State: dto.Uf ?? "",
                Complement: dto.Complemento
            );
        }

        private sealed class ViaCepDto
        {
            [JsonPropertyName("cep")] public string? Cep { get; set; }
            [JsonPropertyName("logradouro")] public string? Logradouro { get; set; }
            [JsonPropertyName("complemento")] public string? Complemento { get; set; }
            [JsonPropertyName("bairro")] public string? Bairro { get; set; }
            [JsonPropertyName("localidade")] public string? Localidade { get; set; }
            [JsonPropertyName("uf")] public string? Uf { get; set; }
            [JsonPropertyName("erro")] public bool Erro { get; set; }
        }
    }
}