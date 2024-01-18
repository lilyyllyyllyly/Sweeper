using Godot;
using System;

public class Broom : Node2D
{
    [Export] private float _rotationOffset; // In degrees

    private AnimationPlayer _anim;
    private bool _isFlipped = false;

    [Export] private NodePath _targetPath;
    private Node2D _target;

    private float _charge = 0;

    [Export] private PackedScene _slashScene;
    [Export] private Vector2 slashPos; // Relative to broom's position

    [Export] private float _timeToMedCharge;
    [Export] private float _timeToFullCharge;

    [Export] private float _attackDelay;
    private float _timeSinceAttack;

    [Signal] public delegate void Swept(int intensity);

    public override void _Ready()
    {
        Initialize();
    }

    private void Initialize()
    {
        _anim = GetNode<AnimationPlayer>("Sprite/AnimationPlayer");
        _target = GetNode<Node2D>(_targetPath);
        _timeSinceAttack = _attackDelay;
    }

    public override void _Process(float delta)
    {
        Rotation = Utils.AngleBetweenPoints(Position, GetGlobalMousePosition(), _rotationOffset);
        ChangeOrder();
        Attack(delta);
        FollowTarget();
    }

    private void FollowTarget()
    {
        Position = _target.Position;
    }

    private void Attack(float delta)
    {
        _timeSinceAttack += delta;
        Charge(delta);
        if (!Input.IsActionJustReleased("attack") || _timeSinceAttack < _attackDelay) 
        {
            return;
        }

        // All of this only happens when the attack button is released and the attack delay passed:
        _timeSinceAttack = 0;

	// Playing correct animation for flip state
        if (!_isFlipped) 
        {
            _anim.Play("Attack");
        }
        else 
        {
            _anim.Play("AttackBack");
        }

	// Getting charge level
	int slashCharge = 0;
        if (_charge > _timeToMedCharge) 
        {
            slashCharge = 1;
        }
        if (_charge > _timeToFullCharge) 
        {
            slashCharge = 2;
        }

        SpawnSlash(slashCharge);
	EmitSignal("Swept", slashCharge+1);

        _isFlipped = !_isFlipped; // Flipping broom after attacking
        _charge = 0;
    }

    private void SpawnSlash(int charge)
    {
        // Instancing

        Node newSlash = _slashScene.Instance();
        Node2D slashNode = newSlash.GetNode<Node2D>(".");
        Slash slash = newSlash.GetNode<Slash>(".");
	slash.charge = charge;

        // Rotating and Repositioning

        slashNode.Rotation = Utils.AngleBetweenPoints(Position, GetGlobalMousePosition(), 180);

        float rot = Rotation + Mathf.Deg2Rad(_rotationOffset);
        Vector2 rotatedPos = Utils.RotateVector(slashPos, rot);
        slashNode.Position = Position + rotatedPos;


        slash.SetChargeVars();
        

        GetTree().CurrentScene.AddChild(newSlash);
    }

    private void Charge(float delta)
    {
        if (!Input.IsActionPressed("attack")) 
        {
            if (_anim.CurrentAnimation == "Charge" || _anim.CurrentAnimation == "ChargeBack") 
            {
                _anim.Stop();
            }
            return;
        }

        _charge += delta;
        
        if (Input.IsActionJustPressed("attack")) 
        {
            if (!_isFlipped) 
            {
                _anim.Play("Charge");
            }
            else 
            {
                _anim.Play("ChargeBack");
            }
        }
    }

    private void ChangeOrder()
    {
        if (GetGlobalMousePosition() > Position) 
        {
            ZIndex = 1;
            return;
        }
        ZIndex = -1;
    }
}
