using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    /// <summary>
    /// Tile�� X,Y�� �Ҵ�
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
