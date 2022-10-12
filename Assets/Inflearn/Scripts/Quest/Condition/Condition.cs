using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//[CreateAssetMenu(menuName = "", fileName = "")]

public abstract class Condition : ScriptableObject
{
    [SerializeField]
    private string description;
    public string Description => description;
    public abstract bool InPass(Quest quest);
}
