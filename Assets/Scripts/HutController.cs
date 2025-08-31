using UnityEngine;

public class HutController : MonoBehaviour
{
    [SerializeField] private bool startOnFire;
    [SerializeField] private float timeBtwFireStates;

    public bool wasTuredOff = false;
    
    private int currentFireLevel = 0;  // 0 es apagado
    private float currentTimeBtwFireStates;

    private string[] animationStates = { "Idle_Hut", "Burn_One_Hut", "Burn_Two_Hut", "Burned_Hut"};
     
    private Animator anim;

    public HutState CurrentHutState;
    
    private void Start()
    {
        anim = GetComponent<Animator>();
        
        if (startOnFire)
        {
            CurrentHutState = HutState.Burning;
            currentFireLevel = 1;
        }
        
        ChangeFireState(currentFireLevel);
    }

    private void Update()
    {
        if (CurrentHutState == HutState.Burning)
        {
            currentTimeBtwFireStates += Time.deltaTime;
            int newFireLevel = currentFireLevel + 1;
            if (currentTimeBtwFireStates >= timeBtwFireStates) ChangeFireState(newFireLevel);
        }
    }

    private void ChangeFireState(int newFireLevel)
    {
        currentFireLevel = newFireLevel;
        currentTimeBtwFireStates = 0;
        
        if (currentFireLevel == 0)  // Se extingue el fuego
        {
            CurrentHutState = HutState.None;
        }
        else if (currentFireLevel == 3)    // Se quema la choza completamente
        {
            CurrentHutState = HutState.Burned;
        }
        
        anim.Play(animationStates[currentFireLevel]);
    }

    public void ExtinguishFire()
    {
        if (currentFireLevel == 3 || CurrentHutState == HutState.Burned) return;

        ChangeFireState(0);

        wasTuredOff = true;
    }
}

public enum HutState
{
    None,
    Burning,
    Burned,
}