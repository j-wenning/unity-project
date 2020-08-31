using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;

 public class AutoChunker : Editor
{
    static PolygonCollider2D mCollider;
    static List<GameObject> mAdjChunks = new List<GameObject>();
    static List<Collider2D> mCollisions = new List<Collider2D>();
    static ContactFilter2D mFilter = new ContactFilter2D();
    static GameObject mOwnPrefab;
    static GameObject mAdjChunkPrefab;
    static GameObject mCurrentChunk;
    static string mPrefabName;

    [MenuItem("Auto Chunker/Join Chunks")]
    static void AutoChunk()
    {
        if (SceneManager.GetActiveScene().name != "Map Editor") return;
        DeleteChunkPrefabs();
        JoinChunks();
        CreateChunkPrefabs();
    }
    static void DeleteChunkPrefabs()
    {
        FileUtil.DeleteFileOrDirectory("Assets/Prefabs/Chunks/");
        FileUtil.DeleteFileOrDirectory("Assets/Prefabs/Chunks.meta");
        AssetDatabase.CreateFolder("Assets/Prefabs", "Chunks");
    }

    static void CreateChunkPrefabs()
    {
        foreach(Transform child in GameObject.Find("/Grid/Tilemap").GetComponentsInChildren<Transform>())
        {
            if (child.gameObject.tag == "Chunk")
            {
                if (PrefabUtility.IsPartOfPrefabAsset(child.gameObject)
                || PrefabUtility.IsPartOfNonAssetPrefabInstance(child.gameObject))
                {
                    PrefabUtility.UnpackPrefabInstance(
                        child.gameObject,
                        PrefabUnpackMode.Completely,
                        InteractionMode.AutomatedAction
                    );
                }
                mPrefabName = AssetDatabase.GenerateUniqueAssetPath($"Assets/Prefabs/Chunks/{child.gameObject.name}.prefab");
                PrefabUtility.SaveAsPrefabAssetAndConnect(
                    child.gameObject,
                    mPrefabName,
                    InteractionMode.AutomatedAction
                );
            }
        }
    }

    static void JoinChunks()
    {
        foreach (Transform child in GameObject.Find("/Grid/Tilemap").GetComponentsInChildren<Transform>())
        {
            if (child.tag == "Chunk")
            {
                GetAdjChunks(child.gameObject);
            }
        }
    }

    static void GetAdjChunks(GameObject chunk)
    {
        mCurrentChunk = chunk;
        mOwnPrefab = PrefabUtility.GetCorrespondingObjectFromSource(mCurrentChunk);
        mFilter.useTriggers = true;
        mAdjChunks.Clear();
        mCollider = mCurrentChunk.GetComponent<PolygonCollider2D>();
        mCollider.OverlapCollider(mFilter, mCollisions);
        mCollisions.ForEach(AddChunk);
        mOwnPrefab.GetComponent<Chunk>().m_AdjChunks = mAdjChunks.ToArray();
        PrefabUtility.SavePrefabAsset(mOwnPrefab);
        mCollisions.Clear();
    }

    static void AddChunk(Collider2D collider)
    {
        if (collider.gameObject.tag == "Chunk")
        {
            mAdjChunkPrefab = PrefabUtility.GetCorrespondingObjectFromSource(collider.gameObject);
            mAdjChunks.Add(mAdjChunkPrefab);
            Debug.Log("Joined chunk \"" + collider.gameObject.name + "\" to chunk \"" + mCurrentChunk.name + "\".");
        }
    }

    static Transform GetTransform(GameObject obj)
    {
        return obj.transform;
    }
}