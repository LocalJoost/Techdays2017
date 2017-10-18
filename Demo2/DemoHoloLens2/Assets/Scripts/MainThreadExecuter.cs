using System;
using System.Collections.Generic;
using System.Linq;
using HoloToolkit.Unity;

public class MainThreadExecuter : Singleton<MainThreadExecuter>
{
    private Queue<Action> _executionQueue;

    // Use this for initialization
    void Start()
    {
        _executionQueue = new Queue<Action>();
    }

    public void Add(Action action)
    {
        _executionQueue.Enqueue(action);
    }

    // Update is called once per frame
    void Update()
    {
        if (_executionQueue.Any())
        {
            var action = _executionQueue.Dequeue();
            if (action != null)
            {
                action();
            }
        }
    }
}
