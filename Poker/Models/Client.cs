namespace Poker.Models
{
    public class Client
    {
        public string ConnectionId { get; set; }

        public string Vote { get; set; }

        public string UserName { get; set; }

        public bool IsReady { get; set; }
    }
}
