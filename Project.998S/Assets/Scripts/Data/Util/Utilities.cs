using System.Collections.Generic;
using System;
using Object = UnityEngine.Object;
using UnityEngine;

namespace Utils
{
    public static class Utilities
    {
        /// <summary>
        /// ���� ������Ʈ���� ĳ���� Ÿ�� ������Ʈ�� ��ȯ�ϱ� ���� �޼ҵ��Դϴ�.
        /// </summary>
        /// <param name="gameObject">���� ������Ʈ</param>
        public static T GetCharacterInGameObject<T>(GameObject gameObject) where T : Object
        {
            T character;

            if (gameObject.TryGetComponent(out character))
            {
                return character;
            }

            return gameObject.GetComponent<T>();

            throw new InvalidOperationException();
        }

        /// <summary>
        /// ���� ������Ʈ���� ĳ���� ������Ʈ�� Ÿ�� ��ȯ�ϱ� ���� �޼ҵ��Դϴ�.
        /// </summary>
        /// <param name="gameObject">���� ������Ʈ</param>
        public static Type GetCharacterTypeInGameObject<T>(GameObject gameObject) where T : Object
            => gameObject.GetCharacterInGameObject<T>().GetType();

        /// <summary>
        /// ���� ������Ʈ���� ĳ���� ������Ʈ�� ��� ���θ� ��ȯ�ϱ� ���� �޼ҵ��Դϴ�.
        /// </summary>
        /// <param name="character">ĳ����</param>
        public static bool IsCharacterDead(Character character)
            => character.characterState.Value == CharacterState.Dead ? true : false;

        /// <summary>
        /// ���� ������Ʈ���� ĳ���� ������Ʈ�� ���� ���θ� ��ȯ�ϱ� ���� �޼ҵ��Դϴ�.
        /// </summary>
        /// <param name="character">ĳ����</param>
        /// <returns></returns>
        public static bool IsCharacterAttack(Character character)
            => character.characterState.Value == CharacterState.NormalAttack ||
               character.characterState.Value == CharacterState.ShortSkill ||
            character.characterState.Value == CharacterState.LongSkill ? true : false;

        /// <summary>
        /// ���ʸ����� FindChild() ������ �Ŀ� ���� ��ȯ�ϴ� �޼ҵ��Դϴ�.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="gameObject">���� ������Ʈ</param>
        /// <param name="name">�̸�</param>
        /// <param name="recursive">��� ����</param>
        public static T FindChild<T>(GameObject gameObject, string name = null, bool recursive = false) where T : Object
        {
            if (gameObject == null)
            {
                throw new InvalidOperationException();
            }

            if (false == recursive)
            {
                for (int i = 0; i < gameObject.transform.childCount; i++)
                {
                    Transform transform = gameObject.transform.GetChild(i);

                    if (string.IsNullOrEmpty(name) || transform.name == name)
                    {
                        T component = transform.GetComponent<T>();

                        if (component != null)
                        {
                            return component;
                        }
                    }
                }
            }
            else
            {
                foreach (T component in gameObject.GetComponentsInChildren<T>())
                {
                    if (false == string.IsNullOrEmpty(name) && component.name != name)
                    {
                        continue;
                    }

                    return component;
                }
            }

            throw new InvalidOperationException();
        }

        /// <summary>
        /// Ʈ������ Ÿ���� ���ʸ����� FindChild() ������ �Ŀ� ���� ��ȯ�ϴ� �޼ҵ��Դϴ�.
        /// </summary>
        /// <param name="gameObject">���� ������Ʈ</param>
        /// <param name="name">�̸�</param>
        /// <param name="recursive">��� ����</param>
        public static GameObject FindChild(GameObject gameObject, string name = null, bool recursive = false)
            => FindChild<Transform>(gameObject, name, recursive).gameObject;

        /// <summary>
        /// ��ųʸ� ���� Ÿ���� �κ� ��Ҹ� ��ȯ�ϱ� ���� �޼ҵ��Դϴ�.
        /// </summary>
        /// <typeparam name="T">TKey</typeparam>
        /// <typeparam name="U">TValue</typeparam>
        /// <param name="key">Ű</param>
        /// <param name="value">��</param>
        public static Dictionary<T, U> ReturnDictionary<T, U>(T key, U value)
            => new Dictionary<T, U>() { { key, value } };

        /// <summary>
        /// ����Ʈ Ÿ���� �κ� ��Ҹ� ��ȯ�ϱ� ���� �޼ҵ��Դϴ�.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">��</param>
        public static List<T> ReturnList<T>(T value)
            => new List<T>() { { value } };

        /// <summary>
        /// �迭 Ÿ���� �κ� ��Ҹ� ��ȯ�ϱ� ���� �޼ҵ��Դϴ�.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values">��</param>
        public static T[] ReturnArray<T>(params T[] values)
            => values;

        /// <summary>
        /// ������ ���ڿ� ���̵� �迭�� ������ ������ ���̵� �迭�� ��ȯ�ϱ� ���� �޼ҵ��Դϴ�.
        /// </summary>
        /// <param name="IdEnum">���ڿ� IdEnum</param>
        public static int[] ReferenceDataByIdEnum(string IdEnum)
        {
            string[] splitByComma = IdEnum.Split('.');
            int[] ids = new int[splitByComma.Length];

            if (false == IdEnum.Contains('.'))
            {
                return new int[] { int.Parse(IdEnum) };
            }

            for (int index = 0; index < splitByComma.Length; ++index)
            {
                ids[index] = int.Parse(IdEnum);
            }

            return ids;
        }

        /// <summary>
        /// ����Ʈ�� �ε����� �����ϴ��� ���θ� �Ǵ��Ͽ� ��ȯ�ϴ� �޼ҵ��Դϴ�.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">����Ʈ</param>
        /// <param name="index">�ε���</param>
        public static bool ArrayIndexExists<T>(T[] array, int index)
        {
            return index >= 0 && index < array.Length;
        }
    }
}