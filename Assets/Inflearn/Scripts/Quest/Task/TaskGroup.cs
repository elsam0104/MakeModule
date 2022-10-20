using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
public enum TaskGroupState
{
    Inactive,
    Running,
    Complete,
}
    [Serializable]
public class TaskGroup
{
    [SerializeField]
    private Task[] tasks;

    public IReadOnlyList<Task> Tasks => tasks;
    public Quest owner { get; private set; }
    public bool IsAllQuestComplete => tasks.All(x => x.IsComplete);
    public bool IsComplete => state == TaskGroupState.Complete;
    public TaskGroupState state { get; private set; }

    public TaskGroup(TaskGroup copyTarget)
    {
        tasks = copyTarget.tasks.Select(x => UnityEngine.Object.Instantiate(x)).ToArray();
    }
    public void SetUp(Quest owner)
    {
        this.owner = owner;
        foreach(Task task in tasks)
            task.SetUp(owner);
    }

    public void Start()
    {
        state = TaskGroupState.Running;
        foreach (var task in tasks)
            task.Start();
    }

    public void End()
    {
        foreach (var task in tasks)
            task.End();
    }

    public void ReceiveReport(string category,object target,int successCnt)
    {
        foreach(var task in tasks)
        {
            if (task.IsTarget(category, target))
                task.ReceiveReport(successCnt);
        }
    }
    public void Complete()
    {
        if (IsComplete)
            return;

        state = TaskGroupState.Complete;

        foreach (var task in tasks)
        {
            if (!task.IsComplete)
                task.Complete();
        }
    }
}
