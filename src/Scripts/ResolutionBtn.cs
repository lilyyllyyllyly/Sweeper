using Godot;
using System;

public class ResolutionBtn : OptionButton
{
    [Export] private string[] _resolutions;
    [Export] private int baseResX;

    public override void _Ready()
    {
        for (int i = 0; i < _resolutions.Length; ++i) AddItem(_resolutions[i], i);
    }

    /* Signal connected through the editor */
    public void ChangeResolution(int index)
    {
	string resText = _resolutions[index];
	int[] res = Array.ConvertAll<string, int>(resText.Split('x'), int.Parse);
	OS.WindowSize = new Vector2(res[0], res[1]);
    }
}
