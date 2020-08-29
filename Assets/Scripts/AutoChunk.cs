using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class AutoChunk : MonoBehaviour
{
    private PolygonCollider2D mCollider;
    private List<GameObject> mAdjChunks;
    private List<Collider2D> mCollisions = new List<Collider2D>();
    private ContactFilter2D mFilter = new ContactFilter2D();
    private GameObject mOwnPrefab;
    private GameObject mAdjChunkPrefab;
    private GameObject mCurrentChunk;
    private string mPrefabName;

    public void DeleteChunkPrefabs()
    {
        FileUtil.DeleteFileOrDirectory("Assets/Prefabs/Chunks/");
        FileUtil.DeleteFileOrDirectory("Assets/Prefabs/Chunks.meta");
        AssetDatabase.CreateFolder("Assets/Prefabs", "Chunks");
    }

    public void CreateChunkPrefabs()
    {
        foreach(Transform child in transform)
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

    public void JoinChunks()
    {
        foreach (Transform child in transform)
        {
            if (child.tag == "Chunk")
            {
                GetAdjChunks(child.gameObject);
            }
        }
    }

    private void GetAdjChunks(GameObject chunk)
    {
        mCurrentChunk = chunk;
        mOwnPrefab = PrefabUtility.GetCorrespondingObjectFromSource(mCurrentChunk);
        mFilter.useTriggers = true;
        mAdjChunks = mCurrentChunk.GetComponent<Chunk>().m_AdjChunks;
        mAdjChunks.Clear();
        mCollider = mCurrentChunk.GetComponent<PolygonCollider2D>();
        mCollider.OverlapCollider(mFilter, mCollisions);
        mCollisions.ForEach(AddChunk);
        mOwnPrefab.GetComponent<Chunk>().m_AdjChunks = mAdjChunks;
        mCollisions.Clear();
    }

    private void AddChunk(Collider2D collider)
    {
        if (collider.gameObject.tag == "Chunk")
        {
            mAdjChunkPrefab = PrefabUtility.GetCorrespondingObjectFromSource(collider.gameObject);
            mAdjChunks.Add(mAdjChunkPrefab);
            Debug.Log("Joined chunk \"" + collider.gameObject.name + "\" to chunk \"" + mCurrentChunk.name + "\".");
        }
    }
}
