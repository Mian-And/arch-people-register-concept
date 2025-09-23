using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.IO;
using System.Runtime.ConstrainedExecution;
using System.Threading.Tasks;

namespace Client_ConceitoCadastro;

// ViewModel que a MainWindow vai usar
public partial class AddressViewModel : ObservableObject
{
    private readonly GetAddressByCep _useCase;

    // Construtor: o DI injeta o caso de uso GetAddressByCep
    public AddressViewModel(GetAddressByCep useCase)
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
        if (string.IsNullOrWhiteSpace(Cep)) return;

        var address = await _useCase.ExecuteAsync(Cep);

        if (address is not null)
        {
            Street = address.Street;
            Neighborhood = address.Neighborhood;
            City = address.City;
            State = address.State;
        }
    }
}
