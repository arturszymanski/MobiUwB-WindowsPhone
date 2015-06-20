#region

using SharedCode.Tasks.Models;

#endregion

namespace SharedCode.Tasks
{
    public interface ITask<Out> where Out : TaskOutput
    {
        void execute(TaskInput input, Out output);
    }
}
