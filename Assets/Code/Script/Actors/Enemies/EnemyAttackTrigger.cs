using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackTrigger : MonoBehaviour
{
    [SerializeField] CapsuleCollider attackCollider;
    Enemy enemy;
    private List<Player> playersHit;
    public bool playerHit = false;
    bool midAttack = false;
    private Dictionary<int, bool> playersBeingHit;

    private void Awake()
    {
        playersHit = new List<Player>();
        playersBeingHit = new Dictionary<int, bool>();
        playersBeingHit.Add(0, false);
        playersBeingHit.Add(1, false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 12 && other.gameObject.GetComponent<Player>())
        {
            Player playerBeingHit = other.gameObject.GetComponent<Player>();
            if (midAttack)
            {
                int characterIndex;
                if (other.GetComponent<CelestialPlayer>())
                {
                    characterIndex = 0;
                }
                else
                {
                    characterIndex = 1;
                }
                if (!playersBeingHit[characterIndex])
                {
                    EnemyStats myStats = enemy.GetEnemyStats();
                    bool playerIsDead;
                    playersBeingHit[characterIndex] = true;
                    int HitPoints = myStats.maxDamage;
                    if (!playerBeingHit.GetShielded())
                    {
                        playerIsDead = playerBeingHit.TakeHit(HitPoints);
                    }
                    else
                    {
                        enemy.TakeHit(HitPoints);
                    }
                    
                }
            }
        }
    }

    public void ToggleAttacking(bool attacking)
    {
        midAttack = attacking;
        if (midAttack)
        {
            attackCollider.enabled = true;
        }
        if (!midAttack)
        {
            attackCollider.enabled = false;
            for (int i = 0; i < playersBeingHit.Count; i++)
            {
                playersBeingHit[i] = false;
            }
            playersHit.Clear();
        }
    }

    public void SetEnemy(Enemy enemy)
    {
        this.enemy = enemy;
    }
}
