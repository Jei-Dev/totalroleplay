using Dalamud.Logging;
using System;

namespace totalRoleplay.Handlers;

public class FakeDialogueHandler
{
	public delegate void onEndDialogueClose();
	public event onEndDialogueClose? OnEndDialogue;
	public record DialogueText
	{
		public string? pageID { get; set; }
		public required string pageText { get; set; }
		public required string characterName { get; set; }
	}
	public int currentPage = 0;
	public FakeDialogueHandler() { }

	public DialogueText[] dialogueText =
	{
		new DialogueText  { pageID = null, pageText = "Woah! I am some text!", characterName = "Zelda Wynters"},
		new DialogueText  { pageID = null, pageText = "I am page 2!! :D!!", characterName = "Zelda Wynters"}
	};

	public void startFakeDialogue() { PluginLog.LogWarning("No Dialogue for character"); }
	public void endFakeDialogue()
	{
		currentPage = 0;
		OnEndDialogue?.Invoke();
	}
	public void proceedToNextPage()
	{
		currentPage++;
		PluginLog.Debug("Page Number: " + currentPage);
		if (currentPage == dialogueText.Length)
		{
			endFakeDialogue();
		}
	}
}
