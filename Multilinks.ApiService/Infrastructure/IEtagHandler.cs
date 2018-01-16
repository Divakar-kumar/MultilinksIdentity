namespace Multilinks.ApiService.Infrastructure
{
   public interface IEtagHandlerFeature
   {
      bool NoneMatch(IEtaggable entity);
   }
}
