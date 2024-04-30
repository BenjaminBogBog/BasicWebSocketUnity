using System;
using System.Collections.Concurrent;
using UnityEngine;

public class MainThreadWorker : MonoBehaviour
{
    public static MainThreadWorker Instance;
    ConcurrentQueue<Action> actions = new ConcurrentQueue<Action>();

    private void Awake()
    {
        if (Instance)
        {
            enabled = false;
            return;
        }
        Instance = this;
    }

    private void Update()
    {
        while (actions.TryDequeue(out var action))
        {
            action?.Invoke();
        }
    }

    public void AddAction(Action action)
    {
        if (action != null) actions.Enqueue(action);
    }
}