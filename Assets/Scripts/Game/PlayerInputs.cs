using System.Collections.Generic;
using UnityEngine;

public class Axis
{
    public string up = "z";
    public string down = "s";
    float _elapsedTime = 0;
    string _lastKeyPress;

    public Axis(PlayerSide side)
    {
        if (side == PlayerSide.RIGHT) {
            up = "up";
            down = "down";
        } else {
            up = "z";
            down = "s";
        }
    }

    public float GetAxis()
    {
        if (Input.GetKey(up)) {
            if (_lastKeyPress != up) {
                _elapsedTime = 0;
                _lastKeyPress = up;
            }
            if (_elapsedTime < 1f && ((_elapsedTime += (Time.deltaTime * 1.5f)) < 1f)) {
                _elapsedTime += Time.deltaTime * 1.5f;
            }
            return _elapsedTime;
        } else if (Input.GetKey(down)) {
            if (_lastKeyPress != down) {
                _elapsedTime = 0;
                _lastKeyPress = down;
            }
            if (_elapsedTime < 1f && ((_elapsedTime += (Time.deltaTime * 1.5f)) < 1f)) {
                _elapsedTime += Time.deltaTime * 1.5f;
            }
            return -1f * _elapsedTime;
        }
        _elapsedTime = 0;
        return 0f;
    }
}

public static class PlayerInputs
{
    public static Dictionary<PlayerSide, Axis> _axis = new Dictionary<PlayerSide, Axis>()
    {
        {PlayerSide.RIGHT, new Axis(PlayerSide.RIGHT)},
        {PlayerSide.LEFT, new Axis(PlayerSide.LEFT)},
    };

    public static float GetAxis(PlayerSide side)
    {
        return _axis[side].GetAxis();
    }

    public static void ModifyUpKey(PlayerSide side, string key)
    {
        _axis[side].up = key;
    }

    public static void ModifyDownKey(PlayerSide side, string key)
    {
        _axis[side].down = key;
    }
}