using AutoMapper;
using Example.Services.Common.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Data;
using TranslateExample.Entities;
using TranslateExample.Models.AppModels;
using TranslateExample.Services.Data.Contracts;
using TranslatorExample.Services.Common.Contracts;
using TranslatorExample.Services.Common.DbConnections;
using Dapper;
using TranslateExample.Models.DTO;

namespace TranslateExample.Services.Data.Implements
{
    public class CacheDataService : ICacheDataService
    {
        private readonly IMapper _mapper;
        private readonly IDataEngineService _dataService;
        private readonly IDatabaseConnection _connService;

        public CacheDataService(IMapper mapper, IDataEngineServiceBuilder builder, IDatabaseConnection connectionService) 
        {
            _dataService = builder.GetDalService<DefaultConnection>();
            _mapper = mapper;
            _connService = connectionService;
        }

        public void WordAdd(WordDTO word)
        {
            Guid guid = Guid.NewGuid();
            Action<DbContext> processing = (context) =>
            {
                var wordSet = context.Set<WordEntity>();
                var eventSet = context.Set<EventEntity>();

                if (wordSet != null)
                {
                    if (wordSet.Any(it => it.Hash == word.Hash))
                    {
                        eventSet.Add(_mapper.Map<EventEntity>(word));
                    }
                    else wordSet.Add(_mapper.Map<WordEntity>(word));
                }
                context.SaveChanges();
            };
            _dataService.ChangeEntityList(processing);
        }

        public WordDTO? GetWord(string hash)
        {
            Func<DbContext, List<WordEntity>> processing = (context) =>
            {
                var wordSet = context.Set<WordEntity>();
                if (wordSet is not null)
                {
                    return wordSet.Where(it => it.Hash == hash).ToList();
                }
                throw new Exception("Не удалось получить set!");
            };
            var entityResult = _dataService.GetEntityList(processing).FirstOrDefault();
            if (entityResult is not null)
            {
                return _mapper.Map<WordDTO>(entityResult);
            }
            else
            {
                return null;
            }
        }

        public List<Top10HashDTO> GetTop10Words()
        {
            Func<DbContext, List<Top10HashEntity>> processing = (context) =>
            {
                var wordSet = context.Set<Top10HashEntity>();
                if (wordSet is not null)
                {
                    return wordSet.ToList();
                }
                throw new Exception("Не удалось получить set!");
            };
            var result = _dataService.GetEntityList(processing);
            return _mapper.Map<List<Top10HashDTO>>(result);
        }

        public List<WordDTO> GetTop10WordsWithDetail()
        {
            Func<DbContext, List<WordDTO>> processing = (context) =>
            {
                var wordVwSet = context.Set<WordVwEntity>();
                if (wordVwSet is not null)
                {
                    return _mapper.Map<List<WordDTO>>(wordVwSet.ToList());
                }
                throw new Exception("Не удалось получить set!");
            };
            return _dataService.GetEntityList(processing);
        }

        public void RefreshTop10Items()
        {
            using (var conn = _connService.GetDbConnection())
            {
                conn.Execute("refresh materialized view concurrently public.top10_hash");
            }
        }

        /// <summary>
        /// Инкрементальный метод
        /// </summary>
        /// <param name="memCacheRem">Количество записей КЭШа-памяти</param>
        /// <param name="dbCacheRem">Количество записей КЭШа БД</param>
        /// <param name="dbBytes">Размер КЭШа БД (без ключей)</param>
        /// <param name="memBytes">Размер КЭШа памяти"</param>
        /// <param name="reqRem">Количество запросов к стороннему сервису</param>
        public void DoActualCacheState(int? memCacheRem = null, int? dbCacheRem = null, int? dbBytes = null, int? memBytes = null, int? reqRem = null)
        {
            Action<DbContext> processing = (context) =>
            {
                var cacheSet = context.Set<CacheStateEntity>();
                if (cacheSet != null)
                {
                    if (!cacheSet.Any())
                    {
                        cacheSet.Add(new CacheStateEntity
                        {
                            DbBytes = dbBytes ?? 0,
                            DbCacheRem = dbCacheRem ?? 0,
                            MemBytes = memBytes ?? 0,
                            MemCacheRem = memCacheRem ?? 0,
                            ReqRem = reqRem ?? 0
                        });
                    } else
                    {
                        var stat = cacheSet.FirstOrDefault();
                        if (stat != null)
                        {
                            if (memCacheRem is not null) stat.MemCacheRem += memCacheRem  ?? 0;
                            if (dbCacheRem is not null) stat.DbCacheRem += dbCacheRem ?? 0;
                            if (dbBytes is not null) stat.DbBytes += dbBytes ?? 0;
                            if (memBytes is not null) stat.MemBytes += memBytes ?? 0;
                            if (reqRem is not null) stat.ReqRem += reqRem ?? 0;
                        }
                    }
                }
                context.SaveChanges();
            };
            _dataService.ChangeEntityList(processing);
        }

        public CacheStateDTO GetCacheState()
        {
            Func<DbContext, List<CacheStateDTO>> processing = (context) =>
            {
                var state = context.Set<CacheStateEntity>();
                if (state is not null)
                {
                    var lst = state.ToList();
                    if (lst.Count > 0)
                    {
                        return _mapper.Map<List<CacheStateDTO>>(lst);
                    }
                    else
                    {
                        return new List<CacheStateDTO>();
                    }
                }
                throw new Exception("Не удалось получить set!");
            };
            return _dataService.GetEntityList(processing).FirstOrDefault() ?? new CacheStateDTO();
        }
    }
}