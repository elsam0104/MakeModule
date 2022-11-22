using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum TaskState
{
    Inactive,
    Running,
    Complele
}
[CreateAssetMenu(menuName = "Quest/Task/Task", fileName = "Task_")]
public class Task : ScriptableObject
{
    #region Events
    public delegate void StateChangeHandler(Task task, TaskState nowTaskState, TaskState prevState);
    public delegate void SuccessChangeHandler(Task task, int currentSuccess, int prevSuccess);

    #endregion

    [SerializeField]
    private Category category;
    [Header("Text")]
    [SerializeField]
    private string codeName = "";
    [SerializeField]
    private string description = "";

    [Header("Action")]
    [SerializeField]
    private TaskAction action;

    [Header("Target")]
    [SerializeField]
    private TaskTarget[] targets;

    [Header("Setting")]
    [SerializeField]
    private InitialSuccessValue initialSuccessValue;
    [SerializeField]
    private int needToSuccess = 0;
    [SerializeField]
    private bool canReceiveAfterComplete;
    //task 완료되었을 때도 계속 성공횟수를 보고 받을 것이냐?

    public int currentSuccess { get; private set; }

    private TaskState state;

    public event StateChangeHandler onStateChanged;
    public event SuccessChangeHandler onSuccessChanged;

    public int CurrentSuccess
    {
        get => currentSuccess;
        set
        {
            int prevSuccess = currentSuccess;
            currentSuccess = Mathf.Clamp(value, 0, needToSuccess);
            if (currentSuccess != prevSuccess)
            {
                state = (currentSuccess == needToSuccess) ? TaskState.Complele : TaskState.Running;
                onSuccessChanged?.Invoke(this, currentSuccess, prevSuccess);
            }
        }
    }
    public Category Category => category;
    public string CodeName => codeName;
    public string Description => description;
    public int NeedToSuccess => needToSuccess;

    public TaskState State
    {
        get => state;
        set
        {
            var prevState = state;
            state = value;
            onStateChanged?.Invoke(this, state, prevState);
        }
    }

    public bool IsComplete => State == TaskState.Complele;
    public Quest Owener { get; private set; }

    public void SetUp(Quest owener)
    {
        Owener = owener;
    }
    public void Start()
    {
        state = TaskState.Running;
        if (initialSuccessValue)
            currentSuccess = initialSuccessValue.GetValue(this);
    }
    public void End()
    {
        onStateChanged = null;
        onSuccessChanged = null;
    }
    public void ReceiveReport(int successCnt)
    {
        currentSuccess = action.Run(this, currentSuccess, successCnt);
    }
    public void Complete()
    {
        currentSuccess = needToSuccess;
    }
    public bool IsTarget(string category, object target)
    => Category == category &&
        targets.Any(a => a.IsEqual(target)) &&
        (!IsComplete || (IsComplete && canReceiveAfterComplete));
}
