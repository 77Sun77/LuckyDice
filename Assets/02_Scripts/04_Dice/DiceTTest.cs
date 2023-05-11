using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceTTest : MonoBehaviour
{
    Vector3 startPos, dir;

    public List<Vector3> diceRotations = new List<Vector3>();

    public bool rotationX, LeftMove;
    public int number, hitCount;
    void Start()
    {
        diceRotations.Add(new Vector3(180, 0, 0));
        diceRotations.Add(new Vector3(0, 0, 270));
        diceRotations.Add(new Vector3(270, 0, 0));
        diceRotations.Add(new Vector3(90, 0, 0));
        diceRotations.Add(new Vector3(0, 0, 90));
        diceRotations.Add(new Vector3(0, 0, 0));

        number = Random.Range(0, 6);
        number = 0;
        dir = diceRotations[number];
        
        switch (number)
        {
            case 0:
            case 2:
            case 3:
                rotationX = true;
                break;
        }

    }

    void Update()
    {
        if(hitCount == 1)
        {
            if (rotationX)
            {
                print(Quaternion.Euler(dir).x - Quaternion.Euler(transform.eulerAngles).x);
               // if (Mathf.Abs(Quaternion.Euler(dir).x - Quaternion.Euler(transform.eulerAngles).x) <= 10) return;

                if (LeftMove)
                {
                    transform.Rotate(Vector3.left * 100 * Time.deltaTime);
                    
                }
                else
                {
                    transform.Rotate(Vector3.right * 100 * Time.deltaTime);
                }

            }
            else
            {
                if (LeftMove)
                {
                    transform.Rotate(Vector3.back * 100 * Time.deltaTime);
                }
                else
                {
                    transform.Rotate(Vector3.forward * 100 * Time.deltaTime);
                }
            }
            
        }
    }

    private void OnCollisionEnter(Collision coll)
    {
        if(hitCount == 0)
        {
            startPos = transform.eulerAngles;
            if (rotationX)
            {
                float x = dir.x - 180;
                if (transform.eulerAngles.x >= x)
                {
                    LeftMove = true;
                }
            }
            else
            {
                float z = dir.z - 180;
                if (transform.eulerAngles.z >= z)
                {
                    LeftMove = true;
                }
            }
            
            hitCount = 1;
        }
        else if(hitCount == 1)
        {

        }
    }
}
