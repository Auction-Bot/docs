namespace Homepage.Models
{
    public class BotCommand
    {
        public string? Name { get; init; }
        public string? Link { get; init; }
        public bool IsNavGroup { get; init; }
        public bool NavGroupExpanded { get; set; }

        public List<BotCommand> GroupCommands { get; init; } = [];

        public string? CommandName { get; init; }
    }
}
