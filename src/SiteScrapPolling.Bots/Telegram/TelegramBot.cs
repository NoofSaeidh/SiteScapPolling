using Microsoft.Extensions.Options;
using SiteScrapPolling.Bots.Common;
using SiteScrapPolling.Bots.Telegram.Commands;
using SiteScrapPolling.Scrapping;
using System.Net.Http;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace SiteScrapPolling.Bots.Telegram;

public class TelegramBot : BotBase
{
    private readonly List<CommandHandlerBase> _handlers;
    private readonly ITelegramBotClient _client;


    public TelegramBot(
        ILogger logger,
        IScrapper scrapper,
        ITelegramBotClient telegramBotClient,
        IEnumerable<CommandHandlerBase> handlers)
        : base(logger, scrapper)
    {
        _client = telegramBotClient;
        _handlers = handlers.ToList();
    }

    public override string Name => "Telegram";

    protected override async Task ExecuteAsyncImpl(CancellationToken stoppingToken)
    {
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        if (_client == null)
        {
            Logger.Warning("Skip starting Telegram bot, no access token provided");
            return;
        }

        await SetupCommands(stoppingToken);

        Logger.Debug("Start listening");

        await _client.ReceiveAsync(
            updateHandler: (_, u, c) => HandleUpdate(u, c),
            pollingErrorHandler:  (_, e, c) => HandleError(e, c),
            receiverOptions: new()
            {
                AllowedUpdates = Array.Empty<UpdateType>(),
            },
            cancellationToken: stoppingToken);
    }

    private async Task HandleUpdate(Update update, CancellationToken cancellationToken)
    {
        Logger.Debug("Update {@Update} received", update);

        foreach (var handler in _handlers)
        {
            if (handler.CanHandle(update))
            {
                await handler.TryHandleAsync(update, cancellationToken);
                break;
            }
        }
    }

    private async Task HandleError(Exception exception, CancellationToken cancellationToken)
    {
        Logger.Error(exception, "Error while handling update");
    }

    private async Task SetupCommands(CancellationToken cancellationToken)
    {
        // ReSharper disable once PossibleMultipleEnumeration
        Logger.Information("Setup commands {@Commands}", _handlers.Select(t => t.Command));
        await _client.SetMyCommandsAsync(
            // ReSharper disable once PossibleMultipleEnumeration
            _handlers.Select(t => new BotCommand { Command = t.Command.FullCommand, Description = t.Command.Description }),
            cancellationToken: cancellationToken);
    }

}
