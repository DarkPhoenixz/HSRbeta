using FreeSR.Gateserver.Manager.Handlers.Core;
using static FreeSR.Gateserver.Manager.Handlers.LineupReqGroup;
using FreeSR.Gateserver.Network;
using FreeSR.Proto;

namespace FreeSR.Gateserver.Manager.Handlers
{
    internal static class BattleReqGroup
    {
        [Handler(CmdType.CmdSetLineupNameCsReq)]
        public static void OnSetLineupNameCsReq(NetSession session, int cmdId, object data)
        {
            var request = data as SetLineupNameCsReq;
            if(request.Name == "battle")
            {
                
                var lineupInfo = new LineupInfo
                {
                    ExtraLineupType = ExtraLineupType.LineupNone,
                    Name = "Squad 1",
                    Mp = 5,
                    MaxMp = 5,
                    LeaderSlot = 0
                };
                List<uint> characters = new List<uint> { Avatar1, Avatar2, Avatar3, Avatar4 };
                foreach (uint id in characters)
                {
                    if (id == 0) continue;
                    lineupInfo.AvatarLists.Add(new LineupAvatar
                    {
                        Id = id,
                        Hp = 10000,
                        Satiety = 100,
                        Sp = new AmountInfo{CurAmount = 10000,MaxAmount = 10000},
                        AvatarType = AvatarType.AvatarFormalType,
                        Slot = (uint)lineupInfo.AvatarLists.Count
                    });
                }

                var sceneInfo = new SceneInfo
                {
                    GameModeType = 2,
                    EntryId = 2010101,
                    PlaneId = 20101,
                    FloorId = 20101001
                };

                var calaxInfoTest = new SceneEntityInfo
                {
                    GroupId = 19,
                    InstId = 300001,
                    EntityId = 4194583,
                    Prop = new ScenePropInfo
                    {
                        PropState = 1,
                        PropId = 808
                    },
                    Motion = new MotionInfo
                    {
                        Pos = new Vector
                        {
                            X = -570,
                            Y = 19364,
                            Z = 4480
                        },
                        Rot = new Vector
                        {
                            Y = 180000
                        }
                    },
                };

                sceneInfo.EntityLists.Add(calaxInfoTest);

                session.Send(CmdType.CmdEnterSceneByServerScNotify, new EnterSceneByServerScNotify
                {
                    Scene = sceneInfo,
                    Lineup = lineupInfo
                });

                session.Send(CmdType.CmdSceneEntityMoveScNotify, new SceneEntityMoveScNotify
                {
                    EntryId = 2010101,
                    EntityId = 0,
                    Motion = new MotionInfo
                    {
                        Pos = new Vector
                        {
                            X = -570,
                            Y = 19364,
                            Z = 4480
                        },
                        Rot = new Vector
                        {
                            Y = 180000
                        }
                    }
                });
            }

            session.Send(CmdType.CmdSetLineupNameScRsp, new SetLineupNameScRsp
            {
                Retcode = 0,
                Name = request.Name,
                Index = request.Index
            });
        }


        [Handler(CmdType.CmdStartCocoonStageCsReq)]
        //[Handler(CmdType.CmdStartTimedCocoonStageCsReq)]
        public static void OnStartCocoonStageCsReq(NetSession session, int cmdId, object data)
        {
            var request = data as StartCocoonStageCsReq;

            Dictionary<uint, List<uint>> monsterIds = new Dictionary<uint, List<uint>>
            {
                { 1, new List<uint> { 3024010 } },
                { 2, new List<uint> { 2023010, 8013010 } },
                { 3, new List<uint> { 3014020 } }
            };

            Dictionary<uint, uint> monsterLevels = new Dictionary<uint, uint>
            {
                {1,95},{2,95},{3,95}
            };

            //basic
            //shape shifter 2023010
            //trampler 8013010
            //mangia anime 3014022 / 3014020
            /*
1: flat hp
2: flat atk
3: flat def
4: hp%
5: atk% 
6: def%
7: spd 
8: crit rate
9: crit damage 
10: Effect Hit Rate
11: Effect RES
12: Break Effect

           ID of the main affixs
The head
1: Flat hp
The hand
1: Flat atk

The clothing
1: Percent hp
2: percent atk
3: Percentage defense
4: Critical hit rate
5: Critical Strike damage
6: Bonus healing amount
7: Effect hit

The shoes
1: Percent hp
2: flat atk
3: Percentage defense
4: Speed

The ball
1: Percent hp
2: Flat atk
3: Percentage defense
4: Physical damage
5: Fire damage
6: Ice damage
7: Thunder damage
8: Wind damage
9: Quantum damage
10: imaginary attribute damage

The rope
1: Defeat special attacks
2: Energy recovery efficiency
3: Percent hp
4: Flat atk
5: Percentage defense
            */

            var battleInfo = new SceneBattleInfo
            {
                StageId = 201012311,
                LogicRandomSeed = 639771447,
                WorldLevel = 6
            };

            var testRelic = new BattleRelic
            {
                Id = 61011,
                Level = 15,
                MainAffixId = 1,
                SubAffixLists = {
                    new RelicAffix
                    {
                        AffixId = 8,
                        Step = 155
                    }
                }
            };

            var aventurineHead = new BattleRelic
            {
                Id = 61031,
                Level = 15,
                MainAffixId = 1,
                SubAffixLists = {
                    new RelicAffix
                    {
                        AffixId = 6,
                        Step = 250
                    },
                    new RelicAffix
                    {
                        AffixId = 8,
                        Step = 65
                    },
                    new RelicAffix
                    {
                        AffixId = 7,
                        Step = 100
                    },
                    new RelicAffix
                    {
                        AffixId = 9,
                        Step = 100
                    }
                }
            };
            var aventurineHands = new BattleRelic
            {
                Id = 61032,
                Level = 15,
                MainAffixId = 1,
                SubAffixLists = {
                    new RelicAffix
                {
                    AffixId = 10,
                    Step = 400
                } }
            };
            var aventurineBody = new BattleRelic
            {
                Id = 61033,
                Level = 15,
                MainAffixId = 3,
                SubAffixLists = {new RelicAffix
                {
                    AffixId = 4,
                    Step = 10
                } }
            };
            var aventurineFeet = new BattleRelic
            {
                Id = 61034,
                Level = 15,
                MainAffixId = 4,
                SubAffixLists = {new RelicAffix
                {
                    AffixId = 4,
                    Step = 10
                } }
            };
            var aventurineBall = new BattleRelic
            {
                Id = 63145,
                Level = 15,
                MainAffixId = 7,
                SubAffixLists = {new RelicAffix
                {
                    AffixId = 4,
                    Step = 10
                } }
            };
            var aventurineRope = new BattleRelic
            {
                Id = 63146,
                Level = 15,
                MainAffixId = 2,
                SubAffixLists = {new RelicAffix
                {
                    AffixId = 4,
                    Step = 10
                } }
            };
var aventurineHeadOP = new BattleRelic
            {
                Id = 61031,
                Level = 15,
                MainAffixId = 1,
                SubAffixLists = {
                    new RelicAffix
                    {
                        AffixId = 6,
                        Step = 1000
                    },
                    new RelicAffix
                    {
                        AffixId = 8,
                        Step = 1000
                    },
                    new RelicAffix
                    {
                        AffixId = 7,
                        Step = 1000
                    },
                    new RelicAffix
                    {
                        AffixId = 9,
                        Step = 10000
                    }
                }
            };
var aventurineBodyOP = new BattleRelic
            {
                Id = 61033,
                Level = 9999,
                MainAffixId = 3,
                SubAffixLists = {new RelicAffix
                {
                    AffixId = 4,
                    Step = 10
                } }
            };
var aventurineBallOP = new BattleRelic
            {
                Id = 63145,
                Level = 9999,
                MainAffixId = 10,
                SubAffixLists = {new RelicAffix
                {
                    AffixId = 4,
                    Step = 10
                } }
            };                       

            var acheronHead = new BattleRelic
            {
                Id = 61171,
                Level = 15,
                MainAffixId = 1,
                SubAffixLists = {
                    new RelicAffix
                    {
                        AffixId = 8,
                        Step = 155
                    },
                    new RelicAffix
                    {
                        AffixId = 9,
                        Step = 100
                    },
                    new RelicAffix
                    {
                        AffixId = 7,
                        Step = 60
                    },
                    new RelicAffix
                    {
                        AffixId = 6,
                        Step = 10
                    }
                }
            };
            var acheronHands = new BattleRelic
            {
                Id = 61172,
                Level = 15,
                MainAffixId = 1,
                SubAffixLists = {
                    new RelicAffix
                {
                    AffixId = 12,
                    Step = 80
                } }
            };
            var acheronBody = new BattleRelic
            {
                Id = 61173,
                Level = 15,
                MainAffixId = 5,
                SubAffixLists = {new RelicAffix
                {
                    AffixId = 4,
                    Step = 10
                } }
            };
            var acheronFeet = new BattleRelic
            {
                Id = 61174,
                Level = 15,
                MainAffixId = 4,
                SubAffixLists = {new RelicAffix
                {
                    AffixId = 4,
                    Step = 10
                } }
            };
            var acheronBall = new BattleRelic
            {
                Id = 63145,
                Level = 15,
                MainAffixId = 7,
                SubAffixLists = {new RelicAffix
                {
                    AffixId = 4,
                    Step = 10
                } }
            };
            var acheronRope = new BattleRelic
            {
                Id = 63146,
                Level = 15,
                MainAffixId = 4,
                SubAffixLists = {new RelicAffix
                {
                    AffixId = 4,
                    Step = 10
                } }
            };
var acheronHeadOP = new BattleRelic
            {
                Id = 61171,
                Level = 15,
                MainAffixId = 1,
                SubAffixLists = {
                    new RelicAffix
                    {
                        AffixId = 8,
                        Step = 1000
                    },
                    new RelicAffix
                    {
                        AffixId = 9,
                        Step = 10000
                    },
                    new RelicAffix
                    {
                        AffixId = 7,
                        Step = 60
                    },
                    new RelicAffix
                    {
                        AffixId = 5,
                        Step = 10000
                    }
                }
            };
var acheronBodyOP = new BattleRelic
{
    Id = 61173,
    Level = 999,
    MainAffixId = 5,
    SubAffixLists = {new RelicAffix
    {
        AffixId = 4,
        Step = 10
    } }
};
var acheronBallOP = new BattleRelic
{
Id = 63145,
Level = 990,
MainAffixId = 7,
SubAffixLists = {new RelicAffix
{
    AffixId = 4,
    Step = 10
} }
};
            
            
            

            var hHHead = new BattleRelic
            {
                Id = 61011,
                Level = 15,
                MainAffixId = 1,
                SubAffixLists = {
                    new RelicAffix
                    {
                        AffixId = 6,
                        Step = 100
                    },
                    new RelicAffix
                    {
                        AffixId = 7,
                        Step = 150
                    },
                    new RelicAffix
                    {
                        AffixId = 4,
                        Step = 120
                    },
                    new RelicAffix
                    {
                        AffixId = 5,
                        Step = 10
                    }
                }
            };
            var hHHands = new BattleRelic
            {
                Id = 61012,
                Level = 15,
                MainAffixId = 1,
                SubAffixLists = {
                    new RelicAffix
                {
                    AffixId = 10,
                    Step = 400
                } }
            };
            var hHBody = new BattleRelic
            {
                Id = 61013,
                Level = 15,
                MainAffixId = 6,
                SubAffixLists = {new RelicAffix
                {
                    AffixId = 6,
                    Step = 10
                } }
            };
            var hHFeet = new BattleRelic
            {
                Id = 61014,
                Level = 15,
                MainAffixId = 4,
                SubAffixLists = {new RelicAffix
                {
                    AffixId = 4,
                    Step = 10
                } }
            };
            var hHBall = new BattleRelic
            {
                Id = 63025,
                Level = 15,
                MainAffixId = 1,
                SubAffixLists = {new RelicAffix
                {
                    AffixId = 6,
                    Step = 10
                } }
            };
            var hHRope = new BattleRelic
            {
                Id = 63026,
                Level = 15,
                MainAffixId = 2,
                SubAffixLists = {new RelicAffix
                {
                    AffixId = 4,
                    Step = 10
                } }
            };
                        
            var fXHead = new BattleRelic
            {
                Id = 61061,
                Level = 15,
                MainAffixId = 1,
                SubAffixLists = {
                    new RelicAffix
                    {
                        AffixId = 6,
                        Step = 100
                    },
                    new RelicAffix
                    {
                        AffixId = 7,
                        Step = 150
                    },
                    new RelicAffix
                    {
                        AffixId = 4,
                        Step = 100
                    },
                    new RelicAffix
                    {
                        AffixId = 5,
                        Step = 10
                    }
                }
            };
            var fXHands = new BattleRelic
            {
                Id = 61062,
                Level = 15,
                MainAffixId = 1,
                SubAffixLists = {
                    new RelicAffix
                {
                    AffixId = 10,
                    Step = 400
                } }
            };
            var fXBody = new BattleRelic
            {
                Id = 61133,
                Level = 15,
                MainAffixId = 1,
                SubAffixLists = {new RelicAffix
                {
                    AffixId = 6,
                    Step = 10
                } }
            };
            var fXFeet = new BattleRelic
            {
                Id = 61134,
                Level = 15,
                MainAffixId = 4,
                SubAffixLists = {new RelicAffix
                {
                    AffixId = 4,
                    Step = 10
                } }
            };
            var fXBall = new BattleRelic
            {
                Id = 63025,
                Level = 15,
                MainAffixId = 1,
                SubAffixLists = {new RelicAffix
                {
                    AffixId = 6,
                    Step = 10
                } }
            };
            var fXRope = new BattleRelic
            {
                Id = 63026,
                Level = 15,
                MainAffixId = 2,
                SubAffixLists = {new RelicAffix
                {
                    AffixId = 4,
                    Step = 10
                } }
            };
                        
            var bsHead = new BattleRelic
            {
                Id = 61161,
                Level = 15,
                MainAffixId = 1,
                SubAffixLists =
                {
                    new RelicAffix
                    {
                        AffixId = 5,
                        Step = 40,
                    },
                    new RelicAffix
                    {
                        AffixId = 7,
                        Step = 70,
                    },
                    new RelicAffix
                    {
                        AffixId = 10,
                        Step = 200,
                    },
                    new RelicAffix
                    {
                        AffixId = 6,
                        Step = 60,
                    }
                }
            };
            var bsHands = new BattleRelic
            {
                Id = 61162,
                Level = 10,
                MainAffixId = 1,
                SubAffixLists = {
                    new RelicAffix
                    {
                        AffixId = 1,
                        Step = 10
                    },
                    
                }
            };
            var bsBody = new BattleRelic
            {
                Id = 61163,
                Level = 10,
                MainAffixId = 7,
                SubAffixLists = {
                    new RelicAffix
                    {
                        AffixId = 1,
                        Step = 10
                    },
                    
                }
            };
            var bsFeet = new BattleRelic
            {
                Id = 61164,
                Level = 10,
                MainAffixId = 4,
                SubAffixLists = {
                    new RelicAffix
                    {
                        AffixId = 1,
                        Step = 10
                    },
                }
            };
            var bsBall = new BattleRelic
            {
                Id = 63035,
                Level = 10,
                MainAffixId = 8,
                SubAffixLists = {
                    new RelicAffix
                    {
                        AffixId = 1,
                        Step = 10
                    }
                
                }
            };
            var bsRope = new BattleRelic
{
    Id = 63036,
    Level = 10,
    MainAffixId = 4,
    SubAffixLists = {
        new RelicAffix
        {
            AffixId = 1,
            Step = 10
        }
    }
};

            var kafkaHead = new BattleRelic
            {
                Id = 61161,
                Level = 15,
                MainAffixId = 1,
                SubAffixLists =
                {
                    new RelicAffix
                    {
                        AffixId = 5,
                        Step = 80,
                    },
                    new RelicAffix
                    {
                        AffixId = 7,
                        Step = 50,
                    },
                    new RelicAffix
                    {
                        AffixId = 10,
                        Step = 200,
                    },
                    new RelicAffix
                    {
                        AffixId = 6,
                        Step = 60,
                    }
                }
            };
            var kafkaHands = new BattleRelic
            {
                Id = 61162,
                Level = 10,
                MainAffixId = 1,
                SubAffixLists = {
                    new RelicAffix
                    {
                        AffixId = 1,
                        Step = 10
                    },
                    
                }
            };
            var kafkaBody = new BattleRelic
            {
                Id = 61163,
                Level = 10,
                MainAffixId = 2,
                SubAffixLists = {
                    new RelicAffix
                    {
                        AffixId = 1,
                        Step = 10
                    },
                    
                }
            };
            var kafkaFeet = new BattleRelic
            {
                Id = 61164,
                Level = 10,
                MainAffixId = 4,
                SubAffixLists = {
                    new RelicAffix
                    {
                        AffixId = 1,
                        Step = 10
                    },
                }
            };
            var kafkaBall = new BattleRelic
            {
                Id = 63115,
                Level = 10,
                MainAffixId = 7,
                SubAffixLists = {
                    new RelicAffix
                    {
                        AffixId = 1,
                        Step = 10
                    }
                
                }
            };
            var kafkaRope = new BattleRelic
            {
                Id = 63116,
                Level = 10,
                MainAffixId = 4,
                SubAffixLists = {
                    new RelicAffix
                    {
                        AffixId = 1,
                        Step = 10
                    }
                
                }
            };

            var rMHead = new BattleRelic
            {
                Id = 61141,
                Level = 15,
                MainAffixId = 1,
                SubAffixLists =
                {
                    new RelicAffix
                    {
                        AffixId = 12,
                        Step = 200,
                    },
                    new RelicAffix
                    {
                        AffixId = 7,
                        Step = 200,
                    },
                    new RelicAffix
                    {
                        AffixId = 1,
                        Step = 10,
                    },
                    new RelicAffix
                    {
                        AffixId = 6,
                        Step = 60,
                    }
                }
            };
            var rMHands = new BattleRelic
            {
                Id = 61142,
                Level = 10,
                MainAffixId = 1,
                SubAffixLists = {
                    new RelicAffix
                    {
                        AffixId = 1,
                        Step = 10
                    },
                    
                }
            };
            var rMBody = new BattleRelic
            {
                Id = 61143,
                Level = 10,
                MainAffixId = 1,
                SubAffixLists = {
                    new RelicAffix
                    {
                        AffixId = 1,
                        Step = 10
                    },
                    
                }
            };
            var rMFeet = new BattleRelic
            {
                Id = 61144,
                Level = 10,
                MainAffixId = 4,
                SubAffixLists = {
                    new RelicAffix
                    {
                        AffixId = 1,
                        Step = 10
                    },
                }
            };
            var rMBall = new BattleRelic
            {
                Id = 63025,
                Level = 10,
                MainAffixId = 3,
                SubAffixLists = {
                    new RelicAffix
                    {
                        AffixId = 1,
                        Step = 10
                    }
                
                }
            };
            var rMRope = new BattleRelic
            {
                Id = 63026,
                Level = 10,
                MainAffixId = 2,
                SubAffixLists = {
                    new RelicAffix
                    {
                        AffixId = 1,
                        Step = 10
                    }
                
                }
            };

            var bHead = new BattleRelic
            {
                Id = 61141,
                Level = 15,
                MainAffixId = 1,
                SubAffixLists =
                {
                    new RelicAffix
                    {
                        AffixId = 9,
                        Step = 100,
                    },
                    new RelicAffix
                    {
                        AffixId = 7,
                        Step = 50,
                    },
                    new RelicAffix
                    {
                        AffixId = 1,
                        Step = 10,
                    },
                    new RelicAffix
                    {
                        AffixId = 6,
                        Step = 60,
                    }
                }
            };
            var bHands = new BattleRelic
            {
                Id = 61142,
                Level = 15,
                MainAffixId = 1,
                SubAffixLists = {
                    new RelicAffix
                    {
                        AffixId = 1,
                        Step = 10
                    },
                    
                }
            };
            var bBody = new BattleRelic
            {
                Id = 61143,
                Level = 15,
                MainAffixId = 5,
                SubAffixLists = {
                    new RelicAffix
                    {
                        AffixId = 1,
                        Step = 10
                    },
                    
                }
            };
            var bFeet = new BattleRelic
            {
                Id = 61144,
                Level = 15,
                MainAffixId = 4,
                SubAffixLists = {
                    new RelicAffix
                    {
                        AffixId = 1,
                        Step = 10
                    },
                }
            };
            var bBall = new BattleRelic
            {
                Id = 63105,
                Level = 15,
                MainAffixId = 3,
                SubAffixLists = {
                    new RelicAffix
                    {
                        AffixId = 11,
                        Step = 120
                    }
                
                }
            };
            var bRope = new BattleRelic
            {
                Id = 63106,
                Level = 15,
                MainAffixId = 2,
                SubAffixLists = {
                    new RelicAffix
                    {
                        AffixId = 1,
                        Step = 10
                    }
                
                }
            };

            var jLHead = new BattleRelic
            {
                Id = 61041,
                Level = 15,
                MainAffixId = 1,
                SubAffixLists = {
                    new RelicAffix
                    {
                        AffixId = 8,
                        Step = 60
                    },
                    new RelicAffix
                    {
                        AffixId = 9,
                        Step = 120
                    },
                    new RelicAffix
                    {
                        AffixId = 7,
                        Step = 60
                    },
                    new RelicAffix
                    {
                        AffixId = 5,
                        Step = 30
                    }
                }
            };
            var jLHands = new BattleRelic
            {
                Id = 61042,
                Level = 15,
                MainAffixId = 1,
                SubAffixLists = {
                    new RelicAffix
                {
                    AffixId = 12,
                    Step = 80
                } }
            };
            var jLBody = new BattleRelic
            {
                Id = 61043,
                Level = 15,
                MainAffixId = 5,
                SubAffixLists = {new RelicAffix
                {
                    AffixId = 4,
                    Step = 10
                } }
            };
            var jLFeet = new BattleRelic
            {
                Id = 61044,
                Level = 15,
                MainAffixId = 4,
                SubAffixLists = {new RelicAffix
                {
                    AffixId = 4,
                    Step = 10
                } }
            };
            var jLBall = new BattleRelic
            {
                Id = 63095,
                Level = 15,
                MainAffixId = 6,
                SubAffixLists = {new RelicAffix
                {
                    AffixId = 4,
                    Step = 10
                } }
            };
            var jLRope = new BattleRelic
            {
                Id = 63096,
                Level = 15,
                MainAffixId = 4,
                SubAffixLists = {new RelicAffix
                {
                    AffixId = 4,
                    Step = 10
                } }
            };


// Add more BattleRelic objects for bsBody, bsFeet, bsBall, bsRope
            



            //avatar
            List<uint> SkillIdEnds = new List<uint> { 1, 2, 3, 4, 7, 101, 102, 103, 201, 202, 203, 204, 205, 206, 207, 208, 209, 210 };
            List<uint> characters = new List<uint> { Avatar1, Avatar2, Avatar3, Avatar4 };
            List<uint> LightCones = new List<uint> { 23024, 23003, 23015, 23023 };
            
            //var FXRelics = new List<BattleRelic> { FXHead, FXHands, FXBody, FXFeet, FXBall, FXRope };

            foreach (uint avatarId in characters)
            {
                Console.WriteLine(avatarId);
                var avatarData = new BattleAvatar();

                var lc = new uint();
                var r = new uint();
                lc = LightCones[characters.IndexOf(avatarId)];
                r = 5;
                // if (lc == 23014) r = 5;
                // else if (lc == 23003) r = 1;
                // /*else if (lc == 21015) r = 5;
                // else if (lc == 23022) r = 5;*/
                // else r = 5;

                if (characters.IndexOf(avatarId) == 0) {
                    avatarData = new BattleAvatar {
                        Id = avatarId,
                        Level = 80,
                        Promotion = 6,
                        Rank = 6,
                        Hp = 10000,
                        AvatarType = AvatarType.AvatarFormalType,
                        WorldLevel = 6,
                        Sp = new AmountInfo { CurAmount = 10000, MaxAmount = 10000 },
                        RelicLists = { acheronHead, acheronHands, acheronBody, acheronFeet, acheronBall, acheronRope},
                        EquipmentLists = {new BattleEquipment
                        {
                            Id = lc,
                            Level = 80,
                            Rank = r,
                            Promotion = 6
                        } }
                    };
                }

                else if (characters.IndexOf(avatarId) == 1){
                    avatarData = new BattleAvatar
                    {
                        Id = avatarId,
                        Level = 80,
                        Promotion = 6,
                        Rank = 6,
                        Hp = 10000,
                        AvatarType = AvatarType.AvatarFormalType,
                        WorldLevel = 6,
                        Sp = new AmountInfo { CurAmount = 10000, MaxAmount = 10000 },
                        RelicLists = { acheronHeadOP, acheronHands, acheronBodyOP, acheronFeet, acheronBallOP, acheronRope},
                        EquipmentLists = {new BattleEquipment
                        {
                            Id = lc,
                            Level = 80,
                            Rank = r,
                            Promotion = 6
                        } }
                    };
                }
                else if (characters.IndexOf(avatarId) == 2){
                    avatarData = new BattleAvatar
                    {
                        Id = avatarId,
                        Level = 80,
                        Promotion = 6,
                        Rank = 6,
                        Hp = 10000,
                        AvatarType = AvatarType.AvatarFormalType,
                        WorldLevel = 6,
                        Sp = new AmountInfo { CurAmount = 10000, MaxAmount = 10000 },
                        RelicLists = { aventurineHead, aventurineHands, aventurineBody, aventurineFeet, aventurineBall, aventurineRope},
                        EquipmentLists = {new BattleEquipment
                        {
                            Id = lc,
                            Level = 80,
                            Rank = r,
                            Promotion = 6
                        } }
                    };
                }

                else {

                    avatarData = new BattleAvatar
                    {
                        Id = avatarId,
                        Level = 80,
                        Promotion = 6,
                        Rank = 6,
                        Hp = 10000,
                        AvatarType = AvatarType.AvatarFormalType,
                        WorldLevel = 6,
                        Sp = new AmountInfo { CurAmount = 10000, MaxAmount = 10000 },
                        RelicLists = { aventurineHeadOP, aventurineHands, aventurineBodyOP, aventurineFeet, aventurineBallOP, aventurineRope},
                        EquipmentLists = {new BattleEquipment
                        {
                            Id = lc,
                            Level = 80,
                            Rank = r,
                            Promotion = 6
                        } }
                    };
                };

                foreach (uint end in SkillIdEnds)
                {
                    uint level = 1;
                    if (end == 1) level = 6;
                    else if (end < 4 || end == 4) level = 10;
                    if (end > 4) level = 1;
                    avatarData.SkilltreeLists.Add(new AvatarSkillTree
                    {
                        PointId = avatarId * 1000 + end,
                        Level = level
                    });
                }

                battleInfo.BattleAvatarLists.Add(avatarData);
            }


            //monster
            for (uint i = 1; i <= monsterIds.Count; i++)
            {
                SceneMonsterWave monsterInfo = new SceneMonsterWave
                {
                    Pkgenfbhofi = i,
                    MonsterParam = new SceneMonsterParam
                    {
                        Level = monsterLevels[i],
                    }
                };

                if (monsterIds.ContainsKey(i))
                {
                    List<uint> monsterIdList = monsterIds[i];

                    foreach (uint monsterId in monsterIdList)
                    {
                        monsterInfo.MonsterLists.Add(new SceneMonsterInfo
                        {
                            MonsterId = monsterId
                        });
                    }
                    
                }
                battleInfo.MonsterWaveLists.Add(monsterInfo);
            }

            var response = new StartCocoonStageScRsp
            {
                Retcode = 0,
                CocoonId = request.CocoonId,
                Wave = request.Wave,
                PropEntityId = request.PropEntityId,
                BattleInfo = battleInfo
            };

            session.Send(CmdType.CmdStartCocoonStageScRsp, response);
        }

        [Handler(CmdType.CmdPVEBattleResultCsReq)]
        public static void OnPVEBattleResultCsReq(NetSession session, int cmdId, object data)
        {
            var request = data as PVEBattleResultCsReq;
            session.Send(CmdType.CmdPVEBattleResultScRsp, new PVEBattleResultScRsp
            {
                Retcode = 0,
                EndStatus = request.EndStatus
            });
        }
    }
}
