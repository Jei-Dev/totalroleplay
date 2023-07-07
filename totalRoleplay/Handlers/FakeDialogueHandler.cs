using Dalamud.Logging;
using System;
using System.Linq;
using totalRoleplay.Model;

namespace totalRoleplay.Handlers;

public class FakeDialogueHandler
{
    public delegate void OnStartDialogueHandler();
    public event OnStartDialogueHandler? OnStartDialogue;

    public delegate void OnEndDialogueHandler();
    public event OnEndDialogueHandler? OnEndDialogue;

    public delegate void TriggerQuestAction(QuestStateTriggerAction action);
    private DialogueSequence? currentDialogueSequence;
    private TriggerQuestAction? currentQuestActionTriggerHandler;
    public int currentLineIndex = 0;
    public DialogueLine? CurrentDialogueLine => currentDialogueSequence?.Lines?.ElementAtOrDefault(currentLineIndex);

    public void startFakeDialogue(DialogueSequence dialogue, TriggerQuestAction questActionTriggerHandler)
    {
        currentDialogueSequence = dialogue;
        currentQuestActionTriggerHandler = questActionTriggerHandler;
        currentLineIndex = 0;
        OnStartDialogue?.Invoke();
    }
    public void endFakeDialogue()
    {
        currentLineIndex = 0;
        OnEndDialogue?.Invoke();
    }
    public void proceedToNextPage()
    {
        foreach (var trigger in CurrentDialogueLine?.Triggers ?? Array.Empty<DialogueLineTrigger>())
        {
            if (trigger.When.Closed)
            {
                if (trigger.Then.Quest != null)
                {
                    currentQuestActionTriggerHandler?.Invoke(trigger.Then.Quest);
                }
            }
        }
        currentLineIndex++;
        PluginLog.Debug("Page Number: " + currentLineIndex);
        if (CurrentDialogueLine == null)
        {
            endFakeDialogue();
        }
    }
}
