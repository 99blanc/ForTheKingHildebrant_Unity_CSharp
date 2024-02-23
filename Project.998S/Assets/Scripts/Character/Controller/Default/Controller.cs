using System;
using System.Collections.Generic;
using UniRx;
using Random = UnityEngine.Random;
using UnityEngine;

public abstract class Controller : MonoBehaviour
{
    protected GameObject target;
    protected int[] targetFindSequence;
    protected bool[] isCharacterDead;

    protected IDisposable updateActionObserver;

    public void Start()
        => Init();

    public virtual void Init()
    {
        target = Managers.Game.target;
        targetFindSequence = new int[StageManager.MAX_CHARACTER_COUNT]
        { 
            SpawnManager.CHARACTER_CENTER, 
            SpawnManager.CHARACTER_LEFT, 
            SpawnManager.CHARACTER_RIGHT 
        };
        isCharacterDead = new bool[StageManager.MAX_CHARACTER_COUNT];

        UpdateActionAsObservable();
    }

    protected abstract void UpdateActionAsObservable();

    protected abstract Character GetSelectCharacter();

    protected virtual void UpdateTurnAsObservable(ReactiveProperty<bool> isCharacterTurn)
    {
        isCharacterTurn
            .Where(_ => Managers.Game.selectCharacter.Value == null)
            .Subscribe(_ =>
            {
                SelectCharacterAtFirstTurn();
            }).AddTo(this);
    }

    protected virtual void SelectCharacterAtFirstTurn()
    {
        
    }

    /// <summary>
    /// ĳ������ Ÿ���� Ȯ���� �� ����� Ȯ���ϴ� �޼ҵ��Դϴ�.
    /// </summary>
    /// <param name="character">ĳ����</param>
    /// <param name="targetType">Ÿ��</param>
    protected virtual void CheckCharacterType(Character character, Type targetType)
    {
        if (character.GetType() != targetType)
        {
            return;
        }

        Managers.Game.selectCharacter.Value = character;
        ReturnCheckCharacterType(character);
    }

    /// <summary>
    /// ĳ���� Ÿ���� ������ Ȯ���Ͽ� ����� ����ϴ� �޼ҵ��Դϴ�.
    /// </summary>
    /// <param name="character">ĳ����</param>
    protected virtual void ReturnCheckCharacterType(Character character)
    {

    }

    /// <summary>
    /// ĳ���� Ÿ���� �迭���� �������� ĳ���͸� ��ȯ�ϴ� �޼ҵ��Դϴ�.
    /// </summary>
    /// <param name="characters">ĳ���� Ÿ���� �迭</param>
    public Character GetRandomCharacterInList(List<Character> characters)
    {
        int random = Random.Range(0, characters.Count);
        bool isDead = true;

        for (int index = 0; index < isCharacterDead.Length; ++index)
        {
            isDead &= isCharacterDead[index];
        }

        if (isDead)
        {
            Array.Fill(isCharacterDead, false);

            return null;
        }

        if (false == characters[random].IsCharacterDead())
        {
            return characters[random].GetCharacterInGameObject<Character>();
        }

        isCharacterDead[random] = true;

        return GetRandomCharacterInList(characters);
    }
}
