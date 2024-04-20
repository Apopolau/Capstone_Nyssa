using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [SerializeField] public GameObject uiPrefab;
    [SerializeField] public Transform target;

    Transform ui;
    Image healthSlider;
    //Transform cam;

    private enum TypeObjectAttached { EARTHPLAYER, CELESTIALPLAYER, MONSTER, BUILD};
    TypeObjectAttached typeObjectAttached;

    EarthPlayer earthPlayer;
    CelestialPlayer celestialPlayer;
    Enemy enemy;
    Creatable creatable;

    WaitForSeconds uiUpdateInterval = new WaitForSeconds(1);

    // Start is called before the first frame update
    void Start()
    {
        //cam = Camera.main.transform;
        foreach(Canvas c in GetComponentsInChildren<Canvas>())
        {
            if(c.renderMode == RenderMode.WorldSpace)
            {
                ui = Instantiate(uiPrefab, c.transform).transform;
                healthSlider = ui.GetChild(0).GetComponent<Image>();
                    break;
            }
        }

        if (GetComponent<EarthPlayer>())
        {
            typeObjectAttached = TypeObjectAttached.EARTHPLAYER;
            earthPlayer = GetComponent<EarthPlayer>();
            GetComponent<EarthPlayer>().OnHealthChanged += OnHealthChanged;
        }
        else if (GetComponent<CelestialPlayer>())
        {
            typeObjectAttached = TypeObjectAttached.CELESTIALPLAYER;
            celestialPlayer = GetComponent<CelestialPlayer>();
            GetComponent<CelestialPlayer>().OnHealthChanged += OnHealthChanged;
        }
        else if (GetComponent<Enemy>())
        {
            typeObjectAttached = TypeObjectAttached.MONSTER;
            enemy = GetComponent<Enemy>();
            GetComponent<Enemy>().OnHealthChanged += OnHealthChanged;
        }
        else if (GetComponent<Plant>())
        {
            typeObjectAttached = TypeObjectAttached.BUILD;
            creatable = GetComponent<Plant>();
            GetComponent<Plant>().OnHealthChanged += OnHealthChanged;
        }
        ui.gameObject.SetActive(false);

        StartCoroutine(RegularHealthUpdate());
        
    }

    // Late Update is called once per frame
        void LateUpdate()
        {
            if(ui != null)
            {
                ui.position = target.position;
            }

        }

    private IEnumerator RegularHealthUpdate()
    {
        yield return uiUpdateInterval;
        if (typeObjectAttached == TypeObjectAttached.EARTHPLAYER)
        {
            OnHealthChanged(earthPlayer.health.max, earthPlayer.health.current);
        }
        else if (typeObjectAttached == TypeObjectAttached.CELESTIALPLAYER)
        {
            OnHealthChanged(celestialPlayer.health.max, celestialPlayer.health.current);
        }
        else if (typeObjectAttached == TypeObjectAttached.MONSTER)
        {
            OnHealthChanged(enemy.health.max, enemy.health.current);
        }
        else if (typeObjectAttached == TypeObjectAttached.BUILD)
        {
            OnHealthChanged(creatable.health.max, creatable.health.current);
        }
        
    }

    void OnHealthChanged(int maxHealth, int currentHealth)
    {
        if(ui != null)
        {
            if(currentHealth != maxHealth)
            {
                ui.gameObject.SetActive(true);

                float healthPercent = (float)currentHealth / maxHealth;
                healthSlider.fillAmount = healthPercent;
                /*
                if (currentHealth <= 0)
                {
                    Destroy(ui.gameObject);
                }
                */
            }
            else if(currentHealth == maxHealth)
            {
                ui.gameObject.SetActive(false);
            }
        }
    }
}
