using System.ComponentModel.DataAnnotations;

namespace MyCoreBanking.Web.Pages.Cartoes;

public class CartaoViewModel
{
    [Required(ErrorMessage = "Nome do titular obrigatório")]
    public string? NomeTitularCartao { get; set; }

    [Required(ErrorMessage = "Informe os 4 primeiros dígitos do cartão")]
    [MinLength(4, ErrorMessage = "Mínimo 4 digitos")]
    [MaxLength(4, ErrorMessage = "Máximo 4 digitos")]
    public string? PrimeirosQuatroDigitos { get; set; }

    [Required(ErrorMessage = "Informe os 4 últimos dígitos do cartão")]
    [MinLength(4, ErrorMessage = "Mínimo 4 digitos")]
    [MaxLength(4, ErrorMessage = "Máximo 4 digitos")]
    public string? UltimosQuatroDigitos { get; set; }
}