using Dalamud.Logging;
using System;

namespace totalRoleplay.Handlers;

public class FakeDialogueHandler
{
	public delegate void onEndDialogueClose();
	public event onEndDialogueClose onEndDialogue;
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

	public void startFakeDialogue() { }
	public void endFakeDialogue()
	{
		currentPage = 0;
		onEndDialogue();
	}
	public void proceedToNextPage()
	{
		var nextPage = 0;
		if (currentPage < currentPage + 1 && nextPage == 0)
		{
			Math.Clamp(currentPage++, 0, dialogueText.Length - 1);
			PluginLog.Debug("Page Number: " + currentPage);
			nextPage = 1;
			if (currentPage == dialogueText.Length)
			{
				endFakeDialogue();
			}
		}
	}
}
