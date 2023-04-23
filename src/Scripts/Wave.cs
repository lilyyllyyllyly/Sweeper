using Godot;
using System;

public class Wave : Label
{
    public void OnNewWave(int wave)
    {
        Text = String.Format("Wave: {0}", wave);
    }
}
