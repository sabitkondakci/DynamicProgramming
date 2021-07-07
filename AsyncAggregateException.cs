using System.Collections.Generic;
using System.Threading.Tasks;

class TaskException
{
    public static async Task<IEnumerable<T>> WhenAll<T>(params Task<T>[] tasks)
    {
        var taskList = Task.WhenAll(tasks);
        
        try
        {
            return await taskList;
        }
        catch (System.Exception){/*ignore exception*/}

        throw taskList.Exception ?? throw new System.Exception("Task list is null");
    }
}