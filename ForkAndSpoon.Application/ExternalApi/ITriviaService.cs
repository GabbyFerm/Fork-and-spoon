namespace ForkAndSpoon.Application.ExternalApi
{
    public interface ITriviaService
    {
        Task<TriviaDto> GetRandomTriviaAsync();
    }
}
