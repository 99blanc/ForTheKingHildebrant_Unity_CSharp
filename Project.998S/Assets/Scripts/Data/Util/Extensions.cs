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
    /// ������ ������ �̺�Ʈ�� ����ϱ� ���� �޼ҵ��Դϴ�.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="model">������ ��</param>
    /// <param name="action">�׼� �̺�Ʈ</param>
    /// <param name="component">������Ʈ</param>
    public static void BindModelEvent<T>(this ReactiveProperty<T> model, Action<T> action, Component component)
        => UserInterface.BindModelEvent(model, action, component);
}
