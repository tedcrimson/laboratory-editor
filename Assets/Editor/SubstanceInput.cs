using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class SubstanceInput : BaseInputNode
{
    private string inputValue = "";

    internal float temperature;

    public SubstanceInput()
    {
        windowTitle = "Input Node";

    }

    public override void DrawWindow()
    {
        base.DrawWindow();

        inputValue = EditorGUILayout.TextField("Value", inputValue);
        temperature = EditorGUILayout.FloatField("Temperature", temperature);


    }

    public override void DrawCurves(Color c)
    {

    }

    public override string getResult()
    {
        return inputValue.ToString();
    }
}
