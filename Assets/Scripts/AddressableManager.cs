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
    public static readonly string BottomBarGroup = "/Components/BottomBarGroup";
    
    public static AddressableManager Instance => instance ??= new AddressableManager();
    private static AddressableManager instance;

    private readonly Dictionary<string, UnityEngine.Object> instanceHandles = new();
    private static Dictionary<string, UnityEngine.Object> Handles => Instance.instanceHandles;
    
    public static async Task<T> GetAsset<T>(string path)
    where T: UnityEngine.Object 
    {
        if (Handles.ContainsKey(path))
        {
            if (Handles[path] is T result)
            {
                return result;
            }
            throw new Exception($"Cashed asset {path} {Handles[path].GetType()} cannot be converted to type {typeof(T)}");
        }
        
        var handle = Addressables.LoadAssetAsync<T>(path);
        await handle.Task;

        if (handle.Status != AsyncOperationStatus.Succeeded)
            throw new Exception($"Load asset {path} failed \n {handle.OperationException.Message}");

        Handles[path] = handle.Result;
        return handle.Result;
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
        try
        {
            Addressables.Release(Handles[path]);
        }
        catch (Exception e)
        {
            Debug.LogWarning(e.Message);
            throw;
        }
        
    }
}