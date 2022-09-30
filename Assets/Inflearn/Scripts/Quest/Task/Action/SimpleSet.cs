using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Quest/Task/Action/Simple Set", fileName = "Simple Set")]
public class SimpleSet : TaskAction
{
    public override int Run(Task task, int currentSuccess, int successCnt)
    {
        return successCnt;
    }
}
