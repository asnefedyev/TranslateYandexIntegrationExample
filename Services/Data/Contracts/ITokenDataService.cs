namespace TranslateExample.Services.Data.Contracts
{
    public interface ITokenDataService
    {
        public void AddTokenToDb(string token);
        public string? GetTokenFromDb();
    }
}