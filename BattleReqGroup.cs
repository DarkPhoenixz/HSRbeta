using FreeSR.Gateserver.Manager.Handlers.Core;
using static FreeSR.Gateserver.Manager.Handlers.LineupReqGroup;
using FreeSR.Gateserver.Network;
using FreeSR.Proto;
using Newtonsoft.Json;

namespace FreeSR.Gateserver.Manager.Handlers
{
    internal static class BattleReqGroup
    {
        [Handler(CmdType.CmdSetLineupNameCsReq)]
        public static void OnSetLineupNameCsReq(NetSession session, int cmdId, object data)
        {
            var request = data as SetLineupNameCsReq;
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
                    Sp = new AmountInfo { CurAmount = 10000, MaxAmount = 10000 },
                    AvatarType = AvatarType.AvatarFormalType,
                    Slot = (uint)lineupInfo.AvatarLists.Count
                });
            }

            var sceneInfo = new SceneInfo
            {
                GameModeType = 2,
                EntryId = 2032101,
                PlaneId = 20321,
                FloorId = 20321001
            };

            //Splitting rename by spaces
            string[] Word = request.Name.Split(' ');
            switch (Word[0])
            {
                case "battle":
                    DreamsEdgeCalyx(session, lineupInfo);
                    break;
                case "battle1":
                    StorageZoneCalyx(session, lineupInfo);
                    break;
                case "battle2":
                    SnowPlainsCalyx(session, lineupInfo);
                    break;
                case "battle3":
                    BackwaterPassCalyx(session, lineupInfo);
                    break;
                case "battle4":
                    GreatMineCalyx(session, lineupInfo);
                    break;
                case "battle5":
                    RivetTownCalyx(session, lineupInfo);
                    break;
                case "battle6":
                    CloudfordCalyx(session, lineupInfo);
                    break;
                case "battle7":
                    DivinationCalyx(session, lineupInfo);
                    break;
                case "battle8":
                    WaterscapeCalyx(session, lineupInfo);
                    break;
                case "battle9":
                    SpookyGardenCavern(session, lineupInfo);
                    break;
                case "battle10":
                    ReverieDreamCalyx(session, lineupInfo);
                    break;
                case "battle11":
                    DreamsEdgeCalyx(session, lineupInfo);
                    break;
                case "battle12":
                    DestructionEcho(session, lineupInfo);
                    break;
                case "battle13":
                    FrozenEcho(session, lineupInfo);
                    break;
                case "battle14":
                    PhantyliaEcho(session, lineupInfo);
                    break;
                case "battle15":
                    BigBugEcho(session, lineupInfo);
                    break;
                case "Tp":
                    //Teleporting by sceneid
                    sceneInfo.EntryId = uint.Parse(Word[1] + "01");
                    sceneInfo.PlaneId = uint.Parse(Word[1]);
                    sceneInfo.FloorId = uint.Parse(Word[1] + "001");

                    session.Send(CmdType.CmdEnterSceneByServerScNotify, new EnterSceneByServerScNotify
                    {
                        Scene = sceneInfo,
                        Lineup = lineupInfo
                    });
                    break;
                default:
                    session.Send(CmdType.CmdEnterSceneByServerScNotify, new EnterSceneByServerScNotify
                    {
                        Scene = sceneInfo,
                        Lineup = lineupInfo
                    });
                    break;
            }

            session.Send(CmdType.CmdSetLineupNameScRsp, new SetLineupNameScRsp
            {
                Retcode = 0,
                Name = request.Name,
                Index = request.Index
            });
        }


        [Handler(CmdType.CmdStartCocoonStageCsReq)]
        public static void OnStartCocoonStageCsReq(NetSession session, int cmdId, object data)
        {
            var request = data as StartCocoonStageCsReq;

            Dictionary<uint, List<uint>> monsterIds = new Dictionary<uint, List<uint>>();

            Dictionary<uint, uint> monsterLevels = new Dictionary<uint, uint>();

            // basic
            var battleInfo = new SceneBattleInfo
            {
                StageId = 201012311, // calyx
                LogicRandomSeed = (uint)new Random().Next(),
                WorldLevel = 6,
                Jlmibpcgfgb = 30, // round limit (not working outside challenge)
            };

            // avatar
            List<uint> SkillIdEnds = new List<uint> { 1, 2, 3, 4, 7, 101, 102, 103, 201, 202, 203, 204, 205, 206, 207, 208, 209, 210 };
            List<uint> characters = new List<uint> { Avatar1, Avatar2, Avatar3, Avatar4 };

            var battleAvatars = new Dictionary<uint, BattleAvatar>();
            var battleAvatarBuffs = new Dictionary<uint, List<uint>>();
            var pureFictionBlessings = new List<uint>();

            // freesrtools json parse
            try
            {
                using StreamReader reader = new StreamReader("freesr-data.json");
                var payload = JsonConvert.DeserializeObject<Payload>(reader.ReadToEnd());
                // avatars
                foreach (var item in payload.avatars)
                {
                    var avatarJson = item.Value;
                    var lightconeIndex = payload.lightcones.FindIndex((v) => v.equipAvatar == avatarJson.avatarId);
                    var relicsJson = payload.relics.FindAll((v) => v.equipAvatar == avatarJson.avatarId);

                    // avatar
                    var avatar = new BattleAvatar
                    {
                        Id = (uint)avatarJson.avatarId,
                        Level = (uint)avatarJson.level,
                        Promotion = (uint)avatarJson.promotion,
                        Rank = (uint)avatarJson.data.rank,
                        Hp = 10000,
                        AvatarType = AvatarType.AvatarFormalType,
                        WorldLevel = 6,
                        Sp = new AmountInfo { CurAmount = 10000, MaxAmount = 10000 },
                        RelicLists = { },
                        EquipmentLists = { }
                    };

                    // set energy
                    if (avatarJson.spValue != null /*&& avatarJson.spValue > 0*/ && avatarJson.spMax != null && avatarJson.spMax >= avatarJson.spValue)
                    {
                        avatar.Sp.CurAmount = (uint)avatarJson.spValue;
                        avatar.Sp.MaxAmount = (uint)avatarJson.spMax;
                    }

                    // set technique
                    if (avatarJson.useTechnique != null && avatarJson.useTechnique.Count > 0)
                    {
                        battleAvatarBuffs.Add(avatar.Id, new List<uint>());
                        foreach (uint techId in avatarJson.useTechnique)
                        {
                            battleAvatarBuffs[avatar.Id].Add(techId);
                        }
                    }

                    // lightcone
                    if (lightconeIndex > -1)
                    {
                        var lightcone = payload.lightcones[lightconeIndex];
                        avatar.EquipmentLists.Add(
                            new BattleEquipment
                            {
                                Id = (uint)lightcone.itemId,
                                Level = (uint)lightcone.level,
                                Promotion = (uint)lightcone.promotion,
                                Rank = (uint)lightcone.rank,
                            }
                        );
                    }


                    // relics
                    foreach (var relic in relicsJson)
                    {
                        var relicData = new BattleRelic
                        {
                            Id = (uint)relic.relicId,
                            Level = (uint)relic.level,
                            MainAffixId = (uint)relic.mainAffixId,
                            SubAffixLists = { }
                        };
                        foreach (var subAffix in relic.subAffixes)
                        {
                            relicData.SubAffixLists.Add(new RelicAffix
                            {
                                AffixId = (uint)subAffix.subAffixId,
                                Cnt = (uint)subAffix.count,
                                Step = (uint)subAffix.step,
                            });
                        }
                        avatar.RelicLists.Add(relicData);
                    }

                    // skills
                    foreach (var skill in avatarJson.data.skills)
                    {
                        var skillId = (uint)skill.Key;
                        var level = (uint)skill.Value;
                        avatar.SkilltreeLists.Add(
                            new AvatarSkillTree
                            {
                                PointId = (uint)skillId,
                                Level = level,
                            }
                        );
                    }


                    battleAvatars.Add(avatar.Id, avatar);
                }

                //monsters
                uint id = 1;
                foreach (var wave in payload.monsters)
                {
                    var monsters = new List<uint>();
                    foreach (var monster in wave)
                    {
                        // set stage id for turbulence
                        if (monster.stageId != null && monster.stageId > 0)
                        {
                            battleInfo.StageId = (uint)monster.stageId;
                        }

                        // cycle count
                        if (monster.cycleCount != null && monster.cycleCount > 0)
                        {
                            battleInfo.Jlmibpcgfgb = (uint)monster.cycleCount;
                        }

                        // detect purefiction
                        if (monster.pureFictionBlessings != null && monster.pureFictionBlessings.Count > 0)
                        {
                            pureFictionBlessings = monster.pureFictionBlessings;
                        }

                        monsters.Add((uint)monster.monsterId);
                    }

                    var level = wave.Aggregate(0, (prev, cur) =>
                        prev < cur.level ? cur.level : prev
                    );
                    monsterIds[id] = monsters;
                    monsterLevels[id] = (uint)level;
                    id++;
                }

                // Simulated
                if (payload.simulatedUniverseData != null && payload.simulatedUniverseData.stageId > 0)
                {
                    // apply stage id
                    battleInfo.StageId = payload.simulatedUniverseData.stageId;

                    // apply buffs
                    foreach (var buff in payload.simulatedUniverseData.buffList)
                    {
                        var battleBuff = new BattleBuff
                        {
                            Bbbejilfdhi = 0xffffffff,
                            Eleplppdjbo = 0xffffffff,
                            Level = buff.level,
                            Id = buff.id,
                        };
                        if (buff.dynamicValue != null)
                        {
                            battleBuff.Gpijnfkggnds.Add(buff.dynamicValue.key, buff.dynamicValue.value);
                        }

                        battleInfo.BuffLists.Add(battleBuff);
                    }

                    // apply battle event (for now it will be full)
                    battleInfo.EventBattleInfoLists.Add(new BattleEventBattleInfo
                    {
                        Beoagdlcfin = payload.simulatedUniverseData.pathResonanceEventId,
                        Status = new BattleEventProperty
                        {
                            Sp = new AmountInfo
                            {
                                CurAmount = 10000,
                                MaxAmount = 10000
                            }
                        }

                    });

                    // apply stat list
                    foreach (var stat in payload.simulatedUniverseData.statList)
                    {
                        foreach (var avatarId in characters)
                        {
                            // create new relic
                            if (battleAvatars[avatarId].RelicLists.Count < 1)
                            {
                                var relicData = new BattleRelic
                                {
                                    Id = 61011,
                                    Level = 1,
                                    MainAffixId = 1,
                                    SubAffixLists = { }
                                };

                                battleAvatars[avatarId].RelicLists.Add(relicData);
                            }

                            var subAffixIndex = battleAvatars[avatarId].RelicLists[0].SubAffixLists.FindIndex(0, (v) => v.AffixId.Equals(stat.subAffixId));

                            // already exist, 
                            if (subAffixIndex > -1)
                            {
                                battleAvatars[avatarId].RelicLists[0].SubAffixLists[subAffixIndex].Cnt += (uint)stat.count;
                            }
                            else
                            {
                                battleAvatars[avatarId].RelicLists[0].SubAffixLists.Add(new RelicAffix
                                {
                                    AffixId = (uint)stat.subAffixId,
                                    Cnt = (uint)stat.count,
                                    Step = (uint)stat.step,
                                });
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            // monsters sanity check
            if (monsterIds.Count == 0)
            {
                monsterIds.Add(1, new List<uint> { 3014022 });
            }
            if (monsterLevels.Count == 0)
            {
                monsterLevels.Add(1, 60);
            }

            // ?????
            int index = -1;
            foreach (uint avatarId in characters)
            {
                // ????????
                index++;

                BattleAvatar avatarData;

                // set avatar
                if (battleAvatars.TryGetValue(avatarId, out BattleAvatar value))
                {
                    avatarData = value;
                }
                // fallback
                else
                {
                    if (avatarId == 0) continue;
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
                        RelicLists = {
                                //begin relic loop
                                new BattleRelic
                                {
                                    Id = 61011,
                                    Level = 999,
                                    MainAffixId = 1,
                                    SubAffixLists = {
                                        new RelicAffix{AffixId = 4, Step = 999 },
                                    }
                                }
                                //end relic loop
                            },
                        EquipmentLists = {
                                //begin equipment
                                new BattleEquipment
                                {
                                    Id = 23006,
                                    Level = 80,
                                    Rank = 5,
                                    Promotion = 6
                                } 
                                //end equipment
                            }
                    };
                    foreach (uint end in SkillIdEnds)
                    {
                        uint level = 1;
                        if (end == 1) level = 6; // basic atk
                        else if (end < 4 || end == 4) level = 10; // skill, talent, ult
                        if (end > 4) level = 1; // technique
                        avatarData.SkilltreeLists.Add(new AvatarSkillTree
                        {
                            PointId = avatarId * 1000 + end,
                            Level = level
                        });
                    }
                }

                // set technique
                if (battleAvatarBuffs.TryGetValue(avatarId, out List<uint> buffs))
                {

                    foreach (var buffId in buffs)
                    {
                        var buff = new BattleBuff();
                        buff.Bbbejilfdhi = 0xffffffff; //(uint)(1 << monsterIds.Count); // waveflag
                        buff.Eleplppdjbo = (uint)index; // ownerid
                        buff.Level = 1;
                        buff.Id = buffId;
                        battleInfo.BuffLists.Add(buff);
                    }
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
                    },
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

            // pure fiction
            if (pureFictionBlessings.Count > 0)
            {
                // pf score counter
                var bt = new BattleTargeList();
                bt.Akhbfomjeibs.Add(new BattleTargeInfo { Id = 10001, Progress = 0 });
                battleInfo.BattleTargeInfoes.Add(1, bt);

                // pf param
                for (uint i = 2; i <= 4; i++)
                {
                    battleInfo.BattleTargeInfoes.Add(i, new BattleTargeList());
                }
                //pf param 
                battleInfo.BattleTargeInfoes.Add(5, new BattleTargeList
                {
                    Akhbfomjeibs =
                    {
                        new BattleTargeInfo{ Id = 2001, Progress = 0 },
                        new BattleTargeInfo{ Id = 2002, Progress = 0 },
                    }
                });


                // pf blessing
                foreach (var blessing in pureFictionBlessings)
                {
                    battleInfo.BuffLists.Add(new BattleBuff
                    {
                        Bbbejilfdhi = 0xffffffff,
                        Eleplppdjbo = 0xffffffff,
                        Level = 1,
                        Id = blessing
                    });
                }
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

        // Calyx config (thanks to neon137.)
        internal static void StorageZoneCalyx(NetSession session, LineupInfo lineupInfo)
        {
            var sceneInfo = new SceneInfo
            {
                GameModeType = 2,
                EntryId = 2000201,
                PlaneId = 20002,
                FloorId = 20002001
            };

            SceneGroupEntityInfo CalyxStorageZone = new SceneGroupEntityInfo
            {
                State = 0,
                GroupId = 99
            };

            CalyxStorageZone.EntityLists.Add(new SceneEntityInfo
            {
                GroupId = 99,
                InstId = 300001,
                EntityId = 5,
                Prop = new ScenePropInfo
                {
                    PropState = 8,
                    PropId = 801
                },
                Motion = new MotionInfo
                {
                    Pos = new Vector
                    {
                        X = -153260,
                        Y = -163,
                        Z = -61520
                    },
                    Rot = new Vector
                    {
                        Y = 180000
                    }
                },
            });

            sceneInfo.SceneGroupEntityLists.Add(CalyxStorageZone);

            session.Send(CmdType.CmdEnterSceneByServerScNotify, new EnterSceneByServerScNotify
            {
                Scene = sceneInfo,
                Lineup = lineupInfo
            });

            session.Send(CmdType.CmdSceneEntityMoveScNotify, new SceneEntityMoveScNotify
            {
                EntryId = 2000201,
                EntityId = 0,
                Motion = new MotionInfo
                {
                    Pos = new Vector
                    {
                        X = -153260,
                        Y = -163,
                        Z = -61520
                    },
                    Rot = new Vector
                    {
                        Y = 180000
                    }
                }
            });
        }

        internal static void SnowPlainsCalyx(NetSession session, LineupInfo lineupInfo)
        {
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

        internal static void BackwaterPassCalyx(NetSession session, LineupInfo lineupInfo)
        {
            var sceneInfo = new SceneInfo
            {
                GameModeType = 2,
                EntryId = 2011101,
                PlaneId = 20111,
                FloorId = 20111001
            };

            SceneGroupEntityInfo CalyxBackwaterPass = new SceneGroupEntityInfo
            {
                State = 0,
                GroupId = 26
            };

            CalyxBackwaterPass.EntityLists.Add(new SceneEntityInfo
            {
                GroupId = 26,
                InstId = 300001,
                EntityId = 23,
                Prop = new ScenePropInfo
                {
                    PropState = 8,
                    PropId = 801
                },
                Motion = new MotionInfo
                {
                    Pos = new Vector
                    {
                        X = -20000,
                        Y = 1876,
                        Z = 9600
                    },
                    Rot = new Vector
                    {
                        Y = 65657
                    }
                },
            });

            sceneInfo.SceneGroupEntityLists.Add(CalyxBackwaterPass);

            session.Send(CmdType.CmdEnterSceneByServerScNotify, new EnterSceneByServerScNotify
            {
                Scene = sceneInfo,
                Lineup = lineupInfo
            });

            session.Send(CmdType.CmdSceneEntityMoveScNotify, new SceneEntityMoveScNotify
            {
                EntryId = 2011101,
                EntityId = 0,
                Motion = new MotionInfo
                {
                    Pos = new Vector
                    {
                        X = -20000,
                        Y = 1876,
                        Z = 9600
                    },
                    Rot = new Vector
                    {
                        Y = 240000
                    }
                }
            });
        }

        internal static void GreatMineCalyx(NetSession session, LineupInfo lineupInfo)
        {
            var sceneInfo = new SceneInfo
            {
                GameModeType = 2,
                EntryId = 2012101,
                PlaneId = 20121,
                FloorId = 20121001
            };

            SceneGroupEntityInfo CalyxGreatMine = new SceneGroupEntityInfo
            {
                State = 0,
                GroupId = 50
            };

            CalyxGreatMine.EntityLists.Add(new SceneEntityInfo
            {
                GroupId = 50,
                InstId = 300001,
                EntityId = 10,
                Prop = new ScenePropInfo
                {
                    PropState = 8,
                    PropId = 801
                },
                Motion = new MotionInfo
                {
                    Pos = new Vector
                    {
                        X = 186417,
                        Y = 55112,
                        Z = -167952
                    },
                    Rot = new Vector
                    {
                        Y = 114081
                    }
                },
            });

            sceneInfo.SceneGroupEntityLists.Add(CalyxGreatMine);

            session.Send(CmdType.CmdEnterSceneByServerScNotify, new EnterSceneByServerScNotify
            {
                Scene = sceneInfo,
                Lineup = lineupInfo
            });

            session.Send(CmdType.CmdSceneEntityMoveScNotify, new SceneEntityMoveScNotify
            {
                EntryId = 2012101,
                EntityId = 0,
                Motion = new MotionInfo
                {
                    Pos = new Vector
                    {
                        X = 186417,
                        Y = 55112,
                        Z = -167952
                    },
                    Rot = new Vector
                    {
                        Y = 240000
                    }
                }
            });
        }

        internal static void RivetTownCalyx(NetSession session, LineupInfo lineupInfo)
        {
            var sceneInfo = new SceneInfo
            {
                GameModeType = 2,
                EntryId = 2012201,
                PlaneId = 20122,
                FloorId = 20122001
            };

            SceneGroupEntityInfo CalyxRivetTown = new SceneGroupEntityInfo
            {
                State = 0,
                GroupId = 37
            };

            CalyxRivetTown.EntityLists.Add(new SceneEntityInfo
            {
                GroupId = 37,
                InstId = 300001,
                EntityId = 72,
                Prop = new ScenePropInfo
                {
                    PropState = 8,
                    PropId = 801
                },
                Motion = new MotionInfo
                {
                    Pos = new Vector
                    {
                        X = -32130,
                        Y = -97,
                        Z = 51230
                    },
                    Rot = new Vector
                    {
                        Y = 171385
                    }
                },
            });

            sceneInfo.SceneGroupEntityLists.Add(CalyxRivetTown);

            session.Send(CmdType.CmdEnterSceneByServerScNotify, new EnterSceneByServerScNotify
            {
                Scene = sceneInfo,
                Lineup = lineupInfo
            });

            session.Send(CmdType.CmdSceneEntityMoveScNotify, new SceneEntityMoveScNotify
            {
                EntryId = 2012201,
                EntityId = 0,
                Motion = new MotionInfo
                {
                    Pos = new Vector
                    {
                        X = -32130,
                        Y = -97,
                        Z = 50230
                    },
                    Rot = new Vector
                    {
                        Y = 171385
                    }
                }
            });
        }

        internal static void CloudfordCalyx(NetSession session, LineupInfo lineupInfo)
        {
            var sceneInfo = new SceneInfo
            {
                GameModeType = 2,
                EntryId = 2021101,
                PlaneId = 20211,
                FloorId = 20211001
            };

            SceneGroupEntityInfo CalyxRivetTown = new SceneGroupEntityInfo
            {
                State = 0,
                GroupId = 234
            };

            CalyxRivetTown.EntityLists.Add(new SceneEntityInfo
            {
                GroupId = 234,
                InstId = 300001,
                EntityId = 69,
                Prop = new ScenePropInfo
                {
                    PropState = 8,
                    PropId = 801
                },
                Motion = new MotionInfo
                {
                    Pos = new Vector
                    {
                        X = -28461,
                        Y = 3929,
                        Z = -64812
                    },
                    Rot = new Vector
                    {
                        Y = 210000
                    }
                },
            });

            sceneInfo.SceneGroupEntityLists.Add(CalyxRivetTown);

            session.Send(CmdType.CmdEnterSceneByServerScNotify, new EnterSceneByServerScNotify
            {
                Scene = sceneInfo,
                Lineup = lineupInfo
            });

            session.Send(CmdType.CmdSceneEntityMoveScNotify, new SceneEntityMoveScNotify
            {
                EntryId = 2021101,
                EntityId = 0,
                Motion = new MotionInfo
                {
                    Pos = new Vector
                    {
                        X = -28461,
                        Y = 3929,
                        Z = -64812
                    },
                    Rot = new Vector
                    {
                        Y = 210000
                    }
                }
            });
        }

        internal static void DivinationCalyx(NetSession session, LineupInfo lineupInfo)
        {
            var sceneInfo = new SceneInfo
            {
                GameModeType = 2,
                EntryId = 2022101,
                PlaneId = 20221,
                FloorId = 20221001
            };

            SceneGroupEntityInfo CalyxDivination = new SceneGroupEntityInfo
            {
                State = 0,
                GroupId = 215
            };

            CalyxDivination.EntityLists.Add(new SceneEntityInfo
            {
                GroupId = 215,
                InstId = 300001,
                EntityId = 80,
                Prop = new ScenePropInfo
                {
                    PropState = 8,
                    PropId = 801
                },
                Motion = new MotionInfo
                {
                    Pos = new Vector
                    {
                        X = 53860,
                        Y = 10912,
                        Z = -114602
                    },
                    Rot = new Vector
                    {
                        Y = 210000
                    }
                },
            });

            sceneInfo.SceneGroupEntityLists.Add(CalyxDivination);

            session.Send(CmdType.CmdEnterSceneByServerScNotify, new EnterSceneByServerScNotify
            {
                Scene = sceneInfo,
                Lineup = lineupInfo
            });

            session.Send(CmdType.CmdSceneEntityMoveScNotify, new SceneEntityMoveScNotify
            {
                EntryId = 2022101,
                EntityId = 0,
                Motion = new MotionInfo
                {
                    Pos = new Vector
                    {
                        X = 53860,
                        Y = 10912,
                        Z = -114602
                    },
                    Rot = new Vector
                    {
                        Y = 240000
                    }
                }
            });
        }

        internal static void WaterscapeCalyx(NetSession session, LineupInfo lineupInfo)
        {
            var sceneInfo = new SceneInfo
            {
                GameModeType = 2,
                EntryId = 2023201,
                PlaneId = 20232,
                FloorId = 20232001
            };

            SceneGroupEntityInfo CalyxWaterscape = new SceneGroupEntityInfo
            {
                State = 0,
                GroupId = 129
            };

            CalyxWaterscape.EntityLists.Add(new SceneEntityInfo
            {
                GroupId = 129,
                InstId = 300001,
                EntityId = 58,
                Prop = new ScenePropInfo
                {
                    PropState = 8,
                    PropId = 801
                },
                Motion = new MotionInfo
                {
                    Pos = new Vector
                    {
                        X = -404640,
                        Y = -44468,
                        Z = -104123
                    },
                    Rot = new Vector
                    {
                        Y = 210000
                    }
                },
            });

            sceneInfo.SceneGroupEntityLists.Add(CalyxWaterscape);

            session.Send(CmdType.CmdEnterSceneByServerScNotify, new EnterSceneByServerScNotify
            {
                Scene = sceneInfo,
                Lineup = lineupInfo
            });

            session.Send(CmdType.CmdSceneEntityMoveScNotify, new SceneEntityMoveScNotify
            {
                EntryId = 2023201,
                EntityId = 0,
                Motion = new MotionInfo
                {
                    Pos = new Vector
                    {
                        X = -404640,
                        Y = -44468,
                        Z = -104123
                    },
                    Rot = new Vector
                    {
                        Y = 240000
                    }
                }
            });
        }

        internal static void SpookyGardenCavern(NetSession session, LineupInfo lineupInfo)
        {
            var sceneInfo = new SceneInfo
            {
                GameModeType = 2,
                EntryId = 2022301,
                PlaneId = 20223,
                FloorId = 20223001
            };

            SceneGroupEntityInfo GardenCavern = new SceneGroupEntityInfo
            {
                State = 0,
                GroupId = 67
            };

            GardenCavern.EntityLists.Add(new SceneEntityInfo
            {
                GroupId = 67,
                InstId = 300001,
                EntityId = 6,
                Prop = new ScenePropInfo
                {
                    PropState = 8,
                    PropId = 901
                },
                Motion = new MotionInfo
                {
                    Pos = new Vector
                    {
                        X = -56390,
                        Y = -12483,
                        Z = 21630
                    },
                    Rot = new Vector
                    {
                        X = 0,
                        Y = 320000,
                        Z = 0
                    }
                }
            });

            sceneInfo.SceneGroupEntityLists.Add(GardenCavern);

            session.Send(CmdType.CmdEnterSceneByServerScNotify, new EnterSceneByServerScNotify
            {
                Scene = sceneInfo,
                Lineup = lineupInfo
            });

            session.Send(CmdType.CmdSceneEntityMoveScNotify, new SceneEntityMoveScNotify
            {
                EntryId = 2022301,
                EntityId = 0,
                Motion = new MotionInfo
                {
                    Pos = new Vector
                    {
                        X = -56390,
                        Y = -12483,
                        Z = 21630
                    },
                    Rot = new Vector
                    {
                        Y = 320000
                    }
                }
            });
        }

        internal static void ReverieDreamCalyx(NetSession session, LineupInfo lineupInfo)
        {
            var sceneInfo = new SceneInfo
            {
                GameModeType = 2,
                EntryId = 2031201,
                PlaneId = 20312,
                FloorId = 20312001
            };

            SceneGroupEntityInfo GardenCavern = new SceneGroupEntityInfo
            {
                State = 0,
                GroupId = 219
            };

            GardenCavern.EntityLists.Add(new SceneEntityInfo
            {
                GroupId = 219,
                InstId = 300001,
                EntityId = 106,
                Prop = new ScenePropInfo
                {
                    PropState = 8,
                    PropId = 801
                },
                Motion = new MotionInfo
                {
                    Pos = new Vector
                    {
                        X = -122759,
                        Y = 11510,
                        Z = -165303
                    },
                    Rot = new Vector
                    {
                        Y = 340000
                    }
                }
            });

            sceneInfo.SceneGroupEntityLists.Add(GardenCavern);

            session.Send(CmdType.CmdEnterSceneByServerScNotify, new EnterSceneByServerScNotify
            {
                Scene = sceneInfo,
                Lineup = lineupInfo
            });

            session.Send(CmdType.CmdSceneEntityMoveScNotify, new SceneEntityMoveScNotify
            {
                EntryId = 2031201,
                EntityId = 0,
                Motion = new MotionInfo
                {
                    Pos = new Vector
                    {
                        X = -122759,
                        Y = 11510,
                        Z = -165303
                    },
                    Rot = new Vector
                    {
                        Y = 320000
                    }
                }
            });
        }

        internal static void DreamsEdgeCalyx(NetSession session, LineupInfo lineupInfo)
        {
            var sceneInfo = new SceneInfo
            {
                GameModeType = 2,
                EntryId = 2031301,
                PlaneId = 20313,
                FloorId = 20313001
            };

            SceneGroupEntityInfo calyxDreamsEdge = new SceneGroupEntityInfo
            {
                State = 0,
                GroupId = 186
            };

            calyxDreamsEdge.EntityLists.Add(new SceneEntityInfo
            {
                GroupId = 186,
                InstId = 300001,
                EntityId = 328,
                Prop = new ScenePropInfo
                {
                    PropState = 8,
                    PropId = 801
                },
                Motion = new MotionInfo
                {
                    Pos = new Vector
                    {
                        X = 31440,
                        Y = 192820,
                        Z = 433790
                    },
                    Rot = new Vector
                    {
                        Y = 60000
                    }
                },
            });

            sceneInfo.SceneGroupEntityLists.Add(calyxDreamsEdge);

            session.Send(CmdType.CmdEnterSceneByServerScNotify, new EnterSceneByServerScNotify
            {
                Scene = sceneInfo,
                Lineup = lineupInfo
            });

            session.Send(CmdType.CmdSceneEntityMoveScNotify, new SceneEntityMoveScNotify
            {
                EntryId = 2031301,
                EntityId = 0,
                Motion = new MotionInfo
                {
                    Pos = new Vector
                    {
                        X = 32342,
                        Y = 192820,
                        Z = 434276
                    },
                    Rot = new Vector
                    {
                        Y = 240000
                    }
                }
            });
        }

        internal static void DestructionEcho(NetSession session, LineupInfo lineupInfo)
        {
            var sceneInfo = new SceneInfo
            {
                GameModeType = 2,
                EntryId = 2000301,
                PlaneId = 20003,
                FloorId = 20003001
            };

            SceneGroupEntityInfo EchoDestruction = new SceneGroupEntityInfo
            {
                State = 0,
                GroupId = 34
            };

            EchoDestruction.EntityLists.Add(new SceneEntityInfo
            {
                GroupId = 34,
                InstId = 300001,
                EntityId = 43,
                Prop = new ScenePropInfo
                {
                    PropState = 8,
                    PropId = 113
                },
                Motion = new MotionInfo
                {
                    Pos = new Vector
                    {
                        X = 247160,
                        Y = 79920,
                        Z = 125050
                    },
                    Rot = new Vector
                    {
                        Y = 50000
                    }
                },
            });

            sceneInfo.SceneGroupEntityLists.Add(EchoDestruction);

            session.Send(CmdType.CmdEnterSceneByServerScNotify, new EnterSceneByServerScNotify
            {
                Scene = sceneInfo,
                Lineup = lineupInfo
            });

            session.Send(CmdType.CmdSceneEntityMoveScNotify, new SceneEntityMoveScNotify
            {
                EntryId = 2000301,
                EntityId = 0,
                Motion = new MotionInfo
                {
                    Pos = new Vector
                    {
                        X = 247160,
                        Y = 79920,
                        Z = 115050
                    },
                    Rot = new Vector
                    {
                        Y = 290000
                    }
                }
            });
        }

        internal static void FrozenEcho(NetSession session, LineupInfo lineupInfo)
        {
            var sceneInfo = new SceneInfo
            {
                GameModeType = 2,
                EntryId = 2013401,
                PlaneId = 20134,
                FloorId = 20134001
            };

            SceneGroupEntityInfo EchoFrozen = new SceneGroupEntityInfo
            {
                State = 0,
                GroupId = 5
            };

            EchoFrozen.EntityLists.Add(new SceneEntityInfo
            {
                GroupId = 5,
                InstId = 300001,
                EntityId = 30,
                Prop = new ScenePropInfo
                {
                    PropState = 8,
                    PropId = 113
                },
                Motion = new MotionInfo
                {
                    Pos = new Vector
                    {
                        X = -26127,
                        Y = 20497,
                        Z = -27302
                    },
                    Rot = new Vector
                    {
                        Y = 216070
                    }
                },
            });

            sceneInfo.SceneGroupEntityLists.Add(EchoFrozen);

            session.Send(CmdType.CmdEnterSceneByServerScNotify, new EnterSceneByServerScNotify
            {
                Scene = sceneInfo,
                Lineup = lineupInfo
            });

            session.Send(CmdType.CmdSceneEntityMoveScNotify, new SceneEntityMoveScNotify
            {
                EntryId = 2013401,
                EntityId = 0,
                Motion = new MotionInfo
                {
                    Pos = new Vector
                    {
                        X = -26127,
                        Y = 20497,
                        Z = -26302
                    },
                    Rot = new Vector
                    {
                        Y = 290000
                    }
                }
            });
        }

        internal static void PhantyliaEcho(NetSession session, LineupInfo lineupInfo)
        {
            var sceneInfo = new SceneInfo
            {
                GameModeType = 2,
                EntryId = 2023201,
                PlaneId = 20232,
                FloorId = 20232001
            };

            SceneGroupEntityInfo EchoWaterscape = new SceneGroupEntityInfo
            {
                State = 0,
                GroupId = 57
            };

            EchoWaterscape.EntityLists.Add(new SceneEntityInfo
            {
                GroupId = 57,
                InstId = 300001,
                EntityId = 15,
                Prop = new ScenePropInfo
                {
                    PropState = 8,
                    PropId = 113
                },
                Motion = new MotionInfo
                {
                    Pos = new Vector
                    {
                        X = -591610,
                        Y = -58599,
                        Z = -1350
                    },
                    Rot = new Vector
                    {
                        Y = 290000
                    }
                },
            });

            sceneInfo.SceneGroupEntityLists.Add(EchoWaterscape);

            session.Send(CmdType.CmdEnterSceneByServerScNotify, new EnterSceneByServerScNotify
            {
                Scene = sceneInfo,
                Lineup = lineupInfo
            });

            session.Send(CmdType.CmdSceneEntityMoveScNotify, new SceneEntityMoveScNotify
            {
                EntryId = 2023201,
                EntityId = 0,
                Motion = new MotionInfo
                {
                    Pos = new Vector
                    {
                        X = -591610,
                        Y = -58599,
                        Z = -350
                    },
                    Rot = new Vector
                    {
                        Y = 290000
                    }
                }
            });
        }

        internal static void BigBugEcho(NetSession session, LineupInfo lineupInfo)
        {
            var sceneInfo = new SceneInfo
            {
                GameModeType = 2,
                EntryId = 2000401,
                PlaneId = 20004,
                FloorId = 20004001
            };

            SceneGroupEntityInfo EchoBug = new SceneGroupEntityInfo
            {
                State = 0,
                GroupId = 49
            };

            EchoBug.EntityLists.Add(new SceneEntityInfo
            {
                GroupId = 49,
                InstId = 300001,
                EntityId = 5,
                Prop = new ScenePropInfo
                {
                    PropState = 8,
                    PropId = 113
                },
                Motion = new MotionInfo
                {
                    Pos = new Vector
                    {
                        X = 2758,
                        Y = 144670,
                        Z = -14085
                    },
                    Rot = new Vector
                    {
                        Y = 290000
                    }
                },
            });

            sceneInfo.SceneGroupEntityLists.Add(EchoBug);

            session.Send(CmdType.CmdEnterSceneByServerScNotify, new EnterSceneByServerScNotify
            {
                Scene = sceneInfo,
                Lineup = lineupInfo
            });

            session.Send(CmdType.CmdSceneEntityMoveScNotify, new SceneEntityMoveScNotify
            {
                EntryId = 2000401,
                EntityId = 0,
                Motion = new MotionInfo
                {
                    Pos = new Vector
                    {
                        X = 2758,
                        Y = 144670,
                        Z = -14085
                    },
                    Rot = new Vector
                    {
                        Y = 290000
                    }
                }
            });
        }
    }


    // FREESR_TOOLS
    internal class RelicJson
    {
        public int level { get; set; }
        public int relicId { get; set; }
        public int relicSetId { get; set; }
        /**
         * THIS IS ONLY THEIR STAT ordering ID (ie: atk=1), not the actual id. get from mainaffixmap[relicid] if you want mainaffixid instead
         */
        public int mainAffixId { get; set; }
        public List<SubAffix> subAffixes { get; set; }
        public bool _hasError { get; set; }
        /**
         * for backend, used as an identifier when fetching relics from backend
         */
        public int? internalUid { get; set; }
        /**
         * for frontend, used as an identifier when scanning relic result
         */
        public string internalUidFrontend { get; set; }
        /**
         * ONLY DEFINED WHEN GETTING RELICS FROM BACKEND,
         */
        public int? equipAvatar { get; set; }
    }

    internal class SubAffix
    {
        public int subAffixId { get; set; }
        public int count { get; set; }
        public int step { get; set; }
        public bool? _isError { get; set; }
        public int? _renderedValue { get; set; }
    }

    internal class GetCharactersResJson
    {
        public int ownerUid { get; set; }
        public int avatarId { get; set; }
        public PartialData data { get; set; }
        public int level { get; set; }
        public int promotion { get; set; }
        public int internalUid { get; set; }

        public int? spValue { get; set; }

        public int? spMax { get; set; }

#pragma warning disable
        public List<uint>? useTechnique { get; set; }
    }

    internal class PartialData
    {
        public int rank { get; set; }
        /**
         * key: skill_id
         * value: skill level
         */
        public Dictionary<int, int> skills { get; set; }
    }

    internal class LightconeJson
    {
        public int level { get; set; }
        public int itemId { get; set; }
        public int equipAvatar { get; set; }
        public int rank { get; set; }
        public int promotion { get; set; }
        public int internalUid { get; set; }
    }

    internal class MonsterJson
    {
        public int amount { get; set; }
        public int level { get; set; }
        public int monsterId { get; set; }

        public int? stageId { get; set; }

        public int? cycleCount { get; set; }

#pragma warning disable
        public List<uint>? pureFictionBlessings { get; set; }
    }

    internal class SimulatedUniverseData
    {
        public List<BuffData> buffList { get; set; }
        public uint stageId { get; set; }
        public uint pathResonanceEventId { get; set; }
        public List<SubAffix> statList { get; set; }
        public class BuffData
        {
            public uint id { get; set; }
            public uint level { get; set; }
            public DynamicValue dynamicValue { get; set; }

            public class DynamicValue
            {
                public string key { get; set; }
                public int value { get; set; }
            }

        }
    }

    internal class Payload
    {
        public Dictionary<string, GetCharactersResJson> avatars { get; set; }
        public List<LightconeJson> lightcones { get; set; }
        public List<List<MonsterJson>> monsters { get; set; }
        public List<RelicJson> relics { get; set; }

        public SimulatedUniverseData simulatedUniverseData { get; set; }
    }
}