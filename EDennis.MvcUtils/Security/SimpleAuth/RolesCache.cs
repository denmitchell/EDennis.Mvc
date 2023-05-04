using System.Collections.Concurrent;

namespace EDennis.MvcUtils
{
    public class RolesCache: ConcurrentDictionary<string, (DateTime ExpiresAt, string Role)>
    {
    }
}
