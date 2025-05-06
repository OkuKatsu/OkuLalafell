using Dalamud.Game.ClientState.Objects.Types;
using FFXIVClientStructs.FFXIV.Client.Game.Object;
using Penumbra.Api.Enums;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static OkuLalafell.Utils.Constant;

namespace OkuLalafell.Utils
{
    internal class Drawer : IDisposable
    {
        public Drawer()
        {
            Service.configWindow.OnConfigChanged += RefreshAllPlayers;
            Service.configWindow.OnConfigChangedSingleChar += RefreshOnePlayer;
            if (Service.configuration.Enabled)
            {
                Plugin.OutputChatLine("Oku Lalafell 初始化模型...");
                RefreshAllPlayers();
            }
        }

        private static void RefreshAllPlayers()
        {
            Service.penumbraApi.RedrawAll(RedrawType.Redraw);
            Service.namePlateGui.RequestRedraw();
        }

        private static void RefreshOnePlayer(string charName)
        {
            if (!Service.configuration.Enabled)
                return;

            int objectIndex = -1;

            foreach (var obj in Service.ObjectTable)
            {
                if (!obj.IsValid()) continue;
                if (obj is not ICharacter) continue;
                if (obj.Name.TextValue != charName) continue;
                objectIndex = obj.ObjectIndex;
                break;
            }

            if (objectIndex == -1)
                return;

            Service.penumbraApi.RedrawOne(objectIndex, RedrawType.Redraw);
        }

        public static unsafe void OnCreatingCharacterBase(nint gameObjectAddress, Guid _1, nint _2, nint customizePtr, nint equipPtr)
        {
            if (!Service.configuration.Enabled) return;

            var equipData = (ulong*)equipPtr;

            // return if not player character
            var gameObj = (GameObject*)gameObjectAddress;
            if (gameObj->ObjectKind != ObjectKind.Pc) return;

            if (Service.configuration.Naked)
                NakedClothes(equipData, equipPtr);

            var customData = Marshal.PtrToStructure<CharaCustomizeData>(customizePtr);
            if (customData.Race == Service.configuration.SelectedRace || customData.Race == Race.UNKNOWN)
                if (customData.Gender == Service.configuration.SelectedGender || Service.configuration.SelectedGender == Gender.KEEP)
                    return;

            ChangeRaceGender(customData, customizePtr, Service.configuration.SelectedRace, Service.configuration.SelectedGender);
        }

        private static unsafe void ChangeRaceGender(CharaCustomizeData customData, nint customizePtr, Race selectedRace, Gender selectedGender)
        {
            if (selectedGender != Gender.KEEP)
            {
                customData.Gender = selectedGender;
            }
            var clan =  (byte)(customData.Tribe % 2);

            customData.Tribe = (byte)(((byte)selectedRace * 2) - 1 + clan);
            customData.Race = selectedRace;
            customData.FaceType %= 4;
            customData.ModelType = clan;

            // Fur pattern should be 1-5 for hrothgar
            if (customData.Race == Race.HROTHGAR)
                customData.LipColor = (byte)(1 + (customData.LipColor % 5));

            // Ears should be 1-4 for viera
            if (customData.Race == Race.VIERA)
                customData.RaceFeatureType = (byte)(1 + (customData.RaceFeatureType % 4));

            customData.HairStyle = (byte)RaceMappings.SelectHairFor(customData.Race, customData.Gender, (Clan)customData.ModelType, customData.HairStyle);

            Marshal.StructureToPtr(customData, customizePtr, true);
        }

        private static unsafe void NakedClothes(ulong* equipData, nint equipPtr)
        {
            for (int i = 0; i <= 9; ++i)
                equipData[i] = 0;
            equipData[3] = Service.configuration.NakedWithEmp ? 279U : 0;
        }

        public void Dispose()
        {
            Service.configWindow.OnConfigChanged -= RefreshAllPlayers;
            Service.configWindow.OnConfigChangedSingleChar -= RefreshOnePlayer;
        }
    }
}
