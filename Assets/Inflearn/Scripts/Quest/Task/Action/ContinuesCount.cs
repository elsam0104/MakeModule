using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Quest/Task/Action/ContinuesCount", fileName = "Continues Count")]
public class ContinuesCount : TaskAction
{
    public override int Run(Task task, int currentSuccess, int successCnt)
    {
        return (successCnt>0) ? currentSuccess + successCnt : 0;
    }
}
