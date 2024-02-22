using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;
using System;
using Unity.VisualScripting;

public abstract class UI_Base : MonoBehaviour
{
    protected Dictionary<Type, UnityEngine.Object[]> _objects = new Dictionary<Type, UnityEngine.Object[]>();

    public abstract void Init(); // �߻�޼���
    
    // Bind<Buttons> => enum Ŭ������ Buttons ������ �ѱ��.
    // Buttons = key, AttackButton = value
    protected void Bind<T>(Type type) where T : UnityEngine.Object // T �߿� UnityEngine.Object Ÿ�Ը� ��ȯ
    {
        string[] names = Enum.GetNames(type);
        UnityEngine.Object[] objects = new UnityEngine.Object[names.Length]; // enum�� ���ǵ� �������� ã�� ������ �´� �迭 ����
        _objects.Add(typeof(T), objects);//objects�� �´� Ÿ���� �߰�

        for(int i = 0; i < names.Length; i++)//�迭�� ���� ��ŭ �ݺ�
        {
            if(typeof(T) == typeof(GameObject)) // T�� GameObject Ÿ���̸� ����
            {
                objects[i] = Utils.FindChild(gameObject, names[i], true); // ���� ������Ʈ�� �̸��� �´� ���� ã�´�.
                //FindChild�� ������ ��� ������Ʈ�� ã�´�. names[i]�� ����� �̸��� ��ġ�ϴ� ������Ʈ�� �ε��Ѵ�.
            }
            else
            {
                objects[i] = Utils.FindChild<T>(gameObject, names[i], true);
                //��ư�̳� �̹���ó�� ������Ʈ�� ���� �� ������Ʈ�� ���� �־ �������´�.
            }
        }
    }

    protected T Get<T>(int idx) where T : UnityEngine.Object
    {
        UnityEngine.Object[] objects = null;
        if (_objects.TryGetValue(typeof(T), out objects) == false)
            return null;

        return objects[idx] as T;
    }

    protected GameObject GetObject(int idx) { return Get<GameObject>(idx); }//���� ������Ʈ ��������
    protected Text GetText(int idx) { return Get<Text>(idx); }//�ؽ�Ʈ�� ��������
    protected Button GetButton(int idx) { return Get<Button>(idx); }//��ư���� ��������
    protected Image GetImage(int idx) { return Get<Image>(idx); }//�̹����� ��������
}
