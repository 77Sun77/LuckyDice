using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class DamageCount : MonoBehaviour
{
    TextMeshPro tmp;
    public float damage;
    float posNumber, alphaNumber;
    Vector3 startPos,endPos;

    void Start()
    {
        tmp = GetComponent<TextMeshPro>();
        
        tmp.text = ((int)damage).ToString();
        startPos = transform.position;
        endPos = transform.position + (Vector3.up*0.5f);
        alphaNumber = 1;
    }

    
    void Update()
    {
        
        transform.position = Vector2.Lerp(startPos, endPos, posNumber);
        posNumber += Time.deltaTime * 10;

        if(posNumber > 10)
        {
            tmp.alpha = alphaNumber;
            alphaNumber -= Time.deltaTime;
        }
        if (alphaNumber < -1) Destroy(gameObject);
    }
}
