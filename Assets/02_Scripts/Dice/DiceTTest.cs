using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceTTest : MonoBehaviour
{
    Vector3 startPos, dir;
    Quaternion target;

    public List<Vector3> diceRotations = new List<Vector3>();

    public bool aing;
    public int number, hitCount;

    Rigidbody myRigid;

    float distance;
    void Start()
    {
        diceRotations.Add(new Vector3(180, 0, 0));
        diceRotations.Add(new Vector3(0, 0, 270));
        diceRotations.Add(new Vector3(270, 0, 0));
        diceRotations.Add(new Vector3(90, 0, 0));
        diceRotations.Add(new Vector3(0, 0, 90));
        diceRotations.Add(new Vector3(0, 0, 0));

        number = Random.Range(0, 6);
        //number = 0;
        dir = diceRotations[number];

        transform.eulerAngles = new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));

        myRigid = GetComponent<Rigidbody>();
        myRigid.AddForce(Vector3.left * 300);

    }

    void Update()
    {
        if (hitCount == 0)
        {
            transform.Rotate(new Vector3(Random.Range(20, 50), Random.Range(20, 50), Random.Range(20, 50)) * Time.deltaTime);
        }
        else if (hitCount == 1)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, target, distance);
            distance += Time.deltaTime * 0.2f;

        }
        else
        {
            //transform.ro
            transform.rotation = Quaternion.Lerp(transform.rotation, target, distance);
            distance += Time.deltaTime;
        }
        
    }

    private void OnCollisionEnter(Collision coll)
    {
        if(hitCount == 0)
        {
            startPos = transform.eulerAngles;
            startPos.x = dir.x;
            startPos.z = dir.z;
            target = Quaternion.Euler(Vector3.Lerp(transform.eulerAngles, startPos, 0.8f));
            
            myRigid.AddForce(Vector3.up * 100f);
            myRigid.AddForce(Vector3.left * 50);
            hitCount = 1;
        }
        else if(hitCount == 1)
        {
            target = Quaternion.Euler(startPos);
            distance = 0;
            myRigid.AddForce(Vector3.up * 50f);
            myRigid.AddForce(Vector3.left * 25);
            hitCount = 2;
        }
        else if (hitCount == 2)
        {
            
           // myRigid.freezeRotation = false;
            
            hitCount = 3;
        }
    }
}
