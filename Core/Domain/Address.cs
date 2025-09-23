using System;

// Core/Contracts
public sealed record Address(
    string Cep,
    string Street,
    string Neighborhood,
    string City,
    string State,
    string? Complement = null
);
