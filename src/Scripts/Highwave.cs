using Godot;
using System;

public class Highwave : Label
{
    public override void _Ready()
    {
	string path = "user://stats.csv";
	string wave = "0";
	File file = new File();

        if (file.FileExists(path)) {
            file.Open(path, File.ModeFlags.Read);
            string waveLine = file.GetAsText().Split('\n')[1];
	    wave = waveLine.Split(',')[1];
	}

	Text = String.Format("Highest Wave: {0}", wave);
    }
}
