using Dalamud.Game.ClientState.Objects.Enums;
using Dalamud.Game.Gui.NamePlate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OkuLalafell.Utils
{
    internal class Nameplate
    {
        public Nameplate()
        {
            Service.NamePlateGui.OnNamePlateUpdate += static (context, handlers) =>
            {
                foreach (var handler in handlers)
                {
                    if (handler.NamePlateKind == NamePlateKind.PlayerCharacter)
                    {
                        unsafe
                        {
                            if (handler.PlayerCharacter == null) return;
                            var customizeByte = handler.PlayerCharacter.Customize;

                            handler.NameParts.Text = $"{handler.Name}";

                            if (Service.configuration.LalaWithGender)
                            {
                                if (customizeByte[(int)CustomizeIndex.Race] == (byte)Constant.Race.LALAFELL)
                                {
                                    if (customizeByte[(int)CustomizeIndex.Gender] == (byte)Constant.Gender.MALE)
                                    {
                                        handler.NameParts.Text += $" ♂";
                                    }
                                    else if (customizeByte[(int)CustomizeIndex.Gender] == (byte)Constant.Gender.FEMALE)
                                    {
                                        handler.NameParts.Text += $" ♀";
                                    }
                                }
                            }

                            if (Service.configuration.ShowHQ)
                            {
                                if (customizeByte[(int)CustomizeIndex.Race] == (byte)Service.configuration.SelectedRace)
                                {
                                    handler.NameParts.Text += $" \uE03C";
                                }
                            }
                        }
                    }
                }
            };
        }

        public void Dispose() { }
    }
}
