using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CalcNode : SubstanceInput
{

    private BaseNode input1;
    private Rect input1Rect;

    private BaseNode input2;
    private Rect input2Rect;



    private CalculationType calculationType;
    public enum CalculationType
    {
        Add,
        Sub,
        Mul,
        Div
    }

    public override void SetWindow(Vector2 mousePos)
    {
        windowTitle = "CalculationType";
        windowRect = new Rect(mousePos.x, mousePos.y, 200, 100);
        // hasInputs = true;
        // hasOutputs = true;
    }

    // public override void DrawWindow()
    // {
    //     base.DrawWindow();
    //     Event e = Event.current;
    //     calculationType = (CalculationType)EditorGUILayout.EnumPopup("Calculation Type", calculationType);

    //     Substance input1Title = null;

    //     if (input1)
    //     {
    //         input1Title = input1.getResult();
    //     }

    //     GUILayout.Label("Input 1: " + input1Title);

    //     if (e.type == EventType.Repaint)
    //     {
    //         input1Rect = GUILayoutUtility.GetLastRect();
    //     }


    //     Substance input2Title;

    //     if (input2)
    //     {
    //         input2Title = input2.getResult();
    //     }

    //     GUILayout.Label("Input 2: " + input2Title);

    //     if (e.type == EventType.Repaint)
    //     {
    //         input2Rect = GUILayoutUtility.GetLastRect();
    //     }

    //     Substance outputTitle = this.getResult();
    //     // if (output)
    //     // {
    //     // }


    //     GUILayout.Label("Output: " + outputTitle);

    //     // if (e.type == EventType.Repaint)
    //     // {
    //     //     outputRect = GUILayoutUtility.GetLastRect();
    //     // }
    // }

    public override void SetInput(BaseNode inputNode, Vector2 clickPos)
    {
        clickPos.x -= windowRect.x;
        clickPos.y -= windowRect.y;

        if (input1Rect.Contains(clickPos))
            input1 = inputNode;
        else if (input2Rect.Contains(clickPos))
            input2 = inputNode;


    }
    // public override void SetOutput(BaseNode outputNode, Vector2 clickPos)
    // {
    //     base.SetOutput(outputNode, clickPos);

    //     output = outputNode;
    //     // clickPos.x -= windowRect.x;
    //     // clickPos.y -= windowRect.y;

    //     // if(outputRect.Contains(clickPos)){
    //     // }

    // }

    public override void DrawCurves(Color c)
    {
        if (input1 != null)
        {
            Rect rect = windowRect;
            rect.x += input1Rect.x;
            rect.y += input1Rect.y + input2Rect.height / 2;
            rect.width = 1;
            rect.height = 1;

            NodeEditor.DrawNodeCurve(input1.windowRect, rect, c);
        }

        if (input2 != null)
        {
            Rect rect = windowRect;
            rect.x += input1Rect.x;
            rect.y += input1Rect.y + input2Rect.height / 2;
            rect.width = 1;
            rect.height = 1;

            NodeEditor.DrawNodeCurve(input2.windowRect, rect, c);
        }
    }

    // public override string getResult()
    // {
    //     float input1Value = 0;
    //     float input2Value = 0;

    //     if (input1)
    //     {
    //         string input1Raw = input1.getResult();
    //         float.TryParse(input1Raw, out input1Value);
    //     }


    //     if (input2)
    //     {
    //         string input2Raw = input2.getResult();
    //         float.TryParse(input2Raw, out input2Value);
    //     }

    //     string result = "false";

    //     switch (calculationType)
    //     {
    //         case CalculationType.Add:
    //             result = (input1Value + input2Value).ToString();
    //             break;
    //         case CalculationType.Sub:
    //             result = (input1Value - input2Value).ToString();
    //             break;
    //         case CalculationType.Mul:
    //             result = (input1Value * input2Value).ToString();
    //             break;
    //         case CalculationType.Div:
    //             result = (input1Value / input2Value).ToString();
    //             break;
    //     }
	// 	return result;
    // }

	public override BaseNode ClickedOnInput(Vector2 pos){
		BaseNode retVal = null;

		pos.x -= windowRect.x;
		pos.y -= windowRect.y;
		if(input1Rect.Contains(pos)){
			retVal = input1;
			input1 = null;
		}

		if(input2Rect.Contains(pos)){
			retVal = input2;
			input2 = null;
		}
		return retVal;
 	}

	public override void NodeDeleted(BaseNode node){
		if(node.Equals(input1)){
			input1 = null;
		}

		if(node.Equals(input2)){
			input2 = null;
		}
	}
}
