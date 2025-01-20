﻿
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EndFieldPS.Resource.ResourceManager;

namespace EndFieldPS.Resource
{
    public class ResourceManager
    {
        public static Dictionary<string, SceneAreaTable> sceneAreaTable = new();
        public static StrIdNumTable strIdNumTable = new StrIdNumTable();
        public static Dictionary<string, CharacterTable> characterTable = new();
        public static Dictionary<string, SystemJumpTable> systemJumpTable = new();
        public static Dictionary<string, SettlementBasicDataTable> settlementBasicDataTable = new();
        public static Dictionary<string, BlocMissionTable> blocMissionTable = new();
        public static Dictionary<string, DialogTextTable> dialogTextTable = new();
        public static Dictionary<string, GameSystemConfigTable> gameSystemConfigTable = new();
        public static Dictionary<string, WikiGroupTable> wikiGroupTable = new();
        public static StrIdNumTable dialogIdTable = new();
        public static List<LevelData> levelDatas = new();


        public static int GetSceneNumIdFromLevelData(string name)
        {
            if (levelDatas.Find(a => a.id == name) == null) return 0;
            return levelDatas.Find(a => a.id == name).idNum;
        }
        public static void Init()
        {
            Server.Print("Loading resources");
            sceneAreaTable=JsonConvert.DeserializeObject<Dictionary<string, SceneAreaTable>>(File.ReadAllText("Excel/SceneAreaTable.json"));
            strIdNumTable = JsonConvert.DeserializeObject<StrIdNumTable>(File.ReadAllText("Excel/StrIdNumTable.json"));
            characterTable = JsonConvert.DeserializeObject<Dictionary<string, CharacterTable>>(File.ReadAllText("Excel/CharacterTable.json"));
            systemJumpTable = JsonConvert.DeserializeObject<Dictionary<string, SystemJumpTable>>(File.ReadAllText("Excel/SystemJumpTable.json"));
            settlementBasicDataTable = JsonConvert.DeserializeObject<Dictionary<string, SettlementBasicDataTable>>(File.ReadAllText("Excel/SettlementBasicDataTable.json"));
            blocMissionTable = JsonConvert.DeserializeObject<Dictionary<string, BlocMissionTable>>(File.ReadAllText("Excel/BlocMissionTable.json"));
            dialogTextTable = JsonConvert.DeserializeObject<Dictionary<string, DialogTextTable>>(File.ReadAllText("Excel/DialogTextTable.json"));
            gameSystemConfigTable = JsonConvert.DeserializeObject<Dictionary<string, GameSystemConfigTable>>(File.ReadAllText("Excel/GameSystemConfigTable.json"));
            wikiGroupTable = JsonConvert.DeserializeObject<Dictionary<string, WikiGroupTable>>(File.ReadAllText("Excel/WikiGroupTable.json"));
            dialogIdTable = JsonConvert.DeserializeObject<StrIdNumTable>(File.ReadAllText("Json/GameplayConfig/DialogIdTable.json"));
            LoadLevelDatas();
        }
        public static void LoadLevelDatas()
        {
            string directoryPath = @"Json/LevelData"; // Percorso della directory principale
            string[] jsonFiles = Directory.GetFiles(directoryPath, "*.json", SearchOption.AllDirectories);
            foreach(string json in jsonFiles)
            {
                LevelData data = JsonConvert.DeserializeObject<LevelData>(File.ReadAllText(json));
                levelDatas.Add(data);
                Print("Loading " + data.id);
            }

            Print($"Loaded {levelDatas.Count} LevelData");
        }
        public static int GetItemTemplateId(string item_id)
        {
            return strIdNumTable.item_id.dic[item_id];
        }
        public class BlocMissionTable
        {
            public string missionId;

        }
        public class WikiGroupTable
        {
            public List<WikiGroup> list;
        }
        public class WikiGroup
        {
            public string groupId;
        }
        public class GameSystemConfigTable
        {
            public int unlockSystemType;
            public string systemId;

        }
        public class LevelData
        {
            public string id;
            public int idNum;
            public string mapIdStr;
            public DefaultState defaultState;
        }
        public class DefaultState
        {
            public string sourceSceneName;
        }
        public class SettlementBasicDataTable
        {
            public string settlementId;
        }
        public class StrIdNumTable
        {
            public StrIdDic skill_group_id;
            public StrIdDic item_id;
            public Dictionary<string, int> dialogStrToNum;
        }
        public class DialogTextTable
        {

        }
        public class SystemJumpTable
        {
            public int bindSystem;
        }
        public class StrIdDic
        {
            public Dictionary<string, int> dic;
        }
        public class SceneAreaTable
        {
            public string areaId;
            public string sceneId;
            public int areaIndex;
        }

        public class CharacterTable
        {
            public List<Attributes> attributes;
            public string charId;

        }
        public class Attributes
        {
            public int breakStage;
            public AttributeList Attribute;
        }
        public class AttributeList
        {
            public List<Attribute> attrs;
        }
        public class Attribute
        {
            public int attrType;
            public double attrValue;
        }
        public static void Print(string text)
        {
            Logger.Log(text);
            Console.WriteLine($"[{Server.ColoredText("ResourceManager", "03fcce")}] " + text);
        }
    }
}
