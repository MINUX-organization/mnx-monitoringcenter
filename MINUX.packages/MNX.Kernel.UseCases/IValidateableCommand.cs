using MediatR;

namespace Kernel.UseCases;

/// <summary>
/// Команда с валидацией
/// </summary>
/// <typeparam name="TResponseValue">Тип ответа</typeparam>
public interface IValidateableCommand<TResponseValue> : IRequest<Result<TResponseValue>>
{ }
