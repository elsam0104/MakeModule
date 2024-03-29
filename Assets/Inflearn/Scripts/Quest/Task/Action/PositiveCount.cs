using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Quest/Task/Action/PositiveCount", fileName = "Positive Count")]

public class PositiveCount : TaskAction
{
    public override int Run(Task task, int currentSuccess, int successCnt)
    {
        return (successCnt>0)? currentSuccess + successCnt : currentSuccess;
    }
}
