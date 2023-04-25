using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 오브젝트를 X,Y축이 있는 배열형으로 관리하기 위한 클래스입니다
/// </summary>
/// <typeparam name="TGridObject"></typeparam>
public class Grid_XY<TGridObject>
{
    public int X { get; set;}
    public int Y { get; set;}
    public TGridObject[,] GridArray{ get; set;}

    public Grid_XY(int _x, int _y)
    {
        X = _x;
        Y = _y;

        GridArray = new TGridObject[X,Y];
    }


}
