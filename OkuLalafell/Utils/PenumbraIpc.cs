using Dalamud.Plugin;
using Penumbra.Api.Enums;
using Penumbra.Api.Helpers;
using Penumbra.Api.IpcSubscribers;
using System;

namespace OkuLalafell.Utils
{
    internal class PenumbraIpc(IDalamudPluginInterface pluginInterface) : IDisposable
    {
        private readonly RedrawObject redrawOne = new(pluginInterface);
        private readonly RedrawAll redrawAll = new(pluginInterface);
        private readonly EventSubscriber<nint, Guid, nint, nint, nint> creatingCharacterBaseEvent =
            CreatingCharacterBase.Subscriber(pluginInterface, Drawer.OnCreatingCharacterBase);

        public void Dispose()
        {
            creatingCharacterBaseEvent.Dispose();
        }

        internal void RedrawOne(int objectIndex, RedrawType setting)
        {
            try
            {
                redrawOne.Invoke(objectIndex, setting);
            }
            catch (Exception ex)
            {
                Plugin.OutputChatLine($"警告: Penumbra 插件没有找到. 错误信息: {ex.Message}\n" +
                                      "注意：如果您在启用此插件之前禁用了 Penumbra，那么拉拉肥模型会保留到下一次模型刷新");
            }
        }

        internal void RedrawAll(RedrawType setting)
        {
            try
            {
                redrawAll.Invoke(setting);
            }
            catch (Exception ex)
            {
                Plugin.OutputChatLine($"警告: Penumbra 插件没有找到. 错误信息: {ex.Message}\n" +
                                      "注意：如果您在启用此插件之前禁用了 Penumbra，那么拉拉肥模型会保留到下一次模型刷新");
            }
        }
    }
}
