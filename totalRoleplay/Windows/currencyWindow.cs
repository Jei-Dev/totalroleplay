using Dalamud.Game.Gui.Toast;
using Dalamud.Interface.Windowing;
using Dalamud.Logging;
using ImGuiNET;
using System;
using System.Numerics;

namespace totalRoleplay.Windows;

public class currencyWindow : Window, IDisposable
{
    public record Currency
    {
        public required string currencyID { get; init; }
        public required string currencyName { get; init; }
        public required int currencyAmount { get; set; }
    };

    public currencyWindow() : base("TRP - Currency", 0)
    {
        Size = new Vector2(200, 200);
        SizeCondition = ImGuiCond.Once;
    }

    private Currency[] currency = {
            new Currency { currencyID = "GIL0001", currencyName = "Gil", currencyAmount = 100 },
    };

    public void Dispose() { }

    public override void Draw()
    {
        ImGui.Text("Currency");
        if (ImGui.BeginListBox("##currency", new Vector2(float.Epsilon, ImGui.GetTextLineHeightWithSpacing() * currency.Length + ImGui.GetStyle().FramePadding.Y)))
        {
            for (var i = 0; i < currency.Length; i++)
            {
                if (ImGui.Selectable(currency[i].currencyName + "  " + currency[i].currencyAmount, false)) { };
            }
        }
    }
}

