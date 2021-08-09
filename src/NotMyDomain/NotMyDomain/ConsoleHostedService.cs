using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NotMyDomain.Exceptions;
using NotMyDomain.Interface;
using NotMyDomain.Models;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace NotMyDomain
{
    internal class ConsoleHostedService : IHostedService
    {
        private readonly ILogger<ConsoleHostedService> _logger;
        private readonly IConsoleOptionSelector<Application> _applicationSelector;
        private readonly IConsoleOptionSelector<Account> _accountSelector;
        private readonly IHostApplicationLifetime _hostApplicationLifetime;

        public ConsoleHostedService(
            ILogger<ConsoleHostedService> logger,
            IConsoleOptionSelector<Application> applicationSelector,
            IConsoleOptionSelector<Account> accountSelector,
            IHostApplicationLifetime hostApplicationLifetime)
        {
            _logger = logger;
            _applicationSelector = applicationSelector;
            _accountSelector = accountSelector;
            _hostApplicationLifetime = hostApplicationLifetime;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                var application = _applicationSelector.Execute();
                Console.Clear();

                var account = _accountSelector.Execute();

                var arguments = $"/k \"@echo off & cls & runas /user:{account.Username} /netonly \"{application.ExecutablePath}\" & exit\"";
                var process = Process.Start("cmd.exe", arguments);
            }
            catch (UserEscapeException)
            {
                _hostApplicationLifetime.StopApplication();
            }
            catch (Exception exc)
            {
                _logger.LogError(exc, "Unhandled exception");
            }
            finally
            {
                bool isAlreadyStopping = _hostApplicationLifetime.ApplicationStopped.IsCancellationRequested
                    || _hostApplicationLifetime.ApplicationStopping.IsCancellationRequested;

                if (!isAlreadyStopping)
                {
                    _hostApplicationLifetime.StopApplication();
                }
            }

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}