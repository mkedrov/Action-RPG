using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderingAI : MonoBehaviour
{
    public float speed = 3.0f;
    public float obstacleRange = 5.0f;
    private bool _alive;
    [SerializeField] private GameObject fireballPrefab;
    private GameObject _fireball;
    public const float baseSpeed = 3.0f;
    
    private Animator _animator; // animator for enemy human character's model
    
    // Start is called before the first frame update
    void Start()
    {
        _alive = true;
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_alive)
        {
            transform.Translate(0,0,speed * Time.deltaTime);
            _animator.SetFloat("Speed", speed);
        
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hit;
            if (Physics.SphereCast(ray, 0.75f, out hit))
            {
                GameObject hitObject = hit.transform.gameObject;
                if (hitObject.GetComponent<PlayerCharacter>())
                {
                    if (_fireball == null)
                    {
                        _fireball = Instantiate(fireballPrefab) as GameObject;
                        _fireball.transform.position = transform.TransformPoint(Vector3.forward * 1.5f);
                        _fireball.transform.rotation = transform.rotation;
                    }
                
                    
                }
                else if (hit.distance < obstacleRange)
                {
                    float angle = Random.Range(-110, 110);
                    transform.Rotate(0, angle, 0);
                }
            
            }
        }
        
    }
    
    private void OnSpeedChanged(float value)
    {
        speed = baseSpeed;
    }
    
    public void SetAlive(bool alive) // method for external change of _alive state
    {
        _alive = alive;
    }
}
