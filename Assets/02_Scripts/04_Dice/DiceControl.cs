using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DiceControl : MonoBehaviour
{
    public Image fillImage;

    float value;

    bool valueUp, enable;
    void OnEnable()
    {
        value = 0;
        valueUp = true;
        enable = true;
    }

    
    void Update()
    {
        if (enable)
        {
            fillImage.fillAmount = value;
            if (valueUp)
            {
                value += Time.deltaTime * 1;
                if (value >= 1)
                {
                    value = 1;
                    valueUp = false;
                }
            }
            else
            {
                value -= Time.deltaTime * 1;
                if (value <= 0)
                {
                    value = 0;
                    valueUp = true;
                }
            }
        }
        
    }

    public void OnClick_Btn()
    {
        enable = false;
        int number = 0;
        if (value <= 0.166f)
        {
            number = 1;
        }
        else if(value <= 0.166f*2)
        {
            number = 2;
        }
        else if (value <= 0.166f * 3)
        {
            number = 3;
        }
        else if (value <= 0.166f * 4)
        {
            number = 4;
        }
        else if (value <= 0.166f * 5)
        {
            number = 5;
        }
        else
        {
            number = 6;
        }
        DiceManager.instance.DiceControl(number);
        //gameObject.SetActive(false);
        
    }
}
