using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.SharedLibrary.Logs
{
	public static class LogException
	{
		public static void LogExceptions(Exception e)
		{
			LogToFile(e.ToString());
			LogToConsole(e.ToString());
			LogToDebugger(e.ToString());
		}

		private static void LogToFile(string message) => Log.Information(message);
		public static void LogToConsole(string message) => Log.Warning(message);
		public static void LogToDebugger(string message) => Log.Debug(message);
	}
}
