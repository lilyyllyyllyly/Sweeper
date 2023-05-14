using Godot;
using System;

public class Highscore : Label
{
    public override void _Ready()
    {
	string path = "user://stats.csv";
	string score = "0";
	File file = new File();

        try {
            file.Open(path, File.ModeFlags.Read);
            string scoreLine = file.GetAsText().Split('\n')[0];
	    score = scoreLine.Split(',')[1];
	} catch {} /* i don't like this but i will just use 0 if something goes wrong... */

	Text = String.Format("Highscore: {0}", score);
    }
}
