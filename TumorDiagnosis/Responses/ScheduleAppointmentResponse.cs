using System.Net;


namespace TumorDiagnoser.Responses
{
    public class ScheduleAppointmentResponse : HttpResponseMessage
    {
        public bool AppointmentConfirmed { get; set; }

        public string AppointmentId { get; set; }
        public string Message { get; set; }

        public ScheduleAppointmentResponse(bool appointmentConfirmed, string appointmentId, string message)
        {
            StatusCode = HttpStatusCode.OK;
            AppointmentConfirmed = appointmentConfirmed;
            AppointmentId = appointmentId;
            Message = message;
        }
    }
}
