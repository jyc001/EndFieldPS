﻿using EndFieldPS.Resource;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EndFieldPS.Game.Character
{
    public class Character
    {
        [BsonId(IdGenerator = typeof(ObjectIdGenerator))]
        public ObjectId _id { get; set; }
        [BsonElement("templateId")]
        public string id;
        public ulong guid;
        public ulong weaponGuid;

        public int level;
        public int xp;
        public ulong owner;
        public double curHp;
        public uint potential = 0;
        public int breakStage = 1;
        public Character()
        {
           
        }
        public Character(ulong owner, string id) : this(owner, id, 1)
        {

        }
        public Character(ulong owner,string id, int level) : this()
        {
            this.owner = owner;
            this.id = id;
            this.level = level;
            guid = GetOwner().random.Next();
            this.weaponGuid = GetOwner().inventoryManager.AddWeapon(ResourceManager.GetDefaultWeapon(ResourceManager.characterTable[id].weaponType),1).guid;
            this.curHp = ResourceManager.characterTable[id].attributes[level].Attribute.attrs.Find(A => A.attrType == (int)AttributeType.MaxHp)!.attrValue;
        }
        public List<ResourceManager.Attribute> GetAttributes()
        {
            int lev = level - 1 + breakStage;
            return ResourceManager.characterTable[id].attributes[lev].Attribute.attrs;
        }
        public Player GetOwner()
        {
            return Server.clients.Find(c=>c.roleId == this.owner); 
        }
        public SceneCharacter ToSceneProto()
        {
            SceneCharacter proto= new SceneCharacter()
            {
                Level = level,
                
                BattleInfo = new()
                {
                    MsgGeneration = 1,
                    
                    SkillList =
                                {
                                    new ServerSkill()
                                    {
                                        Blackboard = new()
                                        {

                                        },
                                        InstId=GetOwner().random.Next(),
                                        Level=1,
                                        Source=BattleSkillSource.Default,
                                        PotentialLv=1,
                                        SkillId=id+"_NormalSkill",
                                    },
                                    new ServerSkill()
                                    {
                                        Blackboard = new()
                                        {

                                        },
                                        InstId=GetOwner().random.Next(),
                                        Level=1,
                                        Source=BattleSkillSource.Default,
                                        PotentialLv=1,
                                        SkillId=id+"_ComboSkill",
                                    },
                                    new ServerSkill()
                                    {
                                        Blackboard = new()
                                        {

                                        },
                                        InstId=GetOwner().random.Next(),
                                        Level=1,
                                        Source=BattleSkillSource.Default,
                                        PotentialLv=1,
                                        SkillId=id+"_UltimateSkill",
                                    },
                                    new ServerSkill()
                                    {
                                        Blackboard = new()
                                        {

                                        },
                                        InstId=GetOwner().random.Next(),
                                        Level=1,
                                        Source=BattleSkillSource.Default,
                                        PotentialLv=1,
                                        SkillId=id+"_NormalAttack",
                                    }
                                }
                },

                Name = $"{ResourceManager.characterTable[id].engName}",
                
                CommonInfo = new()
                {
                    Hp = curHp,
                    Id = guid,
                    Position = GetOwner().position.ToProto(),
                    Rotation = GetOwner().rotation.ToProto(),
                    SceneNumId = GetOwner().curSceneNumId,
                    Templateid = id,
                    Type = (int)0,
                    
                },
                Attrs =
                {

                }
            };
            GetAttributes().ForEach(attr =>
            {
                proto.Attrs.Add(new AttrInfo()
                {
                    AttrType = attr.attrType,
                    BasicValue = 0,
                    Value = attr.attrValue

                });

            });
            return proto;
        }
        public string GetBreakNode()
        {
            string lastNode = "";
            if (ResourceManager.charBreakNodeTable.Values.ToList().Find(b => b.breakStage == breakStage)!=null)
            {
                lastNode = ResourceManager.charBreakNodeTable.Values.ToList().Find(b => b.breakStage == breakStage).nodeId;
            }
            return lastNode;
        }
        public CharInfo ToProto()
        {
            CharInfo info = new CharInfo()
            {
                Exp = xp,
                Level = level,
                IsDead = curHp < 1,
                
                Objid = guid,
                Templateid = id,
                CharType = CharType.DefaultType,
                OwnTime = 1,
                NormalSkill = id + "_NormalSkill",
                WeaponId = weaponGuid,
                PotentialLevel = potential,
                
                Talent = new()
                {
                    LatestBreakNode= GetBreakNode(),
                },
                BattleMgrInfo = new()
                {
                    
                },
                BattleInfo = new()
                {
                    Hp = curHp,

                },
                SkillInfo = new()
                {
                    
                    NormalSkill = id + "_NormalSkill",
                    ComboSkill = id + "_ComboSkill",
                    UltimateSkill = id + "_UltimateSkill",
                    DispNormalAttackSkill = id + "_NormalAttack",
                    LevelInfo =
                    {
                        new SkillLevelInfo()
                        {
                            SkillId=id+"_NormalAttack",
                            SkillLevel=1,
                            SkillMaxLevel=1,
                            SkillEnhancedLevel=1
                        },
                        new SkillLevelInfo()
                        {
                            SkillId=id+"_NormalSkill",
                            SkillLevel=1,
                            SkillMaxLevel=1,
                            SkillEnhancedLevel=1
                        },
                        new SkillLevelInfo()
                        {
                            SkillId=id+"_UltimateSkill",
                            SkillLevel=1,
                            SkillMaxLevel=1,
                            SkillEnhancedLevel=1
                        },
                        new SkillLevelInfo()
                        {
                            SkillId=id+"_ComboSkill",
                            SkillLevel=1,
                            SkillMaxLevel=1,
                            SkillEnhancedLevel=1
                        },

                    }
                }
            };

            return info;
        }
    }
}
