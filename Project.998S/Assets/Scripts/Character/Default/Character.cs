using System;
using UniRx;
using UnityEngine;
using static Utils.Utilities;

public abstract class Character : MonoBehaviour
{
    public ReactiveProperty<int> maxHealth { get; private set; }
    public ReactiveProperty<int> maxAttack { get; private set; }
    public ReactiveProperty<int> maxDefense { get; private set; }
    public ReactiveProperty<int> maxLuck { get; private set; }
    public ReactiveProperty<int> maxFocus { get; private set; }

    public ReactiveProperty<int> maxAccuracy { get; private set; }

    public ReactiveProperty<int> maxLevel { get; private set; }
    public ReactiveProperty<int> level { get; set; }
    public ReactiveProperty<int> exp { get; set; }
    public ReactiveProperty<int> requireExp { get; private set; }

    public ReactiveProperty<int> currentHealth { get; set; }
    public ReactiveProperty<int> currentAttack { get; set; }
    public ReactiveProperty<int> currentDefense { get; set; }
    public ReactiveProperty<int> currentLuck { get; set; }
    public ReactiveProperty<int> currentFocus { get; set; }

    public ReactiveProperty<int> currentAccuracy { get; set; }

    public Skill biasSkill { get; set; }

    public ReactiveProperty<SkillData> currentSkill { get; set; }
    public ReactiveProperty<int[]> currentEffect { get; set; }


    public ReactiveProperty<CharacterID> characterId { get; private set; }
    public ReactiveProperty<string> characterName { get; private set; }
    public ReactiveProperty<CharacterState> characterState { get; set; }

    public ReactiveProperty<EquipmentID> equipmentId { get; set; }
    public ReactiveProperty<int[]> skillIdEnum { get; set; }
    public ReactiveProperty<int[]> effectIdEnum { get; set; }

    public ReactiveProperty<bool> isAttack { get; set; }

    protected IDisposable lookAtTargetAsObservable;
    protected Transform lookTransform;
    protected Animator animator;

    protected virtual void Awake()
        => animator = gameObject.GetComponentAssert<Animator>();

    public virtual void Init(CharacterID id)
    {
        maxHealth = new ReactiveProperty<int>();
        maxAttack = new ReactiveProperty<int>();
        maxDefense = new ReactiveProperty<int>();
        maxLuck = new ReactiveProperty<int>();
        maxFocus = new ReactiveProperty<int>();
        
        maxAccuracy = new ReactiveProperty<int>();

        maxLevel = new ReactiveProperty<int>();
        level = new ReactiveProperty<int>();
        exp = new ReactiveProperty<int>();
        requireExp = new ReactiveProperty<int>();

        currentHealth = new ReactiveProperty<int>();
        currentAttack = new ReactiveProperty<int>();
        currentDefense = new ReactiveProperty<int>();
        currentLuck = new ReactiveProperty<int>();
        currentFocus = new ReactiveProperty<int>();

        currentAccuracy = new ReactiveProperty<int>();

        biasSkill = new Skill();
        currentSkill = new ReactiveProperty<SkillData>();
        currentEffect = new ReactiveProperty<int[]>();

        characterId = new ReactiveProperty<CharacterID>();
        characterName = new ReactiveProperty<string>();
        characterState = new ReactiveProperty<CharacterState>();
        equipmentId = new ReactiveProperty<EquipmentID>();
        skillIdEnum = new ReactiveProperty<int[]>();
        effectIdEnum = new ReactiveProperty<int[]>();

        isAttack = new ReactiveProperty<bool>();

        InitInfo(id);
        InitStat(id);

        LookAtTargetAsObservable();
        StateChangeAsObservable();
        SkillChangeAsObservable();
    }

    private void InitInfo(CharacterID id)
    {
        CharacterData data = Managers.Data.Character[id];

        characterId.Value = data.Id;
        characterName.Value = data.Name;
        characterState.Value = CharacterState.Idle;
    }

    private void InitStat(CharacterID id)
    {
        CharacterData data = Managers.Data.Character[id];

        level.Value = (int)data.IdLevel;
        exp.Value = 0;

        UpdateMaxStat(id);
        UpdateCurrentStat();
    }

    public void UpdateMaxStat(CharacterID id)
    {
        CharacterData data = Managers.Data.Character[id];
        LevelData levelData = Managers.Data.Level[data.IdLevel];
        EquipmentData equipmentData = Managers.Data.Equipment[data.IdEquipment];

        maxLevel.Value = Managers.Data.Level.Count;
        requireExp.Value = levelData.Exp;

        equipmentId.Value = equipmentData.Id;
        skillIdEnum.Value = ReferenceDataByIdEnum(equipmentData.IdSkillEnum);

        maxHealth.Value = Define.Calculate.Health(data.Health, equipmentData.Health, levelData.HealthPerLevel);
        maxAttack.Value = Define.Calculate.Attack(data.Attack, equipmentData.Attack, levelData.AttackPerLevel);
        maxDefense.Value = Define.Calculate.Defense(data.Defense, equipmentData.Defense, levelData.DefensePerLevel);
        maxLuck.Value = Define.Calculate.LuckOrAccuracy(data.Luck, equipmentData.Luck, levelData.LuckPerLevel);
        maxFocus.Value = data.Focus;

        maxAccuracy.Value = Managers.Data.Equipment[equipmentId.Value].Accuracy;
    }

    public void UpdateCurrentStat()
    {
        currentHealth.Value = maxHealth.Value;
        currentAttack.Value = maxAttack.Value;
        currentDefense.Value = maxDefense.Value;
        currentLuck.Value = maxLuck.Value;
        currentFocus.Value = maxFocus.Value;

        currentAccuracy.Value = maxAccuracy.Value;
    }

    protected void LookAtTargetAsObservable()
    {
        lookAtTargetAsObservable = Observable.EveryUpdate()
            .Where(_ => lookTransform != null)
            .Subscribe(_ => 
            {
                Quaternion targetRotation = Quaternion.LookRotation(lookTransform.position - transform.position);
                Quaternion newRotation = Quaternion.Slerp(transform.rotation, targetRotation, 30f * Time.deltaTime);
                transform.rotation = newRotation;
            }).AddTo(this);
    }

    protected void StateChangeAsObservable()
    {
        characterState
            .Subscribe(state =>
            {
                switch (state)
                {
                    case CharacterState.Dead: animator.SetTrigger(AnimatorParameter.Dead); break;
                    case CharacterState.Dodge: animator.SetTrigger(AnimatorParameter.Dodge); break;
                    case CharacterState.Damage: animator.SetTrigger(AnimatorParameter.Damage); break;
                    case CharacterState.NormalAttack: animator.SetTrigger(AnimatorParameter.NormalAttack); break;
                    case CharacterState.ShortSkill: animator.SetTrigger(AnimatorParameter.ShortSkill); break;
                    case CharacterState.LongSkill: animator.SetTrigger(AnimatorParameter.LongSkill); break;
                };
            }).AddTo(this);
    }

    private void SkillChangeAsObservable()
    {
        currentSkill.Subscribe(data => 
        {
            if (data == null)
            {
                return;
            }

            GetSkillAndEffect(data);
        }).AddTo(this);
    }

    /// <summary>
    /// ĳ������ ���¸� ��ȯ�ϴ� �޼ҵ��Դϴ�.
    /// </summary>
    /// <param name="state">ĳ���� ����</param>
    public void ChangeCharacterState(CharacterState state)
    {
        // NOTE : üũ�� �ʿ䰡 ��������..
        //if (false == this.IsCharacterDead())
        {
            characterState.Value = state;
        }
    }

    /// <summary>
    /// ĳ������ ���� ��ǥ�� ȸ�� ���� ��ȯ�ϴ� �޼ҵ��Դϴ�.
    /// </summary>
    /// <param name="target">���� ��ǥ</param>
    public void LookAtTarget(Transform target)
        => lookTransform = target;

    /// <summary>
    /// ĳ���Ͱ� ���� ���¸� �Ǵ��ϴ� �޼ҵ��Դϴ�.
    /// </summary>
    /// <param name="state"></param>
    public virtual void Attack(int state)
    {
        isAttack.Value = true;

        // HACK : �������� Controller.DelayForEndTurnCo���� Idle�� �ٲ�����
        // ���� ���� Idle�� ��ȯ���� �ʴ� ��Ȳ�� �߻���.
        // ���� ������ ���� �� Idle�� ���� State�� �ٲ㼭 2��° ���ݺ��ʹ� �ȵǴ� ���� �ذ�
        characterState.Value = CharacterState.Idle;
    }

    /// <summary>
    /// ĳ���Ͱ� �������� �ް� �ϴ� �޼ҵ��Դϴ�.
    /// </summary>
    /// <param name="damage">�� ������</param>
    public virtual void GetDamage(int damage)
    {
        currentHealth.Value -= damage;

        if (currentHealth.Value > 0)
        {
            ChangeCharacterState(CharacterState.Damage);
        }
        else
        {
            currentHealth.Value = 0;
            ChangeCharacterState(CharacterState.Dead);
        }
    }

    /// <summary>
    /// ���� ������ ��ų�� ����Ʈ�� ��ȯ�ϴ� �޼ҵ��Դϴ�.
    /// </summary>
    /// <param name="data">��ų ������</param>
    public Skill GetSkillAndEffect(SkillData data)
    {
        biasSkill.Damage = data.Damage;
        biasSkill.Accuracy = data.Accuracy;
        maxAccuracy.Value = Managers.Data.Equipment[equipmentId.Value].Accuracy + biasSkill.Accuracy;
        currentAccuracy.Value = maxAccuracy.Value;
        effectIdEnum.Value = ReferenceDataByIdEnum(data.IdEffectEnum);

        for (int index = 0; index < effectIdEnum.Value.Length; ++ index)
        {
            EffectData effectData = Managers.Data.Effect[(EffectID)effectIdEnum.Value[index]];

            if (true == Define.Calculate.IsChance(effectData.Chance))
            {
                GetEffectInBiasSkill(effectData);

                continue;
            }

            if (true == ArrayIndexExists(effectIdEnum.Value, index + 1))
            {
                EffectData nextEffectData = Managers.Data.Effect[(EffectID)effectIdEnum.Value[index + 1]];

                GetEffectInBiasSkill(nextEffectData);
            }
        }

        return biasSkill;
    }

    /// <summary>
    /// ���� ������ ��ų�� ȿ�� Ȯ����ŭ�� ����Ʈ�� ��ȯ�ϴ� �޼ҵ��Դϴ�.
    /// </summary>
    /// <param name="data">����Ʈ ������</param>
    private void GetEffectInBiasSkill(EffectData data)
    {
        biasSkill.Effect = data.Effect;
        biasSkill.Prefab = Managers.Resource.LoadPrefab(data.Prefab);
        biasSkill.Target = data.Target;
        biasSkill.Animation = data.Animation;
    }
}
