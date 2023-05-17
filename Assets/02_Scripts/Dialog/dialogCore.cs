using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "dialogCore", menuName = "ScriptableObject/dialogCore", order = int.MaxValue)]
public class dialogCore : ScriptableObject
{
    public string[] names;
    public int[] talker;
    //[TextArea()]
    public string[] sentences;
}
