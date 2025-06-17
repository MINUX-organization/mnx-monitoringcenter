namespace MNX.MonitoringCenter.RigsApi.Core.LifeCycle.Mining;

/// <summary>
/// Машина состояний жизненного цикла майнинга.
/// </summary>
public abstract class MiningLifeCycleStateMachine
{
    /// <summary>
    /// Текущий статус.
    /// </summary>
    public abstract MiningLifeCycleStatus Status { get; }

    /// <summary>
    /// Создать машину состояний.
    /// </summary>
    /// <param name="status"> Текущий статус. </param>
    /// <returns> Машина состояний. </returns>
    /// <exception cref="NotImplementedException"> Нет реализации для переданного статуса. </exception>
    public static MiningLifeCycleStateMachine CreateStateMachine(MiningLifeCycleStatus status)
    {
        return status switch
        {
            MiningLifeCycleStatus.Disable => new MiningDisabled(),
            MiningLifeCycleStatus.AwaitsEnable => new MiningAwaitsEnable(),
            MiningLifeCycleStatus.Enable => new MiningEnabled(),
            MiningLifeCycleStatus.AwaitsDisable => new MiningAwaitsDisable(),
            _ => throw new NotImplementedException()
        };
    }

    /// <summary>
    /// Инициировать запуск.
    /// </summary>
    /// <returns> Машина состояний. </returns>
    /// <exception cref="LifeCycleException"> Операция недоступна. </exception>
    public virtual MiningLifeCycleStateMachine InitiateStart()
        => throw new LifeCycleException("Инициация запуска майнинга недоступна.", Status.ToString());

    /// <summary>
    /// Прервать запуск майнинга.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="LifeCycleException"> Операция недоступна. </exception>
    public virtual MiningLifeCycleStateMachine TerminateStart()
        => throw new LifeCycleException("Прерывание запуска майнинга недоступно.", Status.ToString());

    /// <summary>
    /// Запустить.
    /// </summary>
    /// <returns> Машина состояний. </returns>
    /// <exception cref="LifeCycleException"> Операция недоступна. </exception>
    public virtual MiningLifeCycleStateMachine Start()
        => throw new LifeCycleException("Запуск майнинга недоступен.", Status.ToString());

    /// <summary>
    /// Инициировать остановку.
    /// </summary>
    /// <returns> Машина состояний. </returns>
    /// <exception cref="LifeCycleException"> Операция недоступна. </exception>
    public virtual MiningLifeCycleStateMachine InitiateStop()
        => throw new LifeCycleException("Инициация остановки майнинга недоступна.", Status.ToString());

    /// <summary>
    /// Прервать остановку майнинга.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="LifeCycleException"> Операция недоступна. </exception>
    public virtual MiningLifeCycleStateMachine TerminateStop()
        => throw new LifeCycleException("Прерывание остановки майнинга недоступно.", Status.ToString());

    /// <summary>
    /// Остановить.
    /// </summary>
    /// <returns> Машина состояний. </returns>
    /// <exception cref="LifeCycleException"> Операция недоступна. </exception>
    public virtual MiningLifeCycleStateMachine Stop()
        => throw new LifeCycleException("Остановка майнинга недоступна.", Status.ToString());

    /// <summary>
    /// Обеспечить остановку.
    /// </summary>
    /// <returns> Машина состояний. </returns>
    public abstract MiningLifeCycleStateMachine EnsureStopping();
}

/// <summary>
/// Майнинг выключен.
/// </summary>
file class MiningDisabled : MiningLifeCycleStateMachine
{
    /// <inheritdoc/>
    public override MiningLifeCycleStatus Status { get => MiningLifeCycleStatus.Disable; }

    /// <inheritdoc/>
    public override MiningLifeCycleStateMachine InitiateStart() => new MiningAwaitsEnable();

    /// <inheritdoc/>
    public override MiningLifeCycleStateMachine Start() => new MiningEnabled();

    /// <inheritdoc/>
    public override MiningLifeCycleStateMachine EnsureStopping() => this;
}

/// <summary>
/// Запуск майнинга инициирован.
/// </summary>
file class MiningAwaitsEnable : MiningLifeCycleStateMachine
{
    /// <inheritdoc/>
    public override MiningLifeCycleStatus Status { get => MiningLifeCycleStatus.AwaitsEnable; }

    /// <inheritdoc/>
    public override MiningLifeCycleStateMachine Start() => new MiningEnabled();

    /// <inheritdoc/>
    public override MiningLifeCycleStateMachine TerminateStart() => new MiningDisabled();

    /// <inheritdoc/>
    public override MiningLifeCycleStateMachine EnsureStopping() => new MiningDisabled();
}

/// <summary>
/// Майнинг запущен.
/// </summary>
file class MiningEnabled : MiningLifeCycleStateMachine
{
    /// <inheritdoc/>
    public override MiningLifeCycleStatus Status { get => MiningLifeCycleStatus.Enable; }

    /// <inheritdoc/>
    public override MiningLifeCycleStateMachine InitiateStop() => new MiningAwaitsDisable();

    /// <inheritdoc/>
    public override MiningLifeCycleStateMachine Stop() => new MiningDisabled();

    /// <inheritdoc/>
    public override MiningLifeCycleStateMachine EnsureStopping() => new MiningDisabled();
}

/// <summary>
/// Остановка майнинга инициирована.
/// </summary>
file class MiningAwaitsDisable : MiningLifeCycleStateMachine
{
    /// <inheritdoc/>
    public override MiningLifeCycleStatus Status { get => MiningLifeCycleStatus.AwaitsDisable; }

    /// <inheritdoc/>
    public override MiningLifeCycleStateMachine Stop() => new MiningDisabled();

    public override MiningLifeCycleStateMachine TerminateStop() => new MiningEnabled();

    /// <inheritdoc/>
    public override MiningLifeCycleStateMachine EnsureStopping() => new MiningDisabled();
}
