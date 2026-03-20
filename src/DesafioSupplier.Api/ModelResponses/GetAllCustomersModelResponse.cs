using DesafioSupplier.Domain.Entities;

namespace DesafioSupplier.Api.ModelResponses;

public class GetAllCustomersModelResponse
{
    public required List<Customer> Customers { get; set; }
}
