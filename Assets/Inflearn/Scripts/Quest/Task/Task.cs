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
    [Header("Setting")]
    [SerializeField]
    private int needToComplete = 0;
}
