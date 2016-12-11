using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// GetDistanceToTarget opkuisen met getdistance etc voor of de state momenteel in combat, alert, of idle is.
// alert loskoppelen van combat.
// enum onderzoeken.

public class Character : MonoBehaviour {

    // character core attributes
    public int MaxHealth;
    public float MovementSpeedIdle;
    public float MovementSpeedCombat;
    public float MinBaseDamage;
    public float MaxBaseDamage;
    public float AttackSpeedSeconds;
    public float AttackDistance;
    public float MaxIdleMovement;
    public float MaxIdleWaitSeconds;
    public float MaxAlertnessDurationSeconds;

    public float IdleDetectionRadius;
    public float CombatDetectionRadius;

    // use this for future additions
    public string Group;
    public string BattleType;
    public List<string> Skills;

    private bool _inCombat = false;
    private bool _isAlive = true;
    private int _currentHealth;
    private float _attackTimer = 0.0f;
    private float _idleMovementTimer = 0.0f;
    private float _idleWaitTimer = 0.0f;
    private float _alertnessTimer = 0.0f;
    private bool _newIdlePositionChosen = false;

    private Vector2 _newIdleMovePosition;
    private Character _currentTarget;
    private List<Character> _targetedBy = new List<Character>();

    // make class Modifier with remaining time and effect or so...
    private float _damageDealtModifier = 1.0f;
    private float _speedModifier = 1.0f;
    private float _attackSpeedModifier = 1.0f;
    private float _damageTakenModifier = 1.0f;

    

	void Start ()
    {
        _currentHealth = MaxHealth;
        _alertnessTimer = MaxAlertnessDurationSeconds; // causes the monster to not be alert at first
        SwitchToIdleState();
	}
	
	void Update ()
    {
        if (_inCombat)
        {
            CharacterCombat();
        }
        else
        {
            CharacterIdle();
        }        
	}

    void SetAnimation(string newAnimation)
    {

    }

    void MoveTo(Vector2 destination, float MovementSpeed)
    {
        // TODO implement pathfinding here proper.
        float MoveStep = MovementSpeed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position,destination, MoveStep);
    }

    void DetectEnemiesInArea(float detectionRadius)
    {
        Collider2D[] Colliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius);
        foreach (Collider2D Collider in Colliders)
        {
            if (Collider.GetComponent<Character>().Group != Group)
            {
                if (Physics2D.Raycast(transform.position, transform.position - Collider.transform.position))
                {
                    _currentTarget = Collider.GetComponent<Character>();
                    _currentTarget.AddTarget(GetComponent<Character>());
                    SwitchToCombatState();
                    return;
                }
            }
        }
    }

    #region CharacterIdle
    void CharacterIdle()
    {
        _idleMovementTimer += Time.deltaTime;
        if (_idleMovementTimer > _idleWaitTimer)
        {
            CharacterIdleMove();
        }
        DetectEnemiesInArea(IdleDetectionRadius);
    }

    void CharacterIdleMove()
    {
        SetAnimation("IdleMove");
        if (!_newIdlePositionChosen)
        {
            SetNewIdlePosition();
        }

        if (Vector2.Distance(_newIdleMovePosition, transform.position) == 0f)
        {
            StartIdleWait();
        }
        else
        {
            MoveTo(_newIdleMovePosition, MovementSpeedIdle);
        }
    }

    void SwitchToIdleState()
    {
        _inCombat = false;
        _idleMovementTimer = 0.0f;
    }

    void StartIdleWait()
    {
        SetAnimation("IdleWait");

        _newIdlePositionChosen = false;
        _idleMovementTimer = 0.0f;
        _idleWaitTimer = Random.value * MaxIdleWaitSeconds;
    }

    void SetNewIdlePosition()
    {
        _newIdleMovePosition = (Vector2) transform.position + Random.insideUnitCircle * MaxIdleMovement;
        _newIdlePositionChosen = true;
    }
    #endregion

    #region CharacterCombat
    void CharacterCombat()
    {
        _attackTimer += Time.deltaTime;
        if (_currentTarget == null)
        {
            SelectNewTarget();
        }
        else
        {
            CharacterCombatMove();
            if (_attackTimer > AttackSpeedSeconds)
            {
                if (GetDistanceToTarget() < AttackDistance)
                {
                    CharacterAttack();
                }
            }
        }
    }

 
    float GetDistanceToTarget()
    {
        if (_inCombat)
        {
            if (_currentTarget == null)
            {
                return Vector2.Distance(transform.position, _newIdleMovePosition);
            }
            else
            {
                return Vector2.Distance(transform.position, _currentTarget.transform.position);
            }
        }
        else
        {
            return Vector2.Distance(transform.position, _newIdleMovePosition);
        }
    }

    void CharacterCombatMove()
    {
        SetAnimation("CombatMove");
        if (GetDistanceToTarget() > AttackDistance)
        {
            MoveTo(_currentTarget.transform.position, MovementSpeedCombat);
        }
    }

    void SelectNewTarget()
    {
        // if no target in range -> search for new target with high radius in alert duration
        DetectEnemiesInArea(CombatDetectionRadius);
        if (_currentTarget == null)
        {
            // this should provide smooth overlap to a slower walk towards the same idle destination
            // once the alert phase is over.
            _alertnessTimer += Time.deltaTime;
            SetAnimation("CombatMove");
            if (!_newIdlePositionChosen)
            {
                StartIdleWait();
                SetNewIdlePosition();
            }
            if (Vector2.Distance(_newIdleMovePosition, transform.position) == 0f)
            {
                SetNewIdlePosition();
            }
            else
            {
                MoveTo(_newIdleMovePosition, MovementSpeedCombat);
            }
        }
        if (_alertnessTimer > MaxAlertnessDurationSeconds)
        {
            SwitchToIdleState();
        }
        
    }
        
    void CharacterAttack()
    {
        _attackTimer = 0.0f;
        SetAnimation("Attack");
        int damageDealt = (int)(Random.Range(MaxBaseDamage, MaxBaseDamage) * _damageDealtModifier);
        _currentTarget.ReceiveDamage(damageDealt);
    }

    void ReceiveDamage(int damage)
    {
        _currentHealth -= (int)_damageTakenModifier * damage;
        if (_currentHealth <= 0)
        {
            CharacterDeath();
        }
    }

    void SwitchToCombatState()
    {
        _inCombat = true;
        _attackTimer = 0.0f;
    }

    void AddTarget(Character character)
    {
        _targetedBy.Add(character);
    }
    #endregion

    #region CharacterDeath
    void CharacterDeath()
    {
        foreach (Character CharacterTargetedBy in _targetedBy)
        {
            CharacterTargetedBy.RemoveTarget();
        }
        // do death animation
        _isAlive = false;
        Destroy(gameObject);
    }

    void RemoveTarget()
    {
        _currentTarget = null;
        _alertnessTimer = 0.0f;
    }
    #endregion
}
