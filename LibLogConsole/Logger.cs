using System;
using System.Threading;
using System.IO;
using System.Runtime.CompilerServices;

namespace LibLogConsole
{
    public class Logger
    {
        // The default log level
        private const LogLevel defaultLogLevel = LogLevel.Info;

        // The current log level. Default is info.
        private static LogLevel currentLogLevel = defaultLogLevel;

        // Mutex for parallel access to the logging library.
        private static Mutex loggingMutex = new Mutex();

        // Number of lines that have been logged since start of the library. This variable is used for unit tests.
        private static ulong totalNumLinesLogged = 0;

        /// <summary>
        /// Method that performs the actual logging. Change this method to change the logging backend.
        /// </summary>
        private static void LogInternal(string sourceFilePath, int sourceLineNumber, LogLevel messageLogLevel,  string message) {
            // acquire mutex
            loggingMutex.WaitOne();

            // only log if the log level is sufficient
            if (messageLogLevel <= currentLogLevel) {
                string logLevelString = $"{messageLogLevel.ToString().ToUpper(), -5}"; // upper-case with added spaces up to 5 chars
                string timeString = $"{DateTime.Now.ToString("o")}"; // ISO 8601 in local representation, e.g. "2020-12-14T12:17:16.3803730-05:00"
                
                // count the number of lines in the message. This is required to know when to print the "last message" deliminator
                int totalLinesInMessage = GetLineCountInString(message);

                // print output, one line at a time
                using (StringReader sr = new StringReader(message)) {
                    string line;
                    int currentLineInMessage = 0;

                    while ((line = sr.ReadLine()) != null) {
                        // pick the right character separating the metadata from the message
                        char splitCharacter = GetMessageDeliminator(currentLineInMessage, totalLinesInMessage);

                        // Log the message. If the current log level is Debug or higher, add information about the position in the code
                        if (currentLogLevel >= LogLevel.Debug) {
                            string source = $"{Path.GetFileName(sourceFilePath)}:{sourceLineNumber}";
                            
                            Console.WriteLine($"{logLevelString} {timeString} {splitCharacter} ({source}) {line}");
                        } else {
                            Console.WriteLine($"{logLevelString} {timeString} {splitCharacter} {line}");
                        }

                        // increase the count of total number of lines printed
                        totalNumLinesLogged++;

                        // increase the counter for the current line of this message
                        currentLineInMessage++;
                    }
                }
            }

            // release mutex
            loggingMutex.ReleaseMutex();
        }

        /// <summary>
        /// Determines the correct deliminator between the timestamp and the message text. This is used to make multi-line log messages easier to read.
        /// </summary>
        /// <returns>A char containing the message deliminator</returns>
        private static char GetMessageDeliminator(int currentLine, int totalLines) {
            if (totalLines > 1) {
                if (currentLine == 0) {
                    return '\\'; // for the first line of multi-line messages
                } else if (currentLine == totalLines-1) {
                    return '/'; // for the last line of multi-line messages
                } else {
                    return '|'; // for everything in the middle
                }
            } else {
                return '>'; // for single lines
            }
        }

        /// <summary>
        /// Counts the number of lines in a string.
        /// </summary>
        /// <param name="multiLineString">A string potentially containing multiple lines</param>
        /// <returns>An int containing the number of lines</returns>
        private static int GetLineCountInString(string multiLineString) {
            int newLineLen = Environment.NewLine.Length;
            int numLineBreaks = multiLineString.Length - multiLineString.Replace(Environment.NewLine, string.Empty).Length;
            if (newLineLen != 0)
            {
                numLineBreaks /= newLineLen;
            }
            return numLineBreaks + 1; // number of lines is the number of line breaks plus one
        }

        /// <summary>
        /// Logs a message annotated by the log level, the current time, and for current log levels of Debug and higher, the filename and line number of the caller, at log level <paramref name="messageLogLevel"/>.
        /// </summary>
        /// <param name="messageLogLevel">The log level of the message. Can be Trace, Debug, Info, Warn, Error, or Fatal</param>
        /// <param name="message">The log message</param>
        /// <returns></returns>
        public static void Log(LogLevel messageLogLevel, string message, 
                [CallerFilePath] string sourceFilePath = "",
                [CallerLineNumber] int sourceLineNumber = 0) {
            LogInternal(sourceFilePath, sourceLineNumber, messageLogLevel, message);
        }

        /// <summary>
        /// Logs a fatal message. Use this log level when an irrecoverable problem has occurred. Logs the message annotated by the log level, the current time, and for current log levels of Debug and higher, the filename and line number of the caller, at log level Fatal.
        /// </summary>
        /// <param name="message">The log message</param>
        /// <returns></returns>
        public static void LogFatal(string message, 
                [CallerFilePath] string sourceFilePath = "",
                [CallerLineNumber] int sourceLineNumber = 0) {
            LogInternal(sourceFilePath, sourceLineNumber, LogLevel.Fatal, message);
        }

        /// <summary>
        /// Logs an error message. Use this log level when a serious issue has occurred, but the program can keep running. Logs the message annotated by the log level, the current time, and for current log levels of Debug and higher, the filename and line number of the caller, at log level Error.
        /// </summary>
        /// <param name="message">The log message</param>
        /// <returns></returns>
        public static void LogError(string message, 
                [CallerFilePath] string sourceFilePath = "",
                [CallerLineNumber] int sourceLineNumber = 0) {
            LogInternal(sourceFilePath, sourceLineNumber, LogLevel.Error, message);
        }

        /// <summary>
        /// Logs a warning. Use this log level when an issue occurred that might cause a larger problem later on. Logs the message annotated by the log level, the current time, and for current log levels of Debug and higher, the filename and line number of the caller, at log level Warn.
        /// </summary>
        /// <param name="message">The log message</param>
        /// <returns></returns>
        public static void LogWarn(string message, 
                [CallerFilePath] string sourceFilePath = "",
                [CallerLineNumber] int sourceLineNumber = 0) {
            LogInternal(sourceFilePath, sourceLineNumber, LogLevel.Warn, message);
        }

        /// <summary>
        /// Logs an info message. Use this log level when a normal, noteworthy application milestone occurred. Logs the message annotated by the log level, the current time, and for current log levels of Debug and higher, the filename and line number of the caller, at log level Info.
        /// </summary>
        /// <param name="message">The log message</param>
        /// <returns></returns>
        public static void LogInfo(string message, 
                [CallerFilePath] string sourceFilePath = "",
                [CallerLineNumber] int sourceLineNumber = 0) {
            LogInternal(sourceFilePath, sourceLineNumber, LogLevel.Info, message);
        }

        /// <summary>
        /// Logs a debug message. Use this log level when a situation that might be interesting for debugging occurred. Logs the message annotated by the log level, the current time, and for current log levels of Debug and higher, the filename and line number of the caller, at log level Debug.
        /// </summary>
        /// <param name="message">The log message</param>
        /// <returns></returns>
        public static void LogDebug(string message, 
                [CallerFilePath] string sourceFilePath = "",
                [CallerLineNumber] int sourceLineNumber = 0) {
            LogInternal(sourceFilePath, sourceLineNumber, LogLevel.Debug, message);
        }

        /// <summary>
        /// Logs a trace message. Use this log level when mentioning a detail that might be interesting for fine-grained debugging. Logs the message annotated by the log level, the current time, and for current log levels of Debug and higher, the filename and line number of the caller, at log level Trace.
        /// </summary>
        /// <param name="message">The log message</param>
        /// <returns></returns>
        public static void LogTrace(string message, 
                [CallerFilePath] string sourceFilePath = "",
                [CallerLineNumber] int sourceLineNumber = 0) {
            LogInternal(sourceFilePath, sourceLineNumber, LogLevel.Trace, message);
        }

        /// <summary>
        /// Sets the current log level.
        /// </summary>
        /// <param name="message"><param name="newLevel">The new log level. Can be Trace, Debug, Info, Warn, Error, or Fatal</param></param>
        /// <returns></returns>
        public static void SetLogLevel(LogLevel newLevel) {
            // acquire mutex
            loggingMutex.WaitOne();

            currentLogLevel = newLevel;
            
            // release mutex
            loggingMutex.ReleaseMutex();
        }

        public static LogLevel GetLogLevel() {
            return currentLogLevel;
        }

        /// <summary>
        /// Gets the number of lines that have been printed so far. This method is needed for unit tests.
        /// </summary>
        /// <returns>The number of lines that have been printed.</returns>
        public static ulong GetTotalNumLinesLogged() {
            return totalNumLinesLogged;
        }

        /// <summary>
        /// Resets the logging library to its initial state. This method is needed for unit tests.
        /// </summary>
        public static void Reset() {
            totalNumLinesLogged = 0ul;
            currentLogLevel = defaultLogLevel;
        }
    }

    /// <summary>
    /// Possible log levels.
    /// </summary>
    public enum LogLevel {
        /// <summary> An irrecoverable problem has occurred </summary>
        Fatal,
        /// <summary> A serious issue has occurred, but the program can keep running </summary>
        Error,
        /// <summary> An issue occurred that might cause a larger problem later on </summary>
        Warn,
        /// <summary> A normal, noteworthy application milestone occurred </summary>
        Info,
        /// <summary> A situation that might be interesting for debugging occurred </summary>
        Debug,
        /// <summary> A detail that might be interesting for fine-grained debugging is being mentioned </summary>
        Trace
    }
}
