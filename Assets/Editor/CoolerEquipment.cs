using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class CoolerEquipment : StorageEquipment
{
    protected SubstanceInput coolerInput;
    private Rect coolerInputRect;
    // private BoilingType boilingType;
    // public enum BoilingType
    // {
    //     mrgvali,
    //     samkutxedi
    // }

    public CoolerEquipment()
    {
        windowTitle = "Cooler";
        // hasInputs = true;
    }

    public override void DrawWindow()
    {
        base.DrawWindow();
        string inputTitle = "None";

        if (coolerInput)
        {
            inputTitle = coolerInput.getResult();
        }

        GUILayout.Label("Cooler Input : " + CalculateTemperature());
        Event e = Event.current;
        if (e.type == EventType.Repaint)
        {
            coolerInputRect = GUILayoutUtility.GetLastRect();
        }

    }

    private string CalculateTemperature()
    {
        float input1Value = 0;
        string result = "";

        if (input && coolerInput)
        {
            string input1Raw = input.getResult();


            float.TryParse(input1Raw, out input1Value);
            input1Value += 10;
            // coolerInput.temperature -= 5;
            result = ""+(coolerInput.temperature + input1Value);
        }

        return result;
    }

    public override string getResult()
    {
        float input1Value = 0;

        if (input && coolerInput)
        {
            string input1Raw = input.getResult();


            float.TryParse(input1Raw, out input1Value);
            input1Value += 10;
            // coolerInput.temperature -= 5;
        }

        // switch (storageType)
        // {
        //     case CalculationType.Add:
        //         break;
        //     case CalculationType.Sub:
        //         result = (input1Value - input2Value).ToString();
        //         break;
        //     case CalculationType.Mul:
        //         result = (input1Value * input2Value).ToString();
        //         break;
        //     case CalculationType.Div:
        //         result = (input1Value / input2Value).ToString();
        //         break;
        // }
		return input1Value.ToString();
    }

    public override void DrawCurves(Color c){
        // base.DrawCurves();
        if (coolerInput)
        {
            Rect rect = windowRect;
            rect.x += inputRect.x;
            rect.y += inputRect.y + inputRect.height / 2;
            rect.width = 1;
            rect.height = 1;

            NodeEditor.DrawNodeCurve(coolerInput.windowRect, rect, c);
        }
    }

    public override void SetInput(BaseInputNode inputNode, Vector2 clickPos)
    {
        if(!input)
            base.SetInput(inputNode, clickPos);
        else{
        clickPos.x -= windowRect.x;
        clickPos.y -= windowRect.y;

        if (coolerInputRect.Contains(clickPos))
            coolerInput = (SubstanceInput) inputNode;
        }

    }


	public override BaseInputNode ClickedOnInput(Vector2 pos){
        BaseInputNode retVal = base.ClickedOnInput(pos);

		pos.x -= windowRect.x;
		pos.y -= windowRect.y;


		if(coolerInputRect.Contains(pos)){
			retVal = coolerInput;
			coolerInput = null;
		}
		return retVal;
 	}


}
