using Godot;
using System;

public class Player : Movable
{
    [Export] private float _kbMultiplier;
    [Export] private float _kbVelocityInfluence;
    
    public AnimationPlayer anim;
    public Sprite sprite;

    [Export] public float maxPoison;
    [Export] private float _posionHeal;
    [Export] private float _maxHealDelay;
    private float _healDelay;
    public float poison;

    [Signal] public delegate void PlayerReady(Player player);
    [Signal] public delegate void PlayerDied();
    [Signal] public delegate void PlayerDamaged();

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

	// Making player not get stuck on walls after gaining too much velocity
	KinematicCollision2D col = GetLastSlideCollision();
	if (col != null &&  col.Collider.IsClass("TileMap")) velocity = velocity.LimitLength(_maxSpeed);
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

    private void OnHit(Area2D area) 
    {
        Enemy enemy = area.GetNode<Enemy>("..");
        if (enemy == null) return;

	Vector2 influence = enemy.velocity * _kbVelocityInfluence;
	velocity += enemy.velocity.Normalized() * _kbMultiplier + influence;
	poison += enemy.stingDmg;
	_healDelay = _maxHealDelay;
	EmitSignal("PlayerDamaged");
	if (poison >= maxPoison) EmitSignal("PlayerDied");
    }
}
