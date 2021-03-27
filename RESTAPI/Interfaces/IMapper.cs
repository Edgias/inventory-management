namespace Edgias.Inventory.Management.RESTAPI.Interfaces
{
    public interface IMapper<TEntity, TFormApiModel, TApiModel>
    {
        TEntity Map(TFormApiModel request);

        TApiModel Map(TEntity entity);

        void Map(TEntity entity, TFormApiModel request);
    }
}
