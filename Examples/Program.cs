using System;
using LibLogConsole;

namespace ShowCase
{
    class Program
    {
        static void Main(string[] args)
        {
            /* This small sample program produces the following output (with varying timestamps):
                INFO  2020-12-14T12:18:40.3551830-05:00 > The default log level is Info, see: Info
                ERROR 2020-12-14T12:18:40.3922810-05:00 > This is an error
                TRACE 2020-12-14T12:18:40.3924310-05:00 > (Program.cs:30) I just set the loglevel to trace!
                INFO  2020-12-14T12:18:40.3925130-05:00 \ (Program.cs:31) Messages can be split over
                INFO  2020-12-14T12:18:40.3925130-05:00 / (Program.cs:31) multiple lines.
                INFO  2020-12-14T12:18:40.3925300-05:00 \ (Program.cs:32) Log messages can
                INFO  2020-12-14T12:18:40.3925300-05:00 | (Program.cs:32) even be split
                INFO  2020-12-14T12:18:40.3925300-05:00 | (Program.cs:32) over a large number of lines.
                INFO  2020-12-14T12:18:40.3925300-05:00 | (Program.cs:32) The log level, the timestamp,
                INFO  2020-12-14T12:18:40.3925300-05:00 | (Program.cs:32) and the source code location
                INFO  2020-12-14T12:18:40.3925300-05:00 | (Program.cs:32) is still added, so that the message always
                INFO  2020-12-14T12:18:40.3925300-05:00 / (Program.cs:32) starts in the same column.
                INFO  2020-12-14T12:18:40.3925500-05:00 > Logging is performed by callingLogger.LogX() (with log level X),
                INFO  2020-12-14T12:18:40.3926170-05:00 > or by explicitly stating the log level in Logger.Log().
            */
            Logger.LogInfo($"The default log level is Info, see: {Logger.GetLogLevel().ToString()}");
            Logger.LogError("This is an error");
            Logger.LogTrace("By default, trace is not shown!"); // this message will not be printed due to the log level
            Logger.SetLogLevel(LogLevel.Trace);
            Logger.LogTrace("I just set the loglevel to trace!");
            Logger.LogInfo("Messages can be split over\nmultiple lines.");
            Logger.LogInfo("Log messages can\neven be split\nover a large number of lines.\nThe log level, the timestamp,\nand the source code location\nis still added, so that the message always\nstarts in the same column.");
            Logger.SetLogLevel(LogLevel.Info);
            Logger.LogInfo("Logging is performed by callingLogger.LogX() (with log level X),");
            Logger.Log(LogLevel.Info, "or by explicitly stating the log level in Logger.Log().");
            
        }
    }
}
