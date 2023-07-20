using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using UnityEngine.UIElements;

public class SpaghettiBossAttacks : NetworkBehaviour
{
    private PlayerTargeter playerTargeter;
    private EnemyMovementController enemyMovementController;
    private EnemyAI enemyAI;
    private Rigidbody2D rb;

    private int arenaMinY = 0;
    private int arenaMaxY = 26;
    private int arenaMinX = -1;
    private int arenaMaxX = 47;

    private int[] xRange = new int[] { -1, 47 };
    private int[] yRange = new int[] { 0, 26 };

    [SerializeField] private GameObject tomatoBombPrefab;
    [SerializeField] private GameObject tomatoProjectilePrefab;
    [SerializeField] private GameObject meatballPrefab;
    [SerializeField] private AudioClip meatballSoundEffect;
    [SerializeField] private AudioClip tomatoProjectileSoundEffect;
    [SerializeField] private int maxTomatoBombs;
    [SerializeField] private int maxMeatballComets;

    public enum Direction
    {
        Horizontal = 1, Vertical = 2
    }

    void Start()
    {
        if (!IsServer) return;
        enemyAI = gameObject.GetComponent<EnemyAI>();
        playerTargeter = gameObject.GetComponent<PlayerTargeter>();
        rb = gameObject.GetComponent<Rigidbody2D>();

        InvokeRepeating(nameof(ShootTomato), 5, 5);
        InvokeRepeating(nameof(SpawnMeatballComets), 5, 5);
        InvokeRepeating(nameof(SummonTomatoBombs), 5, 5);
        Debug.Log("Invoked everything");
    }

    private void ShootTomato()
    {
        if (!IsServer) return;
        var projectile = Instantiate(tomatoProjectilePrefab, transform.position, Quaternion.identity);

        if (projectile.GetComponent<EnemyStraightProjectile>() != null)
        {
            projectile.GetComponent<EnemyStraightProjectile>().targetPosition = playerTargeter.GetCurrentTargetPlayer().transform.position;
        }
        projectile.GetComponent<CharacterDamager>().damage = enemyAI.currAttack;
        projectile.GetComponent<Lifetime>().lifetime = 8;
        AudioManager.Instance.PlaySoundEffect(tomatoProjectileSoundEffect);
        ServerManager.Spawn(projectile);
    }

    private void SpawnMeatballComets()
    {
        if (!IsServer) return;
        int numOfComets = UnityEngine.Random.Range(1, maxMeatballComets + 1);
        Array values = Enum.GetValues(typeof(Direction));
        System.Random rng = new System.Random();
        Direction randomDirection = (Direction) values.GetValue(rng.Next(values.Length));

        var xCoord = 0;
        var yCoord = 0;
        Vector3 spawnLocation = new Vector3();

        switch (randomDirection)
        {
            case Direction.Horizontal:
                for (int i = 0; i < numOfComets; i++)
                {
                    var xIndex = rng.Next(0, xRange.Length);
                    var oppXIndex = xIndex == 0 ? 1 : 0;
                    xCoord = xRange[xIndex];
                    yCoord = rng.Next(arenaMinY, arenaMaxY);
                    spawnLocation = new Vector3(xCoord, yCoord);
                    var horizontalProjectile = Instantiate(meatballPrefab, spawnLocation, Quaternion.identity);

                    horizontalProjectile.GetComponent<EnemyStraightProjectile>().targetPosition = new Vector3(xRange[oppXIndex], yCoord);
                    horizontalProjectile.GetComponent<CharacterDamager>().damage = enemyAI.currAttack;
                    horizontalProjectile.GetComponent<Lifetime>().lifetime = 10;
                
                    ServerManager.Spawn(horizontalProjectile);
                }
                break;
            case Direction.Vertical:
                for (int i = 0; i < numOfComets; i++)
                {
                    var yIndex = rng.Next(0, yRange.Length);
                    var oopYIndex = yIndex == 0 ? 1 : 0;
                    xCoord = rng.Next(arenaMinX, arenaMaxX);
                    yCoord = yRange[yIndex];
                    spawnLocation = new Vector3(xCoord, yCoord);
                    var verticalProjectile = Instantiate(meatballPrefab, spawnLocation, Quaternion.identity);

                    verticalProjectile.GetComponent<EnemyStraightProjectile>().targetPosition = new Vector3(xCoord, yRange[oopYIndex]);
                    verticalProjectile.GetComponent<CharacterDamager>().damage = enemyAI.currAttack;
                    verticalProjectile.GetComponent<Lifetime>().lifetime = 10;
                    
                    ServerManager.Spawn(verticalProjectile);
                }
                break;
        }
        AudioManager.Instance.PlaySoundEffect(meatballSoundEffect);
    }

    private void SummonTomatoBombs()
    {
        if (!IsServer) return;
        System.Random rng = new System.Random();
        var numOfTomato = rng.Next(1, maxTomatoBombs);

        for (int i = 0; i < numOfTomato; i++)
        {
            var position = new Vector3(UnityEngine.Random.Range(transform.position.x - 5, transform.position.x + 5),
                UnityEngine.Random.Range(transform.position.y - 5, transform.position.y + 5));
            var tomato = Instantiate(tomatoBombPrefab, position, Quaternion.identity);
            ServerManager.Spawn(tomato);
        }
    }
}
