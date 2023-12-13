using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class SetController : MonoBehaviour
{   private float temp = 1.0f;
    public TextMeshProUGUI output;
    public TMP_InputField sens;
    public float currSens = MouseLook.sensitivityX;
    [SerializeField] Slider slider;
    public void UpdateSensitivty(){
        if(sens.text == ""){
            MouseLook.sensitivityX = float.Parse(output.text);
            MouseLook.sensitivityY = float.Parse(output.text);
        }
        else if(float.TryParse(sens.text, out temp)) {
        output.text = sens.text;
        slider.value = float.Parse(sens.text)/100;
         MouseLook.sensitivityX = float.Parse(output.text);
        MouseLook.sensitivityY = float.Parse(output.text);
        }
        

    }
}
