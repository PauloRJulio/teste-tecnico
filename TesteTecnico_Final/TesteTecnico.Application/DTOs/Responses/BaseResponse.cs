namespace TesteTecnico.Application.DTOs.Responses;

// DTO base para respostas de API sem tipo específico
// Contém informações padrão de sucesso, mensagem e status da operação
public class BaseResponse(bool success, string? message = null, Status? status = null)
{
    // Indica se a operação foi bem-sucedida
    public bool Success { get; set; } = success;

    // Mensagem opcional detalhando o resultado da operação
    public string? Message { get; set; } = message;

    // Status da operação (Success, Error, Warning, NotFound, Default)
    public string? Status { get; set; } = status.ToString();
}

// DTO base genérico para respostas de API com um item específico
// Herda BaseResponse e adiciona o item retornado
public class BaseResponse<T>(bool success, string? message = null, T? item = default, Status? status = null) 
    : BaseResponse(success, message, status)
{
    // Item retornado pela operação (pode ser uma entidade, DTO ou valor)
    public T? Item { get; set; } = item;
}

// Enum que representa o status de uma operação
public enum Status
{
    Success,   // Operação concluída com sucesso
    Error,     // Ocorreu um erro durante a operação
    Warning,   // Operação concluída, mas com alguma advertência
    NotFound,  // Recurso não encontrado
    Default    // Valor padrão, usado quando nenhum outro status se aplica
}