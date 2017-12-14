using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public abstract class BaseNode : ScriptableObject
{
    public Rect windowRect;
    // public List<BaseNode> InputNodes;
    // public BaseNode outputNode;

    public string windowTitle = "";

    // public BaseNode(){}

    public abstract void SetWindow(Vector2 p);

    public virtual void DrawWindow()
    {
        windowTitle = EditorGUILayout.TextField("Title", windowTitle);
    }

    public virtual void DrawCurves(Color c){
        // if (outputNode)
        // {
        //     Rect rect = outputNode.windowRect;
        //     // rect.x += outputRect.x;
        //     rect.y += outputNode.windowRect.height/2;
        //     rect.width = 1;
        //     rect.height = 1;

        //     // if(c == Color.black)
        //     //     c = Color.red;

        //     NodeEditor.DrawNodeCurve(windowRect, rect, c);
        // }
    }

    public abstract void SetInput(BaseNode inputNode, Vector2 clickPos);

    // public virtual void SetOutput(BaseNode outPutNode, Vector2 clickPos)
    // {
    //     Debug.Log("Set " + windowTitle + " To : " + outPutNode.windowTitle);
        
    //     this.outputNode = outPutNode;
    // }

    public virtual void NodeDeleted(BaseNode node)
    {
        // if(node.Equals(outputNode)){
		// 	outputNode = null;
		// }
    }

    public virtual BaseNode ClickedOnInput(Vector2 pos)
    {
		return null;
    }

    public virtual Substance getResult(){
        return null;
    }
}
