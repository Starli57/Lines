using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class Ball : MonoBehaviour
{
    public void SetColor(int color)
    {
        _image.color = BallColorsData.instance.GetColor(color);
    }

    public void SetDefaultPosition()
    {
        _rectTransform.anchoredPosition = Vector2.zero;
    }

    private Image _image;
    private RectTransform _rectTransform;

    private void Awake()
    {
        _image = GetComponent<Image>();
        _rectTransform = GetComponent<RectTransform>();
    }
}
