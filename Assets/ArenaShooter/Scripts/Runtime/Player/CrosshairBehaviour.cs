using UnityEngine;
using System.Collections;

[AddComponentMenu("Scripts/Player/Crosshair Behaviour")]
public class CrosshairBehaviour : MonoBehaviour {

    // settings
    public Texture crosshair;
    public Color crosshairColor = Color.white;
    public float crosshairSize = 0.05f;

    void OnGUI() {
        float size = Screen.height * crosshairSize;
        GUI.color = crosshairColor;
        GUI.DrawTexture(new Rect(
            Screen.width / 2f - size / 2f,
            Screen.height / 2f - size / 2f,
            size,
            size), crosshair);
    }
}
