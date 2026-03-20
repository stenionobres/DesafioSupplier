namespace DesafioSupplier.Api.Shared;

public class ErroModelResponse : IStatusResponse
{
    public string Status => "ERRO";
    public required string DetalheErro { get; set; }
}
