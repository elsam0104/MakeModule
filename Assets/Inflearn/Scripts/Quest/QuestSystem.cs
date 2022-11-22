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

    private List<Quest> activeAchievements = new List<Quest>();
    private List<Quest> completeAchievements = new List<Quest>();

    private QuestDataBase questDataBase, achievementDataBase;

    public event QuestRegisteredHandler onQuestRegistered;
    public event QuestCompletedHandler onQuestCompleted;
    public event QuestCanceledHandler onQuestCanceled;

    public event QuestRegisteredHandler onachievementRegistered;
    public event QuestCompletedHandler onachievementCompleted;

    public IReadOnlyList<Quest> ActiveQuests => activeQuests;
    public IReadOnlyList<Quest> CompletedQuests => completeQuests;

    public IReadOnlyList<Quest> Activeachievements => activeAchievements;
    public IReadOnlyList<Quest> Completeachievements => completeAchievements;

    private void Awake()
    {
        questDataBase = Resources.Load<QuestDataBase>("QuestDataBase");
        achievementDataBase = Resources.Load<QuestDataBase>("AchievementDataBase");
    }

    public Quest Register(Quest quest)
    {
        var newQuest = quest.Clone();

        if(newQuest is Achievement)
        {
            newQuest.onCompleted += OnAchievementCompleted;

            activeAchievements.Add(newQuest);

            newQuest.OnRegister();
            onachievementRegistered.Invoke(newQuest);
        }
        else
        {
            newQuest.onCompleted += OnQuestCompleted;
            newQuest.onCancled += OnQuestCanceled;

            activeQuests.Add(newQuest);

            newQuest.OnRegister();
            onQuestRegistered?.Invoke(newQuest);
        }
        return newQuest;
    }

    public void ReceiveReport(string category, object target,int successCount)
    {
        ReceiveReport(activeQuests, category, target, successCount);
        ReceiveReport(activeAchievements, category, target, successCount);
    }

    public void ReceiveReport(Category category, TaskTarget target, int successCount)
    => ReceiveReport(category.CodeName, target.Value, successCount);

    private void ReceiveReport(List<Quest> quests, string category, object target, int successCount)
    {
        foreach (var quest in quests.ToArray())
            quest.ReciveReport(category, target, successCount);
    }

    public bool ContainsInActiveQuests(Quest quest) => activeQuests.Any(x => x.CodeName == quest.CodeName);
    public bool ContainsInCompleteQuests(Quest quest) => completeQuests.Any(x => x.CodeName == quest.CodeName);
    public bool ContainsInActiveAchievements(Quest quest) => activeAchievements.Any(x => x.CodeName == quest.CodeName);
    public bool ContainsInCompleteAchievements(Quest quest) => completeAchievements.Any(x => x.CodeName == quest.CodeName);

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
        activeAchievements.Remove(achievement);
        completeAchievements.Add(achievement);

        onachievementCompleted?.Invoke(achievement);
    }
    #endregion
}
