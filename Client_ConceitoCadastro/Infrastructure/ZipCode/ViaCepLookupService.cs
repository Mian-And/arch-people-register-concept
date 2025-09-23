// Infra/Providers/ViaCepLookupService.cs
using Client_ConceitoCadastro.Core.Application.Ports;
using Client_ConceitoCadastro.Core.Domain;
using System.Net.Http;
using System.Net.Http.Json;
using System.Runtime.ConstrainedExecution;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

public sealed class ViaCepLookupService : ICepLookupService
{
    private readonly HttpClient _http;
    public ViaCepLookupService(HttpClient http) => _http = http;

    public async Task<Address?> LookupAsync(string normalizedCep, CancellationToken ct = default)
    {
        using var resp = await _http.GetAsync($"https://viacep.com.br/ws/{normalizedCep}/json/", ct);
        resp.EnsureSuccessStatusCode();
        var dto = await resp.Content.ReadFromJsonAsync<ViaCepDto>(cancellationToken: ct);
        if (dto is null || dto.Error) return null;

        return new Address(
            ZipCode: dto.ZipCode?.Replace("-", "") ?? normalizedCep,
            Street: dto.Street ?? "",
            Neighborhood: dto.Neighborhood ?? "",
            City: dto.City ?? "",
            State: dto.State ?? "",
            Complement: dto.Complement
            );
    }

    private sealed class ViaCepDto
    {
        public string? ZipCode { get; set; }
        public string? Street { get; set; }
        public string? Complement { get; set; }
        public string? Neighborhood { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public bool Error { get; set; }
    }
}