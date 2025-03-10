using UnityEngine;

public class PlayerAnimEvent : AnimEvent
{
    public LayerMask attackLayer;
    public Transform attackPoint;
    private Player _player;

    protected override void Start()
    {
        base.Awake();
        _player = PlayerManager.Instance.player;
    }

    public void Attack1()
    {
        Attack(0);
    }

    public void Attack2()
    {
        Attack(1);
    }

    public void Attack3()
    {
        Attack(2);
    }

    private void Attack(int rangeIndex)
    {
        // LayerMask.NameToLayer("Enemy") 检测不了 默认检测default层
        GameEventDispatcher.PlayerAttack?.Invoke(attackPoint);

        // 施加轻微攻击移动力
        _player.SetVelocity(new Vector2(_player.attackSlightForce[rangeIndex] * _player.facingDir,
            _player.GetVelocity().y));

        Collider2D[] cds =
            Physics2D.OverlapCircleAll(_player.attackPoint.position, _player.attackRangeArray[rangeIndex], attackLayer);
        foreach (var cd in cds)
        {
            // 施加击退力
            cd.GetComponent<Entity>().Knockback(_player.knockbackForce);

            AlmightyStats almightyStats = _player.entityStats as AlmightyStats;
            EntityStats stats = cd.GetComponent<Entity>().entityStats;
            switch (stats.statsType)
            {
                case E_CharacterStats.Almighty:
                    almightyStats.DoDamage(stats as AlmightyStats);
                    break;
                case E_CharacterStats.Mage:
                    almightyStats.DoDamage(stats as MageStats);
                    break;
                case E_CharacterStats.Warrior:
                    almightyStats.DoDamage(stats as WarriorStats);
                    break;
                default:
                    Debugger.Error(
                        $"Unknown CharacterStats: {stats.statsType}. Please check the input value and ensure it is one of the valid CharacterStats enum values.");
                    break;
            }
        }
    }
}