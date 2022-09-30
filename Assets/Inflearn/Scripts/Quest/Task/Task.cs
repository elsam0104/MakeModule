using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName ="Quest/Task/Task",fileName ="Task_")]
public class Task : ScriptableObject
{ 
    [Header("Text")]
    [SerializeField]
    private string codeName ="";
    [SerializeField]
    private string description = "";

    [Header("Action")]
    [SerializeField]
    private TaskAction action;

    [Header("Setting")]
    [SerializeField]
    private InitialSuccessValue initialSuccessValue;
    [SerializeField]
    private int needToSuccess = 0;


    public int currentSuccess { get; private set; }
    public string CodeName => codeName;
    public string Description => description;
    public int NeedToSuccess => needToSuccess;

    public void ReceiveReport(int successCnt)
    {
        currentSuccess = action.Run(this,currentSuccess,successCnt);
    }

}
