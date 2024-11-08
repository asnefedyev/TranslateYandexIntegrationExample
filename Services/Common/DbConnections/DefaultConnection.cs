using TranslatorExample.Services.Common.Contracts;

namespace TranslatorExample.Services.Common.DbConnections
{
    public class DefaultConnection : IAgentDbConnection
    {
        public override string ToString()
        {
            return "DefaultConnection";
        }
    }
}