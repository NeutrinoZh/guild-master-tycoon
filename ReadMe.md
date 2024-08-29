# Guild Master Tycoon

I use Git LFS so when cloning a repository you need to run pull --force to download resources!

A total of approximately 20 hours of work time was spent on the project.

Since external libraries could not be used, I wrote my own set of utilities located in the AssemblyDefinition 'MyToolkit'. It includes the following modules:
- MyTween - a minimal custom alternative to DOTween. Automatically creates an instance of the tweener in RuntimeInitializeOnLoadMethod. Example usage:
```
    MyTween.To(
        () => _filling.fillAmount,
        value => _filling.fillAmount = value,
        duration,
        1
    );
```
- ServiceLocator - a minimal custom alternative to DI frameworks, but without reflection. Essentially, we have two service containers: a global one, which is added to DontDestroyOnLoad, and a local one, which is the context of the scene. Both are created after the launch of GameStateMachine. When retrieving a service, it is first searched for in the scene context, and if not found, we then try to find it in the application context. Example usage:
```
// Registering services in the scene context
ServiceContainer.Instance.Register(new BuildingFactory());
ServiceContainer.Instance.Register(new PlayerInput());
ServiceContainer.Instance.Register(new TableManager());

// Registering a service in the application context
ServiceContainer.Instance.RegisterGlobal(new SavesManager());

// Retrieving services
ServiceContainer.Instance.Get<PlayerStats>(),
ServiceContainer.Instance.Get<SavesManager>()
```
- NavigationGraph - for creating and finding paths on the scene. It includes features such as:
    - A custom editor for editing the graph in EditMode.
    - Graph merging (used for creating prefabs).
    - Pathfinding through the graph (uses the A* algorithm).
- SaveSystem - a system for saving data. It works with a serializer interface and provides a ready-made JSONSerializer. It also supports AES encryption.
- StateMachine - a basic implementation of a state machine. Used in GameStateMachine and in NPC AI. Example usage:
```
public class AdventurerSM : StateMachine
{
    public AdventurerSM(Transform transform) : base(new()
    {
        [typeof(WalkingState)] = new WalkingState(transform),
        [typeof(ServingState)] = new ServingState(transform),
        [typeof(StayingState)] = new StayingState(transform)
    })
    {
    }
}
```
- Timer - used for creating intervals. Automatically registered as a service in the scene context. Example:
```
ServiceContainer.Instance.SetCallback<MTKTimer>(() =>
{
    ServiceContainer.Instance.Get<MTKTimer>().StartInterval(SaveInterval, k_saveInterval);
});
```
 
The game itself consists of the following modules:

- Game.cs - Game.cs - The entry point of the program. Highest priority in ScriptOrderExecution. Creates GameStateMachine.
- GameStateMachine -  Responsible for global game state management, such as the main menu, active gameplay, pause, etc. (In fact, only BootstrapState is used. BootstrapState registering basic services.)
- Adventurer -  NPC created in AdventurersPool (uses ObjectPool). Uses StateMachine. Main states:
    - WalkingState - Represents the state where the adventurer is moving from one point to another within the navigation graph.
    - ServingState -  Represents the state where the adventurer is being serviced
    - StayingState - Represents the idle or resting state of the adventurer when they are not actively engaged in tasks.
- Guild - This module is responsible for managing departments, buildings, and individual workers within the guild.
    - Building - Created through a factory and configured via a ScriptableObject (BuildingSO).
    - Department - Configured through a ScriptableObject (DepartmentSO).
    - Table (Worker)

