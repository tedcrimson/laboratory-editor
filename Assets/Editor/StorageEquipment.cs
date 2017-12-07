using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class StorageEquipment : BaseInputNode
{

    protected BaseInputNode input;
    protected Rect inputRect;

	protected float Volume;
	protected float quantity;

    // private BaseInputNode input2;
    // private Rect input2Rect;

    private StorageType storageType;
    public enum StorageType
    {
        Beaker,
		Volumetric_Flask,
		Test_Tube,
		Watch_Glass
    }

    public StorageEquipment()
    {
        windowTitle = "Storage Flask";
        // hasInputs = true;
    }

    public override void DrawWindow()
    {
        base.DrawWindow();
        Event e = Event.current;
        storageType = (StorageType)EditorGUILayout.EnumPopup("Storage Type", storageType);

        string inputTitle = "None";

        if (input)
        {
            inputTitle = input.getResult();
        }

        GUILayout.Label("Input : " + inputTitle);
        Volume = EditorGUILayout.FloatField("Volume: ", Volume);
        EditorGUILayout.FloatField("Quantity: ", quantity);

        if (e.type == EventType.Repaint)
        {
            inputRect = GUILayoutUtility.GetLastRect();
        }


    }

    public override void SetInput(BaseInputNode inputNode, Vector2 clickPos)
    {
        clickPos.x -= windowRect.x;
        clickPos.y -= windowRect.y;

        if (inputRect.Contains(clickPos))
            input = inputNode;

    }

    public override void DrawCurves(Color c)
    {
        if (input)
        {
            Rect rect = windowRect;
            rect.x += inputRect.x;
            rect.y += inputRect.y + inputRect.height / 2;
            rect.width = 1;
            rect.height = 1;

            NodeEditor.DrawNodeCurve(input.windowRect, rect,c);
        }

    }

    public override string getResult()
    {
        float input1Value = 0;
        float input2Value = 0;

        if (input)
        {
            string input1Raw = input.getResult();
            float.TryParse(input1Raw, out input1Value);
        }


        string result = "false";

		result = (input1Value + input2Value).ToString();
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
		return result;
    }

	public override BaseInputNode ClickedOnInput(Vector2 pos){
		BaseInputNode retVal = null;

		pos.x -= windowRect.x;
		pos.y -= windowRect.y;
		if(inputRect.Contains(pos)){
			retVal = input;
			input = null;
		}
		return retVal;
 	}

	public override void NodeDeleted(BaseNode node){
		if(node.Equals(input)){
			input = null;
		}
	}
}
