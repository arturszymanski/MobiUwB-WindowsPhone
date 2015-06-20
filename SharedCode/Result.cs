#region

using System;
using System.Collections.Generic;
using System.Text;

#endregion

namespace SharedCode
{
    public abstract class Result
    {
        private readonly List<String> _errors;

        public void AddErrorMessage(String error)
        {
            _errors.Add(error);
        }

        public void AddErrorMessages(ICollection<String> errors)
        {
            _errors.AddRange(errors);
        }

        public void AddError(Exception exception)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("Error Message: ");
            stringBuilder.Append(exception.Message);
            stringBuilder.Append("\nInner Exception: ");
            stringBuilder.Append(exception.InnerException);
            stringBuilder.Append("\nCallStack: ");
            stringBuilder.Append(exception.StackTrace);
            
            _errors.Add(
                stringBuilder.ToString());
        }

        public IList<String> GetErrors()
        {
            return _errors;
        }

        public bool Succeeded
        {
            get
            {
                return _errors.Count == 0; 
            }
        }

        public Result()
        {
            _errors = new List<String>();
        }
    }
}
