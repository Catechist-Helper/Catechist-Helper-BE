namespace CatechistHelper.Domain.Constants
{
    public static class ApiEndPointConstant
    {
        static ApiEndPointConstant()
        {

        }

        public const string RootEndPoint = "/api";
        public const string ApiVersion = "/v1";
        public const string ApiEndpoint = RootEndPoint + ApiVersion;

        public const string ByIdRoute = "/{id}";
        public static class Authentication
        {
            /// <summary>"api/v1/login"</summary>
            public const string LoginEndPoint = ApiEndpoint + "/login";
            // <summary>"api/v1/google/login"</summary>
            public const string RegisterEndPoint = ApiEndpoint + "/register";
        }
        public static class Account
        {
            /// <summary>"api/v1/accounts"</summary>
            public const string AccountsEndPoint = ApiEndpoint + "/accounts";
            // <summary>"api/v1/accounts/{id}"</summary>
            public const string AccountEndPoint = AccountsEndPoint + ByIdRoute;
        }
        public static class Catechist
        {
            public const string CatechistsEndpoint = ApiEndpoint + "/catechists";
            public const string CatechistEndpoint = CatechistsEndpoint + ByIdRoute;
        }
    }
}
