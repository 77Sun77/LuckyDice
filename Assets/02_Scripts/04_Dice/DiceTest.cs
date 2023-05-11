using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceTest : MonoBehaviour
{
    Rigidbody myRIgid;
    bool isHit;

    Vector3 target, dir;
    public List<Vector3> diceRotations = new List<Vector3>();
    public int number;

    public float ranXTorque, ranYTorque, ranZTorque;
    float t = 0;

    bool rotationX;

    public float tte;
    void Start()
    {
        diceRotations.Add(new Vector3(180, 0, 0));
        diceRotations.Add(new Vector3(0, 0, 270));
        diceRotations.Add(new Vector3(270, 0, 0));
        diceRotations.Add(new Vector3(90, 0, 0));
        diceRotations.Add(new Vector3(0, 0, 90));
        diceRotations.Add(new Vector3(0, 0, 0));

        myRIgid = GetComponent<Rigidbody>();

        

        number = Random.Range(0, 6);
        dir = diceRotations[number];
        switch (number)
        {
            case 0:
            case 2:
            case 3:
                rotationX = true;
                break;
        }

        ranXTorque = Random.Range(50, 100);
        ranYTorque = Random.Range(50, 100);
        ranZTorque = Random.Range(50, 100);

        //myRIgid.AddTorque(new Vector3(5, 5, 5));
        //myRIgid.AddForce(Vector3.left * 200f);

        //transform.rotation = Quaternion.Euler(Vector3.Lerp(transform.eulerAngles, dir, tte));
    }

    void Update()
    {
        if (isHit)
        {
            if (rotationX)
            {
                if (t >= 0.9f) return;

                if (dir.x - transform.eulerAngles.x <= 160)
                {
                    target.x = Mathf.Lerp(transform.eulerAngles.x, dir.x, t);
                }
                else
                {
                    float x = -(360 - this.dir.x);
                    target.x = Mathf.Lerp(transform.eulerAngles.x, x, t);
                }
                
                transform.rotation = Quaternion.Euler(target);
                t += Time.deltaTime;
                
            }
            else
            {
                if (t >= 0.9f) return;

                if (dir.z - transform.eulerAngles.z <= 160)
                {
                    target.z = Mathf.Lerp(transform.eulerAngles.z, dir.z, t);
                }
                else
                {
                    float z = -(360 - this.dir.z);
                    target.z = Mathf.Lerp(transform.eulerAngles.z, z, t);
                }
         
                transform.rotation = Quaternion.Euler(target);
                t += Time.deltaTime;
                
            }
            print(t);
        }
        else
        {
            //transform.rotation = Quaternion.Euler((new Vector3(ranXTorque, ranYTorque, ranZTorque)*Time.deltaTime) + transform.eulerAngles);
            
            Vector3 vec = transform.eulerAngles;
            if (rotationX) vec.z = Mathf.Clamp(vec.z, -40, 40);
            else vec.x = Mathf.Clamp(vec.x, -40, 40);
            //transform.rotation = Quaternion.Euler(vec);
        }
        

    }

    private void OnCollisionEnter(Collision coll)
    {
        if (!isHit)
        {
            myRIgid.AddForce(Vector2.up * 100f);
            
            isHit = true;
            target = transform.eulerAngles;
            if (rotationX) target.x = dir.x;
            else target.z = dir.z;
            //transform.rotation = Quaternion.Euler(target);
            
        }
        if (isHit)
        {
            //myRIgid.freezeRotation = false;
        }
        
    }
}
