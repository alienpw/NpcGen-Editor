﻿using System;
using System.Linq;

namespace pwAdminLocal.Elements
{
    public class Definicoes
    {
        public static string ListDefNames(string name)
        {
            switch (name)
            {
                case "EQUIPMENT_ADDON": return "Atributos";
                case "WEAPON_MAJOR_TYPE": return "Tipo de Arma";
                case "WEAPON_SUB_TYPE": return "Sub-Tipo de Arma";
                case "WEAPON_ESSENCE": return "Armas";
                case "ARMOR_MAJOR_TYPE": return "Tipo de Armadura";
                case "ARMOR_SUB_TYPE": return "Sub-Tipo de Armadura";
                case "ARMOR_ESSENCE": return "Armaduras";
                case "DECORATION_MAJOR_TYPE": return "Tipo de Acessório";
                case "DECORATION_SUB_TYPE": return "Sub-Tipo de Acessório";
                case "DECORATION_ESSENCE": return "Acessórios";
                case "MEDICINE_MAJOR_TYPE": return "Tipo de Remédio";
                case "MEDICINE_SUB_TYPE": return "Sub-Tipo de Remédio";
                case "MEDICINE_ESSENCE": return "Remédios";
                case "MATERIAL_MAJOR_TYPE": return "Tipo de Material";
                case "MATERIAL_SUB_TYPE": return "Sub-Tipo de Material";
                case "MATERIAL_ESSENCE": return "Materiais";
                case "DAMAGERUNE_SUB_TYPE": return "Sub-Tipo de Runa de Dano";
                case "DAMAGERUNE_ESSENCE": return "Amuletos de Dano";
                case "ARMORRUNE_SUB_TYPE": return "Sub-Tipo de Runa de Armadura";
                case "ARMORRUNE_ESSENCE": return "Amuletos de Defesa";
                case "SKILLTOME_SUB_TYPE": return "Sub-Tipo de Livro de Habilidade";
                case "SKILLTOME_ESSENCE": return "Livros de Habilidades";
                case "FLYSWORD_ESSENCE": return "Vôos";
                case "WINGMANWING_ESSENCE": return "Asas de Alado";
                case "TOWNSCROLL_ESSENCE": return "Pergaminho de Teleporte";
                case "UNIONSCROLL_ESSENCE": return name;
                case "REVIVESCROLL_ESSENCE": return "Pergaminho de Renascimento";
                case "ELEMENT_ESSENCE": return "Pedras de Elementos";
                case "TASKMATTER_ESSENCE": return "Itens de Missão";
                case "TOSSMATTER_ESSENCE": return "Projéteis Voadores";
                case "PROJECTILE_TYPE": return "Tipo de Projétil";
                case "PROJECTILE_ESSENCE": return "Projéteis";
                case "QUIVER_SUB_TYPE": return "Sub-Tipo de Aljava";
                case "QUIVER_ESSENCE": return "Aljavas";
                case "STONE_SUB_TYPE": return "Sub-Tipo de Pedra";
                case "STONE_ESSENCE": return "Pedras";
                case "MONSTER_ADDON": return "Atributos de Monstros";
                case "MONSTER_TYPE": return "Tipo de Monstro";
                case "MONSTER_ESSENCE": return "Monstros";
                case "NPC_TALK_SERVICE": return "NPC: Serviço de Conversa";
                case "NPC_SELL_SERVICE": return "NPC: Serviço de Vendas";
                case "NPC_BUY_SERVICE": return "NPC: Serviço de Compras";
                case "NPC_REPAIR_SERVICE": return "NPC: Serviço de Reparação";
                case "NPC_INSTALL_SERVICE": return "NPC: Serviço de Instalação";
                case "NPC_UNINSTALL_SERVICE": return "NPC: Serviço de Desinstação";
                case "NPC_TASK_IN_SERVICE": return "NPC: Missões de Entrega";
                case "NPC_TASK_OUT_SERVICE": return "NPC: Missões de Saída";
                case "NPC_TASK_MATTER_SERVICE": return "NPC: Missões com Materiais";
                case "NPC_SKILL_SERVICE": return "NPC: Serviço de Habilidades";
                case "NPC_HEAL_SERVICE": return "NPC: Serviço de Cura";
                case "NPC_TRANSMIT_SERVICE": return "NPC: Serviço de Teletransporte";
                case "NPC_TRANSPORT_SERVICE": return "NPC: Serviço de Transporte";
                case "NPC_PROXY_SERVICE": return "NPC: Serviço de Consignação";
                case "NPC_STORAGE_SERVICE": return "NPC: Serviço de Armazenamento";
                case "NPC_MAKE_SERVICE": return "NPC: Serviço de Criação";
                case "NPC_DECOMPOSE_SERVICE": return "NPC: Serviço de Decomposição";
                case "NPC_TYPE": return "Tipo de NPC";
                case "NPC_ESSENCE": return "NPC's";
                case "FACE_TEXTURE_ESSENCE": return "Face: Texturas";
                case "FACE_SHAPE_ESSENCE": return "Face: Modelos";
                case "FACE_EMOTION_TYPE": return "Face: Tipos de Emoções";
                case "FACE_EXPRESSION_ESSENCE": return "Face: Expressões";
                case "FACE_HAIR_ESSENCE": return "Face: Cabelos";
                case "FACE_MOUSTACHE_ESSENCE": return "Face: Bigode";
                case "COLORPICKER_ESSENCE": return "Seleção de Cores";
                case "CUSTOMIZEDATA_ESSENCE": return "Personalização";
                case "RECIPE_MAJOR_TYPE": return "Tipo de Fórmula";
                case "RECIPE_SUB_TYPE": return "Sub-Tipo de Fórmula";
                case "RECIPE_ESSENCE": return "Fórmulas";
                case "ENEMY_FACTION_CONFIG": return "Configuração de Facção Inimiga";
                case "CHARRACTER_CLASS_CONFIG": return "Configuração de Classes";
                case "PARAM_ADJUST_CONFIG": return "Ajuste de Parâmetros";
                case "PLAYER_ACTION_INFO_CONFIG": return "Configuração de Ações";
                case "TASKDICE_ESSENCE": return "Materiais de Tarefa com Missão";
                case "TASKNORMALMATTER_ESSENCE": return "Materiais de Missão";
                case "FACE_FALING_ESSENCE": return name;
                case "PLAYER_LEVELEXP_CONFIG": return "Configurações de Experiência do Jogador";
                case "MINE_TYPE": return "Tipo de Minério";
                case "MINE_ESSENCE": return "Minérios";
                case "NPC_IDENTIFY_SERVICE": return "NPC: Serviço de Identificação";
                case "FASHION_MAJOR_TYPE": return name;
                case "FASHION_SUB_TYPE": return name;
                case "FASHION_ESSENCE": return name;
                case "FACETICKET_MAJOR_TYPE": return name;
                case "FACETICKET_SUB_TYPE": return name;
                case "FACETICKET_ESSENCE": return name;
                case "FACEPILL_MAJOR_TYPE": return name;
                case "FACEPILL_SUB_TYPE": return name;
                case "FACEPILL_ESSENCE": return name;
                case "SUITE_ESSENCE": return name;
                case "GM_GENERATOR_TYPE": return name;
                case "GM_GENERATOR_ESSENCE": return name;
                case "PET_TYPE": return name;
                case "PET_ESSENCE": return name;
                case "PET_EGG_ESSENCE": return name;
                case "PET_FOOD_ESSENCE": return name;
                case "PET_FACETICKET_ESSENCE": return name;
                case "FIREWORKS_ESSENCE": return name;
                case "WAR_TANKCALLIN_ESSENCE": return name;
                case "NPC_WAR_TOWERBUILD_SERVICE": return name;
                case "PLAYER_SECONDLEVEL_CONFIG": return name;
                case "NPC_RESETPROP_SERVICE": return name;
                case "NPC_PETNAME_SERVICE": return name;
                case "NPC_PETLEARNSKILL_SERVICE": return name;
                case "NPC_PETFORGETSKILL_SERVICE": return name;
                case "SKILLMATTER_ESSENCE": return name;
                case "REFINE_TICKET_ESSENCE": return name;
                case "DESTROYING_ESSENCE": return name;
                case "NPC_EQUIPBIND_SERVICE": return name;
                case "NPC_EQUIPDESTROY_SERVICE": return name;
                case "NPC_EQUIPUNDESTROY_SERVICE": return name;
                case "BIBLE_ESSENCE": return name;
                case "SPEAKER_ESSENCE": return name;
                case "AUTOHP_ESSENCE": return name;
                case "AUTOMP_ESSENCE": return name;
                case "DOUBLE_EXP_ESSENCE": return name;
                case "TRANSMITSCROLL_ESSENCE": return name;
                case "DYE_TICKET_ESSENCE": return name;
                case "GOBLIN_ESSENCE": return name;
                case "GOBLIN_EQUIP_TYPE": return name;
                case "GOBLIN_EQUIP_ESSENCE": return name;
                case "GOBLIN_EXPPILL_ESSENCE": return name;
                case "SELL_CERTIFICATE_ESSENCE": return name;
                case "TARGET_ITEM_ESSENCE": return name;
                case "LOOK_INFO_ESSENCE": return name;
                case "UPGRADE_PRODUCTION_CONFIG": return name;
                case "ACC_STORAGE_BLACKLIST_CONFIG": return name;
                case "FACE_HAIR_TEXTURE_MAP": return name;
                case "MULTI_EXP_CONFIG": return name;
                case "INC_SKILL_ABILITY_ESSENCE": return name;
                case "GOD_EVIL_CONVERT_CONFIG": return name;
                case "WEDDING_CONFIG": return name;
                case "WEDDING_BOOKCARD_ESSENCE": return name;
                case "WEDDING_INVITECARD_ESSENCE": return name;
                case "SHARPENER_ESSENCE": return name;
                case "FACE_THIRDEYE_ESSENCE": return name;
                case "FACTION_FORTRESS_CONFIG": return name;
                case "FACTION_BUILDING_SUB_TYPE": return name;
                case "FACTION_BUILDING_ESSENCE": return name;
                case "FACTION_MATERIAL_ESSENCE": return name;
                case "CONGREGATE_ESSENCE": return name;
                case "ENGRAVE_MAJOR_TYPE": return name;
                case "ENGRAVE_SUB_TYPE": return name;
                case "ENGRAVE_ESSENCE": return name;
                case "NPC_ENGRAVE_SERVICE": return name;
                case "NPC_RANDPROP_SERVICE": return name;
                case "RANDPROP_TYPE": return name;
                case "RANDPROP_ESSENCE": return name;
                case "WIKI_TABOO_CONFIG": return name;
                case "FORCE_CONFIG": return name;
                case "FORCE_TOKEN_ESSENCE": return name;
                case "NPC_FORCE_SERVICE": return name;
                case "PLAYER_DEATH_DROP_CONFIG": return name;
                case "DYNSKILLEQUIP_ESSENCE": return name;
                case "CONSUME_POINTS_CONFIG": return name;
                case "ONLINE_AWARDS_CONFIG": return name;
                case "COUNTRY_CONFIG": return name;
                case "GM_ACTIVITY_CONFIG": return name;
                case "FASHION_WEAPON_CONFIG": return name;
                case "PET_EVOLVE_CONFIG": return name;
                case "PET_EVOLVED_SKILL_CONFIG": return name;
                case "MONEY_CONVERTIBLE_ESSENCE": return name;
                case "STONE_CHANGE_RECIPE_TYPE": return name;
                case "STONE_CHANGE_RECIPE": return name;
                case "MERIDIAN_CONFIG": return name;
                case "PET_EVOLVED_SKILL_RAND_CONFIG": return name;
                case "AUTOTASK_DISPLAY_CONFIG": return name;
                case "TOUCH_SHOP_CONFIG": return name;
                case "TITLE_CONFIG": return name;
                case "COMPLEX_TITLE_CONFIG": return name;
                case "MONSTER_SPIRIT_ESSENCE": return name;
                case "PLAYER_SPIRIT_CONFIG": return name;
                case "PLAYER_REINCARNATION_CONFIG": return name;
                case "HISTORY_STAGE_CONFIG": return name;
                case "HISTORY_ADVANCE_CONFIG": return name;
                case "AUTOTEAM_CONFIG": return name;
                case "PLAYER_REALM_CONFIG": return name;
                case "CHARIOT_CONFIG": return name;
                case "CHARIOT_WAR_CONFIG": return name;
                case "POKER_LEVELEXP_CONFIG": return name;
                case "POKER_SUITE_ESSENCE": return name;
                case "POKER_DICE_ESSENCE": return name;
                case "POKER_SUB_TYPE": return name;
                case "POKER_ESSENCE": return name;
                case "TOKEN_SHOP_CONFIG": return name;
                case "SHOP_TOKEN_ESSENCE": return name;
                case "GT_CONFIG": return name;
                case "RAND_SHOP_CONFIG": return name;
                case "PROFIT_TIME_CONFIG": return name;
                case "FACTION_PVP_CONFIG": return name;
                case "UNIVERSAL_TOKEN_ESSENCE": return name;
                case "TASK_LIST_CONFIG": return name;
                case "TASK_DICE_BY_WEIGHT_CONFIG": return name;
                case "FASHION_SUITE_ESSENCE": return name;
                case "FASHION_BEST_COLOR_CONFIG": return name;
                case "SIGN_AWARD_CONFIG": return name;
                case "ASTROLABE_ESSENCE": return name;
                case "ASTROLABE_RANDOM_ADDON_ESSENCE": return name;
                case "ASTROLABE_INC_INNER_POINT_VALUE_ESSENCE": return name;
                case "ASTROLABE_INC_EXP_ESSENCE": return name;
                case "ITEM_PACKAGE_BY_PROFESSION_ESSENCE": return name;
                case "ASTROLABE_LEVELEXP_CONFIG": return name;
                case "ASTROLABE_ADDON_RANDOM_CONFIG": return name;
                case "ASTROLABE_APPEARANCE_CONFIG": return name;
                case "EQUIP_MAKE_HOLE_CONFIG": return name;
                case "SOLO_TOWER_CHALLENGE_LEVEL_CONFIG": return name;
                case "SOLO_TOWER_CHALLENGE_AWARD_PAGE_CONFIG": return name;
                case "SOLO_TOWER_CHALLENGE_AWARD_LIST_CONFIG": return name;
                case "SOLO_TOWER_CHALLENGE_SCORE_COST_CONFIG": return name;
                case "MNFACTION_WAR_CONFIG": return name;
                case "NPC_CROSS_SERVER_SERVICE": return name;
                case "FIREWORKS2_ESSENCE": return name;
                case "FIX_POSITION_TRANSMIT_ESSENCE": return name;
                case "HOME_CONFIG": return name;
                case "TALK_PROC": return name;
                case "HOME_PRODUCTS_CONFIG": return name;
                case "HOME_RESOURCE_PRODUCE_CONFIG": return name;
                case "HOME_FORMULAS_PRODUCE_RECIPE": return name;
                case "HOME_FORMULAS_ITEM_ESSENCE": return name;
                case "HOME_PRODUCE_SERVICE_CONFIG": return name;
                case "HOME_FACTORY_CONFIG": return name;
                case "HOME_ITEM_MAJOR_TYPE": return name;
                case "HOME_ITEM_SUB_TYPE": return name;
                case "HOME_ITEM_ENTITY": return name;
                case "HOME_FORMULAS_PRODUCE_MAJOR_TYPE": return name;
                case "HOME_FORMULAS_PRODUCE_SUB_TYPE": return name;
                case "HOME_FORMULAS_ITEM_MAJOR_TYPE": return name;
                case "HOME_FORMULAS_ITEM_SUB_TYPE": return name;
                case "HOME_STORAGE_TASK_CONFIG": return name;
                case "WISH_TRIBUTE_ESSENCE": return name;
                case "RED_PACKET_PAPER_ESSENCE": return name;
                case "LOTTORY_TICKET_STORAGE_CONFIG": return name;
                case "LOTTORY_TICKET_COST_CONFIG": return name;
                case "LOTTORY_TICKET_OTHER_AWARD_CONFIG": return name;
                case "HOME_TEXTURE_ENTITY": return name;
                case "HOME_GRASS_ENTITY": return name;
                case "HOME_UNLOCK_PAPER_ESSENCE": return name;
                case "GUARDIAN_BEAST_EGG_ESSENCE": return name;
                case "GUARDIAN_BEAST_ENTITY": return name;
                case "GUARDIAN_BEAST_RACE_CONFIG": return name;
                case "GUARDIAN_BEAST_UPGRADE_CONFIG": return name;
                case "GUARDIAN_BEAST_REWARD_CONFIG": return name;
                case "HOME_SKIN_ENTITY": return name;
                case "WORLD_SPEAK_COST_CONFIG": return name;
                case "EASY_PRODUCE_ITEM_ESSENCE": return name;
                case "ARENA_CONFIG": return name;
                case "ARENA_SCENE_CONFIG": return name;
                case "ARENA_POINT_CONFIG": return name;
                case "ARENA_ENTITY_CONFIG": return name;
                case "NPC_ARENA_SELL_SERVICE": return name;
                case "UNLOCK_RUNE_SLOT_ITEM_ESSENCE": return name;
                case "RUNE_ITEM_ESSENCE": return name;
                case "RUNE_SKILL_CONFIG": return name;
                case "RUNE_UPGRADE_CONFIG": return name;
                case "ARENA_MAP_CONFIG": return name;
                case "NPC_STATUE_CREATE_SERVICE": return name;
                case "ARENA_FORBID_ITEM_CONFIG": return name;
                case "ARENA_FORBID_SKILL_CONFIG": return name;
                case "CARRIER_CONFIG": return name;
                case "CHANGE_PROPERTY_CONFIG": return name;
                case "PROFESSION_PROPERTY_CONFIG": return name;
                case "FIX_MONSTER_ITEM_ESSENCE": return name;
                case "NPC_LIB_PRODUCE_SERVICE": return name;
                case "LIB_PRODUCE_RECIPE": return name;
                case "VOTE_CONFIG": return name;
                case "SIMULATOR_ITEM_LIST_CONFIG": return name;
                case "EQUIPMENT_PRODUCE_METHOD_CONFIG": return name;
                case "PRODUCTION_LINE_CONFIG": return name;
                case "SOURCE_OF_MATERIAL_CONFIG": return name;
                case "FACTION_TALENT_SINGLE_CONFIG": return name;
                case "FACTION_TALENT_TREE_CONFIG": return name;
                case "FACTION_TARGET_CONFIG": return name;
                case "INSTANCE_STAGE_CONFIG": return name;
                case "FACTION_PET_CONFIG": return name;
                case "FACTION_PET_FEED_CONFIG": return name;
                case "FACTION_TILLAGE_CONFIG": return name;
                case "FACTION_PET_BLESS_CONFIG": return name;
                case "ITEM_USED_FOR_AREA_ESSENCE": return name;
                case "CAPTURE_ITEM_ESSENCE": return name;
                case "DRIFT_BOTTLE_DROP_CONFIG": return name;
                case "NPC_FACTION_SELL_SERVICE": return name;
                case "FACTION_INSTANCE_DROP_CONFIG": return name;
                case "NPC_PARK_ENTER_SERVICE": return name;
                case "FACTION_STORAGE_WHITELIST_CONFIG": return name;
                case "NPC_BOSSRUSH_AWARD_SERVICE": return name;
                case "PROFESSION_STATE_CONFIG": return name;
                case "MENTAL_PARAM_ADJUST_CONFIG": return name;
                case "BIBLE_REFINE_CONFIG": return name;
                case "BIBLE_REFINE_TICKET_ESSENCE": return name;
                case "TITLE_PROGRESS_CONFIG": return name;
                case "NPC_MYSTERIOUS_SHOP_SERVICE": return name;
                case "OFFLINEBATTLE_CONFIG": return name;
                case "NPC_PRIDICTABLE_ENGRAVE_SERVICE": return name;
                case "NEW_ARMOR_MAJOR_TYPE": return name;
                case "NEW_ARMOR_SUB_TYPE": return name;
                case "NEW_ARMOR_ESSENCE": return name;
                case "QIHUN_ESSENCE": return name;
                case "QILING_ESSENCE": return name;
                case "STORY_BOOK_MAJOR_TYPE": return name;
                case "STORY_BOOK_CONFIG": return name;
                case "MENTOR_LEVEL_CONFIG": return name;
                case "STUDENT_AWARD_CONFIG": return name;
                case "QIHUN_COVER_ESSENCE": return name;
                case "GROWTH_TARGET_CONFIG": return name;
                case "QUESTION_TIP_CONFIG": return name;
                case "QUESTION_AWARD_CONFIG": return name;
                case "SLIDE_SKILL_ESSENCE": return name;
                case "CONSUME_MONEY_CONFIG": return name;
                case "USE_FOR_SELF_ITEM_ESSENCE": return name;
                case "HIGH_POLY_FACE_TEXTURE_ESSENCE": return name;
                case "HIGH_POLY_FACE_TEMPL_CONFIG": return name;
                case "NEW_LOTTERY_CONFIG": return name;
                case "CURRENCY_ITEM_WHITELIST_CONFIG": return name;
                case "HIGH_POLY_CUSTOMIZEDATA_ESSENCE": return name;
                case "HIGH_POLY_RACE_FEATURE": return name;
                case "BLACK_WHITE_LIST_MAJOR_TYPE": return name;
                case "BLACK_WHITE_LIST_CONFIG": return name;
                case "LOANS_PRODUCE_MAJOR_TYPE": return name;
                case "LOANS_PRODUCE_SUB_TYPE": return name;
                case "LOANS_PRODUCE_RECIPE": return name;
                case "RED_BOOK_CONFIG": return name;
                case "RED_BOOK_UPGRADE_ITEM": return name;
                case "RED_BOOK_LOTTERY_CONFIG": return name;
                case "ARENA_TICKET_CONFIG": return name;
                case "CAMP_LEVEL_CONFIG": return name;
                case "CAMP_TOKEN_CONFIG": return name;
                case "CAMP_TOKEN_TREE_CONFIG": return name;
                case "CAMP_TOKEN_ESSENCE": return name;
                case "CAMP_TOKEN_UPGRADE_ESSENCE": return name;
                case "CAMP_TOKEN_ROLLBACK_ESSENCE": return name;
                case "CAMP_TOKEN_PROB_ADJUST_ESSENCE": return name;
                case "CAMP_BATTLE_TECH_TREE_CONFIG": return name;
                case "SPECIAL_ACTIVITY_RESPAWN_CONFIG": return name;
                case "MAP_EVENT_CONFIG": return name;
                case "SPECIAL_CAMP_REGION_MAJOR_TYPE": return name;
                case "SPECIAL_CAMP_REGION_CONFIG": return name;
                case "PET_SKIN_CONFIG": return name;
                case "PET_MULTIRIDE_CONFIG": return name;
                case "FASHION_NEW_MAJOR_TYPE": return name;
                case "FASHION_NEW_SUB_TYPE": return name;
                case "FASHION_NEW_ESSENCE": return name;
                case "WAR_AREA_CONFIG": return name;
                case "ILLUSTRATED_FASHION_SERIES_CONFIG": return name;
                case "ILLUSTRATED_FASHION_ESSENCE": return name;
                case "ILLUSTRATED_WING_EGG_ESSENCE": return name;
                case "ILLUSTRATED_WING_SERIES_CONFIG": return name;
                case "ILLUSTRATED_PET_EGG_ESSENCE": return name;
                case "ILLUSTRATED_PET_SERIES_CONFIG": return name;
                case "ACROSS_SERVER_MATCH_CONFIG": return name;
                case "NPC_ASAL_CONFIG": return name;
                case "WILDERNESS_SURVIVAL_CONFIG": return name;
                case "ILLUSTRATED_REWARD_CONFIG": return name;
                case "FAST_PRODUCE_ITEM_TYPE": return name;
                case "FAST_PRODUCE_ITEM_ESSENCE": return name;
                case "KID_SYSTEM_CONFIG": return name;
                case "COURSE_ESSENCE": return name;
                case "COURSE_SUITE_ESSENCE": return name;
                case "KID_PROPERTY_CONFIG": return name;
                case "KID_QUALITY_CONFIG": return name;
                case "KID_EXP_CONFIG": return name;
                case "KID_UPGRADE_STAR_CONFIG": return name;
                case "KID_LEVEL_REWARD_CONFIG": return name;
                case "KID_LEVEL_MAX_CONFIG": return name;
                case "KID_DEBRIS_ESSENCE": return name;
                case "KID_SKILL_CONFIG": return name;
                case "KID_DEBRIS_GENERATOR_ESSENCE": return name;
                case "KID_PROFESSION_CONFIG": return name;
                case "BIS_SOI_USE_LIMIT_CONFIG": return name;
                case "REWARD_INTERFACE_CONFIG": return name;
                case "HISTORY_VARIABLE_PROGRESS_CONFIG": return name;
                case "ANNIVERSARY_EVENT_INTERFACE_CONFIG": return name;
                case "NEW_HAND_EVENT_EXPUP": return name;
                case "NEW_HAND_EVENT_CHALLENGE": return name;
                case "NEW_HAND_EVENT_CONSUMPTION": return name;
                case "NEW_HAND_EVENT_SHOP": return name;
                case "REWARD_SHOW_CONFIG": return name;
                case "EXTR_PROP_SKILL_CONFIG": return name;
                case "MATERIAL_REFINE_ESSENCE": return name;
                case "MATERIAL_REFINE_TICKET_ESSENCE": return name;
                default: return name;

            }
        }
    }
}
