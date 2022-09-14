using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SliderScript : MonoBehaviour
{
    private Slider sliderRef;
    public MeshDeformerInput buttonRef;
    public float sliderValue;
    public float sliderValueStep = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        sliderRef = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        ChangeSliderValue(ButtonInputValue());
    }

    private float ButtonInputValue()
    {
        if (buttonRef.isTouchingButton)
        {
             sliderValue += sliderValueStep * Time.deltaTime;
        }
        else  sliderValue -= sliderValueStep * Time.deltaTime;

        return sliderValue = Mathf.Clamp(sliderValue, 0, 1);
    }
    private float ConvertForceToSliderValue()
    {
        return (buttonRef.accumulatedForce / buttonRef.force) / 100;
    }

    private void ChangeSliderValue(float value)
    {
        sliderRef.value = value;
    }

    
}
