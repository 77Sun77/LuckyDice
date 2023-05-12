using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DIDIce : MonoBehaviour
{
    public float MaxTorqueScale;
    Rigidbody rigid;

    public int count;

    float ranXTorque, ranYTorque, ranZTorque, dirX, dirY, dirZ;

    public bool isSimulated = true;
    public void SetDice(int count, float x, float y, float z)
    { 
        if(count == 0) isSimulated = true;
        else
        {
            isSimulated = false;
            gameObject.SetActive(false);
        }
        ranXTorque = x;
        ranYTorque = y;
        ranZTorque = z;

        
    }
    private void Start()
    {       
        rigid = transform.GetComponent<Rigidbody>();
        rigid.AddForce(Vector3.left * 200);
        AddRandomRotation();
        if (isSimulated)
        {
            
            Physics.autoSimulation = false;

            for (int i = 0; i < 600; i++)
            {
                if (!Physics.autoSimulation)
                    Physics.Simulate(Time.fixedDeltaTime);


            }
        }
    }
    void AddRandomRotation()
    {

        rigid.AddTorque(new Vector3(ranXTorque, ranYTorque, ranZTorque));
    }

    void Update()
    {
        
        if (Vector3.Magnitude(rigid.velocity) < 0.1f)
        {
            //gameObject.SetActive(false);
            Physics.autoSimulation = true;
            
        }

        
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(count == 0)
        {
            count++;
            if (isSimulated)
            {
                //rigid.velocity = Vector3.zero;
                //rigid.AddForce(Vector3.up * 100f);
                
            }
            
            //Destroy(gameObject);
        }
        else if(count == 1 && isSimulated)
        {
          //  Physics.autoSimulation = true;
            print("??");
        }
    }
}
