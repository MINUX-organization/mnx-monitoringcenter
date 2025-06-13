namespace MNX.MonitoringCenter.RigsApi.Core.LifeCycle.Power;

/// <summary>
/// Машина состояний жизненного цикла рига.
/// </summary>
public abstract class RigLifeCycleStateMachine
{
    /// <summary>
    /// Текущий статус.
    /// </summary>
    public abstract RigLifeCycleStatus Status { get; }

    /// <summary>
    /// Создать машину состояний.
    /// </summary>
    /// <param name="status"> Текущий статус. </param>
    /// <returns> Машина состояний. </returns>
    /// <exception cref="NotImplementedException"> Нет реализации для переданного статуса. </exception>
    public static RigLifeCycleStateMachine CreateStateMachine(RigLifeCycleStatus status)
    {
        return status switch
        {
            RigLifeCycleStatus.Disable => new RigDisabled(),
            RigLifeCycleStatus.AwaitsEnable => new RigAwaitsEnable(),
            RigLifeCycleStatus.Enable => new RigEnabled(),
            RigLifeCycleStatus.AwaitsDisable => new RigAwaitsDisable(),
            _ => throw new NotImplementedException()
        };
    }

    /// <summary>
    /// Инициировать включение.
    /// </summary>
    /// <returns> Машина состояний. </returns>
    /// <exception cref="LifeCycleException"> Операция недоступна. </exception>
    public virtual RigLifeCycleStateMachine InitiateTurnOn()
        => throw new LifeCycleException("Инициация включения рига недоступна.", Status.ToString());

    /// <summary>
    /// Включить.
    /// </summary>
    /// <returns> Машина состояний. </returns>
    /// <exception cref="LifeCycleException"> Операция недоступна. </exception>
    public virtual RigLifeCycleStateMachine TurnOn()
        => throw new LifeCycleException("Включение рига недоступно.", Status.ToString());

    /// <summary>
    /// Инициировать выключение.
    /// </summary>
    /// <returns> Машина состояний. </returns>
    /// <exception cref="LifeCycleException"> Операция недоступна. </exception>
    public virtual RigLifeCycleStateMachine InitiatePowerOff()
        => throw new LifeCycleException("Инициация выключения рига недоступна.", Status.ToString());

    /// <summary>
    /// Выключить.
    /// </summary>
    /// <returns> Машина состояний. </returns>
    /// <exception cref="LifeCycleException"> Операция недоступна. </exception>
    public virtual RigLifeCycleStateMachine PowerOff()
        => throw new LifeCycleException("Выключение рига недоступно.", Status.ToString());
}

/// <summary>
/// Риг выключен.
/// </summary>
file class RigDisabled : RigLifeCycleStateMachine
{
    /// <inheritdoc/>
    public override RigLifeCycleStatus Status { get => RigLifeCycleStatus.Disable; }

    /// <inheritdoc/>
    public override RigLifeCycleStateMachine InitiateTurnOn() => new RigAwaitsEnable();

    /// <inheritdoc/>
    public override RigLifeCycleStateMachine TurnOn() => new RigEnabled();
}

/// <summary>
/// Риг ожидает включения.
/// </summary>
file class RigAwaitsEnable : RigLifeCycleStateMachine
{
    /// <inheritdoc/>
    public override RigLifeCycleStatus Status { get => RigLifeCycleStatus.AwaitsEnable; }

    /// <inheritdoc/>
    public override RigLifeCycleStateMachine TurnOn() => new RigEnabled();
}

/// <summary>
/// Риг включен.
/// </summary>
file class RigEnabled : RigLifeCycleStateMachine
{
    /// <inheritdoc/>
    public override RigLifeCycleStatus Status { get => RigLifeCycleStatus.Enable; }

    /// <inheritdoc/>
    public override RigLifeCycleStateMachine InitiatePowerOff() => new RigAwaitsDisable();

    /// <inheritdoc/>
    public override RigLifeCycleStateMachine PowerOff() => new RigDisabled();
}

/// <summary>
/// Риг ожидает выключения.
/// </summary>
file class RigAwaitsDisable : RigLifeCycleStateMachine
{
    /// <inheritdoc/>
    public override RigLifeCycleStatus Status { get => RigLifeCycleStatus.AwaitsDisable; }

    /// <inheritdoc/>
    public override RigLifeCycleStateMachine PowerOff() => new RigDisabled();
}
