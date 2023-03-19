using FluentValidation;
using System.Text.Json.Serialization;

namespace MyCoreBanking.Args;

public class TransacoesPostArgs
{
    [JsonPropertyName("descricao")]
    public string Descricao { get; set; } = default!;

    [JsonPropertyName("observacao")]
    public string? Observacao { get; set; }

    [JsonPropertyName("valor")]
    public decimal Valor { get; set; }

    [JsonPropertyName("dataPagamento")]
    public DateTime DataPagamento { get; set; }

    [JsonPropertyName("meioDePagamentoId")]
    public Guid MeioDePagamentoId { get; set; }

    public sealed class Validator : AbstractValidator<TransacoesPostArgs>
    {
        public Validator()
        {
            RuleFor(x => x.Descricao)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.Observacao)
                .MaximumLength(100);

            RuleFor(x => x.Valor)
                .GreaterThan(0);

            RuleFor(x => x.DataPagamento)
                .NotEmpty();

            RuleFor(x => x.MeioDePagamentoId)
                .NotEmpty();
        }
    }
}
