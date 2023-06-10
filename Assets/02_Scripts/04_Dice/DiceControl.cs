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

    public void OnClick_Ally_Btn()
    {
        enable = false;
        int number = 0;
        
        if (value < 0.083f)
        {
            number = 1;
        }
        else if (value < 0.249f) // 0.083 ~ 0.249
        {
            number = ValueCalculator(value, 0.249f, 0.083f, 1, 2);
        }
        else if (value < 0.415f) // 0.249 ~ 0.415
        {
            number = ValueCalculator(value, 0.415f, 0.249f, 2, 3);
        }
        else if (value < 0.581f) // 0.415 ~ 0.581
        {
            number = ValueCalculator(value, 0.581f, 0.415f, 3, 4);
        }
        else if (value < 0.747f) // 0.581 ~ 0.747
        {
            number = ValueCalculator(value, 0.747f, 0.581f, 4, 5);
        }
        else if (value < 0.913f) // 0.747 ~ 0.913
        {
            number = ValueCalculator(value, 0.913f, 0.747f, 5, 6);
        }
        else
        {
            number = 6;
        }
        DiceManager.instance.DiceControl(number, DiceRotation.DIceKind.Ally);
        gameObject.SetActive(false);
    }
    public void OnClick_Item_Btn()
    {
        enable = false;
        int number = 0;

        if (value < 0.125f)
        {
            number = 1;
        }
        else if (value < 0.375f) // 0.125 ~ 0.375
        {
            number = ValueCalculator(value, 0.375f, 0.125f, 1, 2);
        }
        else if (value < 0.625f) // 0.375 ~ 0.625
        {
            number = ValueCalculator(value, 0.625f, 0.375f, 2, 3);
        }
        else if (value < 0.875f) // 0.875 ~ 0.625
        {
            number = ValueCalculator(value, 0.875f, 0.625f, 3, 4);
        }
        else
        {
            number = 4;
        }
        DiceManager.instance.DiceControl(number, DiceRotation.DIceKind.Item);
        gameObject.SetActive(false);
    }
    int ValueCalculator(float value, float max, float min, int blink1, int blink2) // value, ÃÖ´ñ°ª, ÃÖ¼Ú°ª, ÃÖ¼Ò ´«²û, ÃÖ´ë ´«²û
    {
        value -= min;
        max -= min;

        float random = Random.Range(0, max);
        if (random <= value)
        {
            return blink2;
        }
        return blink1;
    }
}
