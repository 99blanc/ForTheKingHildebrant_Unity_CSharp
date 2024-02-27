using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;
using UnityEngine;
using static Utils.Utilities;

public static class Define
{
    public static class Keyword
    {
        public const string INFO = "I";
        public const string HOVER = "H";
        public const string BASIC = "Basic";
        public const string SUCCESS = "Success";
        public const string FOCUS = "Focus";
        public const string FAIL = "Fail";
    }

    public static class Table
    {
        public const string STAGE = "StageTable.csv";
        public const string REWARD = "RewardTable.csv";
        public const string CHARACTER = "CharacterTable.csv";
        public const string LEVEL = "LevelTable.csv";
        public const string SKILL = "SkillTable.csv";
        public const string EFFECT = "EffectTable.csv";
        public const string PREFAB = "PrefabTable.csv";
        public const string EQUIPMENT = "EquipmentTable.csv";
        public const string CONSUMPTION = "ConsumptionTable.csv";
    }

    public static class Path
    {
        public const string ANIMATOR = "Animators/";
        public const string TEXTURE = "Textures/";
        public const string IMAGE = "Images/";
        public const string MATERIAL = "Materials/";
        public const string PREFAB = "Prefabs/";
        public const string SPRITE = "Sprites/";

        public const string TABLE = "Assets/Resources/Tables/";
    }

    public static class Calculate
    {
        public const float CRITICAL_MULTI = 0.5f, SLOT_LOSS = 0.3f;
        public const int MAX_SLOT = 3, MAX_STAT = 100, MIN_STAT = 0;

        /// <summary>
        /// ĳ������ ������� ����ϱ� ���� �޼ҵ��Դϴ�.
        /// </summary>
        /// <param name="health">ĳ���� ���� �����</param>
        /// <param name="equipmentHealth">���� ��� �����</param>
        /// <param name="healthPerLevel">ĳ���� ���� ����</param>
        /// <returns></returns>
        public static int Health(int health, int equipmentHealth, int healthPerLevel = 0)
            => health + equipmentHealth + healthPerLevel;

        /// <summary>
        /// ĳ������ ���ݷ��� ����ϱ� ���� �޼ҵ��Դϴ�.
        /// </summary>
        /// <param name="attack">ĳ���� ���� ���ݷ�</param>
        /// <param name="equipmentAttack">���� ��� ���ݷ�</param>
        /// <param name="attackPerLevel">ĳ���� ���� ����</param>
        /// <returns></returns>
        public static int Attack(int attack, int equipmentAttack = 0, int attackPerLevel = 0) 
            => attack + equipmentAttack + attackPerLevel;

        /// <summary>
        /// ĳ������ ������ ����ϱ� ���� �޼ҵ��Դϴ�.
        /// </summary>
        /// <param name="defense">ĳ���� ���� ����</param>
        /// <param name="equipmentDefense">���� ��� ����</param>
        /// <param name="defensePerLevel">���� �� ������</param>
        public static int Defense(int defense, int equipmentDefense = 0, int defensePerLevel = 0) 
            => defense + equipmentDefense + defensePerLevel < 0 ? 0 : defense + equipmentDefense + defensePerLevel;

        /// <summary>
        /// ĳ������ ��� Ȥ�� ���� ��Ȯ���� ����ϱ� ���� �޼ҵ��Դϴ�.
        /// </summary>
        /// <param name="currentLOA">ĳ���� ���� ��� �Ǵ� ��� ��Ȯ��</param>
        /// <param name="newLOA">���� ��� ��� �Ǵ� ��ų ��Ȯ��</param>
        /// <param name="luckPerLevel">���� �� ������</param>
        public static int LuckOrAccuracy(int currentLOA, int newLOA = 0, int luckPerLevel = 0)
        {
            int result = currentLOA + newLOA + luckPerLevel;
            result = result < 100 ? result < 0 ? 0 : result : 100;

            return result;
        }

        /// <summary>
        /// ĳ������ ���� �������� ����ϱ� ���� �޼ҵ��Դϴ�.
        /// </summary>
        /// <param name="damage">������</param>
        /// <param name="accuracy">��Ȯ��</param>
        public static List<Dictionary<bool, int>> Accuracy(int damage, int focus = 0, int accuracy = 0)
        {
            List<Dictionary<bool, int>> result = new List<Dictionary<bool, int>>();

            for (int index = 0; index < MAX_SLOT; ++index)
            {
                if (index < focus)
                {
                    result.Add(ReturnDictionary(true, damage));

                    continue;
                }

                bool isCheckChance = IsChance(accuracy);

                if (isCheckChance)
                {
                    result.Add(ReturnDictionary(isCheckChance, damage));
                }
                else
                {
                    result.Add(ReturnDictionary(isCheckChance, Convert.ToInt32(damage * SLOT_LOSS)));
                }
            }

            return result;
        }

        /// <summary>
        /// ĳ������ �� �������� ����ϱ� ���� �޼ҵ��Դϴ�.
        /// </summary>
        /// <param name="attack">ĳ���� �ջ� ���ݷ�</param>
        /// <param name="Defense">ĳ���� �ջ� ����</param>
        /// <param name="luck">ĳ���� �ջ� ���</param>
        public static int Damage(int attack, int Defense = 0, int luck = 0)
        {
            luck = luck < 100 ? luck < 0 ? 0 : luck : 100;
            int tempLuck = IsChance(luck) == true ? 1 : 0;
            int result = Convert.ToInt32(attack + (Math.Floor(attack * CRITICAL_MULTI * tempLuck)) - Defense);

            if (result <= 0)
            {
                Debug.Log(result + " case : 1 ");
                return 1;
            }

            return result;
        }

        /// <summary>
        /// ��� �Ǵ� ��Ȯ���� Ȯ���� ����ϱ� ���� �޼ҵ��Դϴ�.
        /// </summary>
        /// <param name="luckOrAccuracy">��� �Ǵ� ��Ȯ��</param>
        public static bool IsChance(int luckOrAccuracy)
        {
            int threshold = Random.Range(0, 100);

            if (threshold < luckOrAccuracy)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// �������� �����Ͽ� ��ġ���� �����մϴ�.
        /// </summary>
        /// <param name="cardinate">�Ӱ谪</param>
        /// <param name="adjustment">������</param>
        public static int AdjustCardinate(int cardinate, int adjustment = 0)
        {
            int random = Random.Range(-5, 5);
            int adjust = cardinate + adjustment + random;

            return Mathf.Clamp(adjust, 0, 100);
        }
    }
}
