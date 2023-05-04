using EDennis.MvcUtils;

namespace EDennis.MvcHits.Services
{
    public class ArtistService : CrudService<HitsContext, Artist>
    {
        public ArtistService(CrudServiceDependencies<HitsContext, Artist> deps): base(deps) { }
    }
}
