using Godot;
using System;

public class PlayerInput : Node, IInput
{
    public Vector2 direction 
    {
        get 
        {
            //this is stupid but i love it
            Vector2 moveDir = new Vector2 (
                x : Convert.ToInt32(Input.IsActionPressed("move_right")) - Convert.ToInt32(Input.IsActionPressed("move_left")),
                y : Convert.ToInt32(Input.IsActionPressed("move_down" )) - Convert.ToInt32(Input.IsActionPressed("move_up"  ))
            );

            moveDir = moveDir.Normalized();
            return moveDir;
        }
    }
}
