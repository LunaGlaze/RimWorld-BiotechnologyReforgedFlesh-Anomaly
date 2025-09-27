 [v0.1.0]
 - 正式发布/release

 [v0.1.1]
 - 修复管道消化器/fix Digester IPipe

 [v0.1.2]
 - 修复血肉温床生成bug/fix primordial flesh cradle
 - 修复酸液喷吐者血肉蠕虫弹药错误/fix spitter fleshworm

 [v0.1.3]
 - 修复恶性血肉活化bug/fix bug about Malignant Flesh Activation
 - 修复静电囊泡无法作为建筑材料的问题/fix Electrostatic Vesicle

 [v0.1.4]
 - 平衡性调整/balance adjustments

 [v0.2.0]
 - 添加设置为虫族寄生虫的新实体/Add a new entity, set as an insect parasite.
 - 添加新的心灵仪式以召唤实体袭击/Add new Psychic Ritual to summon entities.
 - 添加来自欲肉教的外来者增益类型/Added new Creep Joiner Benefit from Sarkicism.

 [v0.2.1]
 - 修复新实体袭击时袭击者内讧的错误/Fix bug with infighting among attackers when an assault occurs on new entity.
 - 调整异象巨石对应异象等级与基于此的额外袭击点数曲线/Modify anomalyThreatTier and add assault extra points curve.

 [v0.2.2]
 - 优化Harmony补丁文件结构/Optimize Harmony Patch file structure.
 - 为血污包子添加了工作速率调整功能/Add working speed multiplier adjustment function for Flesh Taint Spore.

 [v0.2.3]
 - 调整部分文件命名格式/Adjusting file naming
 - 添加rjw联动基础仿生体/Add rjw linkage base bionome

 [v0.2.4]
 - 修复rjw联动配方缺失问题/Fix rjw recipe missing.

 [v0.2.5]
 - 修复rjw联动多版本兼容性问题/Fix RJW multi-version compatibility problem.

 [v0.2.6]
 - 为生物材料添加食物中毒系数修正/Adding Food Poisoning Factor Correction for Biological Materials.
 - 修改为对猎杀人类添加的Harmony补丁判断结构，以避免因LunaDefof类初始化顺序引发的空指针问题/Modify the previously added Harmony Patch judgment structure for Manhunter to avoid the null pointer problem caused by the LunaDefof class initialization order.
 - 调整VEF联动结构，部分节点在存在VEF时改为使用VEF节点/Adjustment of VEF linkage structure, some nodes are changed to use VEF nodes when VEF exists.
 - 调整部分纹理与灯光色调/Adjust some texture and light tones.
 - 为肢团能力添加捕食动物许可/Allows Clump ability to prey on animals.
 - 小幅度优化部分tick性能/Minor optimization of some tick performance.

 [v0.2.7]
 - 平衡性调整/balance adjustments
 - 修复重塑血肉无法使用的bug/fix transmute flesh coulnd't use

 [v0.3.0]
 - 平衡性调整并加强原有实体的战斗力/Balance Adjustments and Entity Combat Strengthening.
 - 更新血肉建筑的受损效果/Updated damage effects on flesh buildings.
 - 修复部分实体图鉴中子项目无法发现的bug/Fix bugs where some entities cannot be discovered in the sub items of the entity codex.
 - 修复血肉建筑工作速度调整器bug/Fix flesh buildings Work Speed Adjuster Tool Bugs.
 - 小幅度优化部分原有tick性能/Minor optimization of some tick performance.
 - 修正HarmonyPatch逻辑/Fix HarmonyPatch logic.
 - 优化弗兰肯斯坦肢体守卫AI，并添加未安装VEF时的征召功能/Optimize Frankenstein Limb Guard AI and add conscription when VEF is not installed.
 - 新实体绯红活菌与其带来的新的血肉类型植物/New entity Crimson Living Fungi and new flesh type plants from it.
 - 基础血肉医药/Basic Flesh Medicine.
 - 添加新的血肉材料/Add new flesh type material.
 - 新实体猩红脑蜇与基础脑蛞蝓科技/New Entity Scarlet Cerebral Jellyfish with Basic Brain Slug Technology.

 [v0.3.1]
 - 推迟绯红活菌最早出现天数，避免前期难度过高/Delay the earliest appearance of Crimson Living Fungi to avoid excessive difficulty in the early stage.
 - 减少绯红窥叶的面板外额外产出/Reduce the extra output outside the panel of Crimson Peeping Leaves.
 - 修复绯红活菌袭击xml配置错误解锁错误的实体的bug/Fix bug of Crimson Living Fungi Assault XML configuration error and unlocking incorrect entities.
 - 修复部分实体能力会选取无法抵达的目标或无法提供门和栅栏追踪目标的bug/Fixed a bug that some entity's ability select unreachable targets or cannot provide tracking bash doors and fences.

 [v0.3.2]
 - 下调了肢团的护甲穿透属性/Reduced the armor penetration attribute of the Clump.
 - 修复了猩红脑蜇错误重复对机械体使用能力的bug；降低选定机械体为目标的概率，当目标随机为机械体时直接进行kill程序防止循环./Fixed bug of Scarlet Cerebral Jellyfish error repeatedly affecting mechanical usage ability.
 - 调整实体额外生命值生成方式/Adjust the method of generating additional health points for entities.
 - 降低实体额外生命值比例/Reduce the proportion of additional health points for entities.
 - 修复纯粹主义可以与欲肉教特性共存的bug/Fix bug that BodyPurist can coexist with Sarkicism Trait.
 - 调整文件夹结构/Adjust folder structure.
 - 规范VEF模块Net代码/Standardize the Net code of VEF module.

 [v0.3.3]
 - 修复部分语言文件文案错误/Fix some language file's wrong content.
 - 当添加了殁骸尸军2时，为部分实体添加殁染抗性。/Provide Necrotion Resistance for some entities when adding The Army of Fetid Corps 1.5.
 - 优化物品分类逻辑/Optimize the logic of item classification.
 - 提升骨质遗骸类材料产出/Enhance the output of bone remains materials.
 - 稍微下调了肢团的攻击能力/Slightly reduced the attack function of Clump.
 - 优化猩红脑蜇索敌代码，不再对机械体使用能力/Optimize Scarlet Cerebral Jellyfish's enemy search code to no longer use the ability on mech.

 [v0.4.0]
 - 重绘部分旧版纹理。/Repainted some old textures.
 - 修复了部分贴图错误。/Fixed some texture errors.
 - 规范语言文件Key前缀。/Standard language file Key prefix.
 - 为蠕动肉块添加可训练性，并提升其生命、体型与生长周期等属性，降低占用点数。/Add trainability to the peristaltic flesh blob and enhance its attributes such as lifespan, body size, and growth cycle to reduce combat power consumption.
 - 修复血肉温床直接生成成年蠕动肉块的bug。/Fixed the bug where Primordial Flesh Crash directly generates adult Peristaltic Flesh Blob.
 - 修改蠕动肉块产物逻辑，由屠宰变为死亡掉落，并根据当前体型可获得更多鲜活肉块。/Modify the peristaltic flesh blob product logic from slaughter to drop upon death, and obtain more living flesh based on current body size.
 - 提升肢团占用的袭击点数。/Increase the attack points occupied by Clump.
 - 修复了肢团恢复能力的判断函数中小数点位置错误的bug。/Fixed the bug that the decimal point position was wrong in the judgment function of Clump's recovery ability.
 - 修复血肉立方无法制作的问题。/Fix bug that Recast Flesh Cube couldn't craft.
 - 调整部分需要食物作为燃料的建筑可用燃料列表，禁止部分高价值食物被作为燃料消耗。/Adjusted the available fuel list for some buildings that require food as fuel, prohibiting some high-value food from being consumed as fuel.
 - 修复模组内特殊武器可能错误地出现在遗迹战利品列表的bug。/Fixed a bug where special weapons in the mod could incorrectly appear in the list of loot in the ruins.
 - 修改部分研究科技等级与所需点数。/Modified some research's techLevel and required points.
 - 为肢体守卫添加复活能力。/Added resurrection ability to (Frankenstein's) Limb Guard.
 - 大幅减少了绯红活菌与猩红脑蜇的额外再生点数。/Significantly reduced the amount of additional regeneration points for Crimson Living Fungi and Scarlet Cerebral Jellyfish.
 - 添加了一个新的仪式专门用以召唤本mod内的袭击。/Added a new ritual specifically for summoning raids within this mod.
 - 添加新实体血藤行尸。/Added new entity Bloodvine Zombie.
 - 添加新实体棘刃血蛆。/Added new entity Thornblade Blood Maggot.
 - 添加新科技血肉发射器。/Added new tech Flesh Shooter.
 - 添加新高级血肉枪械。/Added new advanced flesh guns.
 - 添加新实体重生灵骸。/Added new entity Rebirth Psycorpse.
 - 添加新科技灵骸魔像与可自行装备武器的防御塔。/Added new tech Psycorpse Golem and defense towers that can equip their own weapons.
 - 添加新实体原生先知与相关科技。/Added new entity Raw Prophet and some tech about it.
 - 完成超凡科技基本框架。/Finish the base framework of Archotech Research Project.
 - 添加神圣的白虫与基础超凡血肉仿生体。/Added Holy White Worm (Akuloth) and Basic Archotech Flesh Bionics.
 - 添加高级脑蛞蝓仿生体。/Added Advanced Brain Slug Bionics.
 - 添加新的冥想类型“血肉”。/Added new meditation focus type of Flesh.
 - 添加新仪式“启迪仪式”以获取欲肉教特质。/Added new Psychic Ritual to gain Sarkicism trait.
 - 添加可以自行铺设的血肉菌毯。/Added new type of Flesh Blanket, which can lay down by self.
 - 更新至1.6。/Update to 1.6.
 - 为部分实体添加真空抗性（仅1.6）/Added VacuumResistance to some enitities (Only 1.6).
 - 1.5版本后续更新将不再维护。/Version 1.5 will no longer be maintained in subsequent updates.

 [v0.4.1]
 - 删除扭曲坩埚错误的冥想类型。/Delete the wrong Meditation Focus type of Twisted Crucible.
 - 补充部分翻译文本缺失。/Supplement some missing translated texts.
 - 修正语言文件结构。/Corrected language file structure.
 - 根据RimWorld - Odyssey DLC的数据重新强化与平衡血肉枪械。/Strengthened and rebalanced the flesh guns according to the numerical levels of the RimWorld - Odyssey DLC.
 - 修复CountAddedAndImplantedPartsFlesh中的空引用错误。/Fix NullReferenceException at CountAddedAndImplantedPartsFlesh.
 - 重构地形转化代码使其支持RimWorld - Odyssey DLC。/Refactored terrain convert code to support the RimWorld - Odyssey DLC.
 - 阻止一些不适合的心灵仪式在太空层使用。/Prevent some inappropriate psychic rituals from being used in the space layer.

 [v0.4.2]
 - 微调部分材料属性。/Fine-tune some material properties.
 - 补充部分翻译文本缺失。/Supplement some missing translated texts.
 - 添加支持血肉菌毯的受转化重型桥梁与逆重飞船基架。/Add new converted heavy bridge and gravship substructure types that support flesh blanket.
 - 为部分材料与结构添加密封性。/Set some resource and structure isAirtight to true.
 - 删除了肉块墙体的CornerOverlay效果，因为Ludeon Studios实际上没有完成它，所以当玩家可建造的建筑具有该效果时始终会出现一条无伤大雅的错误报告；为了避免骚扰，我选择牺牲美观度。/Removed the CornerOverlay effect for flsehmass wall, since Ludeon Studios didn't actually finish it, so there was always a harmless bug report when player buildable structures had that effect; I chose to sacrifice aesthetics to avoid harassment.
 - 修改燃料逻辑并使用HarmonyPatch使得模组内部分燃料组件变为使用营养值比例作为燃料数据，以此兼容RimWorld - Odyssey DLC中不符合原有营养值规则的生食。/Modified the fuel logic and used HarmonyPatch to change some modded fuel comps to use the things Nutrition stat value ratio as the fuel data, so as to be compatible with the raw food in RimWorld - Odyssey DLC that does not conform to the original nutrient value rules.
 - 为使用营养糊剂作为燃料的工作台提供了弹出燃料功能。/Add EjectFuel functionality to crafting tables that use Nutritional Paste as fuel.
 - 提升了超凡等级的仿生体在没有神圣的白虫的情况下造成的反噬。/The backlash caused by the bionic body that has been upgraded to the archotech level without Akuloth.
 - 修复神圣的白虫可能无法正常计入仿生体计数的bug。/Fix a bug where the Akuloth was not counted towards the Bionics count properly.
 - 为重生血肉之肺添加部分真空抗性。/Add partial vacuum resistance to rebirth flesh lung.
 - 添加重生血肉肌肤。/Add rebirth flesh skin.
 - 由于某些玩家的未知原因导致实际游戏渲染亮度与饱和度远超原始贴图，现小幅度降低墙体与地面纹理亮度与饱和度以使最终效果接近可接受范围。/Due to unknown reasons of some players, the brightness and saturation of actual game rendering far exceed the original textures. Now the brightness and saturation of wall and terrain textures are slightly reduced to make the final effect close to the acceptable range.
 - 添加RimWorld - Odyssey DLC联动材料血曜石。/Add Bloodsidian, a DLC material for RimWorld - Odyssey.
 - 支持部分建筑使用原版设计形状。/Support for some buildings using vanilla design shapes.
 - 将重生灵骸与原生先知的跳跃能力修改为折跃。/Changed the Entity Longjump ability of Rebirth Psycorpse and Raw Prophet to Skip.

 [v0.4.3]
 - 热修：修复因泰南为飞船基架修改了逻辑并没有在更新公告中提示导致的mod飞船基架失效。/Hotfix: Fixed the issue where the mod's gravship substructure was invalid because Ludeon Studios change the logic of the gravship substructure without notifying it in the update announcement.

 [v0.4.4]
 - 修复缺失RimWorld - Odyssey DLC时的def错误。/Fixed def error when RimWorld - Odyssey DLC is missing.
 - 修复因配方引用父类错误导致的重生血肉肌肤顶替全部仿生体的bug。/Fixed a bug where the rebirth flesh skin replaced all bionic bodies due to incorrect parent class reference in the recipe.
 - 彻底重构原有的地形支持系统以避免RimWorld - Odyssey DLC引入的多层地形机制引发的bug。/Completely refactored the original terrain support system to avoid bugs caused by the multi-layer terrain mechanism introduced by RimWorld - Odyssey DLC.
 - 修改地形转化代码逻辑使其不会被可通过的建筑阻止。/Modified terrain conversion code logic so that it is not blocked by traversable buildings.
 - 重构了地形判断器，使血污孢子可以在小行星与轨道站点上工作。/Refactored terrain detectors to allow Flesh Taint Spore to work on asteroids and orbital stations.
 - 提高了消化器的工作效率。/Improved the working efficiency of the digester.
 - 极大幅度提升营养储囊的容量。/Greatly increase the capacity of the Nutrient Sac.
 - 提升血藤行尸与棘刃血蛆的占用点数，较大幅度降低其生命倍率。/Increases the combatPower points used by Bloodvine Zombie and Thornblade Blood Maggot, and significantly reduces their health multipliers.
 - 将污血蜱虫寄生与脑姧所需的准备时长翻倍。/Doubled the warmup time required for Bloodstained Tick Parasitic and Jellyfish Brain Insertion.
 - 添加RimWorld - Odyssey DLC新增的虫族单位至污血蜱虫寄生袭击清单。/Added the insect units added by the RimWorld - Odyssey DLC to the BloodstainedTickParasiticable list.
 - 修改饥渴之门仪式，当不能召唤原生先知时添加提示并重置仪式冷却时间，并提升原生先知眩晕时长。/Modified the Psychic Ritual of Thirst Gate to add a notification when the Raw Prophet cannot be summoned and reset the ritual cooldown. And add shock duration of Raw Prophet when summoned.
 - 修改驱逐原生先知仪式，使其不能在异常黑暗中工作。/Modified the Psychic Ritual of Exile Raw Prophet so that it does not work in Unnatural Darkness.

 [v0.4.5]
 - 修复了因RimWorld对AI的改动致使肢团会主动对预期外的目标使用实体噬尸能力的bug。/Fixed a bug where Clump would use his Entity Necrophagia on unintended targets due to RimWorld AI changes.
 - 为猩红狂热hediff添加了增加对非肉体单位的伤害的效果。/Added increased damage to non-flesh pawns to the Scarlet Fever hediff.
 - 提升了全部血肉武器的伤害与DPS。/Increased the damage and DPS of all flesh weapons.
 - 修改实体图鉴解锁机制，使得本mod实体在经由其他mod事件生成时也能解锁实体图鉴条目。/Modified the unlocking mechanism of the entity codex so that entities in this mod can also discovery the entity codex entry when spawned in events added by other mods.
 - 添加了电鱼mod联动。/Link with Electrofishing mod.
 - 提升了高级仪式的吟诵者数量。/Increased the number of chanter role for advanced rituals.
 - 添加基础神餐仪式研究（包括一个基于异象研究点数的仪式与一个降低饥饿速率的仪式）。/Added basic theophagy ritual research (including a ritual that grants anomaly knowledge points and a ritual that reduces the participant's hunger rate).
 - 添加Vanilla Factions Expanded - Insectoids 2的皇家种系虫族单位与骸鸾的污秽虫族至污血蜱虫寄生袭击清单。/Added the royal type insect units and Waste Insect from Vanilla Factions Expanded - Insectoids 2 to the BloodstainedTickParasiticable list.
 - 大幅度降低降低污血蜱虫寄生袭击自然发生概率。/Significantly reduces the chance of Bloodstained Tick Parasiticed attacks occurring naturally.
 - 使得借出殖民者的任务有低概率令殖民者被伪装的重生灵骸替换。/Now allows quests that loan out colonists (PawnLend) to have a small chance of the colonist being replaced by a disguised Rebirth Psycorpse.
 - 当重生灵骸显现时，为目击者添加负面情绪。/Added negative emotions for witnesses when the Rebirth Psycorpse manifests.
 - 修复详细信息页面报错，并添加缺失的冥想效率。/Fixed an error in the details page and added missing meditation efficiency.

 [v0.4.6]
 - 降低机器的营养值消耗速率。/Reduces the rate at which the bio-machine consumes nutrients.
 - 提升血柑橘的属性和性价比。/Improves the stats and cost-effectiveness of Bloody Citrus Reticulata.
 - 新增高级神餐仪式研究与三个新心灵仪式（召唤血肉心脏、给予超速再生、杀死全图蹒跚怪）。/Added advanced Theophagy ritual research and three new mind rituals (summon fleshmass heart, grant rapid regeneration, kill all shambler of map).
 - 微调研究树布局。/Fine-tuned the research tree layout.
 - 为部分HarmonyPatch新增更多非空保护以避免与其他模组不规范或预期外的输入产生冲突。/Added more non-null protections to some HarmonyPatches to prevent conflicts with non-standard or unexpected inputs from other modules.
 - 削弱重生灵骸，使其不能折跃至看不见的地点。/Nerfed Rebirth Psycorpse so it can no longer skip to unseen locations.
 - 初步调整部分实体音效，为重生灵骸添加了独特叫声。/Made preliminary adjustments to some entity sound effects and added a unique cry for the Rebirth Psycorpse.