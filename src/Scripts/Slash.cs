using Godot;
using Godot.Collections;
using System;

public class Slash : KinematicBody2D
{
    [Export] private float _lifetime = 1;
    private float _timePassed = 0;

    public float speed = 600;
    public float deacel = 0;
    public float stun = 0.5f;

    [Export] public int charge = 0;
    [Export] private float[] _chargeSize;
    [Export] private float[] _chargeSpeed;
    [Export] private float[] _chargeDeacel;
    [Export] private float[] _chargeKnockback;
    [Export] private float[] _chargeStun;

    public float knockbackForce = 600;

    public void SetChargeVars()
    {
        Scale = new Vector2(_chargeSize[charge], _chargeSize[charge]);
        speed = _chargeSpeed[charge];
        deacel = _chargeDeacel[charge];
        knockbackForce = _chargeKnockback[charge];
        stun = _chargeStun[charge];
    }

    public override void _Process(float delta)
    {
        _timePassed += delta;

        if (_timePassed + delta >= _lifetime) 
        {
            QueueFree();
        }

        Move(delta);
    }

    private void Move(float delta)
    {
        Vector2 move = new Vector2(speed, 0);
        Vector2 rotatedMove = Utils.RotateVector(move, Rotation);
        Position += rotatedMove * delta;

        speed -= deacel * delta;
        speed = Mathf.Clamp(speed, 0, float.PositiveInfinity);
    }
}
