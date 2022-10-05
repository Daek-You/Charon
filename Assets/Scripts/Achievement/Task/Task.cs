using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum TaskState
{
    Inactive,
    Running,
    Complete
}

[CreateAssetMenu(menuName = "Quest/Task/Task", fileName = "Task_")]
public class Task : ScriptableObject
{
    public delegate void StateChangedHandler(Task task, TaskState curState, TaskState preState);
    public delegate void SuccessChangedHandler(Task task, int curSuccess, int preSuccess);

    public event StateChangedHandler onStateChanged;
    public event SuccessChangedHandler onSuccessChanged;

    [Header("Category")]
    [SerializeField]
    private Category category;

    [Header("Text")]
    [SerializeField]
    private string codeName;
    [SerializeField]
    private string description;

    [Header("Action")]
    [SerializeField]
    private TaskAction action;

    [Header("Target")]
    [SerializeField]
    private TaskTarget[] targets;

    [Header("Setting")]
    [SerializeField]
    private int needSuccessToComplete;
    [SerializeField]
    private InitialSuccessValue initialSuccessValue;

    private TaskState state;
    private int currentSuccess;

    public Category Category => category;
    public string CodeName => codeName;
    public string Description => description;
    public InitialSuccessValue InitialSuccessValue => initialSuccessValue;
    public int NeedSuccessToComplete => needSuccessToComplete;

    public int CurrentSuccess
    {
        get => currentSuccess;
        set
        {
            int preSuccess = currentSuccess;
            currentSuccess = Mathf.Clamp(value, 0, needSuccessToComplete);
            if (currentSuccess != preSuccess)
            {
                State = currentSuccess == needSuccessToComplete ? TaskState.Complete : TaskState.Running;
                onSuccessChanged?.Invoke(this, currentSuccess, preSuccess);
            }
        }
    }

    public TaskState State
    {
        get => state;
        set
        {
            var preState = state;
            state = value;
            onStateChanged?.Invoke(this, state, preState);
        }
    }

    public bool IsComplete => State == TaskState.Complete;
    public Quest Owner { get; private set; }

    public void Setup(Quest owner)
    {
        Owner = owner;
    }

    public void Start()
    {
        State = TaskState.Running;
        if (initialSuccessValue != null)
            CurrentSuccess = initialSuccessValue.GetValue(this);
    }

    public void End()
    {
        onStateChanged = null;
        onSuccessChanged = null;
    }

    public void ReceiveReport(int successCount)
    {
        // Action을 통해 값을 변화 (모듈화)
        // 해당 과정을 실행한 주체가 누구인지 아는 것이 관리하기 수월하므로 인자로 Task(this)를 넘겨줌
        CurrentSuccess = action.Run(this, CurrentSuccess, successCount);
    }

    public void Complete()
    {
        CurrentSuccess = needSuccessToComplete;
    }

    public bool IsTarget(string category, object target)
        => Category.CodeName == category &&
        targets.Any(x => x.IsEqual(target)) &&
        !IsComplete;
}
