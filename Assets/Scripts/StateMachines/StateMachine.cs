using UnityEngine;

public class StateMachine : MonoBehaviour
{
    [field: SerializeField] public Animator Animator  { get; private set; }
    public State currentState;

    public void SwitchState(State newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        currentState?.Tick(Time.deltaTime);
    }

}
