using Microsoft.Extensions.Options;
using SiteScrapPolling.Bots.Common;
using SiteScrapPolling.Scrapping;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace SiteScrapPolling.Bots.Telegram
{
    public class TelegramBot : BotBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly TelegramBotOptions _options;

        public TelegramBot(
            ILogger logger,
            IScrapper scrapper,
            IOptions<TelegramBotOptions> options,
            IHttpClientFactory httpClientFactory)
            : base(logger, scrapper)
        {
            _options           = options.Value;
            _httpClientFactory = httpClientFactory;
        }

        public override string Name => "Telegram";

        protected override async Task ExecuteAsyncImpl(CancellationToken stoppingToken)
        {
            if (_options.AccessToken == null)
            {
                Logger.Error("No access token provided");
                return;
            }

            var client = new TelegramBotClient(_options.AccessToken, _httpClientFactory.CreateClient());

            await SetupCommands(client, stoppingToken);
            
            Logger.Debug("Start listening");

            await client.ReceiveAsync(
                updateHandler: HandleUpdate,
                pollingErrorHandler: HandleError,
                receiverOptions: new()
                {
                    AllowedUpdates = new[] { UpdateType.Message },
                },
                cancellationToken: stoppingToken);
        }

        private async Task HandleUpdate(ITelegramBotClient client, Update update,
                                        CancellationToken cancellationToken)
        {
            if (update.Message is not { Text: { } text } message)
            {
                Logger.Debug("Update {@Update} skipped", update);
                return;
            }

            Logger.Debug("Update {@Update} received", update);
            Logger.Information("Received message from {ChatId}: {Text}", message.Chat.Id, message.Text);

            await Task.Delay(1000, cancellationToken);

            await client.SendTextMessageAsync(message.Chat, "Hi!", cancellationToken: cancellationToken);
        }

        private async Task HandleError(ITelegramBotClient client, Exception exception,
                                       CancellationToken cancellationToken)
        {
            Logger.Error(exception, "Error while handling update");
        }

        private async Task SetupCommands(ITelegramBotClient client, CancellationToken cancellationToken)
        {
            Logger.Information("Setup commands {@Commands}", TelegramBotCommand.All);
            await client.SetMyCommandsAsync(
                TelegramBotCommand.All.Select(c => new BotCommand
                {
                    Command     = c.Command,
                    Description = c.Description
                }),
                cancellationToken: cancellationToken);
        }
    }
}
