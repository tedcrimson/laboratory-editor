using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class OutputNode : BaseNode {

	private SubstanceInput inputNode;
	private Rect inputNodeRect;

	public override void SetWindow(Vector2 mousePos){
		// base.SetWindow(mousePos);
		windowTitle = "Output Node";
		windowRect = new Rect(mousePos.x, mousePos.y, 200, 200);

		// hasInputs = true;
	}

	public override void DrawWindow(){
		// base.DrawWindow();

		GUILayout.Label("Result:");
		Event e = Event.current;

		string inputTitle = "" ;
		if(inputNode != null)
		{
			inputTitle = inputNode.substance.name;
			EditorGUILayout.FloatField("Amount", inputNode.amount);
			EditorGUILayout.FloatField("Temperature", inputNode.temperature);

		}
		GUILayout.Label("Input: "+ inputTitle);


		if(e.type == EventType.Repaint)
		{
			inputNodeRect = GUILayoutUtility.GetLastRect();
		}

		
	}

	public override void DrawCurves(Color c)
	{
		if(inputNode != null){
			Rect rect = windowRect;
			rect.x += inputNodeRect.x;
			rect.y += inputNodeRect.y + inputNodeRect.height/2;
			rect.width = 1;
			rect.height = 1;

			NodeEditor.DrawNodeCurve(inputNode.windowRect, rect, c);
		}
	}
	public override void NodeDeleted(BaseNode node){
		if(node.Equals(inputNode)){
			inputNode = null;
		}
	}

	public override BaseNode ClickedOnInput(Vector2 pos){
		BaseNode retVal = null;
		pos.x -= windowRect.x;
		pos.y -= windowRect.y;

		if(inputNodeRect.Contains(pos)){
			retVal = inputNode;
			inputNode = null;
		}
		return retVal;
	}

	public override void SetInput(BaseNode inputNode, Vector2 clickPos){

		clickPos.x -= windowRect.x;
		clickPos.y -= windowRect.y;

		if(inputNodeRect.Contains(clickPos)){
		Debug.Log("Set input" + windowTitle + " To : " + inputNode.windowTitle);
			this.inputNode = (SubstanceInput)inputNode;
		}
	}
}
