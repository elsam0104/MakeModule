using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Quest/Task/Target/GameObject", menuName = "Target_")]
public class GameObjectTarget : TaskTarget
{
    [SerializeField]
    private GameObject value;
    public override object Value => value;

    public override bool IsEqual(object target)
    {
        GameObject targetAsGameObj = target as GameObject;
        if (targetAsGameObj == null)
            return false;
        return targetAsGameObj.name.Contains(value.name);
    }
}
