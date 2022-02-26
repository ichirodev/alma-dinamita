using System.Collections.Generic;
using UnityEngine;

public class chaoticSpawner : MonoBehaviour
{
    [SerializeField] public Object commonZombie;
    [SerializeField] public string zone1 = "zone1"; // apocalypse
    [SerializeField] public string zone2 = "zone2"; // death zone
    [SerializeField] public string zone3 = "zone3"; // stress zone
    [SerializeField] public string zone4 = "zone4"; // common zone
    [SerializeField] public string zone5 = "zone5"; // chill zone
    public List<Transform> mapZones;
    public class Zone
    {
        private string zoneTag;
        private bool allowRespawn;
        public int maxBotsAtZone;
        private int initialBotsAtZone;
        public int botsSpawnedAtZone;

        public bool SetZone(string zone)
        {
            this.zoneTag = zone;
            return zone == this.zoneTag;
        }

        public string GetZone()
        {
            return zoneTag;
        }

        public bool SetAllowRespawn()
        {
            allowRespawn = !allowRespawn;
            return allowRespawn;
        }

        public bool GetAllowRespawn()
        {
            return allowRespawn;
        }

        public int GetInitialBotsAtZone()
        {
            return initialBotsAtZone;
        }
        
        public Zone(string zone, bool allowRespawn, int initialBotsAtZone)
        {
            this.zoneTag = zone;
            this.allowRespawn = allowRespawn;
            this.maxBotsAtZone = 0;
            this.initialBotsAtZone = initialBotsAtZone;
            this.botsSpawnedAtZone = 0;
        }
    }

    public void Start()
    {
        Zone chillZone = new Zone(zone5, false, 4);
        chillZone.maxBotsAtZone = 8;
        Zone commonZone = new Zone(zone4, true, 14);
        commonZone.maxBotsAtZone = 28;
        Zone stressZone = new Zone(zone3, true, 24);
        stressZone.maxBotsAtZone = 35;
        Zone deathZone = new Zone(zone2, true, 30);
        deathZone.maxBotsAtZone = 44;
        Zone apocalypse = new Zone(zone1, true, 18);
        deathZone.maxBotsAtZone = 75;
        
        // Spawn zombies for the start of the match
        foreach (Transform mz in GetComponentsInChildren<Transform>())
        {
            if (mz.name != "ChaoticSpawner") mapZones.Add(mz);
            
            var mapZonePositionX = mz.localPosition.x;
            var mapZonePositionZ = mz.localPosition.z;
            var mapZoneScaleX = mz.localScale.x * 10 / 2;
            var mapZoneScaleZ = mz.localScale.z * 10 / 2;
            var zoneX0 = mapZonePositionX - mapZoneScaleX;
            var zoneX1 = mapZonePositionX + mapZoneScaleX;
            var zoneZ0 = mapZonePositionZ -  mapZoneScaleZ;
            var zoneZ1 = mapZonePositionZ + mapZoneScaleZ;

            if (mz.CompareTag(zone1)) 
            {
                for (var j = 0; j < apocalypse.GetInitialBotsAtZone(); j++)
                {
                    var respawnPosition = new Vector3(Random.Range(zoneX0, zoneX1), 1, Random.Range(zoneZ0, zoneZ1));
                    GameObject.Instantiate(commonZombie, respawnPosition, Quaternion.identity);
                }
            } else if (mz.CompareTag(zone2)) 
            {
                for (var j = 0; j < deathZone.GetInitialBotsAtZone(); j++)
                {
                    var respawnPosition = new Vector3(Random.Range(zoneX0, zoneX1), 1, Random.Range(zoneZ0, zoneZ1));
                    GameObject.Instantiate(commonZombie, respawnPosition, Quaternion.identity);
                }
            } else if (mz.CompareTag(zone3)) 
            {
                for (var j = 0; j < stressZone.GetInitialBotsAtZone(); j++)
                {
                    var respawnPosition = new Vector3(Random.Range(zoneX0, zoneX1), 1, Random.Range(zoneZ0, zoneZ1));
                    GameObject.Instantiate(commonZombie, respawnPosition, Quaternion.identity);
                }
            } else if (mz.CompareTag(zone4)) 
            {
                for (var j = 0; j < commonZone.GetInitialBotsAtZone(); j++)
                {
                    var respawnPosition = new Vector3(Random.Range(zoneX0, zoneX1), mz.localPosition.y + 1.0f, Random.Range(zoneZ0, zoneZ1));
                    var lastInstantiatedZombie = GameObject.Instantiate(commonZombie, respawnPosition, Quaternion.identity);
                }
            } else if (mz.CompareTag(zone5)) 
            {
                for (var j = 0; j < chillZone.GetInitialBotsAtZone(); j++)
                {
                    var respawnPosition = new Vector3(Random.Range(zoneX0, zoneX1), 1, Random.Range(zoneZ0, zoneZ1));
                    GameObject.Instantiate(commonZombie, respawnPosition, Quaternion.identity);
                }
            }
        }
    }
}
