using Godot;
using System;
using System.Collections.Generic;

public class Enemy : KinematicBody2D
{
    [Export] private float _acel;
    [Export] private float _deacel;
    [Export] private float _maxSpeed;
    private Vector2 _velocity;
    private Node2D _target;

    [Export] private float _knockbackMultiplier;

    [Export] private float _repelForce;
    private List<Area2D> repelAreas = new List<Area2D>();


    public override void _Process(float delta)
    {
        if (_target != null) 
        {
            CalculateMovement(delta);
        }
        else if (GameManager.instance.player != null)
        {
            _target = GameManager.instance.player;
        }

        Repel();
        Collision(MoveAndCollide(_velocity * delta), delta);
    }

    private void Collision(KinematicCollision2D collision, float delta)
    {
        if (collision == null) 
        {
            return;
        }

        _velocity = Vector2.Zero;

        Node2D other = (Node2D)collision.Collider;

        if (other.IsInGroup("Slash")) 
        {
            Slash slash = other.GetNode<Slash>(".");
            Vector2 knockback = new Vector2(slash.knockbackForce, 0);
            _velocity += Utils.RotateVector(knockback, slash.Rotation) * _knockbackMultiplier;
        }
        MoveAndCollide(_velocity * delta);
    }

    private void CalculateMovement(float delta)
    {
        // Calculating the movement direction
        Vector2 diff = Position - _target.Position;
        Vector2 heading = -diff.Normalized();

        // Calculating the change that will be applied to the velocity
        Vector2 velocityChange = Vector2.Zero;
        velocityChange += heading * _acel;
        Vector2 newVelocity = velocityChange + _velocity;
        if (newVelocity.Length() > _maxSpeed) 
        {
            velocityChange -= _velocity.Normalized() * _deacel;
        }

        _velocity += velocityChange;
    }

    private void Repel() 
    {
        foreach (Area2D area in repelAreas) 
        {
            Vector2 repelDirection = (Position - area.GlobalPosition).Normalized();
            _velocity += repelDirection * _repelForce;
        }
    }

    private void AddToRepel(Area2D area) 
    {
        if (repelAreas.Count < 5) 
        {
            repelAreas.Add(area);
        }
    }

    private void RemoveFromRepel(Area2D area) 
    {
        if (repelAreas.Contains(area)) 
        {
            repelAreas.Remove(area);
        }
    }
}
