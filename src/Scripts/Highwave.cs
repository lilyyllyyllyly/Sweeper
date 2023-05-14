using Godot;
using System;

public class Highwave : Label
{
    public override void _Ready()
    {
	string path = "user://stats.csv";
	string wave = "0";
	File file = new File();

        try {
            file.Open(path, File.ModeFlags.Read);
            string waveLine = file.GetAsText().Split('\n')[1];
	    wave = waveLine.Split(',')[1];
	} catch {} /* i don't like this but i will just use 0 if something goes wrong... */

	Text = String.Format("Highest Wave: {0}", wave);
    }
}
