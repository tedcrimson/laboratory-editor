using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

[System.Serializable]
public class StorageEquipment : SubstanceInput
{
    public List<SubstanceInput> inputs;
    public List<Rect> inputRects;

    public int inputCount;

    public float Volume;

    // private BaseNode input2;
    // private Rect input2Rect;

    public StorageType storageType;

    [System.Serializable]
    public enum StorageType
    {
        Beaker,
        Volumetric_Flask,
        Test_Tube,
        Watch_Glass
    }
    public override void SetWindow(Vector2 mousePos)
    {
        base.SetWindow(mousePos);
        windowTitle = "Storage Flask";
        this.windowRect = new Rect(mousePos.x, mousePos.y, 200, 300);

        // hasInputs = true;
        inputs = new List<SubstanceInput>();
        inputRects = new List<Rect>();
    }

    public override void DrawWindow()
    {
        base.DrawWindow();
        Event e = Event.current;
        storageType = (StorageType)EditorGUILayout.EnumPopup("Storage Type", storageType);

        int old = inputCount;
        inputCount = EditorGUILayout.IntSlider(inputCount, 1, 10);
        if (old != inputCount)
        {
            for (int i = 0; i < Mathf.Max(inputs.Count, inputCount); i++)
            {
                if (i == inputs.Count)
                {
                    inputs.Add(null);
                    inputRects.Add(GUILayoutUtility.GetLastRect());

                    windowRect.height += 20;
                }
                else if (i >= inputCount)
                {
                    inputs.RemoveAt(i);
                    inputRects.RemoveAt(i);
                    windowRect.height -= 20;
                }
            }
        }
        // Substance input;

        // if (this.input)
        // {
        //     input = this.input.getResult();
        // GUILayout.Label("Input : " + input.name);
        // }
        int z = 0;
        float sum=0;
        foreach (var v in inputs)
        {
            string info = " ";
            if(v != null){
                sum += v.amount;
                info += v.substance.aggregate_state + " " + v.amount + " " + v.temperature;
            }
            GUILayout.Label("Input: " + (z+1) + info);
            if(e.type == EventType.Repaint){
                inputRects[z] = GUILayoutUtility.GetLastRect();
                        // Debug.Log(col);
            }
            z++;
        }
        // Debug.Log(inputs.Count +  " " + inputRects.Count);

        Volume = EditorGUILayout.FloatField("Volume: ", Volume);
        amount = sum;
        // Quantity = EditorGUILayout.FloatField("Quantity: ", sum);


    }

    public override void SetInput(BaseNode inputNode, Vector2 clickPos)
    {
        clickPos.x -= windowRect.x;
        clickPos.y -= windowRect.y;

        for (int i = 0; i < inputRects.Count; i++)
        {
            Debug.LogWarning("CHecking " + i);
            if (inputRects[i].Contains(clickPos)){
                inputs[i] = (SubstanceInput)inputNode;
                Debug.Log("input  "+i);
                return;
            }
        }

    }

    public override void DrawCurves(Color c)
    {
        for (int i = 0; i < inputCount; i++)
        {
            if (inputs[i] != null && inputRects[i] != null)
            {
                Rect rect = windowRect;
                rect.x += inputRects[i].x;
                rect.y += inputRects[i].y + inputRects[i].height / 2;
                rect.width = 1;
                rect.height = 1;

                NodeEditor.DrawNodeCurve(inputs[i].windowRect, rect, c);
            }
        }

    }

    // public override Substance getResult()
    // {
    //     // float input1Value = 0;
    //     // float input2Value = 0;

    //     // if (input)
    //     // {
    //     //     Substance input1Raw = input.getResult();
    //     //     float.TryParse(input1Raw, out input1Value);
    //     // }


    //     // string result = "false";

    //     // result = (input1Value + input2Value).ToString();
    //     // // switch (storageType)
    //     // // {
    //     // //     case CalculationType.Add:
    //     // //         break;
    //     // //     case CalculationType.Sub:
    //     // //         result = (input1Value - input2Value).ToString();
    //     // //         break;
    //     // //     case CalculationType.Mul:
    //     // //         result = (input1Value * input2Value).ToString();
    //     // //         break;
    //     // //     case CalculationType.Div:
    //     // //         result = (input1Value / input2Value).ToString();
    //     // //         break;
    //     // // }
    //     // return result;
    //     return null;
    // }

    public override BaseNode ClickedOnInput(Vector2 pos)
    {
        BaseNode retVal = null;

        pos.x -= windowRect.x;
        pos.y -= windowRect.y;
        for (int i = 0; i < inputCount; i++)
        {
            if (inputRects[i].Contains(pos))
            {
                retVal = inputs[i];
                // input = null;
                // inputs.Remove(input);
            }
        }
        return retVal;
    }

    public override void NodeDeleted(BaseNode node)
    {
        for (int i = 0; i < inputCount; i++)
        {
            if(node.Equals(inputs[i])){
                inputs[i] = null;
            }
        }
    }
}
