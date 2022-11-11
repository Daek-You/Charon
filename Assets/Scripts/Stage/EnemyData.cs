using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnemyData
{
    public string mstage;
    public string sstage;
    public string type;
    public float[] position;
}

[Serializable]
public class EnemyDataForLoad : ILoader<MainStageType, List<EnemyData>>
{
    // Load한 Json 리스트가 저장됨
    public List<EnemyData> enemies = new List<EnemyData>();

    public Dictionary<MainStageType, List<EnemyData>> MakeDict()
    {
        Dictionary<MainStageType, List<EnemyData>> dict = new Dictionary<MainStageType, List<EnemyData>>();
        List<EnemyData> list = new List<EnemyData>();
        MainStageType curStage = MainStageType.Stage1;

        foreach (EnemyData enemy in enemies)
        {
            if (curStage != (MainStageType)Enum.Parse(typeof(MainStageType), enemy.mstage))
            {
                // 다음 스테이지 EnemyData에 접근한 경우, 기존 리스트를 딕셔너리에 등록한 후 리스트 초기화
                dict.Add(curStage, list);
                list.Clear();
            }
            list.Add(enemy);
            curStage = (MainStageType)Enum.Parse(typeof(MainStageType), enemy.mstage);
        }
        dict.Add(curStage, list);
        return dict;
    }
}
