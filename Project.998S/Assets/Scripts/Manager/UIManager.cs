using System.Collections.Generic;
using UnityEngine;

public class UIManager
{
    private Stack<UIPopup> popupStack;

    private static readonly Vector3 DEFAULT_SCALE = Vector3.one;
    private int currentCanvasOrder = -20;

    private GameObject UIRoot;

    public void Init()
    {
        popupStack = new Stack<UIPopup>();
        UIRoot = new GameObject(nameof(UIRoot));
    }

    /// <summary>
    /// �������̽� ĵ���� ������Ʈ�� ����ϴ� �޼ҵ��Դϴ�.
    /// </summary>
    /// <param name="gameObject">���� ������Ʈ</param>
    /// <param name="sort">���� ����</param>
    public void SetCanvas(GameObject gameObject, bool sort = true)
    {
        Canvas canvas = gameObject.GetComponentAssert<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.overrideSorting = true;

        if (true == sort)
        {
            canvas.sortingOrder = currentCanvasOrder;
            currentCanvasOrder += 1;
        }
        else
        {
            canvas.sortingOrder = 0;
        }
    }

    /// <summary>
    /// �˾� �������̽��� �����ϴ� �޼ҵ��Դϴ�.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="parent">�θ� Ʈ������</param>
    public T OpenPopup<T>(Transform parent = null) where T : UIPopup
    {
        T popup = SetupUI<T>(parent);

        popupStack.Push(popup);

        return popup;
    }

    /// <summary>
    /// ���� ������ �������̽��� �����ϴ� �޼ҵ��Դϴ�.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="parent">�θ� Ʈ������</param>
    public T OpenSubItem<T>(Transform parent = null) where T : UISubItem
        => SetupUI<T>(parent);

    /// <summary>
    /// �������̽��� �����ϴ� �޼ҵ��Դϴ�.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="parent">�θ� Ʈ������</param>
    /// <returns></returns>
    private T SetupUI<T>(Transform parent = null) where T : UserInterface
    {
        GameObject prefab = Managers.Resource.LoadPrefab(typeof(T).Name);
        GameObject gameObject = Managers.Resource.Instantiate(prefab);

        if (parent != null)
        {
            gameObject.transform.SetParent(parent);
        }
        else
        {
            gameObject.transform.SetParent(UIRoot.transform);
        }

        gameObject.transform.localScale = DEFAULT_SCALE;
        gameObject.transform.localPosition = prefab.transform.position;

        return gameObject.GetComponentAssert<T>();
    }

    /// <summary>
    /// ���ÿ� ��ϵ� �ϳ��� �˾��� ã�� �ݴ� �޼ҵ��Դϴ�.
    /// </summary>
    /// <param name="popup">�˾� Ÿ��</param>
    public void ClosePopupUI(UIPopup popup)
    {
        if (popupStack.Count == 0)
        {
            return;
        }

        if (popupStack.Peek() != popup)
        {
            return;
        }

        ClosePopupUI();
    }

    /// <summary>
    /// ���ÿ� ��ϵ� �ϳ��� �˾��� ���� �ݴ� �޼ҵ��Դϴ�.
    /// </summary>
    public void ClosePopupUI()
    {
        if (popupStack.Count == 0)
        {
            return;
        }

        UIPopup popup = popupStack.Pop();
        Managers.Resource.Destroy(popup.gameObject);
        currentCanvasOrder -= 1;
    }

    /// <summary>
    /// ���ÿ� ��ϵ� ��� �˾��� �ݴ� �޼ҵ��Դϴ�.
    /// </summary>
    public void CloseAllPopupUI()
    {
        while (popupStack.Count > 0)
        {
            ClosePopupUI();
        }
    }
}
