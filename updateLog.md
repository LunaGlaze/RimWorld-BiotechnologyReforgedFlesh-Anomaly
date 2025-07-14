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