using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Kernel.UseCases.DI;

/// <summary>
/// Расширения DI для добавления валидации
/// </summary>
public static class ServiceCollectionValidatorExtension
{
    /// <summary>
    /// Добавить валидацию в пайплйн обработки команд.
    /// </summary>
    /// <param name="services"> Коллекция сервисов </param>
    /// <param name="assemblies"> Сборки с командами или валидаторами </param>
    /// <returns> Коллекция сервисов </returns>
    public static IServiceCollection AddValidationPipelines(
        this IServiceCollection services, params Assembly[] assemblies)
    {
        services.AddValidatorsFromAssemblies(assemblies);
        var types = assemblies.SelectMany(assembly => assembly.GetTypes());
        AddValidationPipilines(services, types);
        return services;
    }

    private static void AddValidationPipilines(IServiceCollection services, IEnumerable<Type> commandTypes)
    {
        foreach (var commandType in commandTypes)
        {
            if (commandType.IsClass && !commandType.IsAbstract && !commandType.ContainsGenericParameters)
            {
                var targetInterfece = commandType
                                        .GetInterfaces()
                                        .FirstOrDefault(i => i.IsGenericType &&
                                                             i.GetGenericTypeDefinition() == typeof(IValidateableCommand<>));

                if (targetInterfece != null)
                {
                    var valueType = targetInterfece.GenericTypeArguments[0];
                    AddScoped(services, commandType, valueType);
                }
            }
        }
    }

    private static void AddScoped(IServiceCollection services, Type commandType, Type valueType)
    {
        var pipe = typeof(IPipelineBehavior<,>).MakeGenericType(commandType, typeof(Result<>).MakeGenericType(valueType));
        services.AddScoped(pipe, typeof(ValidationBehavior<,>).MakeGenericType(commandType, valueType));
    }
}
