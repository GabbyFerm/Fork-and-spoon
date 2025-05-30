namespace ForkAndSpoon.Application.Interfaces.Generic
{
    // Combines Create, Read, Update, and Delete into one generic repository interface
    public interface IGenericRepository<T> :
        ICreateRepository<T>,
        IReadRepository<T>,
        IUpdateRepository<T>,
        IDeleteRepository<T>,
        IExistsRepository<T>
        where T : class
    { }
}
