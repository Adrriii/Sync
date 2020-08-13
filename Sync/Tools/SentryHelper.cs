using Sync.Plugins;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Sync.Tools
{

    /// <summary>
    /// A standard error reporter for plugins
    /// </summary>
    /// <typeparam name="T">Plugin Error impl</typeparam>
    public abstract class ErrorRepoter<T> where T : Plugin
    {
        private Action<Exception> errorHandler;
        public ErrorRepoter()
        {
            errorHandler = SentryHelper.Instance.RegisterErrorReporter(typeof(T));
        }

        public void ReportException(Exception e) => errorHandler(e);
    }

    /// <summary>
    /// Sentry helper for error reporter
    /// </summary>
    internal class SentryHelper
    {
        private Dictionary<Plugins.Plugin, object> registedErrorReporter = new Dictionary<Plugins.Plugin, object>();
#if SyncRelease
        private const bool notInDebugger = true;
#else
        private const bool notInDebugger = false;
#endif
        private SentryHelper()
        {
        }

        internal Action<Exception> RegisterErrorReporter(Type type)
        {
            if (type == null ||
                !type.IsClass || !type.IsPublic ||
                !typeof(Plugin).IsAssignableFrom(type) ||
                typeof(Plugin) == type)
            {
                return (x) => Console.WriteLine("Type must inherit Plugin class");
            }
            else
            {
                var logger = type.FullName;
                var version = type.Assembly.GetName().Version.ToString();
                return (x) => this.Error(logger, version, x);
            }
        }

        internal void Error(string logger, string version, Exception e, bool silent = false)
        {
            Console.WriteLine("A error was raised but not repoted to Sync developer");
            Console.WriteLine("Now a error report will print, you can report in github issue");
            Console.WriteLine(e.Message.ToString());
            Console.WriteLine($"Scope: {logger}, Version: {version}");
            Console.WriteLine("- TRACE -");
            Console.WriteLine(e.StackTrace);
        }

        internal void RepoterError(Exception e, bool silent = false) => Error("Sync", Assembly.GetEntryAssembly().GetName().Version.ToString(), e, silent);

        internal static readonly SentryHelper Instance = new SentryHelper();
    }
}
