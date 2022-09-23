using Godot;
using System;

public class PoisonBar : ProgressBar
{
    private Player _currPlayer;
    private ProgressBar _bar;

    public override void _Ready()
    {
        _bar = GetNode<ProgressBar>(".");
    }

    private void OnPlayerReady(Player player) 
    {
        _currPlayer = player;
    }

    public override void _Process(float delta)
    {
        float poisonNormal = (_currPlayer.poison/_currPlayer.maxPoison);
        _bar.Value = poisonNormal * 100;
    }
}
