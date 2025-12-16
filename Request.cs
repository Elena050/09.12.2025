using System.ComponentModel.DataAnnotations;
using models;

namespace Request
{
    public class StartAttemptRequest
    {
        [Required(ErrorMessage = "StudentId обязателен")]
        [Range(1, int.MaxValue, ErrorMessage = "StudentId должен быть положительным числом")]
        public int StudentId { get; set; }

        [Required(ErrorMessage = "TestId обязателен")]
        [Range(1, int.MaxValue, ErrorMessage = "TestId должен быть положительным числом")]
        public int TestId { get; set; }
    }

    public class UpdateAttemptRequest
    {
        public AttemptStatus? Status { get; set; }

        [ValidateComplexType]
        public List<AnswerRequest>? Answers { get; set; }
    }

    public class AnswerRequest
    {
        [Required]
        public int QuestionId { get; set; }

        [Required]
        [MaxLength(1000)]
        public string AnswerText { get; set; } = string.Empty;
    }
}
