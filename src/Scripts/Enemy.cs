using Godot;
using System;
using System.Collections.Generic;

public class Enemy : Movable
{    
    public Node2D target;

    [Export] private float _knockbackMultiplier;

    [Export] private float _repelForce;
    private List<Area2D> repelAreas = new List<Area2D>();

    private float stun = 0;
    [Export] private int _maxHealth;
    public int hp;
    private int nextDmg;

    [Export] public float stingDmg;

    [Signal] public delegate void EnemyDie(Node2D enemy);

    public override void _Ready()
    {
        base._Ready();
        hp = _maxHealth;
    }

    public override void _Process(float delta)
    {
        Repel();
        Collision(GetLastSlideCollision(), delta);
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
        EmitSignal("EnemyDie", GetNode<Node2D>("."));
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
