using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeuronVR_RoomManager : MonoBehaviour {

    [Header("Enemy Room")]
    public NeuronVR_Door enemyDoor;
    public NeuronVR_Enemy[] enemies;

    [Header("Cannon Room")]
    public NeuronVR_Door cannonDoor;
    public NeuronVR_Cannon[] cannons;

    [Header("Great Door")]
    public NeuronVR_Door yellowDoorLeft;
    public NeuronVR_Door yellowDoorRight;
    public NeuronVR_Door blueDoorLeft;
    public NeuronVR_Door blueDoorRight;
    public NeuronVR_Door greenDoorLeft;
    public NeuronVR_Door greenDoorRight;

    public Material yellowMaterial;
    public Material blueMaterial;
    public Material greenMaterial;

    public NeuronVR_HoverButton greatDoorButton;

    [Header("Item Spawn")]
    public NeuronVR_Item m_itemBlue;
    public NeuronVR_Item m_itemGreen;
    public Transform m_itemSpawnBlue;
    public Transform m_itemSpawnGreen;

    // How many items remain to be placed on their socket to open door
    private int remainingItems = 3;
    private int remainingCannons;
    private int remainingEnemies;

    // Use this for initialization
    void Start () {
        remainingCannons = cannons.Length;
        remainingEnemies = enemies.Length;

        greatDoorButton.Lock();
    }
	
	// Update is called once per frame
	void Update () {
		// Do nothing
    }

    public void CannonDestroid()
    {
        remainingCannons--;
        if(remainingCannons == 0)
        {
            cannonDoor.OpenDoor();
            // Spawn Cube
            m_itemBlue.Spawn(m_itemSpawnBlue.position);
        }
    }

    public void EnemyKilled()
    {
        remainingEnemies--;
        if(remainingEnemies == 0)
        {
            enemyDoor.OpenDoor();
            // Spawn Ball;
            m_itemGreen.Spawn(m_itemSpawnGreen.position);
        }
    }

    public void StartCannonEvent()
    {
        cannonDoor.CloseDoor();

        foreach (var cannon in cannons)
        {
            cannon.StartShooting();
        }
    }

    public void StartEnemyEvent()
    {
        enemyDoor.CloseDoor();

        foreach (var enemy in enemies)
        {
            enemy.SetAgressive();
            enemy.gameObject.transform.localPosition = new Vector3(enemy.gameObject.transform.localPosition.x , - 4.675153f, enemy.gameObject.transform.localPosition.z);
        }
    }

    public void PutStar()
    {
        yellowDoorLeft.GetComponentInChildren<Renderer>().material = yellowMaterial;
        yellowDoorRight.GetComponentInChildren<Renderer>().material = yellowMaterial;

        remainingItems--;

        if(remainingItems == 0)
        {
            ActivateFinalButton();
        }
    }

    public void PutCube()
    {
        blueDoorLeft.GetComponentInChildren<Renderer>().material = blueMaterial;
        blueDoorRight.GetComponentInChildren<Renderer>().material = blueMaterial;

        remainingItems--;

        if (remainingItems == 0)
        {
            ActivateFinalButton();
        }
    }

    public void PutBall()
    {
        greenDoorLeft.GetComponentInChildren<Renderer>().material = greenMaterial;
        greenDoorRight.GetComponentInChildren<Renderer>().material = greenMaterial;

        remainingItems--;

        if (remainingItems == 0)
        {
            ActivateFinalButton();
        }
    }

    private void ActivateFinalButton()
    {
        greatDoorButton.Unlock();
    }

    public void OpenGreatDoor()
    {
        yellowDoorLeft.OpenDoor();
        yellowDoorRight.OpenDoor();

        blueDoorLeft.OpenDoor();
        blueDoorRight.OpenDoor();

        greenDoorLeft.OpenDoor();
        greenDoorRight.OpenDoor();
    }
}
