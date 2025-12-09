namespace models
{
    public class Attempt
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int TestId { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public int Score { get; set; }
        public int MaxScore { get; set; }
        public AttemptStatus Status { get; set; } = AttemptStatus.Started;
        public virtual Student? Student { get; set; }
        public virtual Test? Test { get; set; }
        public virtual ICollection<Answer> Answer { get; set; } = new List<Answer>();

    }

    public enum AttemptStatus
    {
        Started, InProgress, Completed, Expired, Failed
    }

    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public virtual ICollection<Attempt> Attempts { get; set; } = new List<Attempt>();
        public virtual ICollection<Test> AvailableTests { get; set; } = new List<Test>();
    }

    public class Test
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsPublished { get; set; } = false;
        public DateTime? AvailableFrom { get; set; }
        public DateTime? AvailableTo { get; set; }
        public int MaxAttempts { get; set; }
        public int TimeLimitMinutes { get; set; }
        public int MawScore { get; set; }

        public virtual ICollection<Attempt> Attempts { get; set; } = new List<Attempt>();
        public virtual ICollection<Student> AllowedStudent { get; set; } = new List<Student>();
    }
    
    public class Answer
    {
        public int Id { get; set; }
        public int AttemptId { get; set; }
        public int questionId { get; set; }
        public string AnswerText { get; set; } = string.Empty;
        public bool IsCorrect { get; set; } = false;
        public virtual Attempt? Attempt { get; set; }
    }
}
