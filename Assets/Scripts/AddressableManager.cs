using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Models;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AddressableManager
{
    public static readonly string StartCardAsset = "/Cards/StartCard";
    public static readonly string QuestionCardAsset = "/Cards/QuestionCard";
    public static readonly string ResultsCardAsset = "/Cards/ResultsCard";
    public static readonly string QuestionOptionAsset = "/Components/QuestionOption";
    
    public static AddressableManager Instance => instance ??= new AddressableManager();
    private static AddressableManager instance;

    private readonly Dictionary<string, AsyncOperationHandle> instanceHandles = new();
    private static Dictionary<string, AsyncOperationHandle> Handles => Instance.instanceHandles;
    
    public static async Task<T> GetAsset<T>(string path)
    where T: UnityEngine.Object 
    {
        if (Handles.ContainsKey(path))
        {
            if (Handles[path].Result is T result)
            {
                return result;
            }
            throw new Exception($"Cashed asset {path} {Handles[path].GetType()} cannot be converted to type {typeof(T)}");
        }
        
        var handle = Addressables.LoadAssetAsync<T>(path);
        await handle.Task;

        if (handle.Status != AsyncOperationStatus.Succeeded)
            throw new Exception($"Load asset {path} failed \n {Handles[path].OperationException.Message}");

        Handles[path] = handle;
        return handle.Result;

        var type = handle.Result.GetType();
        Handles.Remove(path);
        Addressables.Release(handle);
        throw new Exception($"Loaded asset {path} {type} cannot be converted to type {typeof(T)}");
    }

    public static IEnumerator GetAssetCoroutine<T>(string path, Action<T> onComplete)
        where T: UnityEngine.Object 
    {
        var task = GetAsset<T>(path);
        yield return new WaitUntil(() => task.IsCompleted);
        onComplete?.Invoke(task.Result);
    }

    public static void ReleaseAsset(string path)
    {
        Addressables.Release(Handles[path]);
    }
}