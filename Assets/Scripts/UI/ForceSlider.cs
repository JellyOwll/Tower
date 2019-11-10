using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ForceSlider : MonoBehaviour
{

    [SerializeField]
    protected Gradient color;

    [SerializeField]
    protected Image imageSlider;
    protected Slider slider;

    [SerializeField, Range(0, 1)]
    protected float value;

    public float Value
    {
        get => value;
        set
        {
            this.value = value;
            ValueChanged(this.value);
        }
    }

    private void ValueChanged(float value)
    {
        slider.value = value;
        imageSlider.color = color.Evaluate(value * 100);
    }

    private void OnValidate()
    {
        slider = GetComponent<Slider>();
        slider.value = value;
        imageSlider.color = color.Evaluate(value);
    }

    void Start()
    {
        slider = GetComponent<Slider>();
        Player.ForceChanged += Player_ForceChanged;
    }

    private void Player_ForceChanged(Player sender)
    {
        Value = sender.Force / sender.ForceMax;
    }
}
