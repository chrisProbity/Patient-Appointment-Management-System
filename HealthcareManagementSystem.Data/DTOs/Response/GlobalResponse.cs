namespace HealthcareManagementSystem.Data.DTOs.Response
{
    public class GlobalResponse<T>
    {
        public bool Status { get; set; }
        public int StatusCode { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }

    }
}
