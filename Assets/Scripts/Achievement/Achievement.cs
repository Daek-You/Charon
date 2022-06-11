using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum AchievementState
{
    Inactive,
    Running,
    Complete
}

[CreateAssetMenu(menuName = "Quest/Achievement", fileName = "Achievement_")]
public class Achievement : ScriptableObject
{
    public delegate void TaskSuccessChangedHandler(Achievement achievement, Task task, int curSuccess, int preSuccess);
    public delegate void CompleteHandler(Achievement achievement);
    public delegate void NewTaskGroupHandler(Achievement achievement, TaskGroup curTaskGroup, TaskGroup preTaskGroup);

    public event TaskSuccessChangedHandler onTaskSuccessChanged;
    public event CompleteHandler onComplete;
    public event NewTaskGroupHandler onNewTaskGroup;

    [SerializeField]
    private Category category;
    [SerializeField]
    private Sprite icon;

    [Header("Text")]
    [SerializeField]
    private string codeName;
    [SerializeField]
    private string displayName;
    [SerializeField, TextArea]
    private string description;

    [Header("Task")]
    [SerializeField]
    private TaskGroup[] taskGroups;

    [Header("Condition")]
    [SerializeField]
    private Condition[] acceptionConditions;

    private int currentTaskGroupIndex;

    public Category Category => category;
    public Sprite Icon => icon;
    public string CodeName => codeName;
    public string DisplayName => displayName;
    public string Description => description;
    public TaskGroup CurrentTaskGroup => taskGroups[currentTaskGroupIndex];
    public IReadOnlyList<TaskGroup> TaskGroups => taskGroups;
    public AchievementState State { get; private set; }

    public bool IsRegisted => State != AchievementState.Inactive;
    public bool IsComplete => State == AchievementState.Complete;

    public bool IsAcceptable => acceptionConditions.All(x => x.IsPass(this));

    public void OnRegister()
    {
        Debug.Assert(!IsRegisted, "This achievement has already been registered.");

        foreach (var taskGroup in taskGroups)
        {
            taskGroup.Setup(this);
            foreach (var task in taskGroup.Tasks)
                task.onSuccessChanged += OnSuccessChanged;
        }

        State = AchievementState.Running;
        CurrentTaskGroup.Start();
    }

    public void ReceiveReport(string category, object target, int successCount)
    {
        Debug.Assert(IsRegisted, "This achievement has already been registered.");

        if (IsComplete)
        {
            Debug.Log("Is Completed.");
            return;
        }

        CurrentTaskGroup.ReceiveReport(category, target, successCount);

        if (CurrentTaskGroup.IsAllTaskComplete)
        {
            if (currentTaskGroupIndex + 1 == taskGroups.Length)
            {
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
            State = AchievementState.Running;
    }

    public void Complete()
    {
        Debug.Assert(IsRegisted, "This achievement has already been registered.");

        foreach (var taskGroup in TaskGroups)
            taskGroup.Complete();

        State = AchievementState.Complete;

        onComplete?.Invoke(this);

        onTaskSuccessChanged = null;
        onComplete = null;
        onNewTaskGroup = null;
    }

    public Achievement Clone()
    {
        var clone = Instantiate(this);
        clone.taskGroups = taskGroups.Select(x => new TaskGroup(x)).ToArray();

        return clone;
    }

    private void OnSuccessChanged(Task task, int curSuccess, int preSuccess)
        => onTaskSuccessChanged?.Invoke(this, task, curSuccess, preSuccess);
}
