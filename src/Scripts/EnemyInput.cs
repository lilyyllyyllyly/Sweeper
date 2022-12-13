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
            return _enemy.Position.DirectionTo(_enemy.navAgent.GetNextLocation());
        }
    }
}
