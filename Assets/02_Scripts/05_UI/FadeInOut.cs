using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class FadeInOut : MonoBehaviour
{
    public CanvasGroup cg;
    
    public void StartFade(GameObject go)
    {
        transform.Find(go.name).gameObject.SetActive(true);
        StartCoroutine(Fade());
    }

    IEnumerator Fade()
    {
        cg.alpha = 0;
        while (cg.alpha < 1)
        {
            yield return new WaitForFixedUpdate();
            cg.alpha += Time.deltaTime;
        }
        cg.alpha = 1;
        yield return new WaitForSeconds(2);
        
        while (cg.alpha > 0)
        {
            yield return new WaitForFixedUpdate();
            cg.alpha -= Time.deltaTime;
        }
        cg.alpha = 0;
        foreach (Transform child in transform) child.gameObject.SetActive(false);
    }
}
