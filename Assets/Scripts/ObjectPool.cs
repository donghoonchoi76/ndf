using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPool : MonoBehaviour {

    // singleton
    private static ObjectPool _instance;
    public static ObjectPool instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<ObjectPool>();
                if (_instance == null)
                    _instance = new GameObject("ObjectPool").AddComponent<ObjectPool>();
            }
            return _instance;
        }
    }

    // Object dictionary
    private Dictionary<int, List<GameObject>> pooledGameObjects = new Dictionary<int, List<GameObject>>();
    public GameObject GetGameObject(GameObject prefab, Vector2 pos, Quaternion rotation)
    {
        int key = prefab.GetInstanceID();

        // if the prefab is not in the dictionary, add it
        if (pooledGameObjects.ContainsKey(key) == false)
            pooledGameObjects.Add(key, new List<GameObject>());

        List<GameObject> gameObjects = pooledGameObjects[key];
        GameObject obj = null;
        for (int i = 0; i < gameObjects.Count; i++)
        {
            obj = gameObjects[i];
            if (obj.activeInHierarchy == false)
            {
                obj.transform.position = pos;
                obj.transform.rotation = rotation;
                obj.SetActive(true);
                return obj;
            }
        }
        // Add object when it doesn't have space
        obj = (GameObject)Instantiate(prefab, pos, rotation);
        obj.transform.parent = transform;
        gameObjects.Add(obj);
        return obj;
    }
    public void ReleaseGameObject(GameObject obj)
    {
        obj.SetActive(false);
    }
}