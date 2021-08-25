
using System;

namespace DAL.Entities
{
    public class Request
    {
        public int Id { get; set; }

        public string Author { get; set; }

        public string Text { get; set; }

        public DateTime CreatedUtc { get; set; }

        public bool IsUsed { get; set; }
    }
}
