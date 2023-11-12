using System.Net;


namespace TumorDiagnoser.Responses
{
    public class AuthenticationResponse : HttpResponseMessage
    {
        public bool Authenticated { get; set; }
        public string ErrorMessage { get; set; }

        public AuthenticationResponse(HttpStatusCode statusCode, string errorMessage, bool authenticated = false)
        {
            StatusCode = statusCode;
            ErrorMessage = errorMessage;
            Authenticated = authenticated;
        }
    }
}
