using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum QuestState
{
    Inactive,
    Running,
    Complete,
    Cancel,
    WaitingForComplete
}

[CreateAssetMenu(menuName = "Quest/Quest", fileName = "Quest_")]
public class Quest : ScriptableObject
{
    #region Events
    public delegate void TaskSuccessChangedHandler(Quest quest, Task task, int curSuccess, int preSuccess);
    public delegate void CompleteHandler(Quest quest);
    public delegate void CancelHandler(Quest quest);
    public delegate void NewTaskGroupHandler(Quest quest, TaskGroup curTaskGroup, TaskGroup preTaskGroup);

    public event TaskSuccessChangedHandler onTaskSuccessChanged;
    public event CompleteHandler onCompleted;
    public event CancelHandler onCanceled;
    public event NewTaskGroupHandler onNewTaskGroup;
    #endregion

    [SerializeField]
    private Category category;

    [Header("Text")]
    [SerializeField]
    private string codeName;
    [SerializeField]
    private string displayName;
    [SerializeField, TextArea]
    private string description;

    [Header("Option")]
    [SerializeField]
    private bool useAutoComplete;
    [SerializeField]
    private bool isCancelable;
    [SerializeField]
    private bool isSavable;

    [Header("Task")]
    [SerializeField]
    private TaskGroup[] taskGroups;

    [Header("Condition")]
    [SerializeField]
    private Condition[] acceptionConditions;

    private int currentTaskGroupIndex;

    public Category Category => category;
    public string CodeName => codeName;
    public string DisplayName => displayName;
    public string Description => description;
    public TaskGroup CurrentTaskGroup => taskGroups[currentTaskGroupIndex];
    public IReadOnlyList<TaskGroup> TaskGroups => taskGroups;
    public QuestState State { get; private set; }

    public bool IsRegisted => State != QuestState.Inactive;
    public bool IsCompletable => State == QuestState.WaitingForComplete;
    public bool IsCompleted => State == QuestState.Complete;
    public bool IsCanceled => State == QuestState.Cancel;
    public virtual bool IsCancelable => isCancelable;
    public virtual bool IsSavable => isSavable;

    public bool IsAcceptable => acceptionConditions.All(x => x.IsPass(this));

    public void OnRegister()
    {
        Debug.Assert(!IsRegisted, "This quest has already been registered.");

        foreach (var taskGroup in taskGroups)
        {
            taskGroup.Setup(this);
            foreach (var task in taskGroup.Tasks)
                task.onSuccessChanged += OnSuccessChanged;
        }

        State = QuestState.Running;
        CurrentTaskGroup.Start();
    }

    public void ReceiveReport(string category, object target, int successCount)
    {
        if (IsCompleted)
        {
            Debug.Log("Is Completed.");
            return;
        }

        CurrentTaskGroup.ReceiveReport(category, target, successCount);

        if (CurrentTaskGroup.IsAllTaskComplete)
        {
            if (currentTaskGroupIndex + 1 == taskGroups.Length)
            {
                State = QuestState.WaitingForComplete;
                if (useAutoComplete)
                    Complete();
            }
            else
            {
                var preTaskGroup = taskGroups[currentTaskGroupIndex++];
                preTaskGroup.End();
                CurrentTaskGroup.Start();
                onNewTaskGroup?.Invoke(this, CurrentTaskGroup, preTaskGroup);
            }
        }
        else
            State = QuestState.Running;
    }

    public void Complete()
    {
        foreach (var taskGroup in TaskGroups)
            taskGroup.Complete();

        State = QuestState.Complete;

        onCompleted?.Invoke(this);

        onTaskSuccessChanged = null;
        onCompleted = null;
        onCanceled = null;
        onNewTaskGroup = null;
    }

    public virtual void Cancel()
    {
        State = QuestState.Cancel;
        onCanceled?.Invoke(this);
    }

    public Quest Clone()
    {
        var clone = Instantiate(this);
        clone.taskGroups = taskGroups.Select(x => new TaskGroup(x)).ToArray();

        return clone;
    }

    private void OnSuccessChanged(Task task, int curSuccess, int preSuccess)
        => onTaskSuccessChanged?.Invoke(this, task, curSuccess, preSuccess);

    public QuestData SaveData()
    {
        return new QuestData(codeName, State, currentTaskGroupIndex, CurrentTaskGroup.Tasks.Select(x => x.CurrentSuccess).ToArray());
    }

    public void LoadData(QuestData saveData)
    {
        State = saveData.state;
        currentTaskGroupIndex = saveData.taskGroupIndex;

        for (int i = 0; i < currentTaskGroupIndex; i++)
        {
            var taskGroup = taskGroups[i];
            taskGroup.Start();
            taskGroup.Complete();
        }

        for (int i = 0; i < saveData.taskSuccessCounts.Length; i++)
        {
            CurrentTaskGroup.Start();
            CurrentTaskGroup.Tasks[i].CurrentSuccess = saveData.taskSuccessCounts[i];
        }
    }
}
