namespace CustomerService.Application.Common.Results;

public static class CustomerErrors
{
    public static Error NotFound(int id) =>
        new(
            "Customer.NotFound",
            $"Cliente com ID {id} não encontrado.");

    public static Error InvalidId(int id) =>
        new(
            "Customer.InvalidId",
            $"O ID {id} é inválido. O ID deve ser superior a zero.");

    public static Error EmailAlreadyExists(string email) =>
        new(
            "Customer.EmailAlreadyExists",
            $"Já existe um cliente com o email '{email}'.");

    public static Error TaxNumberAlreadyExists(string taxNumber) =>
        new(
            "Customer.TaxNumberAlreadyExists",
            $"Já existe um cliente com o número fiscal '{taxNumber}'.");

    public static Error InvalidPageNumber(int pageNumber) =>
        new(
            "Customer.InvalidPageNumber",
            $"PageNumber deve ser superior a zero. Valor recebido: {pageNumber}.");

    public static Error InvalidPageSize(int pageSize) =>
        new(
            "Customer.InvalidPageSize",
            $"PageSize deve ser superior a zero. Valor recebido: {pageSize}.");
}