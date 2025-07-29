using UnityEngine;
using UnityEngine.UI;

public class WheelSegment : MonoBehaviour
{
    [Header("Segment Components")]
    public Image segmentImage;
    
    private Color segmentColor;
    
    void Awake()
    {
        if (segmentImage == null)
        {
            segmentImage = GetComponent<Image>();
        }
    }
    
    public void SetColor(Color color)
    {
        segmentColor = color;
        if (segmentImage != null)
        {
            segmentImage.color = color;
        }
    }
    
    public Color GetColor()
    {
        return segmentColor;
    }
}