using System;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UniRx.Triggers;
using Object = UnityEngine.Object;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

public enum ViewEvent
{
    Click,
    Enter
}

public abstract class UserInterface : MonoBehaviour
{
    private Dictionary<Type, Object[]> _objects = new Dictionary<Type, Object[]>();

    public virtual void Init()
    {

    }

    private void Start() => Init();

    protected void Bind<T>(Type type) where T : Object
    {
        string[] names = Enum.GetNames(type);
        Object[] objects = new Object[names.Length];
        _objects.Add(typeof(T), objects);

        for (int i = 0; i < names.Length; ++i)
        {
            if (typeof(T) == typeof(GameObject))
            {
                objects[i] = Utils.FindChild(gameObject, names[i], true);
            }
            else
            {
                objects[i] = Utils.FindChild<T>(gameObject, names[i], true);
            }
        }
    }

    /// <summary>
    /// ���� ������Ʈ ����� ���� �޼ҵ��Դϴ�.
    /// </summary>
    /// <param name="type">���ε� ���� ������Ʈ</param>
    protected void BindObject(Type type) => Bind<GameObject>(type);

    /// <summary>
    /// �̹��� ����� ���� �޼ҵ��Դϴ�.
    /// </summary>
    /// <param name="type">���ε� �̹���</param>
    protected void BindImage(Type type) => Bind<Image>(type);

    /// <summary>
    /// �ؽ�Ʈ ����� ���� �޼ҵ��Դϴ�.
    /// </summary>
    /// <param name="type">���ε� �ؽ�Ʈ</param>
    protected void BindText(Type type) => Bind<TMP_Text>(type);

    /// <summary>
    /// ��ư ����� ���� �޼ҵ��Դϴ�.
    /// </summary>
    /// <param name="type">���ε� ��ư</param>
    protected void BindButton(Type type) => Bind<Button>(type);

    protected T Get<T>(int index) where T : Object
    {
        if (_objects.TryGetValue(typeof(T), out Object[] objects))
        {
            return objects[index] as T;
        }

        throw new InvalidOperationException($"Failed to Get({typeof(T)}, {index}). Binding must be completed first.");
    }

    /// <summary>
    /// ���� ������Ʈ ��ȯ�� ���� �޼ҵ��Դϴ�.
    /// </summary>
    /// <param name="index">�ε��� ��ȣ</param>
    /// <returns></returns>
    protected GameObject GetObject(int index) => Get<GameObject>(index);
    /// <summary>
    /// �̹��� ��ȯ�� ���� �޼ҵ��Դϴ�.
    /// </summary>
    /// <param name="index">�ε��� ��ȣ</param>
    /// <returns></returns>
    protected Image GetImage(int index) => Get<Image>(index);
    /// <summary>
    /// �ؽ�Ʈ ��ȯ�� ���� �޼ҵ��Դϴ�.
    /// </summary>
    /// <param name="index">�ε��� ��ȣ</param>
    /// <returns></returns>
    protected TMP_Text GetText(int index) => Get<TMP_Text>(index);
    /// <summary>
    /// ��ư ��ȯ�� ���� �޼ҵ��Դϴ�.
    /// </summary>
    /// <param name="index">�ε��� ��ȣ</param>
    /// <returns></returns>
    protected Button GetButton(int index) => Get<Button>(index);

    public static void BindViewEvent(UIBehaviour view, Action<PointerEventData> action, ViewEvent type, Component component)
    {
        switch (type)
        {
            case ViewEvent.Click:
                view.OnPointerClickAsObservable().Subscribe(action).AddTo(component);
                break;
            case ViewEvent.Enter:
                view.OnPointerEnterAsObservable().Subscribe(action).AddTo(component);
                break;
        };
    }

    public static void BindModelEvent<T>(ReactiveProperty<T> model, Action<T> action, Component component)
       => model.Subscribe(action).AddTo(component);
}
