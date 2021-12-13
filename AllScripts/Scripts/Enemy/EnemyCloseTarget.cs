using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SphereCollider))]
public class EnemyCloseTarget : MonoBehaviour {

    private EnemyAI enemyAI;
    private EnemyAIAnim enemyAIAnim;
    private GameObject enemy;
   

	void Start ()
    {
        GetComponent<SphereCollider>().radius = 8;
        enemy = transform.parent.gameObject;
        Debug.Log(enemy);
        enemyAI = enemy.GetComponent<EnemyAI>();
        GetComponent<SphereCollider>().isTrigger = true;

	}
	
    void OnTriggerEnter (Collider other)
    {
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Ally")
        {
            Debug.Log(other.gameObject);
            GameObject currentTarget = enemyAI.player;
            float distanceCurrentTarget = Vector3.Distance(enemy.transform.position, currentTarget.transform.position);
            float distanceOther = Vector3.Distance(enemy.transform.position, other.gameObject.transform.position);
            if (distanceCurrentTarget > distanceOther)
            {
                enemyAI.player = other.gameObject;
                enemy.GetComponent<EnemyAttack>().target = other.gameObject;
            }
    //        Debug.Log(distanceCurrentTarget + "/" + distanceOther);

        }
    }
}
