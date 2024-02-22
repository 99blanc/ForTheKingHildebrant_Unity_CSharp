public class UISubItem : UserInterface
{
    #region Methods
    /// <summary>
    /// �������̽� ��� ������ ���� �޼ҵ��Դϴ�.
    /// </summary>
    public override void Init()
    {
        base.Init();

        Managers.UI.SetCanvas(gameObject, false);
    }

    /// <summary>
    /// �������̽� ��� ���Ḧ ���� �޼ҵ��Դϴ�.
    /// </summary>
    public virtual void CloseSubItem()
    {
        Managers.Resource.Destroy(gameObject);
    }
    #endregion
}