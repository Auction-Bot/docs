namespace Homepage.Models
{
    public class BotCommand
    {
        public string? Name { get; set; }
        public string? Link { get; set; }
        public bool IsNavGroup { get; set; }
        public bool NavGroupExpanded { get; set; }

        public List<BotCommand> GroupCommands { get; set; } = new();

        public string? CommandName { get; private set; }
    }
}
