// Core/Domain/ValueObjects/Cep.cs
namespace Core.Domain.ValueObjects;
public readonly struct BrazilCep
{
    public string Value { get; }
    private BrazilCep(string value) => Value = value;

    public static bool TryCreate(string input, out BrazilCep cep)
    {
        var digits = new string(input.Where(char.IsDigit).ToArray());
        if (digits.Length == 8)
        {
            cep = new BrazilCep(digits);
            return true;
        }
        cep = default;
        return false;
    }

    public override string ToString() => Value;
}
