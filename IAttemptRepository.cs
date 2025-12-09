using dtos;
using Request.Attempt;

namespace Repositories
{
    public interface IAttemptRepository
    {
        Task<AttemptDto?> GetByIdAsync(int id);
        Task<IAttemptDto> CreateAsync(StartAttemptRequest requests);
        Task<AttemptDto> UpdateAsync(int id, UpdateAttemptRequest requests);
        Task<IEnumerable<AttemptDto>> GatByStudentIdAsync(int studentId);
        Task<IEnumerable<AttemptDto>> GetByTestIdAsync(int testId);
        Task<bool> CanStudentStartAttemptAsync(int studentId, int testId);
        Task<bool> ExistsAsync(int Id);
        Task,int> GetAttemptsCountAsync(int studentId, int testId);
    }
}
