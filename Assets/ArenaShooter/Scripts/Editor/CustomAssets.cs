using UnityEditor;

// Class for creating menu items for custom Assets
public class CustomAssets {

    [MenuItem("Assets/Create/Weapon Config")]
    public static void NewWeaponConfig() {
        EditorHelpers.CreateAssetAndSelect<WeaponConfig>();
    }
}
