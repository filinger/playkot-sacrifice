using System;
using System.Linq;
using System.Collections.Generic;


/// <summary> 
/// This is a base interface for all game events.
/// Each event type usually notifies about some significant ingame action.
/// </summary>
/// <example>
/// ObjectDamagedEvent, ObjectDestroyedEvent.
/// </example>
interface IGameEvent {

    /// <summary> 
    /// Retrieves the optional context of this game event.
    /// </summary>
    /// <remarks>
    /// Typically the context is the origin game object, but no concrete
    /// implementation should be expected from this interface.
    /// </remarks>
    /// <returns>
    /// Context object of this event or null if no context was provided.
    /// </returns>    
    Object GetContext();
}

/// <summary> 
/// This is a base interface for all game components.
/// Typically each component type is responsible for distinct game aspect and 
/// holds its implementation.
/// To indirectly interact with each other game components use game events.
/// </summary>
/// <example>
/// TransformComponent which holds ingame object's position and rotation;
/// RendererComponent which implements game object's mesh data rendering.
/// </example>
interface IGameComponent {
    
    /// Sends arbitrary game event into the game component.
    /// The component implementation defines if and how it should react to the
    /// specified event.
    /// </summary>
    /// <param name="gameEvent">
    /// Ingame event to react on. 
    /// </param> 
    void OnEvent(IGameEvent gameEvent);
}

/// <summary> 
/// This is a base interface for all game objects.
/// Each game object has one or more game components which define the object's
/// behaviour.
/// </summary>
/// <remarks>
/// You may indirectly interact with game object via broadcasting game events.
/// Game object can have only one instance of specific component type attached
/// at the same time.
/// </remarks>
/// <example>
/// Tree, Grass, Rock, House, Player.. actually any entity which is considered
/// to be part of the game world.
/// </example>
interface IGameObject {
    
    /// <summary> 
    /// Adds new component to the game object.
    /// </summary>
    /// <param name="gameComponent">
    /// New game component to add.
    /// </param> 
    /// <exception cref="InvalidArgumentException">
    /// If object already has a component of the specified type.
    /// Consider updating this game object's component parameters instead.
    /// </exception>
    void AddComponent(IGameComponent gameComponent);
    
    /// <summary> 
    /// Removes the component from the game object.
    /// </summary>
    /// <param name="gameComponent">
    /// Game component to remove.
    /// </param> 
    /// <exception cref="InvalidArgumentException">
    /// If object actually has no component of the specified type.
    /// </exception>
    void RemoveComponent(IGameComponent gameComponent);
    
    /// <summary> 
    /// Retrieves all game components of this game object.
    /// </summary>
    /// <remarks>
    /// Order is not preserved.
    /// </remarks>
    /// <returns>
    /// Set of this game object's components.
    /// </returns>
    IEnumerable<IGameComponent> GetComponents();
    
    /// <summary> 
    /// Retrieves concrete game component of the specified type.
    /// </summary>
    /// <typeparam name="T">
    /// Desired game component type.
    /// </typeparam> 
    /// <returns>
    /// Game component of the specified type if this game object has such, 
    /// null otherwise.
    /// </returns>
    T GetComponent<T>() where T : IGameComponent;

    /// <summary> 
    /// Broadcasts an arbitrary game event to the game object.
    /// </summary>
    /// <param name="gameEvent">
    /// Game event to send.
    /// </param> 
    void SendEvent(IGameEvent gameEvent);
}

/// <summary> 
/// Scene is a collection of game objects and holds the representation of the
/// game world.
/// </summary>
/// <typeparam name="TGameObject">
/// Scene's game objects type.
/// </typeparam> 
/// <remarks>
/// There's usually only one active scene in the game.
/// </remarks>
interface IScene<TGameObject> where TGameObject : IGameObject {
    
    /// <summary> 
    /// Adds the specified game object to the scene.
    /// It means that the object should be retreivable by other scene methods.
    /// </summary>
    /// <param name="gameObject">
    /// Game object to add.
    /// </param>
    /// <exception cref="InvalidArgumentException">
    /// If scene already has a reference to the specified game object.
    /// </exception>
    void Add(TGameObject gameObject);
    
    /// <summary> 
    /// Removes the specified game object from the scene.
    /// </summary>
    /// <param name="gameObject">
    /// Game object to remove.
    /// </param>
    /// <exception cref="InvalidArgumentException">
    /// If scene actually has no reference to the specified object.
    /// </exception>
    void Remove(TGameObject gameObject);
    
    /// <summary> 
    /// Retrieves all game objects that are currently present in the scene.
    /// </summary>
    /// <returns>
    /// All game objects of the scene.
    /// </returns>
    IEnumerable<TGameObject> GetAllGameObjects();
    
    /// <summary> 
    /// Retrieves game components of specified type which are currently present
    /// in the scene.
    /// </summary>
    /// <typeparam name="T">
    /// Desired game components type.
    /// </typeparam> 
    /// <returns>
    /// Game components of the specified type if any are present.
    /// </returns>    
    IEnumerable<T> GetComponentsOf<T>() where T : IGameComponent;
}

/// <summary>
/// This is a more specialized scene for two-dimensional game objects.
/// It allows to query objects by arbitrary two-dimensional ranges.
/// </summary>
interface IScene2D : IScene<IGameObject2D> {
    
    /// <summary> 
    /// Finds game objects which positionally are inside the specified range.
    /// </summary>
    /// <param name="start">
    /// Start point of the two-dimensional range.
    /// </param>
    /// <param name="end">
    /// End point of the two-dimensional range.
    /// </param>
    /// <returns>
    /// Set of game objects within the specified range.
    /// </returns>
    IEnumerable<IGameObject2D> FindGameObjectsIn(Point2D start, Point2D end);
    
    /// <summary> 
    /// Finds game objects which positionally are around target game object.
    /// </summary>
    /// <param name="target">
    /// Target game object to look around from.
    /// </param>
    /// <param name="radius">
    /// How far from the target the lookup should extend.
    /// </param>
    /// <returns>
    /// Set of game objects within the radius of the specified target object.
    /// </returns>
    IEnumerable<IGameObject2D> FindGameObjectsAround(IGameObject2D target, int radius);
}




/**
 Example partial implementations for the supplied task are below.
*/


// Data types


/// <summary>
/// This struct is basically a specialized version of Tuple and is intendent
/// for two-dimensional grid point data.
/// </summary>
[Serializable()]
struct Point2D {
    
    public int x;
    public int y;
}

/// <summary>
/// A range tree on a set of 1-dimensional points is a balanced binary search 
/// tree on those points. The points stored in the tree are stored in the leaves of 
/// the tree; each internal node stores the largest value contained in its left 
/// subtree. A range tree on a set of points in d-dimensions is a recursively 
/// defined multi-level binary search tree. Each level of the data structure is a 
/// binary search tree on one of the d-dimensions. The first level is a binary 
/// search tree on the first of the d-coordinates. Each vertex v of this tree 
/// contains an associated structure that is a (d−1)-dimensional range tree on the 
/// last (d−1)-coordinates of the points stored in the subtree of v.
/// </summary>
/// <remarks>
/// Using fractional cascading optimization:
/// Expected query complexity: O(log^(d-1)(n) + k)
/// Space complexity: O(n(log(n) / log(log(n)))^(d-1))
/// </remarks>
[Serializable()]
sealed class RangeTree<TPoint, TNode> {
    
    public void Put(TPoint point, TNode node) {
        // Implementation omitted.        
    }

    public void Move(TNode node, TPoint newPoint) {
        // Implementation omitted.      
    }
    
    /// <summary>
    /// Most implementations of this method require partial tree rebuilding.
    /// </summary>
    public IEnumerable<TNode> FindIn(TPoint start, TPoint end) {
        // Implementation omitted.
        return Enumerable.Empty<TNode>();
    }

    public void Remove(TNode node) {
        // Implementation omitted.   
    }
}

/// <summary>
/// This is a special kind of dictionary that permits duplicate keys (as well 
/// as duplicate values). Or, viewed from a different perspective, it allows 
/// multiple values to be associated with a single key. It is comparable to 
/// the C++ std::multimap class.
/// Much of the power of the multimap API comes from the view collections it 
/// provides. These always reflect the latest state of the multimap itself. 
/// When they support modification, the changes are write-through (they 
/// automatically update the backing multimap).
/// </summary>
/// <remarks>
/// Complexity for equal operations is identical to BTree.
/// </remarks>
[Serializable()]
sealed class MultiMap<TKey, TValue> {
    
    public void Put(TKey key, TValue value) {
        // Implementation omitted.
    }
    
    public ICollection<TValue> Get(TKey key) {
        // Implementation omitted.
        return new List<TValue>();
    }
    
    public void Remove(TKey key, TValue value) {
        // Implementation omitted.
    }
    
    public void RemoveAll(TKey key) {
        // Implementation omitted.
    }
    
    public ICollection<TKey> KeySet() {
        // Implementation omitted.
        return new List<TKey>();
    }
}


// Events


/// <summary> 
/// This is a partial implementation for game events which are produced by
/// other game objects and their components.
/// </summary>
abstract class GameObjectEvent : IGameEvent {
    
    private IGameObject origin;
    
    public GameObjectEvent(IGameObject origin) {
        this.origin = origin;    
    }
    
    public Object GetContext() {
        return origin;
    }
}

/// <summary> 
/// This event indicates that the object has been damaged.
/// </summary>
sealed class ObjectDamagedEvent : GameObjectEvent {
    
    public ObjectDamagedEvent(IGameObject origin) : base(origin) { }
    
    public int DamageAmount { get; set; }
}

/// <summary> 
/// This event indicates that the object has been repaired.
/// </summary>
sealed class ObjectRepairedEvent : GameObjectEvent {
        
    public ObjectRepairedEvent(IGameObject origin) : base(origin) { }
    
    public int RepairAmount { get; set; }
}

/// <summary> 
/// This event indicates that the object has been destroyed.
/// </summary>
sealed class ObjectDestroyedEvent : GameObjectEvent {
        
    public ObjectDestroyedEvent(IGameObject origin) : base(origin) { }
}


// Components


/// <summary>
/// This is a base class for passive game components.
/// <summary> 
abstract class PassiveGameComponent : IGameComponent {

    /// <remarks>
    /// Does not interact with any game events.
    /// </remarks>
    public void OnEvent(IGameEvent gameEvent) {
        return;
    }    
}

/// <summary>
/// This component provides position in the two-dimensional game world.
/// </summary>
[Serializable()]
sealed class Transform2D : PassiveGameComponent {
    
    public Point2D Position { get; set; }
}

/// <summary>
/// This component provides ability to damage and destroy other game objects.
/// </summary>
[Serializable()]
sealed class Destroyer : PassiveGameComponent {
    
    public int DamagePoints { get; set; }
    
    /// <summary>
    /// Sends appropriate ObjectDamagedEvent to the target.
    /// </summary>
    public void Damage(IGameObject target) {
        // Implementation omitted.
    }
}

/// <summary>
/// This component provides ability to damage and destroy other game objects.
/// </summary>
[Serializable()]
sealed class Repairer : PassiveGameComponent {
    
    public int RepairPoints { get; set; }
    
    /// <summary>
    /// Sends appropriate ObjectRepairEvent to the target.
    /// </summary>
    public void Repair(IGameObject target) {
        // Implementation omitted.
    }
}

/// <summary>
/// This is a base class for game components which may propagate or emit
/// new events.
/// <summary> 
abstract class PropagatingGameComponent : IGameComponent {

    private IGameObject parent;
    
    public PropagatingGameComponent(IGameObject parent) {
        this.parent = parent;    
    }
    
    public abstract void OnEvent(IGameEvent gameEvent);
    
    protected void PropagateEvent(IGameEvent gameEvent) {
        parent.SendEvent(gameEvent);
    }
}

/// <summary>
/// This component makes objects destroyable via addition of "health points" (HP)
/// mechanism. Some actions deplete HP and others may restore it. If HP reaches
/// 0 the object is automatically destroyed.
/// </summary>
/// <remarks>
/// Component is initialized with defined amount of max HP.
/// HP can not exceed the amount component was initialized with.
/// Produces ObjectDestroyedEvent upon object destruction.
/// </remarks>
[Serializable()]
sealed class Destroyable : PropagatingGameComponent {
    
    private int maxHealthPoints;
    private int currentHealthPoints;
    
    public Destroyable(IGameObject parent, int maxHealthPoints) : base(parent) {
        this.maxHealthPoints = maxHealthPoints;
        this.currentHealthPoints = maxHealthPoints;
    }
    
    /// <summary>
    /// Reacts on ObjectDamagedEvent and ObjectRepairedEvent.
    /// Emits ObjectDestroyedEvent.
    /// </summary>
    public override void OnEvent(IGameEvent gameEvent) {
        // Implementation omitted.
    }
}

/// <summary>
/// Will instantiate new game object after the current game object is destroyed.
/// </summary>
[Serializable()]
sealed class Remains : IGameComponent {
    
    public IGameObject remains;
    
    public Remains(IGameObject remains) {
        this.remains = remains;
    }
    
    /// <summary>
    /// Reacts on ObjectDestroyedEvent.
    /// </summary>
    public void OnEvent(IGameEvent gameEvent) {
        // Implementation omitted.
    }
}

/// <summary>
/// Will damage surrounding objects after the current game object is destroyed.
/// </summary>
[Serializable()]
sealed class Explosive : IGameComponent {
    
    public int explosionRadius;
    
    public Explosive(int explosionRadius) {
        this.explosionRadius = explosionRadius;
    }
    
    /// <summary>
    /// Reacts on ObjectDestroyedEvent.
    /// </summary>
    public void OnEvent(IGameEvent gameEvent) {
        // Implementation omitted.
    }
}


// Game objects


/// <summary>
/// This is a more specialized game object interface for two-dimensional game
/// worlds.
/// </summary>
interface IGameObject2D : IGameObject {
    
    /// <returns>
    /// Transform2D attached to this game object.
    /// </returns>
    Transform2D GetTransform();
}


/// <summary>
/// This is an implementation of static game object which is tied to the 
/// two-dimensional game scene.
/// Instantiating this object automatically places it in the current scene.
/// </summary>
[Serializable()]
sealed class GameObject2D : IGameObject2D {
    
    private Transform2D transform;
    private Dictionary<Type, IGameComponent> components;
        
    /// <summary>
    /// Creates a new two-dimensional game object with the default position
    /// (0,0).
    /// </summary>
    public GameObject2D() {
        this.transform = new Transform2D();
        this.components = new Dictionary<Type, IGameComponent>();
        
        components.Add(transform.GetType(), transform);
    }
    
    public Transform2D GetTransform() {
        return transform;
    }
    
    public void AddComponent(IGameComponent gameComponent) {
        // Implementation omitted.
    }
    
    public void RemoveComponent(IGameComponent gameComponent) {
        // Implementation omitted.
    }
    
    public IEnumerable<IGameComponent> GetComponents() {
        // Implementation omitted.
        return Enumerable.Empty<IGameComponent>();
    }
    
    public T GetComponent<T>() where T : IGameComponent  {
        // Implementation omitted.
        return default(T);
    }
    
    public void SendEvent(IGameEvent gameEvent) {
        // Implementation omitted.
    }
}


// Game scene


/// <summary> 
/// This is sample partial implementation which uses range tree and multimap to
/// store game objects.
/// </summary>
sealed class Scene2D : IScene2D {
    
    private HashSet<IGameObject2D> objects;
    private RangeTree<Point2D, IGameObject2D> grid;
    private MultiMap<Type, IGameComponent> components;
    
    public void Add(IGameObject2D gameObject) {
        // Implementation omitted.
    }
    
    public void Remove(IGameObject2D gameObject) {
        // Implementation omitted.        
    }
    
    public IEnumerable<IGameObject2D> GetAllGameObjects() {
        // Implementation omitted.      
        return Enumerable.Empty<IGameObject2D>();
        
    }
    
    public IEnumerable<T> GetComponentsOf<T>() where T : IGameComponent {
        // Implementation omitted.      
        return Enumerable.Empty<T>();
    }
    
    public IEnumerable<IGameObject2D> FindGameObjectsIn(Point2D start, Point2D end) {
        // Implementation omitted.      
        return Enumerable.Empty<IGameObject2D>();        
    }
    
    public IEnumerable<IGameObject2D> FindGameObjectsAround(IGameObject2D target, int radius) {
        // Implementation omitted.      
        return Enumerable.Empty<IGameObject2D>();        
    }
}
