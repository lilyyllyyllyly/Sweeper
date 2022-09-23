using Godot;
using System;

public class Player : KinematicBody2D
{
    [Export] public int speed = 400;
    public bool isMoving = false;
    private Vector2 _otherForces;
    [Export] private float _forcesCap;
    [Export(PropertyHint.Range, "0,1,0.05")] private float _drag;
    [Export] private float _kbMultiplier;
    
    public AnimationPlayer anim;
    public Sprite sprite;

    [Export] public float maxPoison;
    [Export] private float _posionHeal;
    [Export] private float _maxHealDelay;
    private float _healDelay;
    public float poison;

    [Signal] public delegate void PlayerReady(Player player);

    public override void _Ready()
    {
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

    public override void _PhysicsProcess(float delta)
    {
        Move(delta);
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

    private void Move(float delta)
    {
        //this is stupid but i love it
        Vector2 moveDir = new Vector2 (
            x : Convert.ToInt32(Input.IsActionPressed("move_right")) - Convert.ToInt32(Input.IsActionPressed("move_left")),
            y : Convert.ToInt32(Input.IsActionPressed("move_down" )) - Convert.ToInt32(Input.IsActionPressed("move_up"  ))
        );

        isMoving = moveDir.Length() != 0;

        _otherForces.x = Mathf.Clamp(_otherForces.x, -_forcesCap, _forcesCap);
        _otherForces.y = Mathf.Clamp(_otherForces.y, -_forcesCap, _forcesCap);

        MoveAndSlide(moveDir.Normalized() * speed + _otherForces);
        _otherForces -= _otherForces * _drag;
    }

    private void OnHit(Node body) 
    {
        Enemy enemy = body.GetNode<Enemy>(".");
        if (enemy != null) 
        {
            _otherForces += enemy.velocity * _kbMultiplier;
            poison += enemy.stingDmg;
            _healDelay = _maxHealDelay;
            if (poison >= maxPoison) 
            {
                Die();
            }
        }
    }

    private void Die()
    {
        // Just like the enemy die function, this function exists for signals later.
        GetTree().ChangeScene("res://Scenes/Title.tscn");
    }
}
