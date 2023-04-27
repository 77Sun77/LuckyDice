using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������Ʈ�� X,Y���� �ִ� �迭������ �����ϱ� ���� Ŭ�����Դϴ�
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
