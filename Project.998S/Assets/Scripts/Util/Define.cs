using System;
using Random = UnityEngine.Random;

public static class Define
{
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
        public const string ICON = "Icons/";
        public const string MATERIAL = "Materials/";
        public const string PREFAB = "Prefabs/";
        public const string SPRITE = "Sprites/";

        public const string TABLE = "Assets/Resources/Tables/";
    }

    public static class Calculate
    {
        public const float CRITICAL_MULTI = 0.5f, LEVEL_PER_HEALTH = 0.1f, LEVEL_PER_ATTACK = 1f;

        /// <summary>
        /// ĳ������ ������� ����ϱ� ���� �޼ҵ��Դϴ�.
        /// </summary>
        /// <param name="health">ĳ���� ���� �����</param>
        /// <param name="level">ĳ���� ���� ����</param>
        /// <returns></returns>
        public static int Health(int health, int healthPerLevel = 0)
            => health + healthPerLevel;

        /// <summary>
        /// ĳ������ ���ݷ��� ����ϱ� ���� �޼ҵ��Դϴ�.
        /// </summary>
        /// <param name="attack">ĳ���� ���� ���ݷ�</param>
        /// <param name="equipAttack">���� ��� ���ݷ�</param>
        /// <param name="level">ĳ���� ���� ����</param>
        /// <returns></returns>
        public static int Attack(int attack, int equipAttack = 0, int attackPerLevel = 0) 
            => attack + equipAttack + attackPerLevel;

        /// <summary>
        /// ĳ������ ������ ����ϱ� ���� �޼ҵ��Դϴ�.
        /// </summary>
        /// <param name="defense">ĳ���� ���� ����</param>
        /// <returns></returns>
        public static int Defense(int defense, int equipDefense = 0, int defensePerLevel = 0) 
            => defense + equipDefense + defensePerLevel < 0 ? 0 : equipDefense;

        /// <summary>
        /// ĳ������ ����� ����ϱ� ���� �޼ҵ��Դϴ�.
        /// </summary>
        /// <param name="currentluck">ĳ���� ���� ��� �Ǵ� ��� ��Ȯ��</param>
        /// <param name="newluck">���� ��� ��� �Ǵ� ��ų ��Ȯ��</param>
        /// <returns></returns>
        public static int Luck(int currentluck, int newluck = 0, int luckPerLevel = 0)
        {
            int result = currentluck + newluck + luckPerLevel;
           
            switch (result)
            {
                case > 100:
                    result = 100;
                    break;
                case < 0:
                    result = 0;
                    break;
            }

            return result;
        }

        /// <summary>
        /// ĳ������ �� �������� ����ϱ� ���� �޼ҵ��Դϴ�.
        /// </summary>
        /// <param name="attack">ĳ���� �ջ� ���ݷ�</param>
        /// <param name="Defense">ĳ���� �ջ� ����</param>
        /// <param name="luck">ĳ���� �ջ� ���</param>
        /// <returns></returns>
        public static int Damage(int attack, int Defense, int luck)
        {
            switch (luck)
            {
                case < 0:
                    luck = 0;
                    break;
                case > 100:
                    luck = 100;
                    break;
            }

            int tempLuck = IsChance(luck) == true ? 1 : 0;
            int result = Convert.ToInt32(attack + (attack * CRITICAL_MULTI * tempLuck) - Defense);

            if (result < 0)
            {
                return 1;
            }

            return result;
        }

        /// <summary>
        /// ��� �Ǵ� ��Ȯ���� Ȯ���� ����ϱ� ���� �޼ҵ��Դϴ�.
        /// </summary>
        /// <param name="luck">��� �Ǵ� ��Ȯ��</param>
        /// <returns></returns>
        public static bool IsChance(int luck)
        {
            int threshold = Random.Range(0, 100);

            if (threshold < luck)
            {
                return true;
            }

            return false;
        }
    }
}
