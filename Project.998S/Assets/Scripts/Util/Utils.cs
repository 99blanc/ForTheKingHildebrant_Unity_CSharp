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
            return go.transform.FindAssert(name).GetComponentAssert<T>();
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
    public static T GetCharacterInGameObject<T>(GameObject gameObject)
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
    public static Type GetCharacterTypeInGameObject<T>(GameObject gameObject)
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
    /// <param name="values"></param>
    public static T[] ReturnArray<T>(params T[] values)
        => values;
}
