using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class Bullet_Movement : MonoBehaviour
{

    //list of bullets and its dmg
    /*struct ProjectileDmg{
        int bulletDmg = 2; 
    }*/

    private HealthManager _HealthManager;

    //public Text debugtext;
    private GameObject Bg;
    private int num_bg;

    private GameObject indicator;
    private Vector3 bg_world_size;
    private float object_max_height;

    private Vector3 bullet_world_size;

    private Camera cam;
    private float cam_height; // Size of camera in y

    private GameObject Bullet_Rest;

    private PauseScript _pauseScript;

    public float bullet_speed_original = 0.1f;
    public float bullet_speed = 0.1f;
    public bool bullet_follow = false;

    public bool bullet_burstshot = false;
    public bool bullet_ghost = false;
    public GameObject ghost_owner;

    // Use this for initialization
    void Start()
    {
        //pass by reference from pauseScript
        _pauseScript = GameObject.Find("Scripts").GetComponent<PauseScript>();
        //pass by reference from health Manaager
        _HealthManager = GameObject.Find("Scripts").GetComponent<HealthManager>();

        Bg = GameObject.FindGameObjectWithTag("Background");
        num_bg = GameObject.FindGameObjectsWithTag("Background").Length;

        Vector2 sprite_size = Bg.GetComponent<SpriteRenderer>().sprite.rect.size;
        Vector2 local_sprite_size = sprite_size / Bg.GetComponent<SpriteRenderer>().sprite.pixelsPerUnit;
        bg_world_size = local_sprite_size;
        bg_world_size.x *= Bg.transform.lossyScale.x;
        bg_world_size.y *= Bg.transform.lossyScale.y;

        // Get Camera
        cam = Camera.main;
        cam_height = 2f * cam.orthographicSize;

        object_max_height = (bg_world_size.y * num_bg) - (cam_height / 2);

        Bullet_Rest = GameObject.Find("Bullet_Rest");

        sprite_size = this.GetComponent<SpriteRenderer>().sprite.rect.size;
        local_sprite_size = sprite_size / this.GetComponent<SpriteRenderer>().sprite.pixelsPerUnit;
        bullet_world_size = local_sprite_size;
        bullet_world_size.x *= this.transform.lossyScale.x;
        bullet_world_size.y *= this.transform.lossyScale.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (_pauseScript.Paused == false)
        {
            //Bullet movement, Delete if outside of background (Y axis)
            if (gameObject.CompareTag("Bullet_1"))
            {
                gameObject.transform.localPosition += Vector3.up * bullet_speed;

                // "Destroy" by placing them back to bullet_rest gameobject
                if (gameObject.transform.position.y > object_max_height)
                {
                    gameObject.tag = "Bullet_Rest";
                    gameObject.transform.position = Bullet_Rest.transform.position;
                    gameObject.transform.SetParent(Bullet_Rest.transform);
                }
            }
            else if (gameObject.CompareTag("Bullet_2"))
            {
                gameObject.transform.localPosition += Vector3.down * bullet_speed;

                // "Destroy" by placing them back to bullet_rest gameobject
                if (gameObject.transform.position.y < (-cam_height / 2))
                {
                    gameObject.tag = "Bullet_Rest";
                    gameObject.transform.position = Bullet_Rest.transform.position;
                    gameObject.transform.SetParent(Bullet_Rest.transform);
                }
            }
            else if (gameObject.CompareTag("Bullet_E"))
            {
                gameObject.transform.localPosition += Vector3.down * bullet_speed;

                // "Destroy" by placing them back to bullet_rest gameobject
                if (gameObject.transform.position.y < (-cam_height / 2))
                {
                    gameObject.tag = "Bullet_Rest";
                    gameObject.transform.position = Bullet_Rest.transform.position;
                    gameObject.transform.SetParent(Bullet_Rest.transform);
                }
            }
            else if (gameObject.CompareTag("Bullet_Ghost_Left") || gameObject.CompareTag("Bullet_Ghost_Right"))
            {
                if (ghost_owner != null)
                {
                    if (ghost_owner.CompareTag("Bullet_1"))
                    {
                        if (this.CompareTag("Bullet_Ghost_Left"))
                        {
                            gameObject.transform.position = new Vector3(ghost_owner.transform.position.x + (bullet_world_size.x / 2), ghost_owner.transform.position.y + bullet_world_size.y, this.transform.position.z);

                        }
                        else
                        {
                            gameObject.transform.position = new Vector3(ghost_owner.transform.position.x - (bullet_world_size.x / 2), ghost_owner.transform.position.y + bullet_world_size.y, this.transform.position.z);
                        }

                        // "Destroy" by placing them back to bullet_rest gameobject
                        if (gameObject.transform.position.y > object_max_height)
                        {
                            gameObject.tag = "Bullet_Rest";
                            gameObject.transform.position = Bullet_Rest.transform.position;
                            gameObject.transform.SetParent(Bullet_Rest.transform);
                        }
                    }
                    else
                    {
                        if (this.CompareTag("Bullet_Ghost_Left"))
                        {
                            gameObject.transform.position = new Vector3(ghost_owner.transform.position.x + (bullet_world_size.x / 2), ghost_owner.transform.position.y - bullet_world_size.y, this.transform.position.z);
                        }
                        else
                        {
                            gameObject.transform.position = new Vector3(ghost_owner.transform.position.x - (bullet_world_size.x / 2), ghost_owner.transform.position.y - bullet_world_size.y, this.transform.position.z);
                        }

                        // "Destroy" by placing them back to bullet_rest gameobject
                        if (gameObject.transform.position.y < (-cam_height / 2))
                        {
                            gameObject.tag = "Bullet_Rest";
                            gameObject.transform.position = Bullet_Rest.transform.position;
                            gameObject.transform.SetParent(Bullet_Rest.transform);
                        }
                    }
                }
            }
        }
    }

    // "Destroy" by placing them back to bullet_rest gameobject
    void OnTriggerEnter2D(Collider2D coll)
    {
        //bullet 1
        if (coll.gameObject.CompareTag("Bullet_1") && !this.CompareTag("Bullet_1") && !this.CompareTag("Barrier_P1"))
        {
            GameObject Bullet_Destroy_1 = GameObject.FindGameObjectWithTag("Bullet_Effect_Stop");
            Bullet_Destroy_1.tag = "Bullet_Effect_Play";
            Bullet_Destroy_1.transform.SetParent(null);
            Bullet_Destroy_1.transform.position = gameObject.transform.position;
            Bullet_Destroy_1.transform.rotation = gameObject.transform.rotation;
            Bullet_Destroy_1.GetComponent<Animator>().SetBool("Destroy", true);

            if (gameObject.CompareTag("Bullet_E"))
            {
                gameObject.tag = "Bullet_Rest_E";
            }
            else if (gameObject.CompareTag("Bullet_2"))
            {
                gameObject.tag = "Bullet_Rest_2";
            }
            else if (gameObject.CompareTag("Bullet_Ghost_Left") || gameObject.CompareTag("Bullet_Ghost_Right"))
            {
                if (ghost_owner.CompareTag("Bullet_1"))
                {
                    gameObject.tag = "Bullet_Rest";
                    bullet_ghost = false;
                    ghost_owner = null;
                }
                else
                {
                    gameObject.tag = "Bullet_Rest_2";
                    bullet_ghost = false;
                    ghost_owner = null;
                }
            }
            else
            {
                gameObject.tag = "Bullet_Rest";
            }
            DestroyGhostBullet(gameObject);
            ResetVarible(gameObject);

            GameObject Bullet_Destroy_2 = GameObject.FindGameObjectWithTag("Bullet_Effect_Stop");
            Bullet_Destroy_2.tag = "Bullet_Effect_Play";
            Bullet_Destroy_1.transform.SetParent(null);
            Bullet_Destroy_2.transform.position = coll.gameObject.transform.position;
            Bullet_Destroy_2.transform.rotation = coll.gameObject.transform.rotation;
            Bullet_Destroy_2.GetComponent<Animator>().SetBool("Destroy", true);

            if (coll.gameObject.CompareTag("Bullet_E"))
            {
                coll.gameObject.tag = "Bullet_Rest_E";
            }
            else if (coll.gameObject.CompareTag("Bullet_2"))
            {
                coll.gameObject.tag = "Bullet_Rest_2";
            }
            else if (coll.gameObject.CompareTag("Bullet_Ghost_Left") || coll.gameObject.CompareTag("Bullet_Ghost_Right"))
            {
                if (ghost_owner.CompareTag("Bullet_1"))
                {
                    coll.gameObject.tag = "Bullet_Rest";
                    coll.GetComponent<Bullet_Movement>().bullet_ghost = false;
                    coll.GetComponent<Bullet_Movement>().ghost_owner = null;
                }
                else
                {
                    coll.gameObject.tag = "Bullet_Rest_2";
                    coll.GetComponent<Bullet_Movement>().bullet_ghost = false;
                    coll.GetComponent<Bullet_Movement>().ghost_owner = null;
                }
            }
            else
            {
                coll.gameObject.tag = "Bullet_Rest";
            }
            DestroyGhostBullet(coll.gameObject);
            ResetVarible(coll.gameObject);
        }
        //bullet 2
        if (coll.gameObject.CompareTag("Bullet_2") && !this.CompareTag("Bullet_2") && !this.CompareTag("Barrier_P2"))
        {
            GameObject Bullet_Destroy_1 = GameObject.FindGameObjectWithTag("Bullet_Effect_Stop");
            Bullet_Destroy_1.tag = "Bullet_Effect_Play";
            Bullet_Destroy_1.transform.SetParent(null);
            Bullet_Destroy_1.transform.position = gameObject.transform.position;
            Bullet_Destroy_1.transform.rotation = gameObject.transform.rotation;
            Bullet_Destroy_1.GetComponent<Animator>().SetBool("Destroy", true);

            if (gameObject.CompareTag("Bullet_E"))
            {
                gameObject.tag = "Bullet_Rest_E";
            }
            else if (gameObject.CompareTag("Bullet_2"))
            {
                gameObject.tag = "Bullet_Rest_2";
            }
            else if (gameObject.CompareTag("Bullet_Ghost_Left") || gameObject.CompareTag("Bullet_Ghost_Right"))
            {
                if (ghost_owner.CompareTag("Bullet_1"))
                {
                    gameObject.tag = "Bullet_Rest";
                    bullet_ghost = false;
                    ghost_owner = null;
                }
                else
                {
                    gameObject.tag = "Bullet_Rest_2";
                    bullet_ghost = false;
                    ghost_owner = null;
                }
            }
            else
            {
                gameObject.tag = "Bullet_Rest";
            }
            DestroyGhostBullet(gameObject);
            ResetVarible(gameObject);

            GameObject Bullet_Destroy_2 = GameObject.FindGameObjectWithTag("Bullet_Effect_Stop");
            Bullet_Destroy_2.tag = "Bullet_Effect_Play";
            Bullet_Destroy_1.transform.SetParent(null);
            Bullet_Destroy_2.transform.position = coll.gameObject.transform.position;
            Bullet_Destroy_2.transform.rotation = coll.gameObject.transform.rotation;
            Bullet_Destroy_2.GetComponent<Animator>().SetBool("Destroy", true);

            if (coll.gameObject.CompareTag("Bullet_E"))
            {
                coll.gameObject.tag = "Bullet_Rest_E";
            }
            else if (coll.gameObject.CompareTag("Bullet_2"))
            {
                coll.gameObject.tag = "Bullet_Rest_2";
            }
            else if (coll.gameObject.CompareTag("Bullet_Ghost_Left") || coll.gameObject.CompareTag("Bullet_Ghost_Right"))
            {
                if (ghost_owner.CompareTag("Bullet_1"))
                {
                    coll.gameObject.tag = "Bullet_Rest";
                    coll.GetComponent<Bullet_Movement>().bullet_ghost = false;
                    coll.GetComponent<Bullet_Movement>().ghost_owner = null;
                }
                else
                {
                    coll.gameObject.tag = "Bullet_Rest_2";
                    coll.GetComponent<Bullet_Movement>().bullet_ghost = false;
                    coll.GetComponent<Bullet_Movement>().ghost_owner = null;
                }
            }
            else
            {
                coll.gameObject.tag = "Bullet_Rest";
            }
            DestroyGhostBullet(coll.gameObject);
            ResetVarible(coll.gameObject);
        }
        //enemy's bullet
        if (coll.gameObject.CompareTag("Bullet_E") && !this.CompareTag("Bullet_E"))
        {
            GameObject Bullet_Destroy_1 = GameObject.FindGameObjectWithTag("Bullet_Effect_Stop");
            Bullet_Destroy_1.tag = "Bullet_Effect_Play";
            Bullet_Destroy_1.transform.SetParent(null);
            Bullet_Destroy_1.transform.position = gameObject.transform.position;
            Bullet_Destroy_1.transform.rotation = gameObject.transform.rotation;
            Bullet_Destroy_1.GetComponent<Animator>().SetBool("Destroy", true);

            if (gameObject.CompareTag("Bullet_E"))
            {
                gameObject.tag = "Bullet_Rest_E";
            }
            else if (gameObject.CompareTag("Bullet_2"))
            {
                gameObject.tag = "Bullet_Rest_2";
            }
            else if (gameObject.CompareTag("Bullet_Ghost_Left") || gameObject.CompareTag("Bullet_Ghost_Right"))
            {
                if (ghost_owner.CompareTag("Bullet_1"))
                {
                    gameObject.tag = "Bullet_Rest";
                    bullet_ghost = false;
                    ghost_owner = null;
                }
                else
                {
                    gameObject.tag = "Bullet_Rest_2";
                    bullet_ghost = false;
                    ghost_owner = null;
                }
            }
            else
            {
                gameObject.tag = "Bullet_Rest";
            }
            DestroyGhostBullet(gameObject);
            ResetVarible(gameObject);

            GameObject Bullet_Destroy_2 = GameObject.FindGameObjectWithTag("Bullet_Effect_Stop");
            Bullet_Destroy_2.tag = "Bullet_Effect_Play";
            Bullet_Destroy_1.transform.SetParent(null);
            Bullet_Destroy_2.transform.position = coll.gameObject.transform.position;
            Bullet_Destroy_2.transform.rotation = coll.gameObject.transform.rotation;
            Bullet_Destroy_2.GetComponent<Animator>().SetBool("Destroy", true);

            if (coll.gameObject.CompareTag("Bullet_E"))
            {
                coll.gameObject.tag = "Bullet_Rest_E";
            }
            else if (coll.gameObject.CompareTag("Bullet_2"))
            {
                coll.gameObject.tag = "Bullet_Rest_2";
            }
            else if (coll.gameObject.CompareTag("Bullet_Ghost_Left") || coll.gameObject.CompareTag("Bullet_Ghost_Right"))
            {
                if (ghost_owner.CompareTag("Bullet_1"))
                {
                    coll.gameObject.tag = "Bullet_Rest";
                    coll.GetComponent<Bullet_Movement>().bullet_ghost = false;
                    coll.GetComponent<Bullet_Movement>().ghost_owner = null;
                }
                else
                {
                    coll.gameObject.tag = "Bullet_Rest_2";
                    coll.GetComponent<Bullet_Movement>().bullet_ghost = false;
                    coll.GetComponent<Bullet_Movement>().ghost_owner = null;
                }
            }
            else
            {
                coll.gameObject.tag = "Bullet_Rest";
            }
            DestroyGhostBullet(coll.gameObject);
            ResetVarible(coll.gameObject);
        }
        //wall 1
        if (coll.gameObject.CompareTag("player1_wall"))
        {
            _HealthManager.objHealth = HealthManager.ObjectsHealth.wall1;
            _HealthManager.SendMessage("ApplyDamage", 1); //damage done

            GameObject Bullet_Destroy_1 = GameObject.FindGameObjectWithTag("Bullet_Effect_Stop");
            Bullet_Destroy_1.tag = "Bullet_Effect_Play";
            Bullet_Destroy_1.transform.SetParent(null);
            Bullet_Destroy_1.transform.position = gameObject.transform.position;
            Bullet_Destroy_1.transform.rotation = gameObject.transform.rotation;
            Bullet_Destroy_1.GetComponent<Animator>().SetBool("Destroy", true);

            if (gameObject.CompareTag("Bullet_E"))
            {
                gameObject.tag = "Bullet_Rest_E";
            }
            else if (gameObject.CompareTag("Bullet_2"))
            {
                gameObject.tag = "Bullet_Rest_2";
            }
            else if (gameObject.CompareTag("Bullet_Ghost_Left") || gameObject.CompareTag("Bullet_Ghost_Right"))
            {
                if (ghost_owner.CompareTag("Bullet_1"))
                {
                    gameObject.tag = "Bullet_Rest";
                    bullet_ghost = false;
                    ghost_owner = null;
                }
                else
                {
                    gameObject.tag = "Bullet_Rest_2";
                    bullet_ghost = false;
                    ghost_owner = null;
                }
            }
            else
            {
                gameObject.tag = "Bullet_Rest";
            }
            DestroyGhostBullet(gameObject);
            ResetVarible(gameObject);
        }
        //wall 2
        if (coll.gameObject.CompareTag("player2_wall"))
        {
            _HealthManager.objHealth = HealthManager.ObjectsHealth.wall2;
            _HealthManager.SendMessage("ApplyDamage", 1); //damage done

            GameObject Bullet_Destroy_1 = GameObject.FindGameObjectWithTag("Bullet_Effect_Stop");
            Bullet_Destroy_1.tag = "Bullet_Effect_Play";
            Bullet_Destroy_1.transform.SetParent(null);
            Bullet_Destroy_1.transform.position = gameObject.transform.position;
            Bullet_Destroy_1.transform.rotation = gameObject.transform.rotation;
            Bullet_Destroy_1.GetComponent<Animator>().SetBool("Destroy", true);

            if (gameObject.CompareTag("Bullet_E"))
            {
                gameObject.tag = "Bullet_Rest_E";
            }
            else if (gameObject.CompareTag("Bullet_2"))
            {
                gameObject.tag = "Bullet_Rest_2";
            }
            else if (gameObject.CompareTag("Bullet_Ghost_Left") || gameObject.CompareTag("Bullet_Ghost_Right"))
            {
                if (ghost_owner.CompareTag("Bullet_1"))
                {
                    gameObject.tag = "Bullet_Rest";
                    bullet_ghost = false;
                    ghost_owner = null;
                }
                else
                {
                    gameObject.tag = "Bullet_Rest_2";
                    bullet_ghost = false;
                    ghost_owner = null;
                }
            }
            else
            {
                gameObject.tag = "Bullet_Rest";
            }
            DestroyGhostBullet(gameObject);
            ResetVarible(gameObject);
        }
        //wall enemy
        if (coll.gameObject.CompareTag("enemy_wall"))
        {
            //_HealthManager.objHealth = HealthManager.ObjectsHealth.wall2;
            //_HealthManager.SendMessage("ApplyDamage", 1); //damage done

            GameObject Bullet_Destroy_1 = GameObject.FindGameObjectWithTag("Bullet_Effect_Stop");
            Bullet_Destroy_1.tag = "Bullet_Effect_Play";
            Bullet_Destroy_1.transform.SetParent(null);
            Bullet_Destroy_1.transform.position = gameObject.transform.position;
            Bullet_Destroy_1.transform.rotation = gameObject.transform.rotation;
            Bullet_Destroy_1.GetComponent<Animator>().SetBool("Destroy", true);

            if (gameObject.CompareTag("Bullet_E"))
            {
                gameObject.tag = "Bullet_Rest_E";
            }
            else if (gameObject.CompareTag("Bullet_2"))
            {
                gameObject.tag = "Bullet_Rest_2";
            }
            else if (gameObject.CompareTag("Bullet_Ghost_Left") || gameObject.CompareTag("Bullet_Ghost_Right"))
            {
                if (ghost_owner.CompareTag("Bullet_1"))
                {
                    gameObject.tag = "Bullet_Rest";
                    bullet_ghost = false;
                    ghost_owner = null;
                }
                else
                {
                    gameObject.tag = "Bullet_Rest_2";
                    bullet_ghost = false;
                    ghost_owner = null;
                }
            }
            else
            {
                gameObject.tag = "Bullet_Rest";
            }
            DestroyGhostBullet(gameObject);
            ResetVarible(gameObject);
        }
        //player 1
        if (coll.gameObject.CompareTag("Player1") && !this.CompareTag("Bullet_1"))
        {
            _HealthManager.objHealth = HealthManager.ObjectsHealth.player1;
            _HealthManager.SendMessage("ApplyDamage", 2); //damage done

            GameObject Bullet_Destroy_1 = GameObject.FindGameObjectWithTag("Bullet_Effect_Stop");
            Bullet_Destroy_1.tag = "Bullet_Effect_Play";
            Bullet_Destroy_1.transform.SetParent(null);
            Bullet_Destroy_1.transform.position = gameObject.transform.position;
            Bullet_Destroy_1.transform.rotation = gameObject.transform.rotation;
            Bullet_Destroy_1.GetComponent<Animator>().SetBool("Destroy", true);

            if (gameObject.CompareTag("Bullet_E"))
            {
                gameObject.tag = "Bullet_Rest_E";
            }
            else if (gameObject.CompareTag("Bullet_2"))
            {
                gameObject.tag = "Bullet_Rest_2";
            }
            else if (gameObject.CompareTag("Bullet_Ghost_Left") || gameObject.CompareTag("Bullet_Ghost_Right"))
            {
                if (ghost_owner.CompareTag("Bullet_1"))
                {
                    gameObject.tag = "Bullet_Rest";
                    bullet_ghost = false;
                    ghost_owner = null;
                }
                else
                {
                    gameObject.tag = "Bullet_Rest_2";
                    bullet_ghost = false;
                    ghost_owner = null;
                }
            }
            else
            {
                gameObject.tag = "Bullet_Rest";
            }
            DestroyGhostBullet(gameObject);
            ResetVarible(gameObject);
        }
        //player 2
        if (coll.gameObject.CompareTag("Player2") && !this.CompareTag("Bullet_2"))
        {
            _HealthManager.objHealth = HealthManager.ObjectsHealth.player2;
            _HealthManager.SendMessage("ApplyDamage", 2); //damage done

            GameObject Bullet_Destroy_1 = GameObject.FindGameObjectWithTag("Bullet_Effect_Stop");
            Bullet_Destroy_1.tag = "Bullet_Effect_Play";
            Bullet_Destroy_1.transform.SetParent(null);
            Bullet_Destroy_1.transform.position = gameObject.transform.position;
            Bullet_Destroy_1.transform.rotation = gameObject.transform.rotation;
            Bullet_Destroy_1.GetComponent<Animator>().SetBool("Destroy", true);

            if (gameObject.CompareTag("Bullet_E"))
            {
                gameObject.tag = "Bullet_Rest_E";
            }
            else if (gameObject.CompareTag("Bullet_2"))
            {
                gameObject.tag = "Bullet_Rest_2";
            }
            else if (gameObject.CompareTag("Bullet_Ghost_Left") || gameObject.CompareTag("Bullet_Ghost_Right"))
            {
                if (ghost_owner.CompareTag("Bullet_1"))
                {
                    gameObject.tag = "Bullet_Rest";
                    bullet_ghost = false;
                    ghost_owner = null;
                }
                else
                {
                    gameObject.tag = "Bullet_Rest_2";
                    bullet_ghost = false;
                    ghost_owner = null;
                }
            }
            else
            {
                gameObject.tag = "Bullet_Rest";
            }
            DestroyGhostBullet(gameObject);
            ResetVarible(gameObject);
        }
        //Enemy
        if (coll.gameObject.CompareTag("Enemy") && !this.CompareTag("Bullet_E"))
        {
            //_HealthManager.objHealth = HealthManager.ObjectsHealth.player2;
            //_HealthManager.SendMessage("ApplyDamage", 2); //damage done
            GameObject Bullet_Destroy_1 = GameObject.FindGameObjectWithTag("Bullet_Effect_Stop");
            Bullet_Destroy_1.tag = "Bullet_Effect_Play";
            Bullet_Destroy_1.transform.SetParent(null);
            Bullet_Destroy_1.transform.position = gameObject.transform.position;
            Bullet_Destroy_1.transform.rotation = gameObject.transform.rotation;
            Bullet_Destroy_1.GetComponent<Animator>().SetBool("Destroy", true);

            if (gameObject.CompareTag("Bullet_E"))
            {
                gameObject.tag = "Bullet_Rest_E";
            }
            else if (gameObject.CompareTag("Bullet_2"))
            {
                gameObject.tag = "Bullet_Rest_2";
            }
            else if (gameObject.CompareTag("Bullet_Ghost_Left") || gameObject.CompareTag("Bullet_Ghost_Right"))
            {
                if (ghost_owner.CompareTag("Bullet_1"))
                {
                    gameObject.tag = "Bullet_Rest";
                    bullet_ghost = false;
                    ghost_owner = null;
                }
                else
                {
                    gameObject.tag = "Bullet_Rest_2";
                    bullet_ghost = false;
                    ghost_owner = null;
                }
            }
            else
            {
                gameObject.tag = "Bullet_Rest";
            }
            DestroyGhostBullet(gameObject);
            ResetVarible(gameObject);
        }
        //obstacle 1
        if (coll.gameObject.CompareTag("Obstacle"))
        {

            if (coll.gameObject.name == "OBSTY TEST")
            {
                _HealthManager.objHealth = HealthManager.ObjectsHealth.obstacle1;

                _HealthManager.SendMessage("ApplyDamage", 1); //damage done

                if (_HealthManager.obstacle1_health <= 0)
                    Destroy(coll.gameObject);

            }
            else if (coll.gameObject.name == "OBSTY TEST 2")
            {
                _HealthManager.objHealth = HealthManager.ObjectsHealth.obstacle2;

                _HealthManager.SendMessage("ApplyDamage", 1); //damage done

                if (_HealthManager.obstacle2_health <= 0)
                    Destroy(coll.gameObject);

            }
            else if (coll.gameObject.name == "OBSTY TEST 3")
            {
                _HealthManager.objHealth = HealthManager.ObjectsHealth.obstacle3;

                _HealthManager.SendMessage("ApplyDamage", 1); //damage done

                if (_HealthManager.obstacle3_health <= 0)
                    Destroy(coll.gameObject);

            }
            else if (coll.gameObject.name == "OBSTY TEST 4")
            {
                _HealthManager.objHealth = HealthManager.ObjectsHealth.obstacle4;

                _HealthManager.SendMessage("ApplyDamage", 1); //damage done

                if (_HealthManager.obstacle4_health <= 0)
                    Destroy(coll.gameObject);
            }

            GameObject Bullet_Destroy_1 = GameObject.FindGameObjectWithTag("Bullet_Effect_Stop");
            Bullet_Destroy_1.tag = "Bullet_Effect_Play";
            Bullet_Destroy_1.transform.SetParent(null);
            Bullet_Destroy_1.transform.position = gameObject.transform.position;
            Bullet_Destroy_1.transform.rotation = gameObject.transform.rotation;
            Bullet_Destroy_1.GetComponent<Animator>().SetBool("Destroy", true);

            if (gameObject.CompareTag("Bullet_E"))
            {
                gameObject.tag = "Bullet_Rest_E";
            }
            else if (gameObject.CompareTag("Bullet_2"))
            {
                gameObject.tag = "Bullet_Rest_2";
            }
            else if (gameObject.CompareTag("Bullet_Ghost_Left") || gameObject.CompareTag("Bullet_Ghost_Right"))
            {
                if (ghost_owner.CompareTag("Bullet_1"))
                {
                    gameObject.tag = "Bullet_Rest";
                    bullet_ghost = false;
                    ghost_owner = null;
                }
                else
                {
                    gameObject.tag = "Bullet_Rest_2";
                    bullet_ghost = false;
                    ghost_owner = null;
                }
            }
            else
            {
                gameObject.tag = "Bullet_Rest";
            }
            DestroyGhostBullet(gameObject);
            ResetVarible(gameObject);
        }

        if (coll.CompareTag("Barrier_P1") && !this.CompareTag("Bullet_1"))
        {
            GameObject Bullet_Destroy_1 = GameObject.FindGameObjectWithTag("Bullet_Effect_Stop");
            Bullet_Destroy_1.tag = "Bullet_Effect_Play";
            Bullet_Destroy_1.transform.SetParent(null);
            Bullet_Destroy_1.transform.position = gameObject.transform.position;
            Bullet_Destroy_1.transform.rotation = gameObject.transform.rotation;
            Bullet_Destroy_1.GetComponent<Animator>().SetBool("Destroy", true);

            if (gameObject.CompareTag("Bullet_E"))
            {
                gameObject.tag = "Bullet_Rest_E";
            }
            else if (gameObject.CompareTag("Bullet_2"))
            {
                gameObject.tag = "Bullet_Rest_2";
            }
            else if (gameObject.CompareTag("Bullet_Ghost_Left") || gameObject.CompareTag("Bullet_Ghost_Right"))
            {
                if (ghost_owner.CompareTag("Bullet_1"))
                {
                    gameObject.tag = "Bullet_Rest";
                    bullet_ghost = false;
                    ghost_owner = null;
                }
                else
                {
                    gameObject.tag = "Bullet_Rest_2";
                    bullet_ghost = false;
                    ghost_owner = null;
                }
            }
            else
            {
                gameObject.tag = "Bullet_Rest";
            }
            DestroyGhostBullet(gameObject);
            ResetVarible(gameObject);
        }

        if (coll.CompareTag("Barrier_P2") && !this.CompareTag("Bullet_2"))
        {
            GameObject Bullet_Destroy_1 = GameObject.FindGameObjectWithTag("Bullet_Effect_Stop");
            Bullet_Destroy_1.tag = "Bullet_Effect_Play";
            Bullet_Destroy_1.transform.SetParent(null);
            Bullet_Destroy_1.transform.position = gameObject.transform.position;
            Bullet_Destroy_1.transform.rotation = gameObject.transform.rotation;
            Bullet_Destroy_1.GetComponent<Animator>().SetBool("Destroy", true);

            if (gameObject.CompareTag("Bullet_E"))
            {
                gameObject.tag = "Bullet_Rest_E";
            }
            else if (gameObject.CompareTag("Bullet_2"))
            {
                gameObject.tag = "Bullet_Rest_2";
            }
            else if (gameObject.CompareTag("Bullet_Ghost_Left") || gameObject.CompareTag("Bullet_Ghost_Right"))
            {
                if (ghost_owner.CompareTag("Bullet_1"))
                {
                    gameObject.tag = "Bullet_Rest";
                    bullet_ghost = false;
                    ghost_owner = null;
                }
                else
                {
                    gameObject.tag = "Bullet_Rest_2";
                    bullet_ghost = false;
                    ghost_owner = null;
                }
            }
            else
            {
                gameObject.tag = "Bullet_Rest";
            }
            DestroyGhostBullet(gameObject);
            ResetVarible(gameObject);
        }
    }

    void ResetVarible(GameObject Rest_Bullet)
    {
        if (Bullet_Rest == null)
        {
            Bullet_Rest = GameObject.Find("Bullet_Rest");
        }
        Rest_Bullet.transform.position = Bullet_Rest.transform.position;
        Rest_Bullet.transform.SetParent(Bullet_Rest.transform);

        Bullet_Movement Bullet_Script = Rest_Bullet.GetComponent<Bullet_Movement>();
        Bullet_Script.bullet_burstshot = false;
        Bullet_Script.bullet_follow = false;
        Bullet_Script.bullet_speed = bullet_speed_original;
        Bullet_Script.bullet_ghost = false;
        Bullet_Script.ghost_owner = null;
    }

    void DestroyGhostBullet(GameObject Bullet_Owner)
    {
        if (bullet_burstshot)
        {
            bullet_burstshot = false;

            GameObject[] ghost_bullet = GameObject.FindGameObjectsWithTag("Bullet_Ghost_Left");
            if (ghost_bullet.Length > 0)
            {
                foreach (GameObject temp_ghost in ghost_bullet)
                {
                    if (temp_ghost.GetComponent<Bullet_Movement>().ghost_owner == Bullet_Owner)
                    {
                        GameObject Bullet_Destroy_1 = GameObject.FindGameObjectWithTag("Bullet_Effect_Stop");
                        Bullet_Destroy_1.tag = "Bullet_Effect_Play";
                        Bullet_Destroy_1.transform.SetParent(null);
                        Bullet_Destroy_1.transform.position = temp_ghost.transform.position;
                        Bullet_Destroy_1.transform.rotation = temp_ghost.transform.rotation;
                        Bullet_Destroy_1.GetComponent<Animator>().SetBool("Destroy", true);

                        if (temp_ghost.GetComponent<Bullet_Movement>().ghost_owner.CompareTag("Bullet_1"))
                        {
                            temp_ghost.tag = "Bullet_Rest";
                        }
                        else if (temp_ghost.GetComponent<Bullet_Movement>().ghost_owner.CompareTag("Bullet_2"))
                        {
                            temp_ghost.tag = "Bullet_Rest_2";
                        }

                        ResetVarible(temp_ghost);
                    }
                }
            }

            ghost_bullet = GameObject.FindGameObjectsWithTag("Bullet_Ghost_Right");
            if (ghost_bullet.Length > 0)
            {
                foreach (GameObject temp_ghost in ghost_bullet)
                {
                    if (temp_ghost.GetComponent<Bullet_Movement>().ghost_owner == Bullet_Owner)
                    {
                        GameObject Bullet_Destroy_1 = GameObject.FindGameObjectWithTag("Bullet_Effect_Stop");
                        Bullet_Destroy_1.tag = "Bullet_Effect_Play";
                        Bullet_Destroy_1.transform.SetParent(null);
                        Bullet_Destroy_1.transform.position = temp_ghost.transform.position;
                        Bullet_Destroy_1.transform.rotation = temp_ghost.transform.rotation;
                        Bullet_Destroy_1.GetComponent<Animator>().SetBool("Destroy", true);

                        if (temp_ghost.GetComponent<Bullet_Movement>().ghost_owner.CompareTag("Bullet_1"))
                        {
                            temp_ghost.tag = "Bullet_Rest";
                        }
                        else if (temp_ghost.GetComponent<Bullet_Movement>().ghost_owner.CompareTag("Bullet_2"))
                        {
                            temp_ghost.tag = "Bullet_Rest_2";
                        }

                        ResetVarible(temp_ghost);
                    }
                }
            }
        }
    }

    public void SetFollow(bool follow)
    {
        bullet_follow = follow;
    }

    public void SetBulletSpeed(float new_speed)
    {
        bullet_speed = new_speed;
    }

    public void SetBurstShot(bool set)
    {
        bullet_burstshot = true;

        if (this.CompareTag("Bullet_1"))
        {
            GameObject ghost = GameObject.FindGameObjectWithTag("Bullet_Rest");

            if (ghost != null)
            {
                ghost.transform.SetParent(null);
                ghost.GetComponent<Bullet_Movement>().bullet_ghost = true;
                ghost.GetComponent<Bullet_Movement>().ghost_owner = this.gameObject;
                ghost.transform.position = new Vector3(this.transform.position.x + (bullet_world_size.x / 2), this.transform.position.y + bullet_world_size.y, this.transform.position.z);
                ghost.transform.rotation = this.transform.rotation;
                ghost.tag = "Bullet_Ghost_Left";
            }
            else
            {
                ghost = Instantiate(this.gameObject, Vector3.zero, Quaternion.identity) as GameObject;
                ghost.transform.SetParent(null);
                ghost.GetComponent<Bullet_Movement>().bullet_ghost = true;
                ghost.GetComponent<Bullet_Movement>().ghost_owner = this.gameObject;
                ghost.transform.position = new Vector3(this.transform.position.x + (bullet_world_size.x / 2), this.transform.position.y + bullet_world_size.y, this.transform.position.z);
                ghost.transform.rotation = this.transform.rotation;
                ghost.tag = "Bullet_Ghost_Left";

                if (GameObject.FindGameObjectWithTag("Bullet_Effect_Stop") != null)
                {
                    GameObject new_effect = Instantiate(GameObject.FindGameObjectWithTag("Bullet_Effect_Stop"), GameObject.Find("Bullet_Effect").transform.position, Quaternion.identity) as GameObject;
                    new_effect.tag = "Bullet_Effect_Stop";
                    new_effect.transform.SetParent(GameObject.Find("Bullet_Effect").transform);
                }
                else
                {
                    GameObject new_effect = Instantiate(GameObject.FindGameObjectWithTag("Bullet_Effect_Play"), GameObject.Find("Bullet_Effect").transform.position, Quaternion.identity) as GameObject;
                    new_effect.tag = "Bullet_Effect_Stop";
                    new_effect.transform.SetParent(GameObject.Find("Bullet_Effect").transform);
                }
            }

            ghost = GameObject.FindGameObjectWithTag("Bullet_Rest");

            if (ghost != null)
            {
                ghost.transform.SetParent(null);
                ghost.GetComponent<Bullet_Movement>().bullet_ghost = true;
                ghost.GetComponent<Bullet_Movement>().ghost_owner = this.gameObject;
                ghost.transform.position = new Vector3(this.transform.position.x - (bullet_world_size.x / 2), this.transform.position.y + bullet_world_size.y, this.transform.position.z);
                ghost.transform.rotation = this.transform.rotation;
                ghost.tag = "Bullet_Ghost_Right";
            }
            else
            {
                ghost = Instantiate(this.gameObject, Vector3.zero, Quaternion.identity) as GameObject;
                ghost.transform.SetParent(null);
                ghost.GetComponent<Bullet_Movement>().bullet_ghost = true;
                ghost.GetComponent<Bullet_Movement>().ghost_owner = this.gameObject;
                ghost.transform.position = new Vector3(this.transform.position.x - (bullet_world_size.x / 2), this.transform.position.y + bullet_world_size.y, this.transform.position.z);
                ghost.transform.rotation = this.transform.rotation;
                ghost.tag = "Bullet_Ghost_Right";

                if (GameObject.FindGameObjectWithTag("Bullet_Effect_Stop") != null)
                {
                    GameObject new_effect = Instantiate(GameObject.FindGameObjectWithTag("Bullet_Effect_Stop"), GameObject.Find("Bullet_Effect").transform.position, Quaternion.identity) as GameObject;
                    new_effect.tag = "Bullet_Effect_Stop";
                    new_effect.transform.SetParent(GameObject.Find("Bullet_Effect").transform);
                }
                else
                {
                    GameObject new_effect = Instantiate(GameObject.FindGameObjectWithTag("Bullet_Effect_Play"), GameObject.Find("Bullet_Effect").transform.position, Quaternion.identity) as GameObject;
                    new_effect.tag = "Bullet_Effect_Stop";
                    new_effect.transform.SetParent(GameObject.Find("Bullet_Effect").transform);
                }
            }
        }
        else
        {
            GameObject ghost = GameObject.FindGameObjectWithTag("Bullet_Rest_2");

            if (ghost != null)
            {
                ghost.transform.SetParent(null);
                ghost.GetComponent<Bullet_Movement>().bullet_ghost = true;
                ghost.GetComponent<Bullet_Movement>().ghost_owner = this.gameObject;
                ghost.transform.position = new Vector3(this.transform.position.x + (bullet_world_size.x / 2), this.transform.position.y - bullet_world_size.y, this.transform.position.z);
                ghost.transform.rotation = this.transform.rotation;
                ghost.tag = "Bullet_Ghost_Left";
            }
            else
            {
                ghost = Instantiate(this.gameObject, Vector3.zero, Quaternion.identity) as GameObject;
                ghost.transform.SetParent(null);
                ghost.GetComponent<Bullet_Movement>().bullet_ghost = true;
                ghost.GetComponent<Bullet_Movement>().ghost_owner = this.gameObject;
                ghost.transform.position = new Vector3(this.transform.position.x + (bullet_world_size.x / 2), this.transform.position.y - bullet_world_size.y, this.transform.position.z);
                ghost.transform.rotation = this.transform.rotation;
                ghost.tag = "Bullet_Ghost_Left";

                if (GameObject.FindGameObjectWithTag("Bullet_Effect_Stop") != null)
                {
                    GameObject new_effect = Instantiate(GameObject.FindGameObjectWithTag("Bullet_Effect_Stop"), GameObject.Find("Bullet_Effect").transform.position, Quaternion.identity) as GameObject;
                    new_effect.tag = "Bullet_Effect_Stop";
                    new_effect.transform.SetParent(GameObject.Find("Bullet_Effect").transform);
                }
                else
                {
                    GameObject new_effect = Instantiate(GameObject.FindGameObjectWithTag("Bullet_Effect_Play"), GameObject.Find("Bullet_Effect").transform.position, Quaternion.identity) as GameObject;
                    new_effect.tag = "Bullet_Effect_Stop";
                    new_effect.transform.SetParent(GameObject.Find("Bullet_Effect").transform);
                }
            }

            ghost = GameObject.FindGameObjectWithTag("Bullet_Rest_2");

            if (ghost != null)
            {
                ghost.transform.SetParent(null);
                ghost.GetComponent<Bullet_Movement>().bullet_ghost = true;
                ghost.GetComponent<Bullet_Movement>().ghost_owner = this.gameObject;
                ghost.transform.position = new Vector3(this.transform.position.x - (bullet_world_size.x / 2), this.transform.position.y - bullet_world_size.y, this.transform.position.z);
                ghost.transform.rotation = this.transform.rotation;
                ghost.tag = "Bullet_Ghost_Right";
            }
            else
            {
                ghost = Instantiate(this.gameObject, Vector3.zero, Quaternion.identity) as GameObject;
                ghost.transform.SetParent(null);
                ghost.GetComponent<Bullet_Movement>().bullet_ghost = true;
                ghost.GetComponent<Bullet_Movement>().ghost_owner = this.gameObject;
                ghost.transform.position = new Vector3(this.transform.position.x - (bullet_world_size.x / 2), this.transform.position.y - bullet_world_size.y, this.transform.position.z);
                ghost.transform.rotation = this.transform.rotation;
                ghost.tag = "Bullet_Ghost_Right";

                if (GameObject.FindGameObjectWithTag("Bullet_Effect_Stop") != null)
                {
                    GameObject new_effect = Instantiate(GameObject.FindGameObjectWithTag("Bullet_Effect_Stop"), GameObject.Find("Bullet_Effect").transform.position, Quaternion.identity) as GameObject;
                    new_effect.tag = "Bullet_Effect_Stop";
                    new_effect.transform.SetParent(GameObject.Find("Bullet_Effect").transform);
                }
                else
                {
                    GameObject new_effect = Instantiate(GameObject.FindGameObjectWithTag("Bullet_Effect_Play"), GameObject.Find("Bullet_Effect").transform.position, Quaternion.identity) as GameObject;
                    new_effect.tag = "Bullet_Effect_Stop";
                    new_effect.transform.SetParent(GameObject.Find("Bullet_Effect").transform);
                }
            }
        }
    }
}
