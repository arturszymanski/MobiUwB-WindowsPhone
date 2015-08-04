#region

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

#endregion

namespace SharedCode
{
    public abstract class Result
    {
        private readonly List<Exception> _exceptions;
        
        public void AddException(Exception exception)
        {
            _exceptions.Add(exception);
        }
        public void AddExceptions(List<Exception> exceptions)
        {
            foreach (Exception exception in exceptions)
            {
                AddException(exception);
            }
        }

        public List<Exception> GetExceptions()
        {
            return _exceptions;
        }

        public bool Succeeded
        {
            get
            {
                return _exceptions.Count == 0; 
            }
        }

        public Result()
        {
            _exceptions = new List<Exception>();
        }

        public void PrintExceptions()
        {
            foreach (Exception exception in _exceptions)
            {
                PrintException(exception);
            }
        }

        public static void PrintException(Exception exception)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("Error Message: ");
            stringBuilder.Append(exception.Message);
            stringBuilder.Append("\nInner Exception: ");
            stringBuilder.Append(exception.InnerException);
            stringBuilder.Append("\nCallStack: ");
            stringBuilder.Append(exception.StackTrace);
            stringBuilder.Append('\n');
            Debug.WriteLine(stringBuilder.ToString());
        }
    }
}
