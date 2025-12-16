using AutoMapper;
using dtos;
using models;
using Request;

namespace mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Attempt, AttemptDto>()
                .ForMember(dest => dest.StudentName,
                    opt => opt.MapFrom(src => src.Student != null ? src.Student.Name : string.Empty))
                .ForMember(dest => dest.TestTitle,
                    opt => opt.MapFrom(src => src.Test != null ? src.Test.Title : string.Empty))
                .ForMember(dest => dest.Status,
                    opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.Answers,
                    opt => opt.MapFrom(src => src.Answers));

            CreateMap<Answer, AnswerDto>();

            CreateMap<StartAttemptRequest, Attempt>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.StartedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(_ => AttemptStatus.Started))
                .ForMember(dest => dest.Answers, opt => opt.Ignore())
                .ForMember(dest => dest.Student, opt => opt.Ignore())
                .ForMember(dest => dest.Test, opt => opt.Ignore());

            CreateMap<AnswerRequest, Answer>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.AttemptId, opt => opt.Ignore())
                .ForMember(dest => dest.Attempt, opt => opt.Ignore());
        }
    }
}
