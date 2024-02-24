using System.Collections.Generic;
using System;
using TMPro;
using UniRx.Triggers;
using UniRx;
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
    private Dictionary<Type, Object[]> objects = new Dictionary<Type, Object[]>();

    public virtual void Init()
    {

    }

    private void Start() => Init();

    protected void Bind<T>(Type type) where T : Object
    {
        string[] names = Enum.GetNames(type);
        Object[] newObjects = new Object[names.Length];
        objects.Add(typeof(T), newObjects);

        for (int i = 0; i < names.Length; ++i)
        {
            if (typeof(T) == typeof(GameObject))
            {
                newObjects[i] = Utils.FindChild(gameObject, names[i], true);
            }
            else
            {
                newObjects[i] = Utils.FindChild<T>(gameObject, names[i], true);
            }
        }
    }

    /// <summary>
    /// ���� ������Ʈ ����� ���� �޼ҵ��Դϴ�.
    /// </summary>
    /// <param name="type">���ε� ���� ������Ʈ</param>
    protected void BindObject(Type type) 
        => Bind<GameObject>(type);

    /// <summary>
    /// �ο� �̹��� ����� ���� �޼ҵ��Դϴ�.
    /// </summary>
    /// <param name="type">���ε� �ο� �̹���</param>
    protected void BindRawImage(Type type) 
        => Bind<RawImage>(type);

    /// <summary>
    /// �̹��� ����� ���� �޼ҵ��Դϴ�.
    /// </summary>
    /// <param name="type">���ε� �̹���</param>
    protected void BindImage(Type type) 
        => Bind<Image>(type);

    /// <summary>
    /// �ؽ�Ʈ ����� ���� �޼ҵ��Դϴ�.
    /// </summary>
    /// <param name="type">���ε� �ؽ�Ʈ</param>
    protected void BindText(Type type) 
        => Bind<TMP_Text>(type);

    /// <summary>
    /// ��ư ����� ���� �޼ҵ��Դϴ�.
    /// </summary>
    /// <param name="type">���ε� ��ư</param>
    protected void BindButton(Type type) 
        => Bind<Button>(type);

    protected T Get<T>(int index) where T : Object
    {
        if (objects.TryGetValue(typeof(T), out Object[] newObjects))
        {
            return newObjects[index] as T;
        }

        throw new InvalidOperationException($"Failed to Get({typeof(T)}, {index}). Binding must be completed first.");
    }

    /// <summary>
    /// ���� ������Ʈ�� ��ȯ�ϴ� �޼ҵ��Դϴ�.
    /// </summary>
    /// <param name="index">�ε��� ��ȣ</param>
    protected GameObject GetObject(int index) 
        => Get<GameObject>(index);

    /// <summary>
    /// �ο� �̹����� ��ȯ�ϴ� �޼ҵ��Դϴ�.
    /// </summary>
    /// <param name="index">�ε��� ��ȣ</param>
    protected RawImage GetRawImage(int index)
        => Get<RawImage>(index);

    /// <summary>
    /// �̹����� ��ȯ�ϴ� �޼ҵ��Դϴ�.
    /// </summary>
    /// <param name="index">�ε��� ��ȣ</param>
    protected Image GetImage(int index) 
        => Get<Image>(index);

    /// <summary>
    /// �ؽ�Ʈ�� ��ȯ�ϴ� �޼ҵ��Դϴ�.
    /// </summary>
    /// <param name="index">�ε��� ��ȣ</param>
    protected TMP_Text GetText(int index) 
        => Get<TMP_Text>(index);

    /// <summary>
    /// ��ư�� ��ȯ�ϴ� �޼ҵ��Դϴ�.
    /// </summary>
    /// <param name="index">�ε��� ��ȣ</param>
    protected Button GetButton(int index) 
        => Get<Button>(index);

    /// <summary>
    /// �������� �̺�Ʈ�� ����ϱ� ���� �޼ҵ��Դϴ�.
    /// </summary>
    /// <param name="view">������</param>
    /// <param name="action">�׼� �̺�Ʈ</param>
    /// <param name="type">������ Ÿ��</param>
    /// <param name="component">������Ʈ</param>
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

    /// <summary>
    /// ������ ������ �̺�Ʈ�� ����ϱ� ���� �޼ҵ��Դϴ�.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="model">������ ����</param>
    /// <param name="action">�׼� �̺�Ʈ</param>
    /// <param name="component">������Ʈ</param>
    public static void BindModelEvent<T>(ReactiveProperty<T> model, Action<T> action, Component component)
       => model.Subscribe(action).AddTo(component);
}
