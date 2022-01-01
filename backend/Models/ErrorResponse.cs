namespace Linkly.Models 
{
    public class ErrorResponse
    {
        public int Status { get; set; }
        public string Error { get; set; }

        public ErrorResponse(int status, string error)
        {
            Status = status;
            Error = error;
        }
    }

}
