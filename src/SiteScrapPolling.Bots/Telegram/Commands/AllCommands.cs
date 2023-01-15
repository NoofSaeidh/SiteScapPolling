using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace SiteScrapPolling.Bots.Telegram.Commands
{
    public class AllCommands
    {
        private readonly Lazy<List<BaseCommand>> _commands;
        private readonly Lazy<TelegramBot> _telegramBot;
        private readonly ILogger _logger;

        public AllCommands(IServiceProvider serviceProvider, ILogger logger)
        {
            _logger = logger;
            // must be lazy to avoid circular dependency
            _commands = new(() => serviceProvider.GetServices<BaseCommand>().ToList());
            _telegramBot = new(serviceProvider.GetRequiredService<TelegramBot>);
        }

        public HelpCommand Help => _commands.Value.OfType<HelpCommand>().First();
        public SettingsCommand Settings => _commands.Value.OfType<SettingsCommand>().First();
        public StartCommand Start => _commands.Value.OfType<StartCommand>().First();
        public StopCommand Stop => _commands.Value.OfType<StopCommand>().First();


        public async Task<bool> TryHandle(Message message, CancellationToken cancellationToken)
        {
            foreach (var command in _commands.Value)
            {
                if (await command.TryHandleAsync(message, cancellationToken))
                    return true;
            }

            return false;
        }

        public async Task SetupCommands(CancellationToken cancellationToken)
        {
            _logger.Information("Setup commands {@Commands}", _commands.Value.Select(c => c.Command.Command));
            await _telegramBot.Value.Client.SetMyCommandsAsync(
                _commands.Value.Select(c => new BotCommand
                {
                    Command     = c.Command.Name,
                    Description = c.Command.Description
                }),
                cancellationToken: cancellationToken);
        }
    }
}
