using UnityEngine;

public static class ColorScript
{
	// Set Color of given GameObject
    public static void SetColor(GameObject go, Color color)
    {
        MaterialPropertyBlock goProperty = new MaterialPropertyBlock();
        Renderer goRenderer = go.GetComponent<Renderer>();
        goRenderer.GetPropertyBlock(goProperty);
        goProperty.SetColor("_Color", color);
        goRenderer.SetPropertyBlock(goProperty);
    }

	// Get Color from GameObject
    public static Color GetColor(GameObject go)
    {
        MaterialPropertyBlock tmp = new MaterialPropertyBlock();
        go.GetComponent<Renderer>().GetPropertyBlock(tmp);
        return tmp.GetColor("_Color");
    }
}
