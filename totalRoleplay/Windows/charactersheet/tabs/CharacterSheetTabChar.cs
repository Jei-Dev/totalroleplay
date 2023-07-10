using ImGuiNET;
using System.Numerics;
using totalRoleplay.Configuration;

namespace totalRoleplay.Windows.charactersheet.tabs;
public class CharacterSheetTabChar : CharacterSheetTab
{
	public string characterName { get; set; } = "Name!"; // Replace this eventually with the characters actual name. For development this is current a custom string.
	public string[] characterJobs { get; set; } = { "My First Job", "Second Job!", "Woah! A third!" };
	public string characterPhysicalBiography { get; set; } = "This character has not set-up a physical biography."; // Is it smart to save a Biography like this? We'll find out...
	public string characterHistoryBiography { get; set; } = "This character does not have a History.";
	public int currentJob { get; set; }
	public string? characterTitle { get; set; } // Should look into getting the title from Honorific.
	public bool isPrefix { get; set; } = false; // In this context, isPrefix just means whether or not the Title is a prefix. This is false by default.
	public bool editMode { get; set; } = false;
	public string _charNameInput = string.Empty;
	private readonly CharacterConfiguration charConfig;
	private readonly CharacterSheetWindow charWindow;

	public CharacterSheetTabChar(CharacterConfiguration _charConfig, CharacterSheetWindow _charWindow)
	{
		charConfig = _charConfig;
		charWindow = _charWindow;
	}
	public override void OnClose()
	{
		base.OnClose();
	}

	public override void Draw()
	{
		var charNameString = isPrefix ? $"Name: {charConfig.characterTitle} {charConfig.characterName}" : $"Name: {charConfig.characterName} {charConfig.characterTitle}";
		ImGui.SetCursorPos(new Vector2(0, 490));
		if (ImGui.Button(editMode ? "Save" : "Edit", new Vector2(50, 20)))
		{
			if (editMode)
			{
				Save();
			}
			else
			{
				editMode = true;
			}
		}

		if (!editMode)
		{
			ImGui.SetCursorPos(new Vector2(0, 0));
			ImGui.Text(charNameString);
			ImGui.Text($"Current Employ: {currentJob}");
			ImGui.NewLine();
		}
		else
		{
			ImGui.SetCursorPos(new Vector2(0, 0));
			var _currentJob = currentJob;
			ImGui.InputTextWithHint("##charNameEdit", charNameString, ref _charNameInput, 150);
			ImGui.ListBox("##charJobSelect", ref _currentJob, characterJobs, characterJobs.Length);
			ImGui.NewLine();
		}

		ImGui.SetCursorPos(new Vector2(460, 140));
		if (ImGui.BeginChildFrame(13, new Vector2(195, 320))) { }

		base.Draw();
	}

	public override void Save()
	{
		if (_charNameInput != null && charConfig.characterName != _charNameInput && _charNameInput != "")
		{
			charConfig.characterName = _charNameInput;
			charConfig.Save();
			editMode = false;
		}
		else { editMode = false; }
	}
	public override string Title => "Character";
}
