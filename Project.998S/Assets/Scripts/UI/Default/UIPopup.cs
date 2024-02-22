public class UIPopup : UserInterface
{
    #region Methods
    /// <summary>
    /// �������̽� �˾� ������ ���� �޼ҵ��Դϴ�.
    /// </summary>
    public override void Init()
    {
        base.Init();

        Managers.UI.SetCanvas(gameObject);
    }

    /// <summary>
    /// �������̽� �˾� ���Ḧ ���� �޼ҵ��Դϴ�.
    /// </summary>
    public virtual void ClosePopupUI()
    {
        Managers.UI.ClosePopupUI(this);
    }
    #endregion
}
