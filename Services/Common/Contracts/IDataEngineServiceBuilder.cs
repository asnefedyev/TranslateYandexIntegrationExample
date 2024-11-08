using Example.Services.Common.Contracts;

namespace TranslatorExample.Services.Common.Contracts
{
    public interface IDataEngineServiceBuilder
    {
        public IDataEngineService GetDalService<IConfigDbConnection>();
    }
}