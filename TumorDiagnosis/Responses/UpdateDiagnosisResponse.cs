using System.Net;


namespace TumorDiagnoser.Responses
{
    public class UpdateDiagnosisResponse : HttpResponseMessage
    {
        public bool DiagnosisUpdated { get; set; }
        public string Message { get; set; }

        public UpdateDiagnosisResponse(bool diagnosisUpdated, string message)
        {
            StatusCode = HttpStatusCode.OK;
            DiagnosisUpdated = diagnosisUpdated;
            Message = message;
        }
    }
}
