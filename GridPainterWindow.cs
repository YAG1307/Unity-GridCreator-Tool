using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class GridCreator : EditorWindow
{
    [MenuItem("Tools/Level Editor/GridCreator")]
    public static void ShowWindow()
    {
        GetWindow<GridCreator>("GridCreator");
    }

    [System.Serializable]

    public class PaletteItem
    {
        public GameObject prefab;
        [Range(1, 100)] public int weight = 100; //integer slider 
    }
    [SerializeField] private List<PaletteItem> palette = new List<PaletteItem>();
    private float gridSize = 1f;
    private bool painting = false;
    private bool isErasing = false; //Tools

    private bool useColor = false;
    private Color myColor = Color.white; //Color Picker

    private Dictionary<Vector3Int, GameObject> PlacedTiles = new Dictionary<Vector3Int, GameObject>();


    private void OnEnable() => SceneView.duringSceneGui += OnSceneGUI;
    private void OnDisable() => SceneView.duringSceneGui -= OnSceneGUI;

    private void OnGUI()
    {
        GUILayout.Label("Grid Creator Settings", EditorStyles.boldLabel);

        gridSize = EditorGUILayout.FloatField("Grid Size", gridSize);

        ScriptableObject target = this;
        SerializedObject newObject = new SerializedObject(target);
        SerializedProperty ListProp = newObject.FindProperty("palette"); //edit palette
        EditorGUILayout.PropertyField(ListProp, true);

        newObject.ApplyModifiedProperties();

        // Color Settings
        useColor = EditorGUILayout.Toggle("Custom Color", useColor);
        if (useColor)
        {
            myColor = EditorGUILayout.ColorField("Color Tint", myColor);
        }

        EditorGUILayout.Space(10);

        // Buttons
        if (GUILayout.Button(painting ? "STOP PAINTING" : "START PAINTING"))
        {
            painting = !painting;
        }

        isErasing = EditorGUILayout.Toggle("Erase Mode (Or Hold Shift)", isErasing);
    }

    private void OnSceneGUI(SceneView sceneView)
    {
        if (!painting)
        {
            return;
        }
        HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));

        Event e = Event.current;

        Vector3 mouseWorldPosition = HandleUtility.GUIPointToWorldRay(e.mousePosition).origin;

        Vector3Int gridPosition = new Vector3Int(
            Mathf.FloorToInt(mouseWorldPosition.x / gridSize),
            Mathf.FloorToInt(mouseWorldPosition.y / gridSize),
            0
        );
        Vector3 centerPosition = new Vector3(
            gridPosition.x * gridSize + (gridSize * 0.5f),
            gridPosition.y * gridSize + (gridSize * 0.5f),
            0f
        );

        bool eraseMode = isErasing || e.shift;

        Handles.color = eraseMode ? Color.red : Color.cyan;
        Handles.DrawWireCube(centerPosition, new Vector3(gridSize, gridSize, 0f));
        sceneView.Repaint();

        if (e.type == EventType.MouseDown && e.button == 0)
        {
            if (eraseMode)
            {
                EraseTile(gridPosition);
            }
            else
            {
                PaintTile(gridPosition, centerPosition);
            }
            e.Use(); 
        }
    }
    private void PaintTile(Vector3Int gridPosition, Vector3 position)
    {
        GameObject selectedPrefab = GetWeightedRandomPrefab();
        if (selectedPrefab == null) 
        {
            return; 
        }
        EraseTile(gridPosition);
        GameObject newTile = (GameObject)PrefabUtility.InstantiatePrefab(selectedPrefab);
        newTile.transform.position = position;
        if (useColor)
        {
            SpriteRenderer sr = newTile.GetComponent<SpriteRenderer>();
            if (sr != null) sr.color = myColor;
        }
        Undo.RegisterCreatedObjectUndo(newTile, "Paint Grid");
        PlacedTiles[gridPosition] = newTile;
    }

    private void EraseTile(Vector3Int gridPosition)
    {
        if (PlacedTiles.TryGetValue(gridPosition, out GameObject oldTile))
        {
            if (oldTile != null) Undo.DestroyObjectImmediate(oldTile);
            PlacedTiles.Remove(gridPosition);
        }
    }

    private GameObject GetWeightedRandomPrefab()
    {
        int totalWeight = 0;
        foreach (var item in palette)
        {
            if (item.prefab != null && item.weight > 0) totalWeight += item.weight;
        }

        if (totalWeight == 0) return null;

        int roll = Random.Range(0, totalWeight);
        int current = 0;

        foreach (var item in palette)
        {
            if (item.prefab == null || item.weight <= 0) continue;
            current += item.weight;
            if (roll < current) return item.prefab;
        }

        return null;
    }
}
