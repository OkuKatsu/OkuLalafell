using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;
using OkuLalafell.Windows;

namespace OkuLalafell.Utils;

internal class Service
{
    internal static Plugin plugin { get; set; } = null!;
    internal static ConfigWindow configWindow { get; set; } = null!;
    internal static Configuration configuration { get; set; } = null!;
    internal static Drawer drawer { get; set; } = null!;
    public static PenumbraIpc penumbraApi { get; set; } = null!;
    [PluginService] public static IDalamudPluginInterface pluginInterface { get; set; } = null!;
    [PluginService] public static IChatGui chatGui { get; private set; } = null!;
    [PluginService] public static ICommandManager commandManager { get; private set; } = null!;
    [PluginService] public static INamePlateGui namePlateGui { get; private set; } = null!;

    [PluginService] public static IObjectTable ObjectTable { get; private set; } = null!;
}
