using Godot;
using System;

public class Highscore : Label
{
    public override void _Ready()
    {
	string path = "user://stats.csv";
	string score = "0";
	File file = new File();

	if (file.FileExists(path)) {
            file.Open(path, File.ModeFlags.Read);
    	    string scoreLine = file.GetAsText().Split('\n')[0];
	    score = scoreLine.Split(',')[1];
	}

	Text = String.Format("Highscore: {0}", score);
    }
}
