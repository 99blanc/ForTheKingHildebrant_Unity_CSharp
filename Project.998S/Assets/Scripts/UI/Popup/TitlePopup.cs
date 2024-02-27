using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UniRx;
using UniRx.Triggers;
using System;

public class TitlePopup : UIPopup
{
    enum Buttons
    {
        NewGameButton,
        LoadGameButton,
        QuitButton
    }

    public override void Init()
    {
        base.Init();

        BindButton(typeof(Buttons));

        foreach (Buttons buttonIndex in Enum.GetValues(typeof(Buttons)))
        {
            Button button = GetButton((int)buttonIndex);
            button.BindViewEvent(OnClickButton, ViewEvent.LeftClick, this);
            SetSpriteStateInButton(button);
        }
    }

    private void SetSpriteStateInButton(Button button)
    {
        button.transition = Selectable.Transition.SpriteSwap;

        SpriteState spriteState = button.spriteState;

        button.spriteState = spriteState;

        button.image.sprite = Managers.Resource.LoadSprite("TitleButton");
        spriteState.selectedSprite = Managers.Resource.LoadSprite("TitleButtonHover");
        spriteState.highlightedSprite = Managers.Resource.LoadSprite("TitleButtonHover");
    }

    private void OnClickButton(PointerEventData eventData)
    {
        Buttons button = Enum.Parse<Buttons>(eventData.pointerEnter.name);
        Debug.Log(button);
        ProcessButton(button);
    }

    private void ProcessButton(Buttons button)
    {
        switch (button)
        {
            case Buttons.NewGameButton:
                OnClickPlayButton();
                break;
            case Buttons.QuitButton:
                OnClickQuitButton();
                break;
        }
    }

    private void OnClickPlayButton()
    {
        Managers.Game.GameStart((StageID)1);
        //StartStageCamera.SetActive(true);
        //cart.m_Speed = 7;
        //��� 1. ���� �����ǰ� ���� �������� ������ count�Ͽ� count == 2�϶� StartStageCamera.SetActive(false);
            // count 2�� ���� : 1�� �ϸ� ó�� ī�޶� ������ �� ���ļ� ī�޶� �ٷ� ����.
            // ���� count ���� �� TurnCamera.SetActive(ture);
        
        //// Player�� ���� ���� �Ǿ��� �� cart.m_Speed(3);
        // Enemies�� ���� ���� �Ǿ��� ���� cart.m_Speed(-3);
        // ���� Enemies�� Count == 0 �� �� TurnCamera.SetActive(false);
        
    }
    private void OnClickQuitButton()
    {
        Application.Quit();
    }
}
