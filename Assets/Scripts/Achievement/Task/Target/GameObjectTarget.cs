using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quest/Task/TaskTarget/GameObject", fileName = "Target_")]
public class GameObjectTarget : TaskTarget
{
    [SerializeField]
    private GameObject value;
    public override object Value => value;

    public override bool IsEqual(object target)
    {
        var targetAsGameObject = target as GameObject;
        if (targetAsGameObject == null)
            return false;
        // Target의 이름이 포함되어 있는지 확인하는 방식 (Clone 등을 고려)
        // 유사한 이름의 Target이 다수 존재한다면 방식을 바꿀 필요가 있음
        return targetAsGameObject.name.Contains(value.name);
    }
}
