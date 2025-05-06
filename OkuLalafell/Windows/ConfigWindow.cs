using Dalamud.Interface.Windowing;
using ImGuiNET;
using OkuLalafell.Utils;
using System;
using System.Numerics;
using static OkuLalafell.Utils.Constant;

namespace OkuLalafell.Windows;

internal class ConfigWindow : Window
{
    private readonly Configuration configuration;
    private readonly string[] race = ["拉拉肥", "人族", "精灵族", "猫魅族", "鲁加族", "敖龙族", "硌狮族", "维埃拉族"];
    private int selectedRaceIndex = 0;

    private readonly string[] gender = ["保持原有的性别", "男性", "女性"];
    private int selectedGenderIndex = 0;

    public event Action? OnConfigChanged;
    public event Action<string>? OnConfigChangedSingleChar;

    public ConfigWindow(Plugin plugin) : base(
        "Oku Lalafell 配置",
        ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoScrollbar |
        ImGuiWindowFlags.NoScrollWithMouse)
    {
        Size = new Vector2(385, 190);
        SizeCondition = ImGuiCond.Always;

        configuration = Service.configuration;
    }

    public override void Draw()
    {
        // select race
        ImGui.AlignTextToFramePadding();
        ImGui.TextUnformatted("目标种族");
        ImGui.SameLine();
        if (ImGui.Combo("###Race", ref selectedRaceIndex, race, race.Length))
        {
            configuration.SelectedRace = MapIndexToRace(selectedRaceIndex);
            configuration.Save();
            OnConfigChanged?.Invoke();
        }

        // select gender
        ImGui.AlignTextToFramePadding();
        ImGui.TextUnformatted("目标性别");
        ImGui.SameLine();
        if (ImGui.Combo("###Gender", ref selectedGenderIndex, gender, gender.Length))
        {
            configuration.SelectedGender = MapIndexToGender(selectedGenderIndex);
            configuration.Save();
            OnConfigChanged?.Invoke();
        }

        // Enabled
        bool _Enabled = configuration.Enabled;
        if (ImGui.Checkbox("启用替换", ref _Enabled))
        {
            configuration.Enabled = _Enabled;
            configuration.Save();
            OnConfigChanged?.Invoke();
        }

        bool _Naked = configuration.Naked;
        if (ImGui.Checkbox("全部爆衣", ref _Naked))
        {
            configuration.Naked = _Naked;
            configuration.Save();
            OnConfigChanged?.Invoke();
        }

        bool _NakedWithEmp = configuration.NakedWithEmp;
        if (ImGui.Checkbox("爆衣使用皇帝的新裤子", ref _NakedWithEmp))
        {
            configuration.NakedWithEmp = _NakedWithEmp;
            configuration.Save();
            OnConfigChanged?.Invoke();
        }

        bool _StayOn = configuration.StayOn;
        if (ImGui.Checkbox("每次启动默认替换", ref _StayOn))
        {
            configuration.StayOn = _StayOn;
            configuration.Save();
        }
    }

    private static Race MapIndexToRace(int index)
    {
        return index switch
        {
            0 => Race.LALAFELL,
            1 => Race.HYUR,
            2 => Race.ELEZEN,
            3 => Race.MIQOTE,
            4 => Race.ROEGADYN,
            5 => Race.AU_RA,
            6 => Race.HROTHGAR,
            7 => Race.VIERA,
            _ => Race.LALAFELL,
        };
    }

    private static Gender MapIndexToGender(int index)
    {
        return index switch
        {
            0 => Gender.KEEP,
            1 => Gender.MALE,
            2 => Gender.FEMALE,
            _ => Gender.KEEP,
        };
    }

    public void InvokeConfigChanged()
    {
        OnConfigChanged?.Invoke();
    }
}
