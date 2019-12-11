using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Hosting;
using Microsoft.Ajax.Utilities;

namespace XIVPlug
{
    public class ErrorLog
    {
        public static ErrorLog GetInstance()
        {
            return instance ?? (instance = new ErrorLog());
        }

        private static ErrorLog instance;

        private string logFilePath;

        public void Initialize(HttpServerUtility server)
        {
            logFilePath = server.MapPath("~/App_Data/errors.txt");
        }

        private ConcurrentQueue<Exception> exceptionLogQueue = new ConcurrentQueue<Exception>();
        private BackgroundWorker worker = new BackgroundWorker();

        private ErrorLog()
        {
        }

        private void ProcessExceptionQueue(CancellationToken cancellationToken)
        {
            if (exceptionLogQueue.Count == 0)
            {
                return;
            }

            using (var ms = new MemoryStream())
            {
                if (File.Exists(logFilePath))
                {
                    using (var stream = File.OpenRead(logFilePath))
                    {
                        stream.CopyTo(ms);
                    }
                }

                while (exceptionLogQueue.TryDequeue(out var ex))
                {
                    var bytes = Encoding.UTF8.GetBytes($"[{DateTime.Now:O}] {ex.Message} {ex.StackTrace}\r\n");

                    ms.Write(bytes, 0, bytes.Length);
                }

                using (var stream = File.OpenWrite(logFilePath))
                {
                    ms.Seek(0, SeekOrigin.Begin);
                    ms.CopyTo(stream);
                }
            }
        }

        public void LogException(Exception exception)
        {
            try
            {
                exceptionLogQueue.Enqueue(exception);

                HostingEnvironment.QueueBackgroundWorkItem(ProcessExceptionQueue);
            }
            catch (Exception)
            {
            }
        }
    }
}