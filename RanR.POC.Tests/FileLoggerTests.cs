using Microsoft.VisualStudio.TestTools.UnitTesting;
using RanR.POC.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RanR.POC.Tests
{
    [TestClass]
    public class FileLoggerTests
    {
        [TestMethod]
        public void generate_info_message_file_logger()
        {
            var path = Path.GetTempPath();
            var logger = FileLogger.Instance;
            logger.LogPath = path;

            var info = "Test message";
            logger.LogInfoMessage(info);
            var result = logger.ReadLog(false);

            File.Delete(logger.GetPath(false));
            Assert.IsTrue(result.Contains(info));
        }

        [TestMethod]
        public void generate_error_message_file_logger()
        {
            var path = Path.GetTempPath();
            var logger = FileLogger.Instance;
            logger.LogPath = path;
            var error = "Test error";
            logger.LogError(error, "");
            var result = logger.ReadLog(true);

            File.Delete(logger.GetPath(true));
            Assert.IsTrue(result.Contains(error));
        }
    }
}
