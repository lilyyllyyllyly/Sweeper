using Godot;
using System;

public class Cobwebs : Label
{
    public void OnSpawnerNumberChanged(int spawners)
    {
        Text = String.Format("Cobwebs: {0}", spawners);
    }
}
