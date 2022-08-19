using Godot;
using Godot.Collections;
using System;

public class Slash : KinematicBody2D
{
    [Export] private float _lifetime = 1;
    private float _timePassed = 0;

    [Export] public float speed = 600;
    [Export] public float deacel = 0;

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