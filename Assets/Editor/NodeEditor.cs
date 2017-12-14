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

    private List<BaseNode> windows;

    private Vector2 mousePos;
    private BaseNode selectedNode;
    private bool makeTransitionMode = false;

    private SubstanceData substanceData;
    private string log;

    private LaboratoryExperiment experiment;

    public Texture2D knobTexture;

    public static NodeEditor Instance;


    [MenuItem("Chemistry/Node Editor")]
    static void ShowEditor()
    {
        NodeEditor editor = EditorWindow.GetWindow<NodeEditor>();
        Instance = editor;
    }


    public string[] SubstanceNames
    {
        get
        {
            string[] strings = new string[substanceData.Substances.Count];
            for (int i = 0; i < substanceData.Substances.Count; i++)
            {
                strings[i] = substanceData.Substances[i].name;
            }

            return strings;
        }
    }
    public Substance GetSubstance(int i)
    {
        return substanceData.Substances[i];
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

        if (GUILayout.Button("New Experiment"))
        {
            Instance = this;
            windows = new List<BaseNode>();
            experiment = null;//new LaboratoryExperiment();
        }
        if (GUILayout.Button("Load Experiment"))
        {
            string absPath = EditorUtility.OpenFilePanel("Select Experiment", "", "");
            if (absPath.StartsWith(Application.dataPath))
            {
                string relPath = absPath.Substring(Application.dataPath.Length - "Assets".Length);
                experiment = AssetDatabase.LoadAssetAtPath(relPath, typeof(LaboratoryExperiment)) as LaboratoryExperiment;
                // if (experiment.stepList == null)
                //     experiment.stepList = new List<StepObject>();
                if (experiment)
                {
                    windows = experiment.nodes;
                    EditorPrefs.SetString("ObjectPath", relPath);
                }
            }

        }
        if (experiment == null && GUILayout.Button("Save Experiment"))
        {

            experiment = ScriptableObject.CreateInstance<LaboratoryExperiment>();
            // List<BaseNode> nodes = new List<BaseNode>();
            CreateAsset<LaboratoryExperiment>("Laboratory", experiment);

            foreach (var w in windows)
            {
                w.name = w.windowTitle;

                AssetDatabase.AddObjectToAsset(w, experiment);
                AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(w));

                // nodes.Add(w);
            }
            experiment.nodes = windows;

            // Reimport the asset after adding an object.
            // Otherwise the change only shows up when saving the project
        }
        else if (experiment != null)
        {
            EditorGUILayout.LabelField("Auto-Save");
        }

        if (windows == null) return;

        Handles.color = Color.gray;
        for (int i = 90; i < position.size.y; i += 30)
        {
            Handles.DrawLine(new Vector3(0, i), new Vector3(position.size.x, i));
        }
        for (int i = 0; i < position.size.x; i += 30)
        {
            Handles.DrawLine(new Vector3(i, 90), new Vector3(i, position.size.y));
        }
        Handles.color = Color.white;

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
                // Debug.Log("Set Output");
                windows[selectIndex].SetInput((BaseNode)selectedNode, mousePos);
                // selectedNode.SetOutput(windows[selectIndex], mousePos);
                // Debug.Log("Set Output");
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
            if (n)
                n.DrawCurves(n == selectedNode ? Color.red : Color.black);
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

    private void AddNode<T>() where T : BaseNode
    {
        T inputNode = ScriptableObject.CreateInstance<T>();
        inputNode.SetWindow(mousePos);
        windows.Add(inputNode);
        if (experiment != null)
        {
            inputNode.name = inputNode.windowTitle;
            AssetDatabase.AddObjectToAsset(inputNode, experiment);
            AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(inputNode));
        }
    }

    private void ContextCallback(object obj)
    {
        string clb = obj.ToString();

        if (clb.Equals("inputNode"))
        {
            AddNode<SubstanceInput>();
        }
        else if (clb.Equals("outputNode"))
        {
            AddNode<OutputNode>();

        }
        else if (clb.Equals("calcNode"))
        {
            AddNode<CalcNode>();

        }
        else if (clb.Equals("storageNode"))
        {
            AddNode<StorageEquipment>();

        }
        else if (clb.Equals("boilingNode"))
        {
            AddNode<BoilingEquipment>();

        }
        else if (clb.Equals("coolerNode"))
        {
            AddNode<CoolerEquipment>();

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
                if (experiment != null)
                {
                    DestroyImmediate(selNode, true);
                    AssetDatabase.SaveAssets();
                }

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
        Color shadowCol = new Color(col.r, col.g, col.b, 0.06f);

        for (int i = 0; i < 3; i++)
        {
            Handles.DrawBezier(startPos, endPos, startTan, endTan, shadowCol, null, (i + 1) * 5);

        }

        if (col == Color.black)
            return;

        Handles.DrawBezier(startPos, endPos, startTan, endTan, col, null, 1);

    }

    private static void CreateAsset<T>(string path, T asset = null) where T : ScriptableObject
    {
        if (asset == null)
            asset = ScriptableObject.CreateInstance<T>();

        if (!AssetDatabase.IsValidFolder(path))
            AssetDatabase.CreateFolder(path, path);

        AssetDatabase.CreateAsset(asset, "Assets/Resources/" + path + Random.Range(0, 100) + ".asset");
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();

        Selection.activeObject = asset;
    }
}
