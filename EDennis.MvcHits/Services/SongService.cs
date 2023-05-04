using EDennis.MvcUtils;

namespace EDennis.MvcHits.Services
{
    public class SongService : CrudService<HitsContext, Song>
    {
        public SongService(CrudServiceDependencies<HitsContext, Song> deps) : base(deps) { }
    }
}
