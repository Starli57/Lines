using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public Action<int> onPressed;
    public Ball ball { get; private set; }

    public void SetPosition(int position)
    {
        _position = position;
    }

    public void SetBall(Ball ball)
    {        
        this.ball = ball;
    }

    public void OnPressed()
    {
        onPressed?.Invoke(_position);
    }

    private int _position;

}
