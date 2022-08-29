using Godot;
using System;

public class Player : KinematicBody2D
{
    [Export] public int speed = 400;
    public bool isMoving = false;
    
    public AnimationPlayer anim;
    public Sprite sprite;

    public override void _Ready()
    {
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

        MoveAndSlide(moveDir.Normalized() * speed);
    }
}
