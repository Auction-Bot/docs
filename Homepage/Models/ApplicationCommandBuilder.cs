namespace Homepage.Models
{
    public class ApplicationCommandBuilder
    {
        private readonly List<BotCommand> _commands = [];

        public ApplicationCommandBuilder AddItem(string name)
        {
            var componentItem = new BotCommand
            {
                Name = name,
                Link = name.ToLowerInvariant().Replace(" ", ""),
                IsNavGroup = false
            };

            _commands.Add(componentItem);

            return this;
        }

        public ApplicationCommandBuilder AddNavGroup(string name, bool expanded, ApplicationCommandBuilder groupItems)
        {
            var componentItem = new BotCommand
            {
                Name = name,
                NavGroupExpanded = expanded,
                GroupCommands = groupItems.Build(),
                IsNavGroup = true
            };

            _commands.Add(componentItem);

            return this;
        }

        internal List<BotCommand> Build() => _commands;
    }
}
