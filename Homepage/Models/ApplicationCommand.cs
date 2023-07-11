namespace Homepage.Models
{
    public class ApplicationCommand
    {
        private readonly List<BotCommand> _slashCommands = new();

        public ApplicationCommand AddItem(string name)
        {
            var componentItem = new BotCommand
            {
                Name = name,
                Link = name.ToLowerInvariant().Replace(" ", ""),
                IsNavGroup = false
            };

            _slashCommands.Add(componentItem);
            
            return this;
        }

        public ApplicationCommand AddNavGroup(string name, bool expanded, ApplicationCommand groupItems)
        {
            var componentItem = new BotCommand
            {
                Name = name,
                NavGroupExpanded = expanded,
                GroupCommands = groupItems.GetCommandsSortedByName(),
                IsNavGroup = true
            };

            _slashCommands.Add(componentItem);
            
            return this;
        }

        internal List<BotCommand> Components => _slashCommands;

        internal List<BotCommand> GetCommandsSortedByName() => _slashCommands.OrderBy(e => e.Name).ToList();
    }
}
