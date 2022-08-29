using Godot;
using System;
using System.Collections.Generic;

public class Enemy : KinematicBody2D
{
    [Export] private float _acel;
    [Export] private float _deacel;
    [Export] private float _maxSpeed;
    [Export(PropertyHint.Range, "0,1,0.05")]
    private float _drag;
    
    public Vector2 velocity;
    private Node2D _target;

    [Export] private float _knockbackMultiplier;

    [Export] private float _repelForce;
    private List<Area2D> repelAreas = new List<Area2D>();

    private float stun = 0;
    [Export] private int _maxHealth;
    public int hp;
    private int nextDmg;

    public override void _Ready()
    {
        hp = _maxHealth;
    }

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
        Collision(MoveAndCollide(velocity * delta), delta);
    }

    private void Collision(KinematicCollision2D collision, float delta)
    {
        if (collision == null) 
        {
            return;
        }

        velocity = Vector2.Zero;

        Node2D other = (Node2D)collision.Collider;

        if (other.IsInGroup("Slash")) 
        {
            Slash slash = other.GetNode<Slash>(".");
            Vector2 knockback = new Vector2(slash.knockbackForce, 0);
            velocity += Utils.RotateVector(knockback, slash.Rotation) * _knockbackMultiplier;
            stun = slash.stun;
            nextDmg = slash.charge + 1;
        }
        else if (stun > 0) 
        {
            Damage(nextDmg > 0? nextDmg : 1);
            nextDmg = 0;
        }

        MoveAndSlide(velocity * delta);
    }

    private void Damage(int dmg)
    {
        // This is a function because i'll probably want to make it a signal later to make implementing
        // sound/visual effects and stuff easier when i do that (same for the die function)
        hp -= dmg;
        if (hp <= 0) 
        {
            Die();
        }
    }

    private void Die()
    {
        QueueFree();
    }

    private void CalculateMovement(float delta)
    {
        velocity -= velocity * _drag;

        if (stun > 0) 
        {
            stun -= delta;
            return;
        }

        // Calculating the movement direction
        Vector2 diff = Position - _target.Position;
        Vector2 heading = -diff.Normalized();

        // Calculating the change that will be applied to the velocity
        Vector2 velocityChange = Vector2.Zero;
        velocityChange += heading * _acel;
        Vector2 newVelocity = velocityChange + velocity;
        if (newVelocity.Length() > _maxSpeed) 
        {
            velocityChange -= velocity.Normalized() * _deacel;
        }

        velocity += velocityChange;
    }

    private void Repel() 
    {
        foreach (Area2D area in repelAreas) 
        {
            Vector2 repelDirection = (Position - area.GlobalPosition).Normalized();
            velocity += repelDirection * _repelForce;
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
