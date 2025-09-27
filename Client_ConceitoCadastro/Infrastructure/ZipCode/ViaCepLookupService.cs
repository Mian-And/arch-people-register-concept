using Client_ConceitoCadastro.Core.Application.Ports;
using Client_ConceitoCadastro.Core.Domain;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Client_ConceitoCadastro.Infrastructure.ZipCode;

public sealed class ViaCepLookupService : ICepLookupService
{
    private readonly HttpClient _http;
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        PropertyNameCaseInsensitive = true
    };

    public ViaCepLookupService(HttpClient http) => _http = http;

    public async Task<Address?> LookupAsync(string normalizedCep, CancellationToken ct = default)
    {
        // Defensive normalization
        normalizedCep = new string(normalizedCep.Where(char.IsDigit).ToArray());
        if (normalizedCep.Length != 8) return null;

        using var resp = await _http.GetAsync($"https://viacep.com.br/ws/{normalizedCep}/json/", ct);
        resp.EnsureSuccessStatusCode();

        var raw = await resp.Content.ReadAsStringAsync(ct);
        if (string.IsNullOrWhiteSpace(raw)) return null;

        ViaCepDto? dto;
        try
        {
            dto = JsonSerializer.Deserialize<ViaCepDto>(raw, JsonOptions);
        }
        catch
        {
            // optionally log 'raw'
            return null;
        }

        if (dto is null || dto.Erro) return null;

        return new Address(
            ZipCode: (dto.Cep ?? normalizedCep).Replace("-", ""),
            Street: dto.Logradouro ?? string.Empty,
            Neighborhood: dto.Bairro ?? string.Empty,
            City: dto.Localidade ?? string.Empty,
            State: dto.Uf ?? string.Empty,
            Complement: dto.Complemento
        );
    }

    // DTO espelha o payload do ViaCEP (português por contrato da API)
    private sealed class ViaCepDto
    {
        [JsonPropertyName("cep")] public string? Cep { get; set; }
        [JsonPropertyName("logradouro")] public string? Logradouro { get; set; }
        [JsonPropertyName("complemento")] public string? Complemento { get; set; }
        [JsonPropertyName("unidade")] public string? Unidade { get; set; }
        [JsonPropertyName("bairro")] public string? Bairro { get; set; }
        [JsonPropertyName("localidade")] public string? Localidade { get; set; }
        [JsonPropertyName("uf")] public string? Uf { get; set; }
        [JsonPropertyName("estado")] public string? Estado { get; set; }
        [JsonPropertyName("regiao")] public string? Regiao { get; set; }
        [JsonPropertyName("ibge")] public string? Ibge { get; set; }
        [JsonPropertyName("gia")] public string? Gia { get; set; }
        [JsonPropertyName("ddd")] public string? Ddd { get; set; }
        [JsonPropertyName("siafi")] public string? Siafi { get; set; }
        [JsonPropertyName("erro")] public bool Erro { get; set; }
    }
}
