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

    private List<Achievement> activeAchievements = new List<Achievement>();
    private List<Achievement> completeAchievements = new List<Achievement>();

    private QuestDataBase questDataBase, achievementDataBase;

    public event QuestRegisteredHandler onQuestRegistered;
    public event QuestCompletedHandler onQuestCompleted;
    public event QuestCanceledHandler onQuestCanceled;

    public event QuestRegisteredHandler onachievementRegistered;
    public event QuestCompletedHandler onachievementCompleted;

    public IReadOnlyList<Quest> ActiveQuests => activeQuests;
    public IReadOnlyList<Quest> CompleteQuests => completeQuests;

    public IReadOnlyList<Achievement> Activeachievements => activeAchievements;
    public IReadOnlyList<Achievement> Completeachievements => completeAchievements;

    private void Awake()
    {
        questDataBase = Resources.Load<QuestDataBase>("QuestDataBase");
        achievementDataBase = Resources.Load<QuestDataBase>("achievementDataBase");
    }

    public void Register(Quest quest)
    {
        var newQuest = quest.Clone();

        if(newQuest is Achievement)
        {
            newQuest.onCompleted += OnAchievementCompleted;

            activeAchievements.Add((Achievement)newQuest);

            newQuest.OnRegister();
            onachievementRegistered.Invoke(newQuest);
        }
    }

    #region Callback
    private void OnQuestCompleted(Quest quest)
    {
        activeQuests.Remove(quest);
        completeQuests.Add(quest);

        onQuestCompleted?.Invoke(quest);
    }

    private void OnQuestCanceled(Quest quest)
    {
        activeQuests.Remove(quest);
        onQuestCanceled?.Invoke(quest);

        Destroy(quest, Time.deltaTime);
    }

    private void OnAchievementCompleted(Quest achievement)
    {
        activeAchievements.Remove(achievement as Achievement);
        completeAchievements.Add(achievement as Achievement);

        onachievementCompleted?.Invoke(achievement);
    }
    #endregion
}
