#region

using System;
using System.Collections.Generic;

#endregion

namespace SharedCode.Tasks.Models
{
    public abstract class TaskOutput
    {
        private readonly List<String> errors;

        protected TaskOutput()
        {
            errors = new List<String>();
        }

        public void addError(String errorMessage)
        {
            errors.Add(errorMessage);
        }

        public void addErrors(IList<String> errorMessages)
        {
            errors.AddRange(errorMessages);
        }

        public List<String> getErrors()
        {
            return errors;
        }

        public bool isValid()
        {
            return errors.Count == 0;
        }
    }
}
