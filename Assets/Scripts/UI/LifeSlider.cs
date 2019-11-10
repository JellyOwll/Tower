using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeSlider : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    protected Gradient color;

    [SerializeField]
    protected Image imageSlider;
    protected Slider slider;

    [SerializeField, Range(0,1)]
    protected float value;

    public float Value { 
        get => value; 
        set 
        {
            this.value = value;
            ValueChanged(this.value);
        }
    }

    void Start()
    {
        slider = GetComponent<Slider>();
    }

    private void ValueChanged(float value)
    {
        slider.value = value;
        imageSlider.color = color.Evaluate(value*100);
    }

    private void OnValidate()
    {
        slider = GetComponent<Slider>();
        slider.value = value;
        imageSlider.color = color.Evaluate(value);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
