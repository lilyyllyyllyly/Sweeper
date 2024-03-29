using Godot;
using System;
using System.Collections.Generic;

public class Enemy : Movable
{    
    public Node2D target;
    public Node2D player;

    public NavigationAgent2D navAgent;
    [Export] private NodePath _navAgentPath;

    [Export] private float _knockbackMultiplier;

    [Export] private float _repelForce;
    private List<Area2D> repelAreas = new List<Area2D>();

    private float stun = 0;
    private float lastStun = 0;
    [Export] private int _maxHealth;
    public int hp;
    private int nextDmg;

    [Export] public float stingDmg;

    [Signal] public delegate void EnemyDie(Node2D enemy);
    [Signal] public delegate void EnemyDamaged();

    [Export] private NodePath _animPath;
    private AnimationPlayer _anim;

    public override void _Ready()
    {
        base._Ready();
        hp = _maxHealth;
        Initialize();
	_anim = GetNode<AnimationPlayer>(_animPath);
	_anim.Play("SpiderMove");
    }

    private void Initialize()
    {
        navAgent = GetNode<NavigationAgent2D>(_navAgentPath);
    }

    public override void _Process(float delta)
    {
        stun -= delta;

        SetTarget();
        Repel();
        Collision(GetLastSlideCollision(), delta);
    }

    protected override void Move(float delta)
    {
        base.Move(delta);
        navAgent.SetVelocity(velocity);
    }

    private void SetTarget()
    {
        if (!IsInstanceValid(target) && IsInstanceValid(player)) 
        {
            target = player;
        }
        else if (stun > 0)
        {
            target = null;
        }

	Vector2 targetPos = Position;
	if (IsInstanceValid(target)) {
		targetPos = target.GetNode<Node2D>("FootPos").GlobalPosition;
	}
        navAgent.SetTargetLocation(targetPos);
    }

    private void SlashEntered(Area2D area) 
    {
        velocity = Vector2.Zero;
        Slash slash = area.GetParent<Slash>();
        Vector2 knockback = new Vector2(slash.knockbackForce, 0);
        velocity += Utils.RotateVector(knockback, slash.Rotation) * _knockbackMultiplier;
        stun = slash.stun;
        nextDmg = slash.charge + 1;
    }

    private void Collision(KinematicCollision2D collision, float delta)
    {
        if (collision == null) 
        {
            return;
        }

        if (stun > 0) 
        {
            Damage(nextDmg > 0? nextDmg : 1);
            nextDmg = 0;
        }
    }

    private async void Damage(int dmg)
    {
        hp -= dmg;
        EmitSignal("EnemyDamaged");
	await ToSignal(_anim, "DamageFinished");
	if (hp <= 0) Die();
    }

    private async void Die()
    {
        EmitSignal("EnemyDie", GetNode<Node2D>("."));
        QueueFree();
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
