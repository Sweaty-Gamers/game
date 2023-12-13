using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class SliderController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI sliderText = null;
    [SerializeField] private float maxSliderAmount = 100.0f;
    public void sliderChange(float value){
        float localValue = value*maxSliderAmount;
        sliderText.text = localValue.ToString("0.0");
    }
    void Start(){
        sliderText.text = MouseLook.sensitivityX.ToString("0.0");
    }
}
