using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(menuName ="Quest/QuestDataBase")]
public class QuestDataBase : ScriptableObject 
{
    [SerializeField]
    private List<Quest> quests;

    public IReadOnlyList<Quest> Quests => quests;
    public Quest FindQuestBy(string codeName) => quests.FirstOrDefault(x => x.CodeName == codeName);
}
