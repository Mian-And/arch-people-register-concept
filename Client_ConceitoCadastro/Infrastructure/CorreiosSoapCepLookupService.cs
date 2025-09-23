// Infra/Providers/CorreiosSoapCepLookupService.cs
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

public sealed class CorreiosSoapCepLookupService : CepLookupServiceBase
{
    private readonly HttpClient _http;
    private const string Endpoint = "https://apps.correios.com.br/SigepMasterJPA/AtendeClienteService/AtendeCliente";

    public CorreiosSoapCepLookupService(HttpClient http) => _http = http;

    protected override async Task<Address?> LookupInternalAsync(string cep, CancellationToken ct)
    {
        var envelope = BuildSoapEnvelope(cep);
        using var req = new HttpRequestMessage(HttpMethod.Post, Endpoint)
        {
            Content = new StringContent(envelope, Encoding.UTF8, "text/xml")
        };
        req.Headers.TryAddWithoutValidation("SOAPAction", "consultaCEP");

        using var resp = await _http.SendAsync(req, ct);
        resp.EnsureSuccessStatusCode();

        var xml = XDocument.Parse(await resp.Content.ReadAsStringAsync(ct));
        // Namespaces
        XNamespace s = "http://schemas.xmlsoap.org/soap/envelope/";
        XNamespace ns = "http://cliente.bean.master.sigep.bsb.correios.com.br/";

        var result = xml
          .Descendants(ns + "return")
          .Select(x => new Address(
              Cep: (string?)x.Element(ns + "cep") ?? cep,
              Street: (string?)x.Element(ns + "end") ?? "",
              Neighborhood: (string?)x.Element(ns + "bairro") ?? "",
              City: (string?)x.Element(ns + "cidade") ?? "",
              State: (string?)x.Element(ns + "uf") ?? "",
              Complement: (string?)x.Element(ns + "complemento2")
          ))
          .FirstOrDefault();

        return result;
    }

    private static string BuildSoapEnvelope(string cep) => $@"<?xml version=""1.0"" encoding=""utf-8""?>
<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:cli=""http://cliente.bean.master.sigep.bsb.correios.com.br/"">
  <soapenv:Header/>
  <soapenv:Body>
    <cli:consultaCEP>
      <cep>{cep}</cep>
    </cli:consultaCEP>
  </soapenv:Body>
</soapenv:Envelope>";
}
