using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Grid))]
public class GridEditor : Editor
{
    protected SerializedObject _serializedObject = null;
    protected SerializedProperty curProperty;
    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
        
        Grid grid = (Grid)target;
        _serializedObject = new UnityEditor.SerializedObject(grid);

        EditorGUILayout.PropertyField(_serializedObject.FindProperty("debDestructableCell"), false);


          
            grid.debugChangeTile = EditorGUILayout.Toggle("Debug change tile", grid.debugChangeTile);



        if (grid.debugChangeTile)
        {
            grid.destructableDebugCell = EditorGUILayout.Toggle("Destructable debug cell", grid.destructableDebugCell);

            EditorGUILayout.Space();

            if (grid.destructableDebugCell)
            {
                DrawField("debDestructableCell");
            }
            else
            {
                DrawField("debCell");
            }
            //Debug.Log(curProperty.name);

        }

        {
            DrawField("gridSize");
            DrawField("occupiedCells");



        }

        if (grid.initializeTileMaps)
        {
            EditorGUILayout.Space();
            DrawField("tilemap");

            EditorGUILayout.Space();
            DrawField("floorTiles");
            DrawField("darknessTiles");
            DrawField("wallTiles");

            EditorGUILayout.Space();
            DrawField("pathTiles");
            DrawField("pathTilesDown");

            EditorGUILayout.Space();
            DrawField("floorSprite");
            DrawField("floorPlankSprite");
            DrawField("darknessSprite");
            DrawField("wallSprite");
            DrawField("wallBlockedSprite");
            DrawField("entranceSprite");
            DrawField("debugSprite");
            DrawField("pathSprite");
            DrawField("pathSpriteRed");
            DrawField("pathSpriteBlue");
            DrawField("pathSpriteFull");
            DrawField("ruinSprite");
            DrawField("boundrySprite");


        }
        _serializedObject.ApplyModifiedProperties();

    }

    protected void DrawField(string propName)
    {
        if (serializedObject != null)
        {
            EditorGUILayout.PropertyField(_serializedObject.FindProperty(propName), true);


        }
    }

}
