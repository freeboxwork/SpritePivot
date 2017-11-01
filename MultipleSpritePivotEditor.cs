using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class MultipleSpritePivotEditor : EditorWindow
{
    TextureImporterSettings TI_Settings = new TextureImporterSettings();
    List<Object> seleceted = new List<Object>();
    List<string> paths = new List<string>();
    List<TextureImporter> TI = new List<TextureImporter>();

    float xValue = 0.5f;
    float yValue = 0.5f;
    bool edidMode = false;

    [MenuItem("Tools/SpritePivot")]
    static void ToolBarWindow()
    {
        MultipleSpritePivotEditor window = (MultipleSpritePivotEditor)EditorWindow.GetWindow(typeof(MultipleSpritePivotEditor));
        window.Show();
    }

    void OnGUI()
    {
        if (!edidMode)
        {
            if (GUILayout.Button("EditPivot"))
            {
                if (Selection.objects.Length > 0)
                {
                    seleceted = Selection.objects.ToList();
                    for (int i = 0; i < seleceted.Count; i++)
                    {
                        string path = AssetDatabase.GetAssetPath(seleceted[i]);
                        paths.Add(path);
                        TI.Add(AssetImporter.GetAtPath(path) as TextureImporter);
                    }
                    for (int i = 0; i < TI.Count; i++)
                    {
                        TI[i].ReadTextureSettings(TI_Settings);
                        TI_Settings.spriteAlignment = (int)SpriteAlignment.Custom;
                        TI[i].SetTextureSettings(TI_Settings);

                    }
                    edidMode = true;
                }
                else
                {
                    EditorUtility.DisplayDialog("Message", "선택된 스프라이트가 없습니다.", "OK");
                }

            }
        }
        if (edidMode)
        {
            xValue = EditorGUILayout.Slider(xValue, -0, 1);
            yValue = EditorGUILayout.Slider(yValue, -0, 1);

            if (GUILayout.Button("Complete"))
            {
                CompleteWork();
            }
           
        }

        if (GUI.changed && edidMode)
        {
            for (int i = 0; i < TI.Count; i++)
            {
                TI[i].spritePivot = new Vector2(xValue, yValue);

            }
        }
    }

    void CompleteWork()
    {
        foreach (var t in paths)
        {
            AssetDatabase.ImportAsset(t, ImportAssetOptions.ForceUpdate);
        }


        seleceted.Clear();
        paths.Clear();
        paths.Clear();
        edidMode = false;
    }
}
