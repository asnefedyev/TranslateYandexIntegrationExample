namespace Example.Contracts.Models.Clients
{
    public class ClientContractModel
    {
        public string ClientId { get; set; }

        public string ClientURI { get; set; }

        public string ClientEndpoint { get; set; }

        public string ClientAgent { get; set; }

        public string? BearerToken { get; set; }
    }
}