using System.Collections.Generic;
using UnityEngine;
using static Utils;

public class SpawnManager
{
    public readonly Vector3[] footboards = new Vector3[9]
    {
        new Vector3(-3, 0, -4), new Vector3(0, 0, -4), new Vector3(3, 0, -4),
        new Vector3(-3, 0, 4), new Vector3(0, 0, 4), new Vector3(3, 0, 4),

        new Vector3(-3, 0, 39f), new Vector3(0, 0, 39f), new Vector3(3, 0, 39f)
    };

    public const int PLAYER_LEFT = 0, PLAYER_CENTER = 1, PLAYER_RIGHT = 2,
                     ENEMY_LEFT = 3, ENEMY_CENTER = 4, ENEMY_RIGHT = 5,
                     PREVIEW_LEFT = 6, PREVIEW_CENTER = 7, PREVIEW_RIGHT = 8;
        
    public const int CHARACTER_LEFT = 0, CHARACTER_CENTER = 1, CHARACTER_RIGHT = 2;

    public const int PLAYER_TYPE = 0, ENEMY_TYPE = 1, PREVIEW_TYPE = 2,
                     MAX_CHARACTER_COUNT = 3;

    public const int PREVIEW = 1;

    private GameObject Entities, Mannequins, Dungeon;

    public void Init()
    {
        Entities = new GameObject(nameof(Entities));
        Mannequins = new GameObject(nameof(Mannequins));
    }

    /// <summary>
    /// Ÿ�� Ŀ���� �����ϴ� �޼ҵ��Դϴ�.
    /// </summary>
    /// <param name="id">Ÿ�� Ŀ�� ���̵�</param>
    public GameObject TargetByID(PrefabID id)
        => Managers.Resource.Instantiate(Managers.Data.Prefab[(int)id].Prefab);

    /// <summary>
    /// ���������� �����ϴ� �޼ҵ��Դϴ�.
    /// </summary>
    /// <param name="id">�������� ���̵�</param>
    public void StageByID(StageID id)
    {
        string dungeonPrefabPath = Managers.Data.Prefab[(int)PrefabID.Dungeon].Prefab;
        string spawnPrefabPath = Managers.Data.Prefab[(int)PrefabID.Spawn].Prefab;

        Dungeon = Managers.Resource.Instantiate(dungeonPrefabPath);
        Managers.Resource.Instantiate(spawnPrefabPath, Dungeon.transform);
        StageManager stage = Managers.Stage;

        stage.previews = ReplaceCharacter(PREVIEW_TYPE, stage.previews, ReturnArray<CharacterID>
        (
            (CharacterID)Managers.Data.Stage[id + PREVIEW].Left,
            (CharacterID)Managers.Data.Stage[id + PREVIEW].Center,
            (CharacterID)Managers.Data.Stage[id + PREVIEW].Right
        ));
        stage.enemies = ReplaceCharacter(ENEMY_TYPE, stage.enemies, ReturnArray<CharacterID>
        (
            (CharacterID)Managers.Data.Stage[id].Left,
            (CharacterID)Managers.Data.Stage[id].Center,
            (CharacterID)Managers.Data.Stage[id].Right
        ));

        if (stage.turnCount.Value > PREVIEW)
        {
            return;
        }

        stage.players = ReplaceCharacter(PLAYER_TYPE, stage.players, ReturnArray<CharacterID>
        (
            (CharacterID)1001,
            (CharacterID)1002,
            (CharacterID)1003
        ));

        EnqueueAllCharacter();
    }

    private void EnqueueAllCharacter()
    {
        Queue<Character> turnQueue = Managers.Stage.turnQueue;

        for (int index = 0; index < PREVIEW_LEFT; ++index)
        {
            if (index < ENEMY_LEFT)
            {
                turnQueue.Enqueue(Managers.Stage.players[index]);

                continue;
            }

            turnQueue.Enqueue(Managers.Stage.enemies[index - ENEMY_LEFT]);
        }
    }

    /// <summary>
    /// �������� ���� ĳ���͵��� ���ġ�Ͽ� ��ȯ�ϴ� �޼ҵ��Դϴ�.
    /// </summary>
    /// <param name="type">ĳ���� Ÿ��</param>
    /// <param name="characters">����Ʈ �迭</param>
    /// <param name="id">ĳ���� ���̵�</param>
    private List<Character> ReplaceCharacter(int type, List<Character> characters = null, params CharacterID[] id)
    {
        characters.Clear();
        int spawnPosition = PREVIEW_TYPE != type ? ENEMY_TYPE != type ? PLAYER_LEFT : ENEMY_LEFT : PREVIEW_LEFT;

        for (int index = 0; index < id.Length; ++index)
        {
            Character character = CharacterByID(id[index], spawnPosition + index);
            character.Init(id[index]);
            characters.Add(character);
        }

        return characters;
    }

    /// <summary>
    /// ������ ���̵� �о� ĳ���� Ÿ���� ���� ������Ʈ�� �����Ͽ� ��ȯ�ϴ� �޼ҵ��Դϴ�.
    /// </summary>
    /// <param name="id">������ ���̵�</param>
    /// <param name="position">���� ��ġ</param>
    /// <param name="parent">�θ� Ʈ������</param>
    public Character CharacterByID(CharacterID id, int position, Transform parent = null)
    {
        if (id != 0)
        {
            CharacterData data = Managers.Data.Character[id];
            parent = PREVIEW_LEFT <= position ? ENEMY_LEFT <= position ? PLAYER_LEFT <= position ?
                Mannequins.transform : Entities.transform : Entities.transform : Entities. transform;

            return CharacterByID(data, position, parent);
        }

        return null;
    }

    /// <summary>
    /// ������ Ÿ���� �о� ĳ���� Ÿ���� ���� ������Ʈ�� �����Ͽ� ��ȯ�ϴ� �޼ҵ��Դϴ�.
    /// </summary>
    /// <param name="data">������</param>
    /// <param name="position">���� ��ġ</param>
    /// <param name="parent">�θ� Ʈ������</param>
    public Character CharacterByID(CharacterData data, int position, Transform parent = null)
    {
        GameObject character = Managers.Resource.Instantiate(data.Prefab, parent);
        character.transform.position = footboards[position];

        return character.GetCharacterInGameObject<Character>();
    }
}
