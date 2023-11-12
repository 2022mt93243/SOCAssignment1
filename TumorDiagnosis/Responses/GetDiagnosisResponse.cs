namespace TumorDiagnoser.Responses
{
    public class GetDiagnosisResponse : HttpResponseMessage
    {
        public string AppointmentId { get; set; }
        public string TumorDetail { get; set; }
        public string Diagnosis { get; set; }

        public GetDiagnosisResponse(string appointmentId, string tumorDetail, string diagnosis)
        {
            AppointmentId = appointmentId;
            TumorDetail = tumorDetail;
            Diagnosis = diagnosis;
        }
    }
}
