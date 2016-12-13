using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine;
using Spine.Unity;

// GetDistanceToTarget opkuisen met getdistance etc voor of de state momenteel in combat, alert, of idle is.
// alert loskoppelen van combat.
// enum onderzoeken.

	[SelectionBase]
public class Character : MonoBehaviour {

    #region Inspector Fields
    [SerializeField]
    [Tooltip("Loot prefab to drop on death.")]
    /// <summary>
    /// Loot prefab to drop on death.
    /// </summary>
    private Loot _lootPrefab;

    [SerializeField]
    [Tooltip("Amount of gold in inventory of this character.")]
    /// <summary>
    /// Amount of gold in inventory of this character.
    /// </summary>
    private int _gold = 10;

	[Tooltip("Price in gold of this character.")]
	/// <summary>
	/// Price in gold of this character.
	/// </summary>
	public int Price = 10;
	#endregion

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

    public bool AutoGoToExit;

    // use this for future additions
    public string Group;
    public string BattleType;
    public List<string> Skills;
    public float TreasurePriority; // how much priority the character gives to either fight or grab treasure.

    private List<GameObject> _detectedGameObjects = new List<GameObject>();
    private SkeletonAnimation _skeletonAnimation;

    private enum _characterStates { Idle, Combat, Alert, Fetch, Death }
    private int _currentState = (int)_characterStates.Idle; // private _characterStates
    private bool _isAlive = true;
    private int _currentHealth;
    private float _attackTimer = 0.0f;
    private float _idleMovementTimer = 0.0f;
    private float _idleWaitTimer = 0.0f;
    private float _alertnessTimer = 0.0f;
    private bool _newIdlePositionChosen = false;

    public Vector2 NewIdleMovePosition;
    private GameObject _currentTarget;
    private List<Character> _targetedBy = new List<Character>();

    // make class Modifier with remaining time and effect or so...
    private float _damageDealtModifier = 1.0f;
    private float _speedModifier = 1.0f;
    private float _attackSpeedModifier = 1.0f;
    private float _damageTakenModifier = 1.0f;

    private bool _animationFinished = true;
    private bool _deathAnimationFinished = false;

    private bool _nearExit = false;

    private float _movementEpsilon = 0.3f;
    #endregion


    void Start()
    {
        
        _skeletonAnimation = GetComponentInChildren<SkeletonAnimation>();
        _currentHealth = MaxHealth;
        _alertnessTimer = MaxAlertnessDurationSeconds; // causes the monster to not be alert at first
        SwitchToIdleState();
        if (_targetedBy.Contains(GetComponent<Character>()))
        {
            _targetedBy.Remove(GetComponent<Character>());
        }
        _skeletonAnimation.state.Complete += delegate {
            _animationFinished = true;
            string CurrentAnimation = _skeletonAnimation.state.GetCurrent(0).ToString();
            if (CurrentAnimation.Equals("death"))
            {
                _deathAnimationFinished = true;
            }
        };

        _skeletonAnimation.state.Event += AnimationHandleEvent;
    }


    // TODO work out how to add complete event to this one?
    void AnimationHandleEvent(TrackEntry trackEntry, Spine.Event e)
    {
        if (e.Data.Name == "AttackHit")
        {
            if (_currentTarget != null)
            {
                if (_currentTarget.GetComponent<Character>()._isAlive)
                {
                    _currentTarget.GetComponent<Character>().TriggerHitAnimation();
                }
            }
        }
        else if (e.Data.Name == "Pickup")
        {
            Debug.Log("PICKUP!");
        }
    }

        void Update()
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
            case (int)_characterStates.Death:
                CharacterDeath();
                break;
            default:
                // Debug.Log("Invalid character state");
                break;
        }
        transform.position = new Vector3(transform.position.x, transform.position.y, -1.0f + transform.position.y / 1000);
        if (AutoGoToExit)
        {
            if (GetDistanceToTarget(TileGrid.ExitTile.transform.position) < 1 && (_currentState == (int)_characterStates.Idle || _currentState == (int)_characterStates.Alert))
            {
                _nearExit = true;
                GameManager.HeroLeavesRoom(gameObject);
            }
        }
    }

    #region GeneralFunctionality
    #region Animations
    void SetNewAnimation(string animation, bool loop)
    {
        _animationFinished = false;
        _skeletonAnimation.state.SetAnimation(0, animation, loop);
    }

    void AnimationManager(string animation, float animationSpeed, bool loop)
    {
        _skeletonAnimation.timeScale = animationSpeed;
        if (_skeletonAnimation.state.GetCurrent(0) == null)
        {
            SetNewAnimation(animation, loop);
        }
        string CurrentAnimation = _skeletonAnimation.state.GetCurrent(0).ToString();
        // if previous animation cycle is rounded up, just do this.
        if (_animationFinished)
        {
            SetNewAnimation(animation, loop);
        }
        else
        {
            if (!animation.Equals(CurrentAnimation) && (CurrentAnimation.Equals("walk") || (CurrentAnimation.Equals("idle"))))
            {
                SetNewAnimation(animation, loop);
            }
            else if (animation.Equals("attack") && !CurrentAnimation.Equals("attack"))
            {
                SetNewAnimation(animation, loop);
            }
            else if (animation.Equals("death") && !CurrentAnimation.Equals("death"))
            {
                SetNewAnimation(animation, loop);
            }
            else if (animation.Equals("hurt"))
            {
                if (CurrentAnimation.Equals("attack"))
                {
                    return;
                }
                if (animation.Equals(CurrentAnimation))
                {
                    SetNewAnimation(animation, loop);
                }
                else if (!CurrentAnimation.Equals("attack") || !CurrentAnimation.Equals("death"))
                {
                    SetNewAnimation(animation, loop);
                }
            }
        }
    }

    void SetAnimation(string newAnimation)
    {

        switch (newAnimation)
        {
            case "IdleWait":
                AnimationManager("idle", 1, false);
                break;
            case "IdleMove":
                AnimationManager("walk", 1, false);
                break;
            case "CombatWait":
                AnimationManager("idle", 1.2f, false);
                break;
            case "CombatMove":
                AnimationManager("walk", 1.2f, false);
                break;
            case "Attack":
                AnimationManager("attack", 1, false);
                break;
            case "Hurt":
                AnimationManager("hurt", 1, false);
                break;
            case "Death":
                AnimationManager("death", 1, false);
                break;
            default:
                Debug.Log("Animation doesn't exist");
                break;
        }
    }

    // not happy with how this works out managing the death inside...
    public void TriggerHitAnimation()
    {
        if (_currentState == (int)_characterStates.Death)
        {
            _isAlive = false;
            SetAnimation("Death");
        }
        else
        {
            SetAnimation("Hurt");
        }
    }
    #endregion

    #region Movement
    void MoveTo(Vector2 destination, float MovementSpeed)
    {
        // TODO implement pathfinding here proper.
        float MoveStep = MovementSpeed * Time.deltaTime;
        // destination = new Vector3(destination.x, destination.y, -1.0f + destination.y / 1000);
        transform.position = Vector3.MoveTowards(transform.position, destination, MoveStep);
        OrientSelf(destination);
    }

    void OrientSelf(Vector2 destination)
    {
        if ((destination - (Vector2)transform.position).x < 0 && transform.localScale.x == 1)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if ((destination - (Vector2)transform.position).x > 0 && transform.localScale.x == -1)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }
    #endregion

    #region ObjectDetection
    void DetectCollidersInArea(float detectionRadius)
    {
        Collider2D[] Colliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius);
        foreach (Collider2D Collider in Colliders)
        {
            if (!_detectedGameObjects.Contains(Collider.gameObject) && Physics2D.Raycast(transform.position, Collider.transform.position - transform.position).collider == Collider)
            {
                if (Collider.GetComponent<Character>() != null && Collider.GetComponent<Character>().Group != Group)
                {
                    DetectedEnemy(Collider);
                }
                if (Collider.GetComponent<Loot>() != null)
                {
                    DetectedTreasure(Collider);
                }
                _detectedGameObjects.Add(Collider.gameObject);
            }

        }
    }

    void DetectedEnemy(Collider2D Collider)
    {
        if (_currentState != (int)_characterStates.Combat && _currentState != (int)_characterStates.Fetch)
        {
            _currentTarget = Collider.gameObject;
            // adds the character to the new target's _targetedBy list.
            _currentTarget.GetComponent<Character>()._targetedBy.Add(GetComponent<Character>());
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
        // _currentTarget.GetComponent<Loot>()._targetedBy.Add(gameObject);
        SwitchToFetchState();
        return;
    }
    #endregion

    #region Targetting
    void ClearTarget()
    {
        // RemoveFromTargetingList();
        _currentTarget = null;
        if (_currentState == (int)_characterStates.Combat)
        {
            SwitchToAlertState();
        }
        _detectedGameObjects = new List<GameObject>();
        // _detectedGameObjects.Clear();
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
        else
        {
            SetAnimation("IdleWait");
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

        if (GetDistanceToTarget(NewIdleMovePosition) <= _movementEpsilon || Physics2D.Raycast(transform.position, NewIdleMovePosition - (Vector2)transform.position).distance < 0.05)
        {
            RaycastHit2D Hit = Physics2D.Raycast(transform.position, NewIdleMovePosition - (Vector2)transform.position);
            if (Hit.collider != null)
            {
                Tile Tile = Hit.collider.GetComponent<Tile>();
                if (Tile != null)
                {
                    if (!Tile.Solid)
                    {
                        MoveTo(NewIdleMovePosition, MovementSpeedIdle);
                        return;
                    }
                    Debug.Log("WwowowoowIdle");
                }
            }
            StartIdleWait();
        }
        else
        {
            MoveTo(NewIdleMovePosition, MovementSpeedIdle);
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
        NewIdleMovePosition = (Vector2)transform.position + Random.insideUnitCircle.normalized * Random.Range(MaxIdleMovement/2, MaxIdleMovement);
        if (AutoGoToExit)
        {
            NewIdleMovePosition = TileGrid.ExitTile.transform.position;
        }
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
        if (GetDistanceToTarget(NewIdleMovePosition) <= _movementEpsilon || Physics2D.Raycast(transform.position, NewIdleMovePosition - (Vector2)transform.position).distance < 0.05)
        {
            RaycastHit2D Hit = Physics2D.Raycast(transform.position, NewIdleMovePosition - (Vector2)transform.position);
            if (Hit.collider != null)
            {
                Tile Tile = Hit.collider.GetComponent<Tile>();
                if (Tile != null)
                {
                    if (!Tile.Solid)
                    {
                        SetAnimation("CombatMove");
                        MoveTo(NewIdleMovePosition, MovementSpeedCombat);
                    }
                }
            }
            SetNewIdlePosition();
        }
        else
        {
            SetAnimation("CombatMove");
            MoveTo(NewIdleMovePosition, MovementSpeedCombat);
        }
    }

    void SwitchToAlertState()
    {
        _alertnessTimer = 0.0f;
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
        if (GetDistanceToTarget(_currentTarget.transform.position) >= AttackDistance)
        {
            SetAnimation("CombatMove");
            MoveTo(_currentTarget.transform.position, MovementSpeedCombat);
        }
        else
        {
            SetAnimation("CombatWait");
            OrientSelf(_currentTarget.transform.position);
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
            SwitchToDeathState();
            return;
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
        if (_deathAnimationFinished)
        {
            Death();
        }
    }
    void SwitchToDeathState()
    {
        _currentState = (int)_characterStates.Death;
    }
    
    void Death()
    {
		Debug.Log("<b>Character died.</b>", this);

        DropLoot();
        foreach (Character CharacterTargetedBy in _targetedBy)
        {
            CharacterTargetedBy.ClearTarget();
        }

		if (Group == "Hero")
			GameManager.HeroKilled();

        Destroy(gameObject);
    }

	/// <summary>
	/// Have the character drop loot.
	/// </summary>
	void DropLoot()
	{
		if (_lootPrefab == null || _gold <= 0)
			return;

		Loot loot = Instantiate(_lootPrefab, transform.position + Vector3.back, Quaternion.identity);

		loot.SetGold(_gold);
		loot.Initialize(null);
	}
	#endregion
}
