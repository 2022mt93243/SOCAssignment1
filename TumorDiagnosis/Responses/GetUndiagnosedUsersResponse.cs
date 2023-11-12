using System.Net;


namespace TumorDiagnoser.Responses
{
    public class GetUndiagnosedUsersResponse : HttpResponseMessage
    {
        public int Count { get; set; }
        public List<string> AppointmentList { get; set; }

        public GetUndiagnosedUsersResponse(List<string> appointmentList)
        {
            StatusCode = HttpStatusCode.OK;
            Count = appointmentList.Count;
            AppointmentList = appointmentList;
        }
    }
}
