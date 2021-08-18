
namespace Models.Models
{
    public static class RequestModel
    {
        public class AddIn
        {
            public string Text { get; set; }
        }

        public class Get
        {
            public int Id { get; set; }
            
            public string Text { get; set; }
        }
    }
}
