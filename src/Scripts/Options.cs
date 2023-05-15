using Godot;
using System;

public class Options : TextureRect
{
    [Export] private NodePath _fullscreenBtnPath;
    private Button _fullscreenBtn;
    [Export] private NodePath _borderlessBtnPath;
    private Button _borderlessBtn;

    private bool fullscreen;
    private bool borderless;

    public override void _Ready()
    {
	_fullscreenBtn = GetNode<Button>(_fullscreenBtnPath);
	_fullscreenBtn.Connect("pressed", this, "OnFullscreen");
	_borderlessBtn = GetNode<Button>(_borderlessBtnPath);
	_borderlessBtn.Connect("pressed", this, "OnBorderless");

	LoadDefaults();
    }

    private void LoadDefaults()
    {
	string path = "user://options.csv";
	File file = new File();
	
	if (file.FileExists(path)) {
	    /* Read from file */
	    file.Open(path, File.ModeFlags.Read);
	    string[] lines = file.GetAsText().Split('\n');

	    /* Set button pressed state */
	    _fullscreenBtn.SetPressed(lines[0].Split(',')[1] == "True");
	    _borderlessBtn.SetPressed(lines[1].Split(',')[1] == "True");

	    /* Update window */
	    OnFullscreen();
	    OnBorderless();
	}
    }

    private void SaveOptions(bool fullscreen, bool borderless)
    {
	string path = "user://options.csv";
	File file = new File();

	/* Write to file */
	file.Open(path, File.ModeFlags.Write);
	file.StoreString($"fullscreen,{fullscreen}\n");
	file.StoreString($"borderless,{borderless}");
	file.Close();
    }

    private void OnFullscreen()
    {
	fullscreen = GetNode<CheckBox>(_fullscreenBtnPath).IsPressed();
	OS.WindowFullscreen = fullscreen;
	SaveOptions(fullscreen, borderless);
    }

    private void OnBorderless()
    {
	borderless = GetNode<CheckBox>(_borderlessBtnPath).IsPressed();
	OS.WindowBorderless = borderless;
	SaveOptions(fullscreen, borderless);
    }
}
