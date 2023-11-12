namespace TumorDiagnoser.Responses
{
    public class UploadDiagnosticImagesResponse : HttpResponseMessage
    {
        public bool UploadSuccessful { get; set; }
        public string ErrorMessage { get; set; }

        public UploadDiagnosticImagesResponse(bool uploadSuccessful, string errorMessage)
        {
            UploadSuccessful = uploadSuccessful;
            ErrorMessage = errorMessage;
        }
    }
}
