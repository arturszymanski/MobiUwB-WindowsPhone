#region

using System;
using System.Collections.Generic;

#endregion

namespace SharedCode.Tasks.Models
{
    public abstract class TaskOutput : Result
    {
        //private readonly List<String> errors;

        //protected TaskOutput()
        //{
        //    errors = new List<String>();
        //}

        //public void AddError(String errorMessage)
        //{
        //    errors.Add(errorMessage);
        //}

        //public void AddErrors(IList<String> errorMessages)
        //{
        //    errors.AddRange(errorMessages);
        //}

        //public void AddErrors(IList<Exception> errorMessages)
        //{
        //    foreach (Exception exception in errorMessages)
        //    {
        //        AddError(exception.Message);
        //    }
        //}

        //public List<String> GetErrors()
        //{
        //    return errors;
        //}

        //public bool IsValid()
        //{
        //    return errors.Count == 0;
        //}
    }
}
