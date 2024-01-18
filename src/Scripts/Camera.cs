using Godot;
using System;

public class Camera : Camera2D
{
	// Screenshake
	private float _shakeIntensity;
	[Export] private float _shakeIntensityScaling; // Exponent for intensity

	[Export] private float _sweepShakeMult;
	[Export] private float _damageShakeIntensity;

	private float _shakeTime;
	private float _shakeMaxTime;
	[Export] private float _shakeTimeMult; // Multiplier for time
	[Export] private float _shakeTimeScaling; // Exponent for time
	// ---

	// Slowdown
	[Export] private float _slowdownPerKill;
	[Export] private float _slowdownCap;
	// ---

	private SceneTree _tree;

	public override void _Ready() {
		_tree = GetTree();
	}

	public override void _Process(float delta) {
		if (_shakeTime > 0) {
			float magnitude = _shakeIntensity * (_shakeTime/_shakeMaxTime);
			float angle = GD.Randf() * 360;

			Position = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * magnitude;

			_shakeTime -= delta;
		}

		float ts;
		if ((ts = Engine.GetTimeScale()) < 1) {
			Engine.SetTimeScale(Mathf.Clamp(ts + _slowdownPerKill, 0, 1));
		}
	}

	private void Screenshake(float intensity) {
		_shakeMaxTime = _shakeTime = Mathf.Pow(intensity, _shakeTimeScaling) * _shakeTimeMult;
		_shakeIntensity = Mathf.Pow(intensity, _shakeIntensityScaling);
	}

	public void OnBroomSwept(int intensity) {
		Screenshake(intensity * _sweepShakeMult);
	}

	public void OnPlayerDamaged() {
		Screenshake(_damageShakeIntensity);
	}

	public void Slowdown() {
		Engine.SetTimeScale(Mathf.Clamp(Engine.GetTimeScale() - _slowdownPerKill, _slowdownCap, 1));
	}
}
