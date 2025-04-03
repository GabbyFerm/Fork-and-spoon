using ForkAndSpoon.Application.DTOs.Trivia;

namespace ForkAndSpoon.Application.Interfaces
{
    public interface ITriviaService
    {
        Task<TriviaDto> GetRandomTriviaAsync();
    }
}
