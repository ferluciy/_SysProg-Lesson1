
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class AsyncExample : MonoBehaviour
{
    async void Start()
    {
        CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
        CancellationToken cancelToken = cancelTokenSource.Token;
        Debug.Log("Задача 2");
        Task<int> task1 = Task.Run(() => Unit1Async(cancelToken, 1));
        Task<int> task2 = Task.Run(() => Unit2Async(cancelToken, 60));
        await Task.WhenAll(task1, task2);

        Debug.Log("Задача 3");
        Task<bool> task3 = Task.Run(() => WhatTaskFasterAsync(cancelTokenSource, Unit1Async(cancelToken, 1), Unit2Async(cancelToken, 1200000)));
        Debug.Log(task3.Result);
    }

    async Task<int> Unit1Async(CancellationToken cancelToken, int times)
    {
        int time = times * 1000;
        int interval = 100;
        while (time > 0)
        {
            if (cancelToken.IsCancellationRequested)
            {
                return 0;
            }
            time -= interval;
            await Task.Delay(interval);
        }
        Debug.Log("TASK 1 Выполнена");
        return 1;
    }

    async Task<int> Unit2Async(CancellationToken cancelToken, int times)
    {
        while (times > 0)
        {
            if (cancelToken.IsCancellationRequested)
            {
                return 0;
            }
            times--;
            
            await Task.Yield();
        }
        Debug.Log("TASK 2 Выполнена");
        return 2;
    }

    public async static Task<bool> WhatTaskFasterAsync(CancellationTokenSource ct, Task<int> task1, Task<int> task2)
    {
        var taskResult = await Task.WhenAny(task1, task2);
        ct.Cancel();
        if (taskResult.Result == 1) return true;
        else return false;
    }
}
