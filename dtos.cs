namespace dtos
{
    public class AttemptDto
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public string StudentName { get; set; } = string .Empty;
        public int TestId { get; set; }
        public string TestTitle { get; set; } = string.Empty;
        public DateTime StartedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public int Score { get; set; }
        public int MawScore { get; set; }
        public string Status { get; set; } = string.Empty;
        public List<AnswerDto> Answers { get; set; } = new List<AnswerDto>();
        public TimeSpan? TimeSpan => CompletedAt.HasValue ? CompletedAt - StartedAt : null;
    }

    public class AnswerDto
    {
        public int QuestionId { get; set; }
        public string AnswerText { get; set; } = string.Empty;
        public bool IsCorrect {  get; set; }
    }
}
