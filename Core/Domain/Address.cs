using System;
namespace Client_ConceitoCadastro.Core.Domain;

// Core/Contracts
public record Address(
    string ZipCode,
    string Street,
    string Neighborhood,
    string City,
    string State,
    string? Complement = null
);
