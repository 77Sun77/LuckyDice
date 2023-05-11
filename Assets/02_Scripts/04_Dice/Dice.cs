using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Dice : MonoBehaviour
{
    Vector3 sideGravity;
    public float GravityScale;
    public float MaxTorqueScale;
    public float ReboundScale;
    public float ReBoundTorqueScale;


    public List<DiceFace> diceFaces;

    public const int SumBothSide = 7;
    public int TargetValue;
    public int GroundValue;
    public int DiceValue;

    public readonly int[] LineX = new int[] { 1, 3, 4, 6 };
    public readonly int[] LineY = new int[] { 2, 3, 4, 5 };
    public readonly int[] LineZ = new int[] { 1, 2, 5, 6 };

    public bool IsMovingDone;

    Rigidbody rigid;

    private void Awake()
    {
        sideGravity = new Vector3(0, 0, GravityScale);
        rigid = transform.GetComponent<Rigidbody>();
        AddRandomRotation();
    }

    void AddRandomRotation()
    {
        float ranXTorque = Random.Range(0f, MaxTorqueScale);
        float ranYTorque = Random.Range(0f, MaxTorqueScale);
        float ranZTorque = Random.Range(0f, MaxTorqueScale);

        rigid.AddTorque(new Vector3(ranXTorque, ranYTorque, ranZTorque));
    }

    private void Update()
    {
        IndicateGroundValue();

        rigid.AddForce(sideGravity * Time.deltaTime);

        IsMovingDone = rigid.angularVelocity.magnitude < 1f && rigid.velocity.magnitude < 1f;
        if (IsMovingDone)
            IndicateNum();
    }

    void IndicateGroundValue()
    {
        List<DiceFace> lookGroundFace = new();
        float rayMinimumDistance = 1000;

        for (int i = 0; i < diceFaces.Count; i++)
        {
            if (diceFaces[i].IsLookAtGround) lookGroundFace.Add(diceFaces[i]);
        }

        for (int i = 0; i < lookGroundFace.Count; i++)
        {
            if (rayMinimumDistance > lookGroundFace[i].RayDistance)
            {
                rayMinimumDistance = lookGroundFace[i].RayDistance;
                GroundValue = diceFaces.IndexOf(lookGroundFace[i]) + 1;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log(collision.gameObject.name);

        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            ReBound();
        }
    }

    void ReBound()
    {
        if ((SumBothSide - GroundValue) == TargetValue) return;

        rigid.angularVelocity = Vector3.zero;
        
        Debug.Log("Rebounding");
        rigid.AddForce(Vector3.back * ReboundScale);
        //Debug.Log(transform.rotation);
        Debug.Log(GroundValue);
        if (LineX.Contains(GroundValue))
        {
            float Rot = transform.rotation.x;
            int MultiIndex = (int)(Rot % 90);
            
            if ((Rot - 90 * MultiIndex) > 45) Rot = 90 * (MultiIndex + 1);
            else Rot = 90 * MultiIndex;

            Quaternion newRot = new Quaternion(Rot, 0, 0, 0);
            transform.rotation = newRot;
        }

        if (LineY.Contains(GroundValue))
        {
            float Rot = transform.rotation.y;
            int MultiIndex = (int)(Rot % 90);

            if ((Rot - 90 * MultiIndex) > 45) Rot = 90 * (MultiIndex + 1);
            else Rot = 90 * MultiIndex;

            Quaternion newRot = new Quaternion(0, Rot, 0, 0);
            transform.rotation = newRot;
        }

        if (LineZ.Contains(GroundValue))
        {
            float Rot = transform.rotation.z;
            int MultiIndex = (int)(Rot % 90);

            if ((Rot - 90 * MultiIndex) > 45) Rot = 90 * (MultiIndex + 1);
            else Rot = 90 * MultiIndex;

            Quaternion newRot = new Quaternion(0, 0, Rot, 0);
            transform.rotation = newRot;
        }

        return;
        if (TargetValue == GroundValue)
        {
            //XYZ중 랜덤 값으로 회전
            int ranRotLine = Random.Range(0, 3);
            switch (ranRotLine)
            {
                case 0:
                    rigid.AddTorque(new Vector3(ReBoundTorqueScale, 0, 0));
                    break;
                case 1:
                    rigid.AddTorque(new Vector3(0, ReBoundTorqueScale, 0));
                    break;
                case 2:
                    rigid.AddTorque(new Vector3(0, 0, ReBoundTorqueScale));
                    break;
            }
            return;
        }
       
        if (CheckInSameLine(LineX))
            rigid.AddTorque(new Vector3(ReBoundTorqueScale, 0, 0));

        if (CheckInSameLine(LineY))
            rigid.AddTorque(new Vector3(0, ReBoundTorqueScale, 0));

        if (CheckInSameLine(LineZ))
            rigid.AddTorque(new Vector3(0, 0, ReBoundTorqueScale));
    }

    bool CheckInSameLine(int[] line)
    {
        return line.Contains(TargetValue) && line.Contains(GroundValue);
    }


    void IndicateNum()
    {
        for (int i = 0; i < diceFaces.Count; i++)
        {
            if (diceFaces[i].IsContacting)
            {
                DiceValue = SumBothSide - (i + 1);
            }
        }
    }

}
