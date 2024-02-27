using System;
using UnityEngine;
using Object = UnityEngine.Object;
using Utils;

public static class CharacterExtensions
{
    /// <summary>
    /// ���� ������Ʈ���� ĳ���� Ÿ�� ������Ʈ�� ��ȯ�ϱ� ���� �޼ҵ��Դϴ�.
    /// </summary>
    /// <param name="gameObject">���� ������Ʈ</param>
    public static T GetCharacterInGameObject<T>(this GameObject gameObject) where T : Object
        => Utilities.GetCharacterInGameObject<T>(gameObject);

    public static T GetCharacterInGameObject<T>(this Character character) where T : Object
        => Utilities.GetCharacterInGameObject<T>(character.gameObject);

    /// <summary>
    /// ���� ������Ʈ���� ĳ���� ������Ʈ�� Ÿ���� ��ȯ�ϱ� ���� �޼ҵ��Դϴ�.
    /// </summary>
    /// <param name="gameObject">���� ������Ʈ</param>
    public static Type GetCharacterTypeInGameObject<T>(this GameObject gameObject) where T : Object
        => Utilities.GetCharacterTypeInGameObject<T>(gameObject);

    public static Type GetCharacterTypeInGameObject<T>(this Character character) where T : Object
        => Utilities.GetCharacterTypeInGameObject<T>(character.gameObject);

    /// <summary>
    /// ���� ������Ʈ���� ĳ���� ������Ʈ�� ��� ���θ� ��ȯ�ϱ� ���� �޼ҵ��Դϴ�.
    /// </summary>
    /// <param name="character"></param>
    public static bool IsCharacterDead(this Character character)
        => Utilities.IsCharacterDead(character);

    public static bool IsCharacterDead(this GameObject gameObject)
        => Utilities.IsCharacterDead(gameObject.GetCharacterInGameObject<Character>());


    /// <summary>
    /// ���� ������Ʈ���� ĳ���� ������Ʈ�� ���� ���θ� ��ȯ�ϱ� ���� �޼ҵ��Դϴ�.
    /// </summary>
    /// <param name="character"></param>
    public static bool IsCharacterAttack(this Character character)
        => Utilities.IsCharacterAttack(character);

    public static bool IsCharacterAttack(this GameObject gameObject)
        => Utilities.IsCharacterAttack(gameObject.GetCharacterInGameObject<Character>());
}
