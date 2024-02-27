using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class DeathPopup : UIPopup
{
    private enum Images
    {
        Background
    }

    public override void Init()
    {
        base.Init();

        BindImage(typeof(Images));

        GetImage((int)Images.Background).BindViewEvent(ReturnTitle, ViewEvent.LeftClick, this);
    }

    private void ReturnTitle(PointerEventData eventData)
    {
        // NOTE : ���� ������ �������� ������� �� �ִ� ȯ���� �ƴ�
        // ���� ���� ����
        Application.Quit();
    }
}