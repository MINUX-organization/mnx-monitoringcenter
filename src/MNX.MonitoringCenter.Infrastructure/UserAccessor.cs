using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace MNX.MonitoringCenter.Infrastructure;

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
        _user = httpContextAccessor.HttpContext?.User ?? throw new ArgumentNullException(nameof(httpContextAccessor));
    }

    /// <summary>
    /// Получить идентификатор пользователя.
    /// </summary>
    /// <returns> Идентификатор пользователя. </returns>
    public long GetUserId()
    {
        if (long.TryParse(_user.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value, out long id))
        {
            return id;
        }

        return -1;
    }
}
