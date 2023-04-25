using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    /// <summary>
    /// Tile의 X,Y값 할당
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void Initialize_Tile(int x, int y)
    {
        X = x;
        Y = y;
    }

    public int X { get; set; }
    public int Y { get; set; }

   

}
