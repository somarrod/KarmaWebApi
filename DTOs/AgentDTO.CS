namespace KarmaWebAPI.DTOs
{
    public class AgentDTO
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public IList<string> Roles { get; set; }
    }
}
