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
    private readonly AllCommands _allCommands;

    public TelegramBot(
        ILogger logger,
        IScrapper scrapper,
        IOptions<TelegramBotOptions> options,
        IHttpClientFactory httpClientFactory,
        AllCommands allCommands)
        : base(logger, scrapper)
    {
        _allCommands = allCommands;
        if (options is { Value: { AccessToken: { } accessToken } })
        {
            Client = new TelegramBotClient(accessToken, httpClientFactory.CreateClient());
        }
        else
        {
            Logger.Warning("No Telegram bot access token provided, cannot create Telegram bot");
            Client = null!;
        }
    }

    public override string Name => "Telegram";
    internal ITelegramBotClient Client { get; }

    protected override async Task ExecuteAsyncImpl(CancellationToken stoppingToken)
    {
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        if (Client == null)
        {
            Logger.Warning("Skip starting Telegram bot, no access token provided");
            return;
        }

        await _allCommands.SetupCommands(stoppingToken);

        Logger.Debug("Start listening");

        await Client.ReceiveAsync(
            updateHandler: (_, u, c) => HandleUpdate(u, c),
            pollingErrorHandler:  (_, e, c) => HandleError(e, c),
            receiverOptions: new()
            {
                AllowedUpdates = new[] { UpdateType.Message },
            },
            cancellationToken: stoppingToken);
    }

    private async Task HandleUpdate(Update update, CancellationToken cancellationToken)
    {
        if (update.Message is not { Text: { } } message)
        {
            Logger.Debug("Update {@Update} skipped", update);
            return;
        }

        Logger.Debug("Update {@Update} received", update);
        Logger.Information("Received message from {ChatId}: {Text}", message.Chat.Id, message.Text);

        await _allCommands.TryHandle(message, cancellationToken);
    }

    private async Task HandleError(Exception exception, CancellationToken cancellationToken)
    {
        Logger.Error(exception, "Error while handling update");
    }

}
