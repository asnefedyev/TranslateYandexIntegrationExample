using AutoMapper;
using TranslateExample.Entities;
using TranslateExample.Models.AppModels;
using TranslateExample.Models.DTO;
using TranslateExample.Models.Logic;

namespace TranslateExample.Mapping.Profiles
{
    public class MainProfile : Profile
    {
        public MainProfile()
        {
            CreateMap<WordEntity, WordDTO>()
                .ForMember(dest => dest.Hash, opt => opt.MapFrom(src => src.Hash))
                .ForMember(dest => dest.SourceLang, opt => opt.MapFrom(src => src.SourceLang))
                .ForMember(dest => dest.TargetLang, opt => opt.MapFrom(src => src.TargetLang))
                .ForMember(dest => dest.SourceText, opt => opt.MapFrom(src => src.SourceText))
                .ForMember(dest => dest.TargetText, opt => opt.MapFrom(src => src.TargetText));

            CreateMap<WordDTO, WordEntity>()
                .ForMember(dest => dest.Hash, opt => opt.MapFrom(src => src.Hash))
                .ForMember(dest => dest.SourceLang, opt => opt.MapFrom(src => src.SourceLang))
                .ForMember(dest => dest.TargetLang, opt => opt.MapFrom(src => src.TargetLang))
                .ForMember(dest => dest.SourceText, opt => opt.MapFrom(src => src.SourceText))
                .ForMember(dest => dest.TargetText, opt => opt.MapFrom(src => src.TargetText));

            CreateMap<Top10HashEntity, Top10HashDTO>()
                .ForMember(dest => dest.Hash, opt => opt.MapFrom(src => src.Hash))
                .ForMember(dest => dest.Count, opt => opt.MapFrom(src => src.Count));

            CreateMap<TranslateSavedModel, WordDTO>()
                .ForMember(dest => dest.Hash, opt => opt.MapFrom(src => src.Hash))
                .ForMember(dest => dest.SourceLang, opt => opt.MapFrom(src => src.SourceLang))
                .ForMember(dest => dest.TargetLang, opt => opt.MapFrom(src => src.TargetLang))
                .ForMember(dest => dest.SourceText, opt => opt.MapFrom(src => src.SourceText))
                .ForMember(dest => dest.TargetText, opt => opt.MapFrom(src => src.TargetText));

            CreateMap<WordDTO, TranslateSavedModel>()
                .ForMember(dest => dest.SourceText, opt => opt.MapFrom(src => src.SourceText))
                .ForMember(dest => dest.SourceLang, opt => opt.MapFrom(src => src.SourceLang))
                .ForMember(dest => dest.TargetLang, opt => opt.MapFrom(src => src.TargetLang))
                .ForMember(dest => dest.TargetText, opt => opt.MapFrom(src => src.TargetText))
                .ForMember(dest => dest.Count, opt => opt.Ignore())
                .ForMember(dest => dest.Source, opt => opt.Ignore());

            CreateMap<WordDTO, EventEntity>()
               .ForMember(dest => dest.EventId, opt => opt.Ignore())
               .ForMember(dest => dest.EventTime, opt => opt.MapFrom(_ => DateTime.UtcNow))
               .ForMember(dest => dest.WordHash, opt => opt.MapFrom(src => src.Hash))
               .ForMember(dest => dest.Word, opt => opt.Ignore());

           CreateMap<WordVwEntity, WordDTO>()
              .ForMember(dest => dest.Hash, opt => opt.MapFrom(src => src.Hash))
              .ForMember(dest => dest.SourceLang, opt => opt.MapFrom(src => src.SourceLang))
              .ForMember(dest => dest.TargetLang, opt => opt.MapFrom(src => src.TargetLang))
              .ForMember(dest => dest.SourceText, opt => opt.MapFrom(src => src.SourceText))
              .ForMember(dest => dest.TargetText, opt => opt.MapFrom(src => src.TargetText));

            CreateMap<CacheStateEntity, CacheStateDTO>()
                .ForMember(dest => dest.MemCacheRem, opt => opt.MapFrom(src => src.MemCacheRem))
                .ForMember(dest => dest.DbCacheRem, opt => opt.MapFrom(src => src.DbCacheRem))
                .ForMember(dest => dest.DbBytes, opt => opt.MapFrom(src => src.DbBytes))
                .ForMember(dest => dest.MemBytes, opt => opt.MapFrom(src => src.MemBytes))
                .ForMember(dest => dest.ReqRem, opt => opt.MapFrom(src => src.ReqRem));

            CreateMap<CacheStateDTO, CacheStateEntity>()
                .ForMember(dest => dest.MemCacheRem, opt => opt.MapFrom(src => src.MemCacheRem))
                .ForMember(dest => dest.DbCacheRem, opt => opt.MapFrom(src => src.DbCacheRem))
                .ForMember(dest => dest.DbBytes, opt => opt.MapFrom(src => src.DbBytes))
                .ForMember(dest => dest.MemBytes, opt => opt.MapFrom(src => src.MemBytes))
                .ForMember(dest => dest.ReqRem, opt => opt.MapFrom(src => src.ReqRem));
        }
    }
}