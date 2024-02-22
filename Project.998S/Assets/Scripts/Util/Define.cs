using System;
using Random = UnityEngine.Random;

public static class Define
{
    public static class Table
    {
        public const string STAGE = "StageTable.csv";
        public const string CHARACTER = "CharacterTable.csv";
        public const string LEVEL = "LevelTable.csv";
        public const string SKILL = "SkillTable.csv";
        public const string EFFECT = "EffectTable.csv";
        public const string GAMEPREFAB = "GamePrefabTable.csv";
        public const string EQUIP = "EquipTable.csv";
        public const string CONSUMP = "ConsumpTable.csv";
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
        public static int Health(int health, int level = 1) 
            => health + level + Convert.ToInt32(LEVEL_PER_HEALTH * level);

        /// <summary>
        /// ĳ������ ���ݷ��� ����ϱ� ���� �޼ҵ��Դϴ�.
        /// </summary>
        /// <param name="attack">ĳ���� ���� ���ݷ�</param>
        /// <param name="weaponAttack">���� ��� ���ݷ�</param>
        /// <param name="level">ĳ���� ���� ����</param>
        /// <returns></returns>
        public static int Attack(int attack, int weaponAttack = 0, int level = 1) 
            => attack + weaponAttack + Convert.ToInt32(LEVEL_PER_ATTACK * level);

        /// <summary>
        /// ĳ������ ������ ����ϱ� ���� �޼ҵ��Դϴ�.
        /// </summary>
        /// <param name="defense">ĳ���� ���� ����</param>
        /// <returns></returns>
        public static int Defense(int defense) 
            => defense < 0 ? 0 : defense;

        /// <summary>
        /// ĳ������ ����� ����ϱ� ���� �޼ҵ��Դϴ�.
        /// </summary>
        /// <param name="currentluck">ĳ���� ���� ��� �Ǵ� ��� ��Ȯ��</param>
        /// <param name="newluck">���� ��� ��� �Ǵ� ��ų ��Ȯ��</param>
        /// <returns></returns>
        public static int Luck(int currentluck, int newluck)
        {
            int result = currentluck + newluck;
           
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
        /// <param name="armor">ĳ���� �ջ� ����</param>
        /// <param name="luck">ĳ���� �ջ� ���</param>
        /// <returns></returns>
        public static int Damage(int attack, int armor, int luck)
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
            int result = Convert.ToInt32(attack + (attack * CRITICAL_MULTI * tempLuck) - armor);

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
