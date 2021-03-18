using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCreator : MonoBehaviour {
    // 用来装饰初始化地图所需物体的数组
    // 0 老家
    // 1 墙
    // 2 障碍
    // 3 出生效果
    // 4 河流
    // 5 草
    // 6 空气墙
    public GameObject[] item;
    private List<Vector3> itemPositionList = new List<Vector3>();

    private void Awake() {
        InitMap();
    }

    private void InitMap() {
        // 实例化老家
        CreateItem(item[0], new Vector3(0, -8, 0), Quaternion.identity);
        // 用墙把老家围起来
        CreateItem(item[1], new Vector3(-1, -8, 0), Quaternion.identity);
        CreateItem(item[1], new Vector3(1, -8, 0), Quaternion.identity);
        CreateItem(item[1], new Vector3(-1, -7, 0), Quaternion.identity);
        CreateItem(item[1], new Vector3(1, -7, 0), Quaternion.identity);
        CreateItem(item[1], new Vector3(0, -7, 0), Quaternion.identity);

        // 实例化外围墙
        for (int i = -11; i < 12; i++) {
            CreateItem(item[6], new Vector3(i, 9, 0), Quaternion.identity);
        }
        for (int i = -11; i < 12; i++) {
            CreateItem(item[6], new Vector3(i, -9, 0), Quaternion.identity);
        }
        for (int i = -8; i < 9; i++) {
            CreateItem(item[6], new Vector3(-11, i, 0), Quaternion.identity);
        }
        for (int i = -8; i < 9; i++) {
            CreateItem(item[6], new Vector3(11, i, 0), Quaternion.identity);
        }

        // 实例化玩家（Born Effect）
        GameObject go = Instantiate(item[3], new Vector3(-2, -8, 0), Quaternion.identity);
        go.GetComponent<Born>().createPlayer = true;

        // 产生敌人
        CreateItem(item[3], new Vector3(-10, 8, 0), Quaternion.identity);
        CreateItem(item[3], new Vector3(0, 8, 0), Quaternion.identity);
        CreateItem(item[3], new Vector3(10, 8, 0), Quaternion.identity);

        InvokeRepeating("CreateEnemy", 4, 5);

        // 实例化地图
        for (int i = 0; i < 60; i++) {
            CreateItem(item[5], createRandomPosition(), Quaternion.identity);
        }
        for (int i = 0; i < 20; i++) {
            CreateItem(item[2], createRandomPosition(), Quaternion.identity);
        }
        for (int i = 0; i < 20; i++) {
            CreateItem(item[4], createRandomPosition(), Quaternion.identity);
        }
        for (int i = 0; i < 20; i++) {
            CreateItem(item[1], createRandomPosition(), Quaternion.identity);
        }
    }

    private void CreateEnemy() {
        int num = Random.Range(0, 3);
        Vector3 EnemyPos = new Vector3();
        switch (num) {
            case 0:
                EnemyPos = new Vector3(-10, 8, 0);
                break;
            case 1:
                EnemyPos = new Vector3(0, 8, 0);
                break;
            case 2:
                EnemyPos = new Vector3(10, 8, 0);
                break;

            default:
                break;
        }
        CreateItem(item[3], EnemyPos, Quaternion.identity);
    }

    private void CreateItem(GameObject createGameObject, Vector3 createPosition, Quaternion createRotation) {
        GameObject itemGo = Instantiate(createGameObject, createPosition, createRotation);
        itemGo.transform.SetParent(gameObject.transform);
        itemPositionList.Add(createPosition);
    }

    private Vector3 createRandomPosition() {
        // 边界不生成物体
        while(true) {
            Vector3 createPosition = new Vector3(Random.Range(-9, 10), Random.Range(-7, 8), 0);
            if (!HasThePosition(createPosition))
                return createPosition;
        }

    }

    private bool HasThePosition(Vector3 createPos) {
        for (int i = 0; i < itemPositionList.Count; i++) {
            if (createPos == itemPositionList[i])
                return true;
        }
        return false;
    }
}
