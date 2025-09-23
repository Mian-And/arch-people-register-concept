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
    public AddressViewModel(GetZipCodeHandler useCase)
    {
        _useCase = useCase;
    }

    // Propriedades observáveis ligadas ao XAML
    [ObservableProperty] private string cep;
    [ObservableProperty] private string street;
    [ObservableProperty] private string neighborhood;
    [ObservableProperty] private string city;
    [ObservableProperty] private string state;

    // Comando que a View pode chamar (ex.: Button "Buscar")
    [RelayCommand]
    private async Task LookupAsync()
    {
        var result = await _useCase.HandleAsync(Cep);

        if (result is not null)
        {
            Street = result.Street;
            Neighborhood = result.Neighborhood;
            City = result.City;
            State = result.State;
        }
        else
        {
            Street = Neighborhood = City = State = "CEP não encontrado.";
        }
    }
}
