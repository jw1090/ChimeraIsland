using UnityEditor;
using UnityEngine;

public class ChimeraToolsMenu
{
    [MenuItem("Chimera Tools/Clear Player Prefs", priority = 50)]
    public static void ClearPlayerPrefs()
    {
        if(EditorUtility.DisplayDialog("Delete Player Prefs", "Are you sure you want to delete all Player Prefs?", "OK"))
        {
            PlayerPrefs.DeleteAll();
        }
    }
}