using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoopController : MonoBehaviour
{
    public ObstacleFactoryController m_northFactory;
    public ObstacleFactoryController m_southFactory;
    public ObstacleFactoryController m_eastFactory;
    public ObstacleFactoryController m_westFactory;

    public float m_spawnTime;
    public int m_pairedChance;

    private GameObject[] m_unpairedObstacleList;
    private GameObject[] m_pairedObstacleList;
    private List<ObstacleFactoryController> m_readyToSpawnFactories;

    private float m_timer;
    private bool m_isPreparingPair;

    // Start is called before the first frame update
    void Start()
    {
        PlayerLivesController.OnPlayerDeath += ResetGameLoop;

        m_unpairedObstacleList = Resources.LoadAll<GameObject>("Prefabs/UnpairedObstacles");
        m_pairedObstacleList = Resources.LoadAll<GameObject>("Prefabs/PairedObstacles");

        m_readyToSpawnFactories = new List<ObstacleFactoryController>();

        m_timer = 0;
        m_isPreparingPair = false;
    }

    // Update is called once per frame
    void Update()
    {
        if ((m_timer += Time.deltaTime) > m_spawnTime)
        {
            m_timer = 0;
            UpdateReadyToSpawnList();
            int random = Random.Range(0, 100);
            if (random <= m_pairedChance)
            {
                m_isPreparingPair = true;
            }

            if (m_readyToSpawnFactories.Count > 0)
            {
                if (m_isPreparingPair && m_readyToSpawnFactories.Count >= 2)
                    SpawnPaired();
                else
                    SpawnUnpaired();
            }
        }
    }

    void SpawnUnpaired()
    {
        // choose from one of the spawners that can spawn and grab a random obstacle
        int random = Random.Range(0, m_unpairedObstacleList.Length - 1);

        m_readyToSpawnFactories[Random.Range(0, m_readyToSpawnFactories.Count - 1)].SpawnObstacle(m_unpairedObstacleList[random]);
    }
    void SpawnPaired()
    {
        // choose which pair to spawn
        int randomPair = Random.Range(1, (m_pairedObstacleList.Length + 1) / 2);

        // check if ready list contains the correct factories
        if (m_readyToSpawnFactories.Contains(m_northFactory) && m_readyToSpawnFactories.Contains(m_southFactory))
        {
            m_northFactory.SpawnObstacle(m_pairedObstacleList[(randomPair * 2) - 2]);
            m_southFactory.SpawnObstacle(m_pairedObstacleList[(randomPair * 2) - 1]);

        }
        else if (m_readyToSpawnFactories.Contains(m_eastFactory) && m_readyToSpawnFactories.Contains(m_westFactory))
        {
            m_eastFactory.SpawnObstacle(m_pairedObstacleList[(randomPair * 2) - 2]);
            m_westFactory.SpawnObstacle(m_pairedObstacleList[(randomPair * 2) - 1]);
        }
    }

    void UpdateReadyToSpawnList()
    {
        m_readyToSpawnFactories.Clear();

        if (m_northFactory.CanSpawn())
            m_readyToSpawnFactories.Add(m_northFactory);

        if (m_southFactory.CanSpawn())
            m_readyToSpawnFactories.Add(m_southFactory);

        if (m_eastFactory.CanSpawn())
            m_readyToSpawnFactories.Add(m_eastFactory);

        if (m_westFactory.CanSpawn())
            m_readyToSpawnFactories.Add(m_westFactory);
    }

    void ResetGameLoop()
    {

    }
}
