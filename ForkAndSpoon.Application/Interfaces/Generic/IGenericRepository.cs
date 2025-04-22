namespace ForkAndSpoon.Application.Interfaces.Generic
{
    public interface IGenericRepository<T> :
    ICreateRepository<T>,
    IReadRepository<T>,
    IUpdateRepository<T>,
    IDeleteRepository<T>
    where T : class
    { }
}
