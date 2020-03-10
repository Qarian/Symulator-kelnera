using UnityEngine;

public static class ColorScript
{
    private static readonly int Color = Shader.PropertyToID("_Color");

    // Set Color of given GameObject
    public static void SetColor(GameObject go, Color color)
    {
        MaterialPropertyBlock goProperty = new MaterialPropertyBlock();
        Renderer goRenderer = go.GetComponent<Renderer>();
        goRenderer.GetPropertyBlock(goProperty);
        goProperty.SetColor(Color, color);
        goRenderer.SetPropertyBlock(goProperty);
    }
    
    // Set Color of given Renderer
    public static void SetColor(Renderer renderer, Color color)
    {
        MaterialPropertyBlock property = new MaterialPropertyBlock();
        renderer.GetPropertyBlock(property);
        property.SetColor(Color, color);
        renderer.SetPropertyBlock(property);
    }

	// Get Color from GameObject
    public static Color GetColor(GameObject go)
    {
        MaterialPropertyBlock tmp = new MaterialPropertyBlock();
        go.GetComponent<Renderer>().GetPropertyBlock(tmp);
        return tmp.GetColor(Color);
    }
}
