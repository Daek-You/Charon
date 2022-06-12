using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(menuName = "Quest/AchievementDatabase", fileName = "AchievementDatabase")]
public class AchievementDatabase : ScriptableObject
{
    [SerializeField]
    private List<Achievement> achievemnets;
    public IReadOnlyList<Achievement> Achievemnets => achievemnets;

    public Achievement FindAchievementBy(string codeName)
        => achievemnets.FirstOrDefault(x => x.CodeName == codeName);

    #if UNITY_EDITOR
    [ContextMenu("FindAchievements")]
    private void FindAchievements()
    {
        FindAchievementsBy<Achievement>();
    }

    private void FindAchievementsBy<T>() where T : Achievement
    {
        achievemnets = new List<Achievement>();

        string[] guids = AssetDatabase.FindAssets($"t:{typeof(T)}");
        foreach (var guid in guids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            var achievement = AssetDatabase.LoadAssetAtPath<T>(assetPath);

            if (achievement.GetType() == typeof(T))
                achievemnets.Add(achievement);

            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }
    }
    #endif
}
