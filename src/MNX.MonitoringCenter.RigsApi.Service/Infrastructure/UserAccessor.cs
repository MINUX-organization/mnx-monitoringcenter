using System.Security.Claims;

namespace MNX.MonitoringCenter.RigsApi.Service.Infrastructure;

/// <summary>
/// Сервис для доступа к данным пользователя.
/// </summary>
public class UserAccessor
{
    /// <summary>
    /// Авторизированный пользователь.
    /// </summary>
    private readonly ClaimsPrincipal _user;

    public UserAccessor(IHttpContextAccessor httpContextAccessor)
    {
        _user = httpContextAccessor.HttpContext?.User
            ?? throw new ArgumentNullException(nameof(httpContextAccessor));
    }

    /// <summary>
    /// Получить идентификатор пользователя.
    /// </summary>
    /// <returns> Идентификатор пользователя. </returns>
    public Guid GetUserId()
    {
        if (Guid.TryParse(_user.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value, out Guid id))
        {
            return id;
        }

        return Guid.Empty;
    }
}
