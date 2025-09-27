using Client_ConceitoCadastro.Core.Application;
using Client_ConceitoCadastro.Core.Application.UseCases.GetZipCode;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.IO;
using System.Reflection.Metadata;
using System.Runtime.ConstrainedExecution;
using System.Threading.Tasks;

namespace Client_ConceitoCadastro;

// ViewModel que a MainWindow vai usar
public partial class AddressViewModel : ObservableObject
{
    private readonly GetZipCodeHandler _useCase;

    // Construtor: o DI injeta o caso de uso GetAddressByCep
    public AddressViewModel(GetZipCodeHandler useCase) => _useCase = useCase;

    // Propriedades observáveis ligadas ao XAML
    [ObservableProperty] private string cep;
    [ObservableProperty] private string street;
    [ObservableProperty] private string neighborhood;
    [ObservableProperty] private string city;
    [ObservableProperty] private string state;
    [ObservableProperty] private string statusMessage;

    // Comando que a View pode chamar (ex.: Button "Buscar")
    [RelayCommand]
    private async Task LookupAsync()
    {
        StatusMessage = "Consultando...";
        try
        {
            var address = await _useCase.HandleAsync(Cep);

            if (address is null || string.IsNullOrEmpty(address.Street))
            {
                Street = Neighborhood = City = State = string.Empty;
                StatusMessage = "CEP não encontrado.";
                return;
            }

            Street = address.Street;
            Neighborhood = address.Neighborhood;
            City = address.City;
            State = address.State;
            StatusMessage = "OK";
        }
        catch (Exception ex)
        {
            // mensagem amigável pra UI; log detalhado fica na Infra
            StatusMessage = "Erro ao consultar CEP.";
            // TODO: expor ILogging se quiser detalhar aqui
        }
    }
}
