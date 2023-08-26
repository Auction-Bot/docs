namespace Homepage.Models
{
    public class ApplicationCommandBilder
    {
        private List<BotCommand> _commands = new();

        public ApplicationCommandBilder AddItem(string name)
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

        public ApplicationCommandBilder AddNavGroup(string name, bool expanded, ApplicationCommandBilder groupItems)
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
