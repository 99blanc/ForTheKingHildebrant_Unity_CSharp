using System.Collections.Generic;
using Object = UnityEngine.Object;
using UnityEngine.UI;
using UnityEngine;

public class ResourceManager
{
    public Dictionary<string, Animator> Animators { get; private set; }
    public Dictionary<string, Texture> Textures { get; private set; }
    public Dictionary<string, Image> Images { get; private set; }
    public Dictionary<string, Material> Materials { get; private set; }
    public Dictionary<string, GameObject> Prefabs { get; private set; }
    public Dictionary<string, Sprite> Sprites { get; private set; }

    public void Init()
    {
        Animators = new Dictionary<string, Animator>();
        Textures = new Dictionary<string, Texture>();
        Images = new Dictionary<string, Image>();
        Materials = new Dictionary<string, Material>();
        Prefabs = new Dictionary<string, GameObject>();
        Sprites = new Dictionary<string, Sprite>();
    }

    private T Load<T>(Dictionary<string, T> dictionary, string path) where T : Object
    {
        if (false == dictionary.ContainsKey(path))
        {
            T resource = Resources.Load<T>(path);
            dictionary.Add(path, resource);

            return dictionary[path];
        }

        return dictionary[path];
    }

    /// <summary>
    /// ������ ���� ��θ� ���� �ִϸ������� ��ȯ�ϴ� �޼ҵ��Դϴ�.
    /// </summary>
    /// <param name="path">������ ���� ���</param>
    public Animator LoadAnimator(string path) 
        => Load(Animators, string.Concat(Define.Path.ANIMATOR, path));

    /// <summary>
    /// ������ ���� ��θ� ���� �ؽ��ĸ� ��ȯ�ϴ� �޼ҵ��Դϴ�.
    /// </summary>
    /// <param name="path">������ ���� ���</param>
    public Image LoadTexture(string path) 
        => Load(Images, string.Concat(Define.Path.TEXTURE, path));

    /// <summary>
    /// ������ ���� ��θ� ���� �̹����� ��ȯ�ϴ� �޼ҵ��Դϴ�.
    /// </summary>
    /// <param name="path">������ ���� ���</param>
    public Image LoadImage(string path) 
        => Load(Images, string.Concat(Define.Path.IMAGE, path));

    /// <summary>
    /// ������ ���� ��θ� ���� ��Ƽ������ ��ȯ�ϴ� �޼ҵ��Դϴ�.
    /// </summary>
    /// <param name="path">������ ���� ���</param>
    public Material LoadMaterial(string path) 
        => Load(Materials, string.Concat(Define.Path.MATERIAL, path));
    
    /// <summary>
    /// ������ ���� ��θ� ���� �������� ��ȯ�ϴ� �޼ҵ��Դϴ�.
    /// </summary>
    /// <param name="path">������ ���� ���</param>
    public GameObject LoadPrefab(string path) 
        => Load(Prefabs, string.Concat(Define.Path.PREFAB, path));

    /// <summary>
    /// ������ ���� ��θ� ���� ��������Ʈ�� ��ȯ�ϴ� �޼ҵ��Դϴ�.
    /// </summary>
    /// <param name="path">������ ���� ���</param>
    public Sprite LoadSprite(string path) 
        => Load(Sprites, string.Concat(Define.Path.SPRITE, path));

    /// <summary>
    /// ������ ���� ��θ� �������� �������� �����Ͽ� ��ȯ�ϴ� �޼ҵ��Դϴ�.
    /// </summary>
    /// <param name="path"></param>
    /// <param name="parent"></param>
    public GameObject Instantiate(string path, Transform parent = null)
    {
        GameObject prefab = LoadPrefab(path);

        Debug.Assert(prefab != null);

        return Instantiate(prefab, parent);
    }

    /// <summary>
    /// �θ� Ʈ�������� �������� �������� �����Ͽ� ��ȯ�ϴ� �޼ҵ��Դϴ�.
    /// </summary>
    /// <param name="prefab">������</param>
    /// <param name="parent">�θ� Ʈ������</param>
    public GameObject Instantiate(GameObject prefab, Transform parent = null)
    {
        GameObject gameObject = Object.Instantiate(prefab, parent);

        gameObject.name = prefab.name;

        return gameObject;
    }

    /// <summary>
    /// ������ ���� ������Ʈ�� �ı��ϴ� �޼ҵ��Դϴ�.
    /// </summary>
    /// <param name="gameObject">���� ������Ʈ</param>
    public void Destroy(GameObject gameObject)
    {
        if (gameObject != null)
        {
            Object.Destroy(gameObject);
        }
    }
}
