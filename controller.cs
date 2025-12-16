using Microsoft.AspNetCore.Mvc;
using dtos;
using Repository;
using Request;

namespace controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class AttemptsController : ControllerBase
    {
        private readonly IAttemptRepository _attemptRepository;
        private readonly ILogger<AttemptsController> _logger;

        public AttemptsController(
            IAttemptRepository attemptRepository,
            ILogger<AttemptsController> logger)
        {
            _attemptRepository = attemptRepository;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AttemptDto>> GetAttempt(int id)
        {
            var attempt = await _attemptRepository.GetByIdAsync(id);
            if (attempt == null)
                return NotFound($"Попытка с ID {id} не найдена");

            return Ok(attempt);
        }

        [HttpPost]
        public async Task<ActionResult<AttemptDto>> StartAttempt([FromBody] StartAttemptRequest request)
        {
            try
            {
                var attempt = await _attemptRepository.CreateAsync(request);
                return CreatedAtAction(nameof(GetAttempt), new { id = attempt.Id }, attempt);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при создании попытки");
                return StatusCode(500, "Внутренняя ошибка сервера");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<AttemptDto>> UpdateAttempt(int id, [FromBody] UpdateAttemptRequest request)
        {
            try
            {
                if (!await _attemptRepository.ExistsAsync(id))
                    return NotFound($"Попытка с ID {id} не найдена");

                var attempt = await _attemptRepository.UpdateAsync(id, request);
                return Ok(attempt);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при обновлении попытки {id}");
                return StatusCode(500, "Внутренняя ошибка сервера");
            }
        }

        [HttpGet("student/{studentId}")]
        public async Task<ActionResult<IEnumerable<AttemptDto>>> GetStudentAttempts(int studentId)
        {
            var attempts = await _attemptRepository.GetByStudentIdAsync(studentId);
            return Ok(attempts);
        }

        [HttpGet("test/{testId}")]
        public async Task<ActionResult<IEnumerable<AttemptDto>>> GetTestAttempts(int testId)
        {
            var attempts = await _attemptRepository.GetByTestIdAsync(testId);
            return Ok(attempts);
        }

        [HttpGet("can-start/{studentId}/{testId}")]
        public async Task<ActionResult<bool>> CanStartAttempt(int studentId, int testId)
        {
            var canStart = await _attemptRepository.CanStudentStartAttemptAsync(studentId, testId);
            return Ok(canStart);
        }
    }
}
