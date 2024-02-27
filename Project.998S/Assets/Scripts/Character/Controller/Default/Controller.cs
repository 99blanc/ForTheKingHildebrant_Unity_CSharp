using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System;
using UniRx;
using Random = UnityEngine.Random;
using UnityEngine;
using UnityEditor;

public abstract class Controller : MonoBehaviour
{
    [HideInInspector] public List<Dictionary<bool, int>> slotAccuracyDamage { get; set; }
    [HideInInspector] public ReactiveProperty<bool> isAllEnemyCharacterDead { get; set; }
    [HideInInspector] protected ReactiveProperty<bool> isSelectCharacter { get; set; }
    [HideInInspector] public int AttackDamage { get; set; } // NOTE : ����....������� ������

    public bool isAttack;
    protected bool[] isEnemyCharacterDead;
    protected IDisposable updateActionObserver, doAttackAsObservable;
    protected GameObject target;
    protected Coroutine damageDelay;

    public void Update()
    {
    }

    private void Start()
        => Init();

    public virtual void Init()
    {
        slotAccuracyDamage = new List<Dictionary<bool, int>>();
        isAllEnemyCharacterDead = new ReactiveProperty<bool>();
        isSelectCharacter = new ReactiveProperty<bool>();
        isEnemyCharacterDead = new bool[SpawnManager.MAX_CHARACTER_COUNT];

        isSelectCharacter.Value = false;
        target = Managers.Stage.target;

        

        ActionAsObservable();
    }

    protected abstract void ActionAsObservable();

    protected virtual void CharacterTurnAsObservable(ReactiveProperty<bool> isCharacterTurn)
    {
        Managers.Stage.turnCount.Where(_ => isCharacterTurn.Value == true)
            .Where(_ => Managers.Stage.selectCharacter.Value == null)
            .Subscribe(_ =>
            {
                isSelectCharacter.Value = false;

                StartTurn(isCharacterTurn);
            });
    }

    protected virtual void StartTurn(ReactiveProperty<bool> isCharacterTurn)
    {
        
    }

    /// <summary>
    /// ĳ���� �迭���� ������ ĳ���͸� ��ȯ�ϴ� �޼ҵ��Դϴ�.
    /// </summary>
    /// <param name="characters">ĳ���� �迭</param>
    protected Character GetCharacterByRandomInList(List<Character> characters)
    {
        if (characters.TrueForAll(c => c.IsCharacterDead()))
        {
            isAllEnemyCharacterDead.Value = true;

            return null;
        }

        int random = Random.Range(0, characters.Count);

        if (true == characters[random].IsCharacterDead())
        {
            return GetCharacterByRandomInList(characters);
        }

        return characters[random];
    }

    /// <summary>
    /// ĳ���� �迭���� ������� �������� ĳ���͸� ��ȯ�ϴ� �޼ҵ��Դϴ�.
    /// </summary>
    /// <param name="characters">ĳ���� �迭</param>
    /// <param name="isMaxHealth">�ִ� �����</param>
    protected Character GetCharacterByHealthInList(List<Character> characters, bool isMaxHealth = false)
    {
        if (characters.TrueForAll(c => c.IsCharacterDead()))
        {
            isAllEnemyCharacterDead.Value = true;

            return null;
        }

        int[] health = characters.Select(character => character.currentHealth.Value).ToArray();

        if (false == isMaxHealth)
        {
            if (health.ToList().IndexOf(health.Min()) <= 0)
            {
                isEnemyCharacterDead[health.ToList().IndexOf(health.Min())] = true;

                return GetCharacterByHealthInList(characters);
            }

            return characters[health.ToList().IndexOf(health.Min())];
        }

        return characters[health.ToList().IndexOf(health.Max())];
    }

    /// <summary>
    /// ���� �׼��� ����ϴ� �޼ҵ��Դϴ�.
    /// </summary>
    public void AttackAction()
    {
        Character targetCharacter = Managers.Stage.selectCharacter.Value;
        Character character = Managers.Stage.turnCharacter.Value;

        EquipmentData equipmentData = Managers.Data.Equipment[character.equipmentId.Value];
        Skill skill = character.GetSkillAndEffect(character.currentSkill.Value);
        character.ChangeCharacterState(skill.Animation);

        doAttackAsObservable = character.isAttack.Where(_ => character.isAttack.Value == true)
        .Subscribe(_ => 
        {
            character.isAttack.Value = false;
            //int totalDamage = slotAccuracyDamage.Select(dictionary => dictionary.Values.Min()).Min();
            targetCharacter.GetDamage(AttackDamage);

            StartCoroutine(DelayForEndTurnCo(0.5f, targetCharacter));
        });
    }

    /// <summary>
    /// ���� ������ ��� �ð��� �����ϴ� �޼ҵ��Դϴ�.
    /// </summary>
    /// <param name="delay">���� �� ��� �ð�</param>
    protected IEnumerator DelayForEndTurnCo(float delay, Character character)
    {
        Managers.UI.ClosePopupUI();
        yield return new WaitForSeconds(delay);

        //character.ChangeCharacterState(CharacterState.Idle);
        doAttackAsObservable.Dispose();
        Managers.Stage.NextCharacterTurn();
        isAttack = false;

        if (Managers.Game.Player.isAllEnemyCharacterDead.Value)
        {
            Managers.Game.GameClear();
        }
        if (Managers.Game.Enemy.isAllEnemyCharacterDead.Value)
        {
            Managers.Game.GameFail();
        }
    }
}
