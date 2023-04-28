namespace ShareClassWebAPI.Interfaces
{
    public interface IEntity<T> where T : class
    {
        void CopyPropertiesWithoutId(T entity);
    }
}
