using Dalamud.Game.ClientState;
using Dalamud.Game.ClientState.Keys;
using Dalamud.Game.ClientState.Objects;
using Dalamud.Interface.Windowing;
using Dalamud.Logging;
using Dalamud.Plugin;
using FFXIVClientStructs.FFXIV.Client.Graphics.Render;
using ImGuiNET;
using System;
using System.Diagnostics;
using System.IO;
using System.Numerics;
using System.Timers;
using totalRoleplay.Configuration;
using totalRoleplay.Handlers;
using totalRoleplay.Service;

namespace totalRoleplay.Windows;

public class FakeDialogueWindow : Window, IDisposable
{
	private readonly ImGuiScene.TextureWrap backgroundTexture;
	private readonly DialogueService dialogueService;
	private readonly KeyState keyState;
	private readonly PluginConfiguration configuration;
	public readonly Stopwatch sw = new();
	private bool prevSpace = false;
	private readonly ClientState clientState;
	private readonly TargetManager targetManager;

	public FakeDialogueWindow(DialogueService dialogueService, KeyState keyState, ClientState clientState, TargetManager targetManager, PluginConfiguration configuration, DalamudPluginInterface pluginInterface) : base("Fake Dialogue Window")
	{
		this.dialogueService = dialogueService;
		this.keyState = keyState;
		Flags = ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse | ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoBackground;

		var imagePath = Path.Combine(pluginInterface.AssemblyLocation.Directory?.FullName!, "FFXIVEmptyDialogueBox.png");
		backgroundTexture = pluginInterface.UiBuilder.LoadImage(imagePath);
		Size = new Vector2(backgroundTexture.Width, backgroundTexture.Height);

		this.configuration = configuration;
		this.clientState = clientState;
		this.targetManager = targetManager;

		dialogueService.OnStartDialogue += OnStartDialogue;
		dialogueService.OnEndDialogue += OnEndDialogue;
	}

	public void Dispose()
	{
		backgroundTexture.Dispose();
		sw.Reset();
		dialogueService.OnStartDialogue -= OnStartDialogue;
		dialogueService.OnEndDialogue -= OnEndDialogue;
	}

	public override void Draw()
	{
		var currentLine = dialogueService.CurrentDialogueLine;
		ImGui.Image(backgroundTexture.ImGuiHandle, new Vector2(backgroundTexture.Width, backgroundTexture.Height));
		ImGui.SetCursorPos(new Vector2(55, 30));
		ImGui.Text(currentLine?.ActorName
				   .Replace("{YOU}", clientState.LocalPlayer?.Name.ToString())
				   .Replace("{TARGET}", targetManager.Target?.Name.ToString())
				   ?? "No Name");
		ImGui.SetCursorPos(new Vector2(45, 70));
		var displayedCharacters = Math.Clamp(sw.Elapsed.TotalMilliseconds * configuration.dialogueDrawSpeed, 0, currentLine?.Content?.Length ?? 0);
		ImGui.TextColored(new Vector4(0, 0, 0, 255),
						  currentLine?.Content?[..(int)displayedCharacters] ?? "");
		ImGui.GetIO().WantTextInput = true;
		HandleInput();
	}
	private void HandleInput()
	{
		var space = ImGui.IsKeyPressed(ImGuiKey.Space);
		if (space && !prevSpace)
		{
			sw.Restart();
			dialogueService.proceedToNextPage();
		}
		prevSpace = space;
	}

	private void OnStartDialogue()
	{
		this.IsOpen = true;
		sw.Restart();
	}
	private void OnEndDialogue()
	{
		this.IsOpen = false;
		sw.Reset();
	}
}
