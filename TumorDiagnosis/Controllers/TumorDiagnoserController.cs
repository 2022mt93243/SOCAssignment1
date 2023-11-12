using System.Collections.Specialized;
using System.Net;
using Microsoft.AspNetCore.Mvc;

using TumorDiagnoser;
using TumorDiagnoser.Model;
using TumorDiagnoser.Responses;


namespace TumorDiagnosis.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TumorDiagnoserController : ControllerBase
    {
        private readonly ILogger<TumorDiagnoserController> _logger;

        public TumorDiagnoserController(ILogger<TumorDiagnoserController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        [Route("api/authenticate")]
        public HttpResponseMessage Authenticate([FromQuery] string username, string password)
        {
            // Validate user credentials 
            if (IsValidUser(username, password))
            {
                return new AuthenticationResponse(HttpStatusCode.OK, string.Empty, true);
            }

            // Unauthorized access
            return new AuthenticationResponse(HttpStatusCode.Unauthorized, "Invalid Credentials");
        }

        private bool IsValidUser(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return false;
            string validPassword = Database.GetPassword(username);
            if (password != validPassword)
                return false;
            return true;
        }

        [HttpPost]
        [Route("api/scheduleDiagnosticVisit")]
        public HttpResponseMessage ScheduleDiagnosticVisit([FromQuery] string username, string dateTime)
        {
            if (IsSlotAvailable(dateTime))
            {
                Database.BookSlot(username, dateTime);
                return new ScheduleAppointmentResponse(true, Guid.NewGuid().ToString(), "Appointment confirmed.");
            }

            return new ScheduleAppointmentResponse(false, Guid.Empty.ToString(), "Please select different slot.");
        }

        private bool IsSlotAvailable(string dateTime)
        {
            return Database.IsSlotAvailable(dateTime);
        }

        [HttpPost]
        [Route("api/uploadDiagnosticImages")]
        public HttpResponseMessage UploadDiagnosticImages([FromQuery]string username, string appointmentId,  string imageUrl)
        {
            if (UploadImagesToDatabase(username, appointmentId, imageUrl))
            {
                return new UploadDiagnosticImagesResponse(true, "Images uploaded successfully.");
            }

            return new UploadDiagnosticImagesResponse(false, "Images upload failed. Images inaccessible.");
        }

        private bool UploadImagesToDatabase(string username, string appointmentId, string imageUrl)
        {
            return Database.UploadImages(username, appointmentId, imageUrl);
        }

        [HttpGet]
        [Route("api/getUndiagnosedAppointments")]
        public HttpResponseMessage GetUndiagnosedAppointments()
        {
            List<string> undiagnosedAppointments = GetUndiagnosedAppointmentsFromDatabase();
            return new GetUndiagnosedUsersResponse(undiagnosedAppointments);
        }

        private List<string> GetUndiagnosedAppointmentsFromDatabase()
        {
            return Database.GetUndiagnosedAppointments();
        }

        [HttpPost]
        [Route("api/updateDiagnosis")]
        public HttpResponseMessage UpdateDiagnosis([FromQuery]string username, string appointmentId, string tumorDetail, string diagnosis)
        {
            if (UpdateDiagnosisInDatabase(username, appointmentId, tumorDetail, diagnosis))
            {
                return new UpdateDiagnosisResponse(true, "Diagnosis uploaded successfully.");
            }

            return new UploadDiagnosticImagesResponse(false, "Diagnosis update failed. Try again.");
        }

        private bool UpdateDiagnosisInDatabase(string username, string appointmentId, string tumorDetail, string diagnosis)
        {
            return Database.UpdateDiagnosis(username, appointmentId, tumorDetail, diagnosis);
        }

        [HttpGet]
        [Route("api/getDiagnosis")]
        public HttpResponseMessage GetDiagnosis(string username)
        {
            List<string> results = GetLatestDiagnosisFromDatabase(username);
            return new GetDiagnosisResponse(results[0], results[1], results[2]);
        }

        private List<string> GetLatestDiagnosisFromDatabase(string username)
        {
            return Database.GetLatestDiagnosis(username);
        }
    }
}