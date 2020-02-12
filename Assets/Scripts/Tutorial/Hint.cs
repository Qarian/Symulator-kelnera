using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hint : MonoBehaviour
{
    [SerializeField]
    private Image symbol;
    [SerializeField]
    private Color backgroundColor;
    [SerializeField]
    private Color symbolColor;
    [SerializeField]
    private float distanceToTrigger;
    [SerializeField]
    [Range(0, 0.1f)] private float visibilityStep;

    private Image image;
    private Color tempBackgroundColor;
    private Color tempSymbolColor;
    private float currVisibility;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    private void Start()
    {
        tempBackgroundColor = backgroundColor;
        tempBackgroundColor.a = 0;
        image.color = tempBackgroundColor;

        tempSymbolColor = symbolColor;
        tempSymbolColor.a = 0;
        symbol.color = tempSymbolColor;

        currVisibility = 0;
    }

    private void Update()
    {
        if (Vector3.Distance(Camera.main.transform.position, transform.position) < distanceToTrigger)
        {
            currVisibility = Mathf.Lerp(currVisibility, 1, visibilityStep); 
        }
        else
        {
            currVisibility = Mathf.Lerp(currVisibility, 0, visibilityStep);
        }

        tempBackgroundColor.a = currVisibility;
        tempSymbolColor.a = currVisibility;
        image.color = tempBackgroundColor;
        symbol.color = tempSymbolColor;
    }
}
