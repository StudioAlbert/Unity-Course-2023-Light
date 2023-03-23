using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorLerping : MonoBehaviour
{

    [SerializeField] private Color _start;
    [SerializeField] private Color _end;
    
    [SerializeField]
    [Range(0.0f,1.0f)]
    private float _ratio;

    [SerializeField] private Image _image;

    // Update is called once per frame
    void Update()
    {
        _image.color = Color.Lerp(_start, _end, _ratio);
    }
}
