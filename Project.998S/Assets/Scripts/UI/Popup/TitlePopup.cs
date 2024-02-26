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
    
    private Buttons _currentButton;
    private Buttons CurrentButton
    {
        get => _currentButton;
        set
        {
            SetButtonNormal(_currentButton);
            SetButtonHighlighted(value);

            _currentButton = value;
        }
    }

    private static readonly Color NORMAL_COLOR = Color.white;
    private static readonly Color HIGHLIGHTED_COLOR = Color.black;

    public override void Init()
    {
        base.Init();

        BindButton(typeof(Buttons));
        
        CurrentButton = _currentButton;
        
        foreach(Buttons buttonIndex in Enum.GetValues(typeof(Buttons)))
        {
            Button button = GetButton((int)buttonIndex);
            button.BindViewEvent(OnEnterButton, ViewEvent.Enter, this);
            button.BindViewEvent(OnClickButton, ViewEvent.LeftClick, this);
            SetSpriteStateInButton(button);
        }
        this.UpdateAsObservable() // �� ������ ���� ��ȭ�� üũ�ϴ� �޼���
            .Subscribe(OnPressKey);

    }

    public void InitButton(int buttonIndex)
    {
        _currentButton = (Buttons)buttonIndex;
    }

    private void OnEnterButton(PointerEventData eventData)
    {
        Buttons nextButton = Enum.Parse<Buttons>(eventData.pointerEnter.name);
        CurrentButton = nextButton;
    }

    private void OnClickButton(PointerEventData eventData)
    {
        Buttons button = Enum.Parse<Buttons>(eventData.pointerClick.name);

        ProcessButton(button);
    }

    private void OnPressKey(Unit unit)
    {
        if (Input.GetMouseButtonDown(0))
        {
            ProcessButton(CurrentButton);
        }
    }
    private void SetSpriteStateInButton(Button button)
    {
        button.transition = Selectable.Transition.SpriteSwap;

        SpriteState spriteState = button.spriteState;

        button.image.sprite = Managers.Resource.LoadSprite("TitleButton");
        spriteState.selectedSprite = Managers.Resource.LoadSprite("TitleButtonHover");
        spriteState.highlightedSprite = Managers.Resource.LoadSprite("TitleButtonHover");
    }

    private void SetButtonNormal(Buttons buttonIndex)
    {
        GetImage((int)buttonIndex).sprite = Managers.Resource.LoadSprite("TitleButton");
        GetText((int)buttonIndex).color = NORMAL_COLOR;
    }

    private void SetButtonHighlighted(Buttons buttonIndex)
    {
        GetImage((int)buttonIndex).sprite = Managers.Resource.LoadSprite("TitleButtonHover");
        GetText((int)buttonIndex).color = HIGHLIGHTED_COLOR;
    }

    private void ProcessButton(Buttons button)
    {
        switch (button)
        {
            case Buttons.NewGameButton:
                Debug.Log("click");
                OnClickPlayButton();
                break;
            case Buttons.QuitButton:
                OnClickQuitButton();
                break;
            default:
                break;
        }
    }

    private void OnClickPlayButton()
    {
        Managers.UI.ClosePopupUI(this);
        Debug.Log("Closed");
        //StartStageCamera.SetActive(true);
        //cart.m_Speed = 7;
        //��� 1. ���� �����ǰ� ���� �������� ������ count�Ͽ� count == 2�϶� StartStageCamera.SetActive(false);
            // count 2�� ���� : 1�� �ϸ� ó�� ī�޶� ������ �� ���ļ� ī�޶� �ٷ� ����.
            // ���� count ���� �� TurnCamera.SetActive(ture);
        Managers.UI.OpenPopup<HUDPopup>();
        
        //// Player�� ���� ���� �Ǿ��� �� cart.m_Speed(3);
        // Enemies�� ���� ���� �Ǿ��� ���� cart.m_Speed(-3);
        // ���� Enemies�� Count == 0 �� �� TurnCamera.SetActive(false);
        
    }
    private void OnClickQuitButton()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
