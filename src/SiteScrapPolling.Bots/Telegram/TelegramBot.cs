using Microsoft.Extensions.Options;
using SiteScrapPolling.Bots.Common;
using SiteScrapPolling.Bots.Telegram.Commands;
using SiteScrapPolling.Common.Extensions;
using SiteScrapPolling.Database.Repositories;
using SiteScrapPolling.Scrapping;
using System.Net.Http;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace SiteScrapPolling.Bots.Telegram;

public class TelegramBot : BotBase
{
    private readonly ITelegramBotClient _client;
    private readonly IUserRepository _userRepository;
    private readonly IReadOnlyCollection<CommandHandlerBase> _commandHandlers;
    private readonly IReadOnlyDictionary<string, HandlerBase> _allHandlers;


    public TelegramBot(
        ILogger logger,
        IScrapper scrapper,
        ITelegramBotClient telegramBotClient,
        IUserRepository userRepository,
        IReadOnlyCollection<CommandHandlerBase> handlers,
        IReadOnlyCollection<HandlerBase> allHandlers)
        : base(logger, scrapper)
    {
        _client          = telegramBotClient;
        _userRepository  = userRepository;
        _commandHandlers = handlers;
        _allHandlers     = allHandlers.ToDictionary(h => h.ToString(), h => h);
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

        var user = await _userRepository.GetOrAddAsync(update.GetUserId(), cancellationToken);
        if (user.LastHandler != null)
        {
            var found = _allHandlers.TryGetValue(user.LastHandler, out var lastHandler);
            Logger.Information("Last handler {LastHandler} for user {UserId} {FoundState}", lastHandler, user.Id, found.ToFoundState());
            if (found && lastHandler is IHasNextHandler hasNext)
            {
                Logger.Debug("Handler {LastHandler} for user {UserId} Has next handlers {NextHandler}", user.LastHandler, user.Id, hasNext.NextHandler);
                
                //var found = _allHandlers.TryGetValue(user.LastHandler, out var lastHandler);
                //Logger.Information("Last handler {LastHandler} for user {UserId} {FoundState}", lastHandler, user.Id, found.ToFoundState());

                if (_allHandlers.)
                
                await _allHandlers[hasNext.NextHandler].TryHandleAsync(update, cancellationToken);
            }
        }

        foreach (var handler in _allHandlers.Values)
        {
            if (handler.CanHandle(update))
            {
                await handler.TryHandleAsync(update, cancellationToken);
                return;
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
        Logger.Information("Setup commands {@Commands}", _commandHandlers.Select(t => t.Command));
        await _client.SetMyCommandsAsync(
            // ReSharper disable once PossibleMultipleEnumeration
            _commandHandlers.Select(t => new BotCommand
                                        { Command = t.Command.FullCommand, Description = t.Command.Description }),
            cancellationToken: cancellationToken);
    }
}
