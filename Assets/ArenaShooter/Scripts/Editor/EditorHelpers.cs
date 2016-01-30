using UnityEngine;
using UnityEditor;
using System.IO;

public class EditorHelpers {

    // create a Scriptable Object asset
    public static T CreateAsset<T>() where T : ScriptableObject {
        T asset = ScriptableObject.CreateInstance<T>();

        string path = AssetDatabase.GetAssetPath(Selection.activeObject);
        if (path == "") {
            path = "Assets";
        } else if (Path.GetExtension(path) != "") {
            path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
        }

        string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/New " + typeof(T).ToString() + ".asset");

        AssetDatabase.CreateAsset(asset, assetPathAndName);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        return asset;
    }

    public static void CreateAssetAndSelect<T>() where T : ScriptableObject {
        T obj = CreateAsset<T>();
        Selection.activeObject = obj;
        EditorUtility.FocusProjectWindow();
    }
}