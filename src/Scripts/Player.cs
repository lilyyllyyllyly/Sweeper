using Godot;
using System;

public class Player : Movable
{
    [Export] private float _kbMultiplier;
    
    public AnimationPlayer anim;
    public Sprite sprite;

    [Export] public float maxPoison;
    [Export] private float _posionHeal;
    [Export] private float _maxHealDelay;
    private float _healDelay;
    public float poison;

    [Signal] public delegate void PlayerReady(Player player);
    [Signal] public delegate void PlayerDied();

    public override void _Ready()
    {
        base._Ready();
        EmitSignal("PlayerReady", this);
        Initialize();
    }

    private void Initialize()
    {
        anim = GetNode<AnimationPlayer>("Sprite/AnimationPlayer");
        sprite = GetNode<Sprite>("Sprite");
    }

    public override void _Process(float delta)
    {
        Animate();
        Heal(delta);
    }

    private void Heal(float delta)
    {
        poison = Mathf.Clamp(poison, 0, maxPoison);
        if (_healDelay > 0) 
        {
            _healDelay -= delta;
            return;
        }
        poison -= _posionHeal * delta;
    }

    private void Animate()
    {
        sprite.FlipH = GetGlobalMousePosition().x < Position.x;

        if (isMoving) 
        {
            anim.Play("Walk");
        }
        else 
        {
            anim.Play("Idle");
        }
    }

    private void OnHit(Node body) 
    {
        Enemy enemy = body.GetNode<Enemy>(".");
        if (enemy != null) 
        {
            velocity += enemy.velocity * _kbMultiplier;
            poison += enemy.stingDmg;
            _healDelay = _maxHealDelay;
            if (poison >= maxPoison) 
            {
                EmitSignal("PlayerDied");
            }
        }
    }
}
