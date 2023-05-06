using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceTest : MonoBehaviour
{
    public float MaxTorqueScale;
    Rigidbody myRIgid;
    bool isHit;
    Vector3 target;
    public List<Vector3> diceRotations = new List<Vector3>();
    public Transform Dice;
    public int number;
    void Start()
    {
        /*
        foreach(Transform child in Dice)
        {
            diceRotations.Add(child.eulerAngles);
        }
        myRIgid = GetComponent<Rigidbody>();

        float ranXTorque = Random.Range(0f, MaxTorqueScale);
        float ranYTorque = Random.Range(0f, MaxTorqueScale);
        float ranZTorque = Random.Range(0f, MaxTorqueScale);

        myRIgid.AddTorque(new Vector3(ranXTorque, ranYTorque, ranZTorque));
        myRIgid.AddForce(Vector3.left * 200f);
        number = Random.Range(1, 7);

        switch (number)
        {
            case 1: target = new Vector3(180, 0, 0);
                break;
            default:
                target = diceRotations[number - 1];
                break;

        }*/
        myRIgid = GetComponent<Rigidbody>();
        myRIgid.AddForce(Vector3.left * 200f);
    }

    void Update()
    {
        if (!isHit)
        {
            //transform.Rotate(Vector3.left+Vector3.down);
            //print(diceRotations[0]);
            //Vector3 dir = new Vector3(180,0,0) - transform.localEulerAngles;
            //transform.Rotate(dir.normalized * Time.deltaTime*20);
            // print(transform.localEulerAngles);
            // print(Vector3.Distance(transform.localEulerAngles, new Vector3(0, 180, 180)));
            //if(Vector3.Distance(transform.localEulerAngles, new Vector3(180,0,0))<= 10){
            //    isHit = true;
            //}
            transform.Rotate(new Vector3(Random.Range(1, 100), Random.Range(1, 100), Random.Range(1, 100)) * 0.05f, Space.Self);
        }
        
    }

    private void OnCollisionEnter(Collision coll)
    {
        if (!isHit)
        {
            myRIgid.AddForce(Vector2.up * 100f);
            
            isHit = true;
            //  startRotation = transform.rotation;
            //myRIgid.freezeRotation = true;
        }
        
    }
}
