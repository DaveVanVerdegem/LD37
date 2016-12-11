using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// GetDistanceToTarget opkuisen met getdistance etc voor of de state momenteel in combat, alert, of idle is.
// alert loskoppelen van combat.
// enum onderzoeken.

public class Character : MonoBehaviour {

    // character core attributes
    #region Properties
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
    public float TreasurePriority; // how much priority the character gives to either fight or grab treasure.

    private List<GameObject> _detectedGameObjects = new List<GameObject>();

    private enum _characterStates { Idle, Combat, Alert, Fetch}
    private int _currentState = (int) _characterStates.Idle;
    private bool _isAlive = true;
    private int _currentHealth;
    private float _attackTimer = 0.0f;
    private float _idleMovementTimer = 0.0f;
    private float _idleWaitTimer = 0.0f;
    private float _alertnessTimer = 0.0f;
    private bool _newIdlePositionChosen = false;

    private Vector2 _newIdleMovePosition;
    private GameObject _currentTarget;
    private List<Character> _targetedBy = new List<Character>();

    // make class Modifier with remaining time and effect or so...
    private float _damageDealtModifier = 1.0f;
    private float _speedModifier = 1.0f;
    private float _attackSpeedModifier = 1.0f;
    private float _damageTakenModifier = 1.0f;
    #endregion


    void Start ()
    {
        _currentHealth = MaxHealth;
        _alertnessTimer = MaxAlertnessDurationSeconds; // causes the monster to not be alert at first
        SwitchToIdleState();
        if (_targetedBy.Contains(GetComponent<Character>()))
        {
            _targetedBy.Remove(GetComponent<Character>());
        }
    }
	
	void Update ()
    {
        switch (_currentState)
        {
            case (int)_characterStates.Combat:
                CharacterCombat();
                break;
            case (int)_characterStates.Alert:
                CharacterAlert();
                break;
            case (int)_characterStates.Idle:
                CharacterIdle();
                break;
            case (int)_characterStates.Fetch:
                CharacterFetch();
                break;
            default:
                Debug.Log("Invalid character state");
                break;
        }
	}

    #region GeneralFunctionality
    #region Animations
    void SetAnimation(string newAnimation)
    {

    }
    #endregion

    #region Movement
    void MoveTo(Vector2 destination, float MovementSpeed)
    {
        // TODO implement pathfinding here proper.
        float MoveStep = MovementSpeed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, destination, MoveStep);
    }
    #endregion

    #region ObjectDetection
    void DetectCollidersInArea(float detectionRadius)
    {
        Collider2D[] Colliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius);
        Debug.Log(Colliders.Length);
        foreach (Collider2D Collider in Colliders)
        {
            Debug.Log(Physics2D.Raycast(transform.position, transform.position - Collider.transform.position).collider == Collider);
            if (!_detectedGameObjects.Contains(Collider.gameObject) && Physics2D.Raycast(transform.position, Collider.transform.position - transform.position).collider == Collider)
            {
                if (Collider.GetComponent<Character>() != null && Collider.GetComponent<Character>().Group != Group)
                {
                    Debug.Log("Euh");
                    DetectedEnemy(Collider);
                }
                if (Collider.GetComponent<Object>() != null)
                {
                    DetectedTreasure(Collider);
                }
                _detectedGameObjects.Add(Collider.gameObject);
            }

        }
    }

    void DetectedEnemy(Collider2D Collider)
    {
        Debug.Log(Collider.name);
        if (_currentState != (int)_characterStates.Combat && _currentState != (int)_characterStates.Fetch)
        {
            _currentTarget = Collider.gameObject;
            // adds the character to the new target's _targetedBy list.
            _currentTarget.GetComponent<Character>()._targetedBy.Add(GetComponent<Character>());
            Debug.Log("WOAH! DETECTED");
            Debug.Log(_currentTarget.name);
            SwitchToCombatState();
            return;
        }
    }

    void DetectedTreasure(Collider2D Collider)
    {
        if (_currentState == (int)_characterStates.Fetch || (_currentState == (int)_characterStates.Combat && Random.value > TreasurePriority))
        {
            return;
        }
        _currentTarget = Collider.gameObject;
        // adds the character to the new target's _targetedBy list.
        // _currentTarget.GetComponent<Treasure>()._targetedBy.Add(gameObject);
        SwitchToFetchState();
        return;
    }
    #endregion

    #region Targetting
    void ClearTarget()
    {
        RemoveFromTargetingList();
        _currentTarget = null;
        if (_currentState == (int)_characterStates.Combat)
        {
            SwitchToAlertState();
        }
    }

    void RemoveFromTargetingList()
    {
        if (_currentTarget.GetComponent<Character>() != null)
        {
            if (_currentTarget.GetComponent<Character>()._targetedBy.Contains(GetComponent<Character>()))
            {
                _currentTarget.GetComponent<Character>()._targetedBy.Remove(GetComponent<Character>());
            }
        }
        /*
        if (_currentTarget.GetComponent<Treasure>() != null)
        {
            if (_currentTarget.GetComponent<Treasure>()._targetedBy.Contains(GetComponent<Character>()))
            {
                _currentTarget.GetComponent<Treasure>()._targetedBy.Remove(GetComponent<Character>());
            }
        }
        */

    }

    #endregion

    #endregion

    #region CharacterIdle
    void CharacterIdle()
    {
        _idleMovementTimer += Time.deltaTime;
        if (_idleMovementTimer > _idleWaitTimer)
        {
            CharacterIdleMove();
        }
        DetectCollidersInArea(IdleDetectionRadius);
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
        _currentState = (int)_characterStates.Idle;
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

    #region CharacterAlert
    void CharacterAlert()
    {
        _alertnessTimer += Time.deltaTime;
        DetectCollidersInArea(CombatDetectionRadius);
        CharacterAlertMove();

        if (_alertnessTimer > MaxAlertnessDurationSeconds)
        {
            SwitchToIdleState();
        }
    }

    void CharacterAlertMove()
    {
        SetAnimation("CombatMove");
        if (Vector2.Distance(_newIdleMovePosition, transform.position) == 0f)
        {
            SetNewIdlePosition();
        }
        else
        {
            MoveTo(_newIdleMovePosition, MovementSpeedCombat);
        }
    }

    void SwitchToAlertState()
    {
        _currentState = (int)_characterStates.Alert;
        SetNewIdlePosition();
    }

    #endregion

    #region CharacterFetch
    void CharacterFetch()
    {

    }

    void SwitchToFetchState()
    {

    }
    #endregion

    #region CharacterCombat
    void CharacterCombat()
    {
        _attackTimer += Time.deltaTime;
        CharacterCombatMove();
        if (_attackTimer > AttackSpeedSeconds)
        {
            Attack();
        }
    }

 
    float GetDistanceToTarget(Vector2 target)
    {
        return Vector2.Distance(transform.position, target);
    }

    void CharacterCombatMove()
    {
        SetAnimation("CombatMove");
        if (GetDistanceToTarget(_currentTarget.transform.position) > AttackDistance)
        {
            MoveTo(_currentTarget.transform.position, MovementSpeedCombat);
        }
    }
        
    void Attack()
    {
        if (GetDistanceToTarget(_currentTarget.transform.position) <= AttackDistance)
        {
            _attackTimer = 0.0f;
            SetAnimation("Attack");
            int damageDealt = (int)(Random.Range(MaxBaseDamage, MaxBaseDamage) * _damageDealtModifier);
            _currentTarget.GetComponent<Character>().ReceiveDamage(gameObject, damageDealt);
        }
    }

    public void ReceiveDamage(GameObject damageDealer, int damage)
    {
        _currentHealth -= (int)_damageTakenModifier * damage;
        if (_currentHealth <= 0)
        {
            Debug.Log("Dying!!");
            CharacterDeath();
        }

        if (_currentState == (int)_characterStates.Fetch)
        {
            if (damageDealer.GetComponent<Character>() != null && Random.value > TreasurePriority)
            {
                _currentTarget = damageDealer;
                SwitchToCombatState();
            }
        }
    }

    void SwitchToCombatState()
    {
        _currentState = (int)_characterStates.Combat;
        _attackTimer = 0.0f;
    }

    
    #endregion

    #region CharacterDeath
    void CharacterDeath()
    {
        foreach (Character CharacterTargetedBy in _targetedBy)
        {
            CharacterTargetedBy.ClearTarget();
        }
        // do death animation
        SetAnimation("Death");
        _isAlive = false;
        Destroy(gameObject, 0.2f);
    }
    #endregion
}
