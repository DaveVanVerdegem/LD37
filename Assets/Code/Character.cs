using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {

    public int MaxHealth;
    public float MovementSpeed;
    public float MinBaseDamage;
    public float MaxBaseDamage;
    public float AttackSpeedSeconds;
    public float AttackDistance;
    public float MaxIdleMovement;
    public float MaxIdleWaitSeconds;
    
    // use this for future additions
    public string BattleType;
    public List<string> Skills;

    private bool _inCombat = false;
    private bool _isAlive = true;
    private int _currentHealth;
    private float _attackTimer = 0.0f;
    private float _idleMovementTimer = 0.0f;
    private float _idleWaitTimer = 0.0f;
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

    void MoveTo(Vector2 destination)
    {
        float MoveStep = MovementSpeed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position,destination, MoveStep);
    }

    #region CharacterIdle
    void CharacterIdle()
    {
        _idleMovementTimer += Time.deltaTime;
        if (_idleMovementTimer > _idleWaitTimer)
        {
            CharacterIdleMove();
        }
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
            MoveTo(_newIdleMovePosition);
        }

    }

    void SwitchToIdleState()
    {
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
        CharacterCombatMove();
    }

    void CharacterCombatMove()
    {

    }

    List<GameObject> GetTargetsInView()
    {
        List<GameObject> Targets = new List<GameObject>();
        return Targets;
    }

    void SelectNewTarget()
    {
        // if no target in range ->
        List<GameObject> TargetsInView = GetTargetsInView();
        if (TargetsInView.Count == 0)
        {
            SwitchToIdleState();
        }
        else
        {
            
        }
        
    }
        
    void CharacterAttack()
    {
        // Time.deltaTime;
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
    }

    void RemoveTarget()
    {
        _currentTarget = null;
    }
    #endregion
}
