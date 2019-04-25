namespace AuthHeader.Models
{
    public class FailiureResponseModel
    {
        public bool Result { get; set; }

        public string ResultDetails { get; set; }

        public string Uri { get; set; }

        public string Timestamp { get; set; }

        public Error Error { get; set; }
    }

    public class Error
    {
        public int Code { get; set; }
        public string Description { get; set; }
        public string Resolve { get; set; }
    }
}