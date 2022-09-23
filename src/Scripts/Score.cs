using Godot;
using System;

public class Score : Label
{
    private Label _labelNode;

    public override void _Ready()
    {
        _labelNode = GetNode<Label>(".");
        _labelNode.Text = "0";
    }

    private void OnScoreUpdated(float newScore) 
    {
        _labelNode.Text = newScore.ToString();
    }
}
