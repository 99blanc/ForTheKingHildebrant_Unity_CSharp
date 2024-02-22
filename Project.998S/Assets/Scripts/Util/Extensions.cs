using System;
using UniRx;
using UnityEngine.EventSystems;
using UnityEngine;

public static class Extensions
{
    /// <summary>
    /// �������� �̺�Ʈ�� ����ϱ� ���� �޼ҵ��Դϴ�.
    /// </summary>
    /// <param name="view">������</param>
    /// <param name="action">�׼� �̺�Ʈ</param>
    /// <param name="type">������ Ÿ��</param>
    /// <param name="component">������Ʈ</param>
    public static void BindViewEvent(this UIBehaviour view, Action<PointerEventData> action, ViewEvent type, Component component)
        => UserInterface.BindViewEvent(view, action, type, component);

    /// <summary>
    /// ������ ���� �̺�Ʈ�� ����ϱ� ���� �޼ҵ��Դϴ�.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="model">������ ��</param>
    /// <param name="action">�׼� �̺�Ʈ</param>
    /// <param name="component">������Ʈ</param>
    public static void BindModelEvent<T>(this ReactiveProperty<T> model, Action<T> action, Component component)
        => UserInterface.BindModelEvent(model, action, component);

    /// <summary>
    /// ���� ������Ʈ���� ĳ���� Ÿ�� ������Ʈ�� ��ȯ�ϱ� ���� �޼ҵ��Դϴ�.
    /// </summary>
    /// <param name="gameObject">���� ������Ʈ</param>
    public static T GetCharacterInGameObject<T>(this GameObject gameObject)
        => Utils.GetCharacterInGameObject<T>(gameObject);

    public static T GetCharacterInGameObject<T>(this Character character)
        => Utils.GetCharacterInGameObject<T>(character.gameObject);

    /// <summary>
    /// ���� ������Ʈ���� ĳ���� ������Ʈ�� Ÿ���� ��ȯ�ϱ� ���� �޼ҵ��Դϴ�.
    /// </summary>
    /// <param name="gameObject">���� ������Ʈ</param>
    public static Type GetCharacterTypeInGameObject<T>(this GameObject gameObject)
        => Utils.GetCharacterTypeInGameObject<T>(gameObject);

    public static Type GetCharacterTypeInGameObject<T>(this Character character)
        => Utils.GetCharacterTypeInGameObject<T>(character.gameObject);

    /// <summary>
    /// ���� ������Ʈ���� ĳ���� ������Ʈ�� ��� ���θ� ��ȯ�ϱ� ���� �޼ҵ��Դϴ�.
    /// </summary>
    /// <param name="gameObject"></param>
    public static bool IsCharacterDead(this GameObject gameObject)
        => Utils.IsCharacterDead(gameObject);

    public static bool IsCharacterDead(this Character character)
        => Utils.IsCharacterDead(character.gameObject);
}
