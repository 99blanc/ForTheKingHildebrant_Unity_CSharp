using System;
using System.Collections.Generic;
using Object = UnityEngine.Object;
using UnityEngine;

public static class Utils
{
    public static T FindChild<T>(GameObject go, string name = null, bool recursive = false) where T : Object
    {
        if (go == null)
        {
            throw new InvalidOperationException($"GameObject is null.");
        }

        if (false == recursive)
        {
            //return go.transform.FindAssert(name).GetComponentAssert<T>();
            for(int i = 0; i < go.transform.childCount; i++)
            {
                Transform transform = go.transform.GetChild(i);
                if(string.IsNullOrEmpty(name) || transform.name == name)
                {
                    T component = transform.GetComponent<T>();
                    if (component != null)
                        return component;
                }
            }
        }
        else
        {
            foreach (T component in go.GetComponentsInChildren<T>())
            {
                if (false == string.IsNullOrEmpty(name) && component.name != name)
                {
                    continue;
                }

                return component;
            }
        }

        throw new InvalidOperationException($"Child {typeof(T).Name} not found.");
    }

    /// <summary>
    /// Ʈ������ FindChild() ���� �߿� �߻��ϴ� ������ �����ϱ� ���� �޼ҵ��Դϴ�.
    /// </summary>
    /// <param name="go">���� ������Ʈ</param>
    /// <param name="name">�̸�</param>
    /// <param name="recursive">��� ����</param>
    public static GameObject FindChild(GameObject go, string name = null, bool recursive = false)
        => FindChild<Transform>(go, name, recursive).gameObject;

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
    /// <param name="gameObject"></param>
    public static bool IsCharacterDead(GameObject gameObject)
    {
        Character character = gameObject.GetCharacterInGameObject<Character>();

        if (character.characterState.Value == CharacterState.Death)
        {
            return true;
        }

        return false;
    }

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
    /// Ư�� Ÿ���� �迭 ��Ҹ� �迭�� �����ϱ� ���� �޼ҵ��Դϴ�.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="values">��</param>
    public static T[] ReturnArray<T>(params T[] values)
        => values;

    /// <summary>
    /// ������ ���ڿ� ���̵� �迭�� ������ ������ ���̵� �迭�� ��ȯ�ϱ� ���� �޼ҵ��Դϴ�.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="IdEnum">���ڿ� IdEnum</param>
    public static T[] ReferenceDataByIdEnum<T>(string IdEnum) where T : Enum
    {
        int[] splitByComma = Array.ConvertAll(IdEnum.Split("."), int.Parse);
        T[] values = new T  [splitByComma.Length];

        for (int index = 0; index < splitByComma.Length; ++index)
        {
            values[index] = (T)Enum.ToObject(typeof(T), splitByComma[index]);
        }

        return values;
    }
}
