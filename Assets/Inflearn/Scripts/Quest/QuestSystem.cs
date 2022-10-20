using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class QuestSystem : MonoSingleton<QuestSystem>
{
    #region Events
    public delegate void QuestRegisteredHandler(Quest newQuest);
    public delegate void QuestCompletedHandler(Quest quest);
    public delegate void QuestCanceledHandler(Quest quest);
    #endregion

    private List<Quest> activeQuests = new List<Quest>();
    private List<Quest> completeQuests = new List<Quest>();

    private List<Achivement> activeAchivements = new List<Achivement>();
    private List<Achivement> completeAchivements = new List<Achivement>();

    private QuestDataBase questDataBase, achivementDataBase;

    public event QuestRegisteredHandler onQuestRegistered;
    public event QuestCompletedHandler onQuestCompleted;
    public event QuestCanceledHandler onQuestCanceled;

    public IReadOnlyList<Quest> ActiveQuests => activeQuests;
    public IReadOnlyList<Quest> CompleteQuests => completeQuests;

    public IReadOnlyList<Achivement> ActiveAchivements => activeAchivements;
    public IReadOnlyList<Achivement> CompleteAchivements => completeAchivements;

    private void Awake()
    {
        questDataBase = Resources.Load<QuestDataBase>("QuestDataBase");
        achivementDataBase = Resources.Load<QuestDataBase>("AchivementDataBase");
    }

    public void Register(Quest quest)
    {
        var newQuest = Instantiate(quest);
    }
}
