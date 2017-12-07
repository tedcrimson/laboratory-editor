using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public enum SusbtanceState
{
    SOLID = 0,
    LIQUID,
    GAS
}

[System.Serializable]
public class Substance
{
    public string name;
    public string chemical_name;
    public float boiling_point;
    public SusbtanceState aggregate_state;
}


[System.Serializable]
public class SubstanceData
{
    public List<Substance> Substances;

    public static Substance FromJSON(string json)
    {
        return JsonUtility.FromJson<Substance>(json);
    }
}

public class NodeEditor : EditorWindow
{

    private List<BaseNode> windows = new List<BaseNode>();

    private Vector2 mousePos;
    private BaseNode selectedNode;
    private bool makeTransitionMode = false;

    private SubstanceData substanceData;
    private string log;

    [MenuItem("Chemistry/Node Editor")]
    static void ShowEditor()
    {
        NodeEditor editor = EditorWindow.GetWindow<NodeEditor>();
    }

    /// <summary>
    /// OnGUI is called for rendering and handling GUI events.
    /// This function can be called multiple times per frame (one call per event).
    /// </summary>
    void OnGUI()
    {
        if (substanceData == null)
        {
            if (GUILayout.Button("Load Substances"))
            {
                TextAsset asset = Resources.Load<TextAsset>("Substances");
                if (asset == null)
                    log = "Failed to Load file.";
                else
                {
                    try
                    {
                        substanceData = JsonUtility.FromJson<SubstanceData>(asset.text);
                        log = "Loaded " + substanceData.Substances.Count + " Substance.";
                    }
                    catch (System.Exception ex)
                    {
                        log = "Something wrong in file. Exception " + ex.Message;
                    }

                }
            }
        }

        EditorGUILayout.LabelField(log);
        if (substanceData == null) return;
        if (GUILayout.Button("Save Experiment"))
        {

        }

        Event e = Event.current;

        mousePos = e.mousePosition;

        if (e.button == 1 && !makeTransitionMode)
        {
            if (e.type == EventType.MouseDown)
            {
                bool clickedOnWindow = false;
                int selectIndex = -1;

                for (int i = 0; i < windows.Count; i++)
                {
                    if (windows[i].windowRect.Contains(mousePos))
                    {
                        selectIndex = i;
                        clickedOnWindow = true;
                        break;
                    }
                }

                if (!clickedOnWindow)
                {
                    GenericMenu menu = new GenericMenu();
                    menu.AddItem(new GUIContent("Add Input Node"), false, ContextCallback, "inputNode");
                    menu.AddItem(new GUIContent("Add Output Node"), false, ContextCallback, "outputNode");
                    menu.AddItem(new GUIContent("Add Calculation Node"), false, ContextCallback, "calcNode");
                    menu.AddItem(new GUIContent("Add Storage Equipment"), false, ContextCallback, "storageNode");
                    menu.AddItem(new GUIContent("Add Boiling Equipment"), false, ContextCallback, "boilingNode");
                    menu.AddItem(new GUIContent("Add Cooler Equipment"), false, ContextCallback, "coolerNode");
                    menu.ShowAsContext();
                    e.Use();
                }
                else
                {
                    GenericMenu menu = new GenericMenu();
                    menu.AddItem(new GUIContent("Make Transition"), false, ContextCallback, "makeTransition");
                    menu.AddSeparator("");
                    menu.AddItem(new GUIContent("Delete Node"), false, ContextCallback, "deleteNode");
                    menu.ShowAsContext();
                    e.Use();
                }
            }
        }
        else if (e.button == 0 && e.type == EventType.MouseDown && makeTransitionMode)
        {
            bool clickedOnWindow = false;
            int selectIndex = -1;

            for (int i = 0; i < windows.Count; i++)
            {
                if (windows[i].windowRect.Contains(mousePos))
                {
                    selectIndex = i;
                    clickedOnWindow = true;
                    break;
                }
            }

            if (clickedOnWindow && !windows[selectIndex].Equals(selectedNode))
            {
                windows[selectIndex].SetInput((BaseInputNode)selectedNode, mousePos);
                makeTransitionMode = false;
                selectedNode = null;
            }
            if (!clickedOnWindow)
            {
                makeTransitionMode = false;
                selectedNode = null;
            }
            e.Use();


        }
        else if (e.button == 0 && e.type == EventType.MouseDown)
        {
            bool clickedOnWindow = false;
            int selectIndex = -1;

            for (int i = 0; i < windows.Count; i++)
            {
                if (windows[i].windowRect.Contains(mousePos))
                {
                    selectIndex = i;
                    clickedOnWindow = true;
                    break;
                }
            }

            Debug.LogError("Bitsh");

            if (clickedOnWindow)
            {

                BaseNode nodeTochange = windows[selectIndex];//.ClickedOnInput(mousePos);
                if (nodeTochange != null)
                {
                    selectedNode = nodeTochange;
                    Debug.LogError("clicked" + selectedNode.name);
                    // makeTransitionMode = true;
                }
            }
        }
        if (makeTransitionMode && selectedNode != null)
        {
            Rect mouseRect = new Rect(e.mousePosition.x, e.mousePosition.y, 10, 10);
            DrawNodeCurve(selectedNode.windowRect, mouseRect, Color.magenta);
            Repaint();
        }

        foreach (BaseNode n in windows)
        {
            // Debug.Log(n == selectedNode ? Color.cyan : Color.black);
            n.DrawCurves(n == selectedNode ? Color.cyan : Color.black);
        }

        BeginWindows();
        for (int i = 0; i < windows.Count; i++)
        {
            windows[i].windowRect = GUI.Window(i, windows[i].windowRect, DrawNodeWindow, windows[i].windowTitle);
        }
        EndWindows();

    }

    private void DrawNodeWindow(int id)
    {
        windows[id].DrawWindow();
        GUI.DragWindow();
    }
    private void ContextCallback(object obj)
    {
        string clb = obj.ToString();

        if (clb.Equals("inputNode"))
        {
            SubstanceInput inputNode = new SubstanceInput();
            inputNode.windowRect = new Rect(mousePos.x, mousePos.y, 200, 150);
            windows.Add(inputNode);
        }
        else if (clb.Equals("outputNode"))
        {
            OutputNode outputNode = new OutputNode();
            outputNode.windowRect = new Rect(mousePos.x, mousePos.y, 200, 100);
            windows.Add(outputNode);
        }
        else if (clb.Equals("calcNode"))
        {
            CalcNode calcNode = new CalcNode();
            calcNode.windowRect = new Rect(mousePos.x, mousePos.y, 200, 100);
            windows.Add(calcNode);
        }
        else if (clb.Equals("storageNode"))
        {
            StorageEquipment stor = new StorageEquipment();
            stor.windowRect = new Rect(mousePos.x, mousePos.y, 200, 200);
            windows.Add(stor);
        }
        else if (clb.Equals("boilingNode"))
        {
            BoilingEquipment stor = new BoilingEquipment();
            stor.windowRect = new Rect(mousePos.x, mousePos.y, 200, 200);
            windows.Add(stor);
        }
        else if (clb.Equals("coolerNode"))
        {
            CoolerEquipment cool = new CoolerEquipment();
            cool.windowRect = new Rect(mousePos.x, mousePos.y, 200, 150);
            windows.Add(cool);
        }

        else if (clb.Equals("makeTransition"))
        {
            bool clickedOnWindow = false;
            int selectIndex = -1;

            for (int i = 0; i < windows.Count; i++)
            {
                if (windows[i].windowRect.Contains(mousePos))
                {
                    selectIndex = i;
                    clickedOnWindow = true;
                    break;
                }
            }
            if (clickedOnWindow)
            {
                selectedNode = windows[selectIndex];
                Debug.Log(selectedNode.windowTitle);
                makeTransitionMode = true;
            }
        }
        else if (clb.Equals("deleteNode"))
        {
            bool clickedOnWindow = false;
            int selectIndex = -1;

            for (int i = 0; i < windows.Count; i++)
            {
                if (windows[i].windowRect.Contains(mousePos))
                {
                    selectIndex = i;
                    clickedOnWindow = true;
                    break;
                }
            }
            if (clickedOnWindow)
            {
                BaseNode selNode = windows[selectIndex];
                windows.RemoveAt(selectIndex);
                foreach (BaseNode b in windows)
                {
                    b.NodeDeleted(selNode);
                }
            }
        }
    }


    public static void DrawNodeCurve(Rect start, Rect end, Color col)
    {
        Vector3 startPos = new Vector3(start.x + start.width, start.y + start.height / 2, 0);
        Vector3 endPos = new Vector3(end.x + end.width / 2, end.y + end.height / 2, 0);
        Vector3 startTan = startPos + Vector3.right * 150;
        Vector3 endTan = endPos + Vector3.left * 150;
        Color shadowCol = new Color(0, 0, 0, 0.06f);

        for (int i = 0; i < 3; i++)
        {
            Handles.DrawBezier(startPos, endPos, startTan, endTan, shadowCol, null, (i + 1) * 5);

        }
        Debug.Log(col);

        if (col == Color.black)
            return;

        Handles.DrawBezier(startPos, endPos, startTan, endTan, col, null, 1);

    }
}
