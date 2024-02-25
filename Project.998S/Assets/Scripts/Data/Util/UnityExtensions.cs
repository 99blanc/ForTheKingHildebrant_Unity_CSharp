using UnityEngine;

public static class UnityExtensions
{
    /// <summary>
    /// Ʈ������ Find() ���� �߿� �߻��ϴ� ������ �����ϱ� ���� �޼ҵ��Դϴ�.
    /// </summary>
    /// <param name="transform">Ʈ������</param>
    /// <param name="name">�̸�</param>
    public static Transform FindAssert(this Transform transform, string name)
    {
        Transform newTransform = transform.Find(name);

        Debug.Assert(newTransform != null);

        return newTransform;
    }

    /// <summary>
    /// ���� ������Ʈ GetComponent<>() ���� �߿� �߻��ϴ� ������ �����ϱ� ���� �޼ҵ��Դϴ�.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="gameObject">���� ������Ʈ</param>
    public static T GetComponentAssert<T>(this GameObject gameObject)
    {
        T component = gameObject.GetComponent<T>();

        Debug.Assert(component != null);

        return component;
    }

    /// <summary>
    /// Ʈ������ GetComponent<>() ���� �߿� �߻��ϴ� ������ �����ϱ� ���� �޼ҵ��Դϴ�.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="transform">Ʈ������</param>
    public static T GetComponentAssert<T>(this Transform transform)
    {
        T component = transform.GetComponent<T>();

        Debug.Assert(component != null);

        return component;
    }

    /// <summary>
    /// ������Ʈ GetComponent<>() ���� �߿� �߻��ϴ� ������ �����ϱ� ���� �޼ҵ��Դϴ�.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="component">������Ʈ</param>
    public static T GetComponentAssert<T>(this Component component)
    {
        T newComponent = component.GetComponent<T>();

        Debug.Assert(newComponent != null);

        return newComponent;
    }

    /// <summary>
    /// ���� ������Ʈ GetComponentInChildren<>() ���� �߿� �߻��ϴ� ������ �����ϱ� ���� �޼ҵ��Դϴ�.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="gameObject">���� ������Ʈ</param>
    public static T GetComponentInChildrenAssert<T>(this GameObject gameObject)
    {
        T component = gameObject.GetComponentInChildren<T>();

        Debug.Assert(component != null);

        return component;
    }

    /// <summary>
    /// Ʈ������ GetComponentInChildren<>() ���� �߿� �߻��ϴ� ������ �����ϱ� ���� �޼ҵ��Դϴ�.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="transform">Ʈ������</param>
    public static T GetComponentInChildrenAssert<T>(this Transform transform)
    {
        T component = transform.GetComponentInChildren<T>();

        Debug.Assert(component != null);

        return component;
    }

    /// <summary>
    /// ������Ʈ GetComponentInChildren<>() ���� �߿� �߻��ϴ� ������ �����ϱ� ���� �޼ҵ��Դϴ�.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="component">������Ʈ</param>
    public static T GetComponentInChildrenAssert<T>(this Component component)
    {
        T newComponent = component.GetComponentInChildren<T>();

        Debug.Assert(newComponent != null);

        return newComponent;
    }
}
