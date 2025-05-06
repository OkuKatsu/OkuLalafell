using Dalamud.Game.ClientState.Objects.Enums;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace OkuLalafell.Utils
{
    internal static class Constant
    {
        public enum Race : byte
        {
            UNKNOWN = 0,
            HYUR = 1,
            ELEZEN = 2,
            LALAFELL = 3,
            MIQOTE = 4,
            ROEGADYN = 5,
            AU_RA = 6,
            HROTHGAR = 7,
            VIERA = 8
        }

        public enum Gender : byte
        {
            MALE = 0,
            FEMALE = 1,
            KEEP = 2,
        }

        public enum Clan : byte
        {
            CLAN0 = 0,
            CLAN1 = 1,
            UNKNOWN = 255
        }

        public class RaceMappings
        {
            public static readonly Dictionary<int, List<int>> RaceHairs = InitializeRaceHairs();

            private static Dictionary<int, List<int>> InitializeRaceHairs()
            {
                var result = new Dictionary<int, List<int>>();

                foreach (var row in HairData.HairDataArray)
                {
                    if (!result.TryGetValue(row.Primary, out var hairs))
                        result.Add(row.Primary, hairs = []);

                    hairs.Add(row.Secondary);
                }

                return result;
            }

            public static int SelectHairFor(Race race, Gender gender, Clan clan, int hairId)
            {
                int raceId = ((race, clan) switch
                {
                    (Race.HYUR, Clan.CLAN0) => 1,
                    (Race.HYUR, Clan.CLAN1) => 3,
                    (Race.ELEZEN, _) => 5,
                    (Race.MIQOTE, _) => 7,
                    (Race.ROEGADYN, _) => 9,
                    (Race.LALAFELL, _) => 11,
                    (Race.AU_RA, _) => 13,
                    (Race.HROTHGAR, _) => 15,
                    (Race.VIERA, _) => 17,
                    _ => 1,
                } + (gender == Gender.FEMALE ? 1 : 0)) * 100 + 1;

                if (!RaceHairs.TryGetValue(raceId, out var hairs) || hairs.Count == 0)
                    return 1;

                var idx = hairs.FindIndex(x => x >= hairId);

                if (idx == -1)
                    idx = 0;

                // Use matching hairstyle if it is available on target race
                // otherwise, pick an index based on the hair ID
                if (hairs[idx] == hairId)
                {
                    return hairId;
                }
                else
                {
                    // A lot of NPC exclusive hair is above 200 and aren't suitable
                    while (hairId > 0 && hairs[hairId % hairs.Count] > 200)
                        --hairId;

                    return hairs[hairId % hairs.Count];
                }
            }
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct CharaCustomizeData
        {
            [FieldOffset((int)CustomizeIndex.Race)] public Race Race;
            [FieldOffset((int)CustomizeIndex.Gender)] public Gender Gender;
            [FieldOffset((int)CustomizeIndex.ModelType)] public byte ModelType;
            [FieldOffset((int)CustomizeIndex.Tribe)] public byte Tribe;
            [FieldOffset((int)CustomizeIndex.FaceType)] public byte FaceType;
            [FieldOffset((int)CustomizeIndex.HairStyle)] public byte HairStyle;
            [FieldOffset((int)CustomizeIndex.FaceFeatures)] public byte FaceFeatures;
            [FieldOffset((int)CustomizeIndex.LipColor)] public byte LipColor;
            [FieldOffset((int)CustomizeIndex.RaceFeatureType)] public byte RaceFeatureType;
        }
    }
}
