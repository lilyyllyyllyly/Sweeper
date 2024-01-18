using Godot;
using System;

public class SmallSpiderEffects: AnimationPlayer
{
	[Signal] public delegate void DamageFinished();

	[Export] private PackedScene _deathParticle;

	public void OnEnemyDamaged() {
		Play("SpiderDamage");
	}

	public void OnEnemyDied(Node2D enemy) {
		Node2D p = (Node2D)_deathParticle.Instance();
		p.Position = enemy.Position;
		GetTree().CurrentScene.AddChild(p);

		p.GetNode<Particles2D>(".").Emitting = true;
	}

	public void OnAnimationFinished(string animName) {
		if (animName == "SpiderDamage") EmitSignal("DamageFinished");
	}
}
