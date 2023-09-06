using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DiceControl : MonoBehaviour
{
    public Image fillImage;

    float value;
    public float Value_A, Value_B;

    public bool valueUp, enable , IsRollingDice;
    public GameObject temp;
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
            #region
            //fillImage.fillAmount = value;
            //if (valueUp)
            //{
            //    value += Time.deltaTime * 1;
            //    if (value >= 1)
            //    {
            //        value = 1;
            //        valueUp = false;
            //    }
            //}
            //else
            //{
            //    value -= Time.deltaTime * 1;
            //    if (value <= 0)
            //    {
            //        value = 0;
            //        valueUp = true;
            //    }
            //}
            #endregion

            if (valueUp)
            {
                value += (Value_A + value / Value_B) * Time.deltaTime;
                if (value >= 1)
                {
                    value = 1;
                    valueUp = false;
                }
            }
            else
            {
                value -= (Value_A + value / Value_B) * Time.deltaTime;
                if (value <= 0)
                {
                    value = 0;
                    valueUp = true;
                }
            }

            fillImage.fillAmount = value;
        }

        if (Input.GetKeyDown(KeyCode.Space)) OnClick_Ally_Btn();
    }

    public void OnClick_Ally_Btn()
    {
        if (DiceManager.instance.TheNumberOfDice <= 0 || IsRollingDice) return;

        enable = false;
        IsRollingDice = true;
        int number = 0;

        if (value < 0.1428571f)
        {
            number = 1;
        }
        else if (value < 0.2857142f) // 0.1428571 ~ 0.2857142
        {
            number = ValueCalculator(value, 0.2857142f, 0.1428571f, 1, 2);
        }
        else if (value < 0.4285713f) // 0.2857142f ~ 0.4285713
        {
            number = ValueCalculator(value, 0.4285713f, 0.2857142f, 2, 3);
        }
        else if (value < 0.5714284f) // 0.4285713 ~ 0.5714284
        {
            number = ValueCalculator(value, 0.5714284f, 0.4285713f, 3, 4);
        }
        else if (value < 0.7142855f) // 0.5714284 ~ 0.7142855
        {
            number = ValueCalculator(value, 0.7142855f, 0.5714284f, 4, 5);
        }
        else if (value < 0.8571426f) // 0.7142855 ~ 0.8571426
        {
            number = ValueCalculator(value, 0.8571426f, 0.7142855f, 5, 6);
        }
        else
        {
            number = 6;
        }
        DiceManager.instance.DiceControl(number, DiceRotation.DIceKind.Ally);
        GameManager.instance.dice_Inventory.Delete_Inventory(temp);
        Debug.Log("DiceNumber is " + number);
        //gameObject.SetActive(false);
    }
    public void OnClick_Item_Btn()
    {
        enable = false;
        int number = 0;

        if (value < 0.2f)
        {
            number = 1;
        }
        else if (value < 0.4f) // 0.2 ~ 0.4
        {
            number = ValueCalculator(value, 0.4f, 0.2f, 1, 2);
        }
        else if (value < 0.6f) // 0.4 ~ 0.6
        {
            number = ValueCalculator(value, 0.6f, 0.4f, 2, 3);
        }
        else if (value < 0.8f) // 0.6 ~ 0.8
        {
            number = ValueCalculator(value, 0.8f, 0.6f, 3, 4);
        }
        else
        {
            number = 4;
        }
        DiceManager.instance.DiceControl(number, DiceRotation.DIceKind.Item);
        GameManager.instance.dice_Inventory.Delete_Inventory(temp);
        print("Value : " + value + ", Number : " + number);
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
