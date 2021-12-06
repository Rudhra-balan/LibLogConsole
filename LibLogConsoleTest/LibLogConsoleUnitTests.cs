using Microsoft.VisualStudio.TestTools.UnitTesting;
using LibLogConsole;

namespace LibLogConsoleTest
{
    [TestClass]
    public class LibLogConsoleUnitTests
    {
        [TestMethod]
        public void LogLevelGetSet()
        {
            // prep work
            Logger.Reset();

            // default log level
            Assert.IsTrue(Logger.GetLogLevel() == LogLevel.Info);

            // set all other log levels
            Logger.SetLogLevel(LogLevel.Error);
            Assert.IsTrue(Logger.GetLogLevel() == LogLevel.Error);

            Logger.SetLogLevel(LogLevel.Warn);
            Assert.IsTrue(Logger.GetLogLevel() == LogLevel.Warn);

            Logger.SetLogLevel(LogLevel.Info);
            Assert.IsTrue(Logger.GetLogLevel() == LogLevel.Info);

            Logger.SetLogLevel(LogLevel.Debug);
            Assert.IsTrue(Logger.GetLogLevel() == LogLevel.Debug);

            Logger.SetLogLevel(LogLevel.Trace);
            Assert.IsTrue(Logger.GetLogLevel() == LogLevel.Trace);
        }

        [TestMethod]
        public void BasicLogging()
        {
            // prep work
            Logger.Reset();

            // single line
            Logger.LogInfo("info message");
            Assert.AreEqual(1ul, Logger.GetTotalNumLinesLogged());

            // multiple lines
            Logger.LogInfo("info message1\ninfo message2");
            Assert.AreEqual(3ul, Logger.GetTotalNumLinesLogged());
        }

        [TestMethod]
        public void LogLevelTrace()
        {
            // prep work
            Logger.Reset();
            Logger.SetLogLevel(LogLevel.Trace);

            // all of these should print
            Logger.LogTrace("trace message");
            Logger.LogDebug("debug message");
            Logger.LogInfo("info message");
            Logger.LogWarn("warn message");
            Logger.LogError("error message");
            Logger.LogFatal("fatal message");
            
            Assert.AreEqual(6ul, Logger.GetTotalNumLinesLogged());
        }

        [TestMethod]
        public void LogLevelDebug()
        {
            // prep work
            Logger.Reset();
            Logger.SetLogLevel(LogLevel.Debug);

            // none of these should print
            Logger.LogTrace("trace message");
            
            Assert.AreEqual(0ul, Logger.GetTotalNumLinesLogged());

            // all of these should print
            Logger.LogDebug("debug message");
            Logger.LogInfo("info message");
            Logger.LogWarn("warn message");
            Logger.LogError("error message");
            Logger.LogFatal("fatal message");
            
            Assert.AreEqual(5ul, Logger.GetTotalNumLinesLogged());
        }

        [TestMethod]
        public void LogLevelInfo()
        {
            // prep work
            Logger.Reset();
            Logger.SetLogLevel(LogLevel.Info);

            // none of these should print
            Logger.LogTrace("trace message");
            Logger.LogDebug("debug message");

            Assert.AreEqual(0ul, Logger.GetTotalNumLinesLogged());

            // all of these should print
            Logger.LogInfo("info message");
            Logger.LogWarn("warn message");
            Logger.LogError("error message");
            Logger.LogFatal("fatal message");
            
            Assert.AreEqual(4ul, Logger.GetTotalNumLinesLogged());
        }

        [TestMethod]
        public void LogLevelWarn()
        {
            // prep work
            Logger.Reset();
            Logger.SetLogLevel(LogLevel.Warn);

            // none of these should print
            Logger.LogTrace("trace message");
            Logger.LogDebug("debug message");
            Logger.LogInfo("info message");

            Assert.AreEqual(0ul, Logger.GetTotalNumLinesLogged());

            // all of these should print
            Logger.LogWarn("warn message");
            Logger.LogError("error message");
            Logger.LogFatal("fatal message");
            
            Assert.AreEqual(3ul, Logger.GetTotalNumLinesLogged());
        }

        [TestMethod]
        public void LogLevelError()
        {
            // prep work
            Logger.Reset();
            Logger.SetLogLevel(LogLevel.Error);

            // none of these should print
            Logger.LogTrace("trace message");
            Logger.LogDebug("debug message");
            Logger.LogInfo("info message");
            Logger.LogWarn("warn message");

            Assert.AreEqual(0ul, Logger.GetTotalNumLinesLogged());

            // all of these should print
            Logger.LogError("error message");
            Logger.LogFatal("fatal message");
            
            Assert.AreEqual(2ul, Logger.GetTotalNumLinesLogged());
        }

        [TestMethod]
        public void LogLevelFatal()
        {
            // prep work
            Logger.Reset();
            Logger.SetLogLevel(LogLevel.Fatal);

            // none of these should print
            Logger.LogTrace("trace message");
            Logger.LogDebug("debug message");
            Logger.LogInfo("info message");
            Logger.LogWarn("warn message");
            Logger.LogError("error message");

            Assert.AreEqual(0ul, Logger.GetTotalNumLinesLogged());

            // all of these should print
            Logger.LogFatal("fatal message");
            
            Assert.AreEqual(1ul, Logger.GetTotalNumLinesLogged());
        }
    }
}
