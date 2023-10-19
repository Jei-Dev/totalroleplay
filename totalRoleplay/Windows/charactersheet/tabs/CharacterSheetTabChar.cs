using ImGuiNET;
using System.Numerics;
using totalRoleplay.Configuration;

namespace totalRoleplay.Windows.charactersheet.tabs;
public class CharacterSheetTabChar : CharacterSheetTab
{
	public bool isPrefix { get; set; } = false; // In this context, isPrefix just means whether or not the Title is a prefix. This is false by default.
	public bool editMode { get; set; } = false;
	public string _charNameInput = string.Empty;
	private readonly CharacterConfiguration charConfig;
	private readonly CharacterSheetWindow charWindow;
	private readonly string[]? jobList;
	public CharacterSheetTabChar(CharacterConfiguration _charConfig, CharacterSheetWindow _charWindow)
	{
		charConfig = _charConfig;
		charWindow = _charWindow;
		//jobList = for(var i = 0; i < charConfig.characterJobList.Length; i++) { };
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
			var _charJob = charConfig.currentJob;
			ImGui.Text($"Current Employ: {charConfig.characterJobList[_charJob].name}");
			ImGui.NewLine();
		}
		else
		{
			ImGui.SetCursorPos(new Vector2(0, 0));
			var _charJob = charConfig.currentJob;

			ImGui.InputTextWithHint("##charNameEdit", charNameString, ref _charNameInput, 150);
			ImGui.ListBox("##charJobSelect", ref _charJob, jobList, charConfig.characterJobList.Length);
			ImGui.NewLine();
		}

		ImGui.SetCursorPos(new Vector2(460, 140));
		if (ImGui.BeginChildFrame(13, new Vector2(195, 320))) { }

		base.Draw();
	}

	public override void Save()
	{
		if (_charNameInput != "" && charConfig.characterName != _charNameInput)
		{
			charConfig.characterName = _charNameInput;
			charConfig.Save();
			editMode = false;
		}
		else { editMode = false; }
	}
	public override string Title => "Character";
}
