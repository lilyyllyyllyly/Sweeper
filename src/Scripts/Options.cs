using Godot;
using System;

public class Options : TextureRect
{
    [Export] private NodePath _fullscreenBtnPath;
    private Button _fullscreenBtn;
    [Export] private NodePath _borderlessBtnPath;
    private Button _borderlessBtn;
    [Export] private NodePath _resolutionBtnPath;
    private OptionButton _resolutionBtn;

    private bool fullscreen;
    private bool borderless;
    private int  resolution;

    [Export] private string[] _resolutions;

    public override void _Ready()
    {
	Initialize();
	LoadDefaults();
    }

    private void Initialize()
    {
	/* Buttons */
	_fullscreenBtn = GetNode<Button>(_fullscreenBtnPath);
	_fullscreenBtn.Connect("pressed", this, "OnFullscreen");
	_borderlessBtn = GetNode<Button>(_borderlessBtnPath);
	_borderlessBtn.Connect("pressed", this, "OnBorderless");
	_resolutionBtn = GetNode<OptionButton>(_resolutionBtnPath);
	_resolutionBtn.Connect("item_selected", this, "OnResolution");

	/* Resolutions */
        for (int i = 0; i < _resolutions.Length; ++i) _resolutionBtn.AddItem(_resolutions[i], i);
    }

    private void LoadDefaults()
    {
	string path = "user://options.csv";
	File file = new File();
	
	if (file.FileExists(path)) {
	    /* Read from file */
	    file.Open(path, File.ModeFlags.Read);
	    string[] lines = file.GetAsText().Split('\n');

	    /* Set button state */
	    _fullscreenBtn.SetPressed(lines[0].Split(',')[1] == "True");
	    _borderlessBtn.SetPressed(lines[1].Split(',')[1] == "True");
	    resolution = int.Parse(lines[2].Split(',')[1]);
	    _resolutionBtn.Select(resolution);

	    /* Update window */
	    OnFullscreen();
	    OnBorderless();
	    OnResolution(resolution);
	}
    }

    private void SaveOptions()
    {
	string path = "user://options.csv";
	File file = new File();

	/* Write to file */
	file.Open(path, File.ModeFlags.Write);
	file.StoreString($"fullscreen,{fullscreen}\n");
	file.StoreString($"borderless,{borderless}\n");
	file.StoreString($"resolution,{resolution}");
	file.Close();
    }

    private void OnFullscreen()
    {
	fullscreen = GetNode<CheckBox>(_fullscreenBtnPath).IsPressed();
	OS.WindowFullscreen = fullscreen;
	SaveOptions();
    }

    private void OnBorderless()
    {
	borderless = GetNode<CheckBox>(_borderlessBtnPath).IsPressed();
	OS.WindowBorderless = borderless;
	SaveOptions();
    }

    public void OnResolution(int index)
    {
	string resText = _resolutions[index];
	int[] res = Array.ConvertAll<string, int>(resText.Split('x'), int.Parse);
	OS.WindowSize = new Vector2(res[0], res[1]);

	resolution = index;
	SaveOptions();
    }
}
