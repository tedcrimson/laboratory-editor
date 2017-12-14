using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
[System.Serializable]
public class SubstanceInput : BaseNode
{
    public float amount;
    public float temperature;
    public Substance substance;
    internal int sIndex=0;
    internal string[] names;

    public override void SetWindow(Vector2 mousePos)
    {
        windowTitle = "Input Node";        
        this.windowRect = new Rect(mousePos.x, mousePos.y, 200, 200);
        sIndex = 0;
        names = NodeEditor.Instance.SubstanceNames;
        substance = NodeEditor.Instance.GetSubstance(sIndex);
    }

    public override void DrawWindow()
    {
        base.DrawWindow();

        EditorGUILayout.LabelField("Value");
        int oldI = sIndex;
        sIndex = EditorGUILayout.Popup(sIndex, names);
        if(oldI != sIndex)
            substance = NodeEditor.Instance.GetSubstance(sIndex);
        amount = EditorGUILayout.FloatField("Amount", amount);
        temperature = EditorGUILayout.FloatField("Temperature", temperature);

        EditorGUILayout.LabelField("Chemical Name: " + substance.chemical_name);
        EditorGUILayout.LabelField("Boiling Point: " + substance.boiling_point);
        EditorGUILayout.LabelField("Aggregate State: " + substance.aggregate_state);
        EditorGUILayout.LabelField("Chemical Name: " + substance.chemical_name);

        // windowTitle = names[index];

    }

    // public override void DrawCurves(Color c)
    // {
    //     // if (outputNode)
    //     // {
    //     //     Rect rect = outputNode.windowRect;
    //     //     // rect.x += outputRect.x;
    //     //     rect.y += outputNode.windowRect.height/2;
    //     //     rect.width = 1;
    //     //     rect.height = 1;

    //     //     NodeEditor.DrawNodeCurve(windowRect, rect, c);
    //     // }

    // }


    public override void SetInput(BaseNode inputNode, Vector2 clickPos)
    {
        // throw new NotImplementedException();
        Debug.Log("Set input" + windowTitle + " To : " + inputNode.windowTitle);

    }
}
