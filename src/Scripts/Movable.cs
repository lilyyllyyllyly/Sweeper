using Godot;
using System;

public class Movable : KinematicBody2D
{
    private IInput _input;
    [Export] private NodePath _inputPath;

    [Export] private float _maxSpeed;
    [Export] private float _maxAccel;
    public Vector2 velocity;
    public bool isMoving;

    public override void _Ready() 
    {
        _input = GetNode<IInput>(_inputPath);
    }

    public override void _PhysicsProcess(float delta)
    {
        Move(delta);
    }

    protected virtual void Move(float delta)
    {
        if (!IsInstanceValid((Node)_input)) 
        {
            return;
        }

        isMoving = _input.direction.Length() > 0;

        Vector2 goalSpeed = _input.direction * _maxSpeed;
        Vector2 neededAccel = goalSpeed - velocity;

        if (neededAccel.Length() > _maxAccel) {
            neededAccel = neededAccel.Normalized() * _maxAccel;
        }

        velocity += neededAccel;
        MoveAndSlide(velocity);
    }
}
