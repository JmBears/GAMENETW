using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PoolingBridge : MonoBehaviour, IPunPrefabPool
{
    private void Start()
    {
        PhotonNetwork.PrefabPool = this;
    }

    public void Destroy(GameObject gameObject)
    {
        ObjectPoolManager.Instance.ReturnObjectToPool(gameObject);
    }

    public GameObject Instantiate(string prefabId, Vector3 position, Quaternion rotation)
    {
        GameObject pooledObject = ObjectPoolManager.Instance.GetPooledObject(prefabId);
        if (pooledObject != null)
        {
            pooledObject.transform.SetPositionAndRotation(position, rotation);
        }
        return pooledObject;
    }
}
