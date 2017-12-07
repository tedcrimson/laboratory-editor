using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BaseInputNode : BaseNode
{
    public virtual string getResult(){
        return "None";
    }
    public override void DrawCurves(Color c)
    {
        // throw new System.NotImplementedException();
    }
}
