using System;

namespace MyCoreBanking.API.Data.Entities;

internal sealed class Usuario : BaseEntity
{
    public string Nome { get; set; } = default!;

    public string Cpf { get; set; } = default!;

    public string Email { get; set; } = default!;

    public DateTime Nascimento { get; set; }

    public string Senha { get; set; } = default!;
}