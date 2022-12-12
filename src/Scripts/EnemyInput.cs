using Godot;
using System;

public class EnemyInput : Node, IInput
{
    private Enemy _enemy;

    public override void _Ready()
    {
        _enemy = GetParent<Enemy>();
    }

    public Vector2 direction 
    {
        get 
        {
            if (IsInstanceValid(_enemy.target)) 
            {
                Vector2 diff = _enemy.target.Position - _enemy.Position;
                return diff.Normalized();
            }
            return Vector2.Zero;
        }
    }
}
