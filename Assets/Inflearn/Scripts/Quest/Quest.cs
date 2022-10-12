using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

public enum QuestState
{
    Inactive,
    Running,
    Complete,
    Cancel,
    WatieCompletion,//완료 버튼 누를 때 까지 대기
}

[CreateAssetMenu(menuName ="Quest/Quest", fileName ="Quest_")]
public class Quest : ScriptableObject
{
    #region Events
    public delegate void TaskSuccessChangeHandler(Quest quest, Task task, int currentSuccess, int prevSuccess);
    public delegate void CompleteHandler(Quest quest);
    public delegate void CancelHandler(Quest quest);
    public delegate void NewTaskGroupHandler(Quest quest, TaskGroup currentTaskGroup, TaskGroup prevTaskGroup);
    #endregion
    [SerializeField]
    private Category category;
    [SerializeField]
    private Sprite icon;

    [Header("Text")]
    [SerializeField]
    private string codeName;
    [SerializeField]
    private string displayName;
    [SerializeField,TextArea]
    private string description;

    [Header("Task")]
    [SerializeField]
    private TaskGroup[] taskGroups;

    [Header("Reward")]
    [SerializeField]
    private Reward[] rewards;

    [Header("Condition")]
    [SerializeField]
    private Condition[] runConditions;
    [SerializeField]
    private Condition[] cancelConditions;

    [Header("Option")]
    [SerializeField]
    private bool useAutoComplete;
    [SerializeField]
    private bool isCancelable;

    private int currentTaskGroupIndex;

    public Category Category => category;
    public Sprite Icon => icon;
    public string CodeName => codeName;
    public string DisplayName => displayName;
    public string Description => description;
    public QuestState State { get; private set; }
    public TaskGroup CurrentTaskGroup => taskGroups[currentTaskGroupIndex];
    public IReadOnlyList<TaskGroup> TaskGroups => taskGroups;
    public IReadOnlyList<Reward> Rewards => rewards;

    public bool IsRegistered => State != QuestState.Inactive;
    public bool IsComplatable => State == QuestState.WatieCompletion;
    public bool IsComplete => State == QuestState.Complete;
    public bool IsCancel => State == QuestState.Cancel;
    public virtual bool IsCancelable => isCancelable && cancelConditions.All(x=>x.InPass(this));
    public bool IsAcceptable => runConditions.All(x => x.InPass(this));

    public event TaskSuccessChangeHandler onTaskSuccessChanged;
    public event CompleteHandler onCompleted;
    public event CancelHandler onCancled;
    public event NewTaskGroupHandler onNewTaskGroup;

    public void OnRegister() //awake
    {
        Debug.Assert(!IsRegistered, "This quest has already been registered."); //앞의 bool 값이 false면 오류 띄움

        foreach(var taskGroup in taskGroups)
        {
            taskGroup.SetUp(this);

            foreach(var task in taskGroup.Tasks)
            {
                task.onSuccessChanged += OnTaskSuccessChanged; //밖에서 일일이 넣어줄 필요 x
            }
        }
        State = QuestState.Running;
        CurrentTaskGroup.Start();
    }
    public void ReciveReport(string category, object target,int successCount)
    {
        Debug.Assert(IsRegistered, "This quest has already been registered.");
        Debug.Assert(!IsCancel, "This quest has been Canceled.");

        if (IsComplete)
            return;
        if (CurrentTaskGroup.IsAllQuestComplete)
        {
            if (currentTaskGroupIndex + 1 == taskGroups.Length) //현재 끝난 task group이 마지막이라면
            {
                State = QuestState.WatieCompletion;
                if (useAutoComplete)
                    Complete();
            }
            else
            {
                var prevTaskGroup = taskGroups[currentTaskGroupIndex++];
                prevTaskGroup.End();
                CurrentTaskGroup.Start();
                onNewTaskGroup?.Invoke(this, CurrentTaskGroup, prevTaskGroup);
            }
        }
        else //수동완료 퀘스트의 경우는 깨졌다가 다시 깨지지 않은 상태로 되돌아 갈 수 있기 떄문
            State = QuestState.Running;
    }
    public void Complete()
    {
        CheckWarning();

        foreach (var taskGroup in taskGroups)
            taskGroup.Complete();

        State = QuestState.Complete;

        foreach(var reward in rewards)
        {
            reward.Give(this);
        }
        onCompleted?.Invoke(this);

        onCompleted = null;
        onCancled = null;
        onTaskSuccessChanged = null;
        onNewTaskGroup = null;
    }
    public virtual void Cancel()
    {
        CheckWarning();
        Debug.Assert(IsCancelable, "This quest can't be Canceled.");

        State = QuestState.Cancel;
        onCancled?.Invoke(this);
    }
    private void OnTaskSuccessChanged(Task task, int curSuccess, int prevSuccess)
    => onTaskSuccessChanged?.Invoke(this,task, curSuccess, prevSuccess);

    [Conditional("UNITY_EDITOR")]
    private void CheckWarning()
    {
        Debug.Assert(IsRegistered, "This quest has already been registered.");
        Debug.Assert(!IsCancel, "This quest has been Canceled.");
        Debug.Assert(!IsComplete, "This quest has already been Completed.");
    }
}