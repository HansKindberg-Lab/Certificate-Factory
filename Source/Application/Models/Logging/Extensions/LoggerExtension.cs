namespace Application.Models.Logging.Extensions
{
	public static class LoggerExtension
	{
		#region Nested types

		extension(ILogger logger)
		{
			#region Methods

			public void LogCriticalIfEnabled(Func<string?>? messageFunction)
			{
				logger.LogIfEnabled(null, LogLevel.Critical, messageFunction);
			}

			public void LogCriticalIfEnabled(Exception? exception, Func<string?>? messageFunction)
			{
				logger.LogIfEnabled(exception, LogLevel.Critical, messageFunction);
			}

			public void LogCriticalIfEnabled(string? message, params object?[] arguments)
			{
				logger.LogIfEnabled(0, null, LogLevel.Critical, message, arguments);
			}

			public void LogCriticalIfEnabled(EventId eventId, string? message, params object?[] arguments)
			{
				logger.LogIfEnabled(eventId, null, LogLevel.Critical, message, arguments);
			}

			public void LogCriticalIfEnabled(Exception? exception, string? message, params object?[] arguments)
			{
				logger.LogIfEnabled(0, exception, LogLevel.Critical, message, arguments);
			}

			public void LogCriticalIfEnabled(EventId eventId, Exception? exception, string? message, params object?[] arguments)
			{
				logger.LogIfEnabled(eventId, exception, LogLevel.Critical, message, arguments);
			}

			public void LogDebugIfEnabled(Func<string?>? messageFunction)
			{
				logger.LogIfEnabled(null, LogLevel.Debug, messageFunction);
			}

			public void LogDebugIfEnabled(Exception? exception, Func<string?>? messageFunction)
			{
				logger.LogIfEnabled(exception, LogLevel.Debug, messageFunction);
			}

			public void LogDebugIfEnabled(string? message, params object?[] arguments)
			{
				logger.LogIfEnabled(0, null, LogLevel.Debug, message, arguments);
			}

			public void LogDebugIfEnabled(EventId eventId, string? message, params object?[] arguments)
			{
				logger.LogIfEnabled(eventId, null, LogLevel.Debug, message, arguments);
			}

			public void LogDebugIfEnabled(Exception? exception, string? message, params object?[] arguments)
			{
				logger.LogIfEnabled(0, exception, LogLevel.Debug, message, arguments);
			}

			public void LogDebugIfEnabled(EventId eventId, Exception? exception, string? message, params object?[] arguments)
			{
				logger.LogIfEnabled(eventId, exception, LogLevel.Debug, message, arguments);
			}

			public void LogErrorIfEnabled(Func<string?>? messageFunction)
			{
				logger.LogIfEnabled(null, LogLevel.Error, messageFunction);
			}

			public void LogErrorIfEnabled(Exception? exception, Func<string?>? messageFunction)
			{
				logger.LogIfEnabled(exception, LogLevel.Error, messageFunction);
			}

			public void LogErrorIfEnabled(string? message, params object?[] arguments)
			{
				logger.LogIfEnabled(0, null, LogLevel.Error, message, arguments);
			}

			public void LogErrorIfEnabled(EventId eventId, string? message, params object?[] arguments)
			{
				logger.LogIfEnabled(eventId, null, LogLevel.Error, message, arguments);
			}

			public void LogErrorIfEnabled(Exception? exception, string? message, params object?[] arguments)
			{
				logger.LogIfEnabled(0, exception, LogLevel.Error, message, arguments);
			}

			public void LogErrorIfEnabled(EventId eventId, Exception? exception, string? message, params object?[] arguments)
			{
				logger.LogIfEnabled(eventId, exception, LogLevel.Error, message, arguments);
			}

			private void LogIfEnabled(Exception? exception, LogLevel logLevel, Func<string?>? messageFunction)
			{
				ArgumentNullException.ThrowIfNull(logger);

				if(!logger.IsEnabled(logLevel))
					return;

				var message = messageFunction?.Invoke();

				logger.Log(logLevel, 0, exception, message, []);
			}

			private void LogIfEnabled(EventId eventId, Exception? exception, LogLevel logLevel, string? message, params object?[] arguments)
			{
				ArgumentNullException.ThrowIfNull(logger);

				if(!logger.IsEnabled(logLevel))
					return;

				logger.Log(logLevel, eventId, exception, message, arguments);
			}

			public void LogInformationIfEnabled(Func<string?>? messageFunction)
			{
				logger.LogIfEnabled(null, LogLevel.Information, messageFunction);
			}

			public void LogInformationIfEnabled(Exception? exception, Func<string?>? messageFunction)
			{
				logger.LogIfEnabled(exception, LogLevel.Information, messageFunction);
			}

			public void LogInformationIfEnabled(string? message, params object?[] arguments)
			{
				logger.LogIfEnabled(0, null, LogLevel.Information, message, arguments);
			}

			public void LogInformationIfEnabled(EventId eventId, string? message, params object?[] arguments)
			{
				logger.LogIfEnabled(eventId, null, LogLevel.Information, message, arguments);
			}

			public void LogInformationIfEnabled(Exception? exception, string? message, params object?[] arguments)
			{
				logger.LogIfEnabled(0, exception, LogLevel.Information, message, arguments);
			}

			public void LogInformationIfEnabled(EventId eventId, Exception? exception, string? message, params object?[] arguments)
			{
				logger.LogIfEnabled(eventId, exception, LogLevel.Information, message, arguments);
			}

			public void LogTraceIfEnabled(Func<string?>? messageFunction)
			{
				logger.LogIfEnabled(null, LogLevel.Trace, messageFunction);
			}

			public void LogTraceIfEnabled(Exception? exception, Func<string?>? messageFunction)
			{
				logger.LogIfEnabled(exception, LogLevel.Trace, messageFunction);
			}

			public void LogTraceIfEnabled(string? message, params object?[] arguments)
			{
				logger.LogIfEnabled(0, null, LogLevel.Trace, message, arguments);
			}

			public void LogTraceIfEnabled(EventId eventId, string? message, params object?[] arguments)
			{
				logger.LogIfEnabled(eventId, null, LogLevel.Trace, message, arguments);
			}

			public void LogTraceIfEnabled(Exception? exception, string? message, params object?[] arguments)
			{
				logger.LogIfEnabled(0, exception, LogLevel.Trace, message, arguments);
			}

			public void LogTraceIfEnabled(EventId eventId, Exception? exception, string? message, params object?[] arguments)
			{
				logger.LogIfEnabled(eventId, exception, LogLevel.Trace, message, arguments);
			}

			public void LogWarningIfEnabled(Func<string?>? messageFunction)
			{
				logger.LogIfEnabled(null, LogLevel.Warning, messageFunction);
			}

			public void LogWarningIfEnabled(Exception? exception, Func<string?>? messageFunction)
			{
				logger.LogIfEnabled(exception, LogLevel.Warning, messageFunction);
			}

			public void LogWarningIfEnabled(string? message, params object?[] arguments)
			{
				logger.LogIfEnabled(0, null, LogLevel.Warning, message, arguments);
			}

			public void LogWarningIfEnabled(EventId eventId, string? message, params object?[] arguments)
			{
				logger.LogIfEnabled(eventId, null, LogLevel.Warning, message, arguments);
			}

			public void LogWarningIfEnabled(Exception? exception, string? message, params object?[] arguments)
			{
				logger.LogIfEnabled(0, exception, LogLevel.Warning, message, arguments);
			}

			public void LogWarningIfEnabled(EventId eventId, Exception? exception, string? message, params object?[] arguments)
			{
				logger.LogIfEnabled(eventId, exception, LogLevel.Warning, message, arguments);
			}

			#endregion
		}

		#endregion
	}
}