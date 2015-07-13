#region

using SharedCode.Tasks.Models;

#endregion

namespace SharedCode.Tasks
{
    public interface ITask<Out> where Out : TaskOutput
    {
        void Execute(TaskInput input, Out output);
    }
}
