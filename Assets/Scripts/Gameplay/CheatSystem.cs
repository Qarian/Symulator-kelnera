using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheatSystem : MonoBehaviour
{
    // Start is called before the first frame update
    string cheat;
    public GameObject inputField;
    private GameObject search;

    void Start()
    {
        search = GameObject.Find("CheatsUI");
        search.SetActive(!search.activeSelf);
    }

    public void CheatField()
    {
        
            cheat = inputField.GetComponent<Text>().text;

            switch (cheat)
            {
                case "grandma":
                    Debug.Log("Cheat activated: add money");
                    GrandmaCheat();
                    break;

                case "holidays":
                    Debug.Log("Cheat activated: more time");
                    HolidaysCheat();
                    break;

                case "chincha":
                    Debug.Log("Cheat activated: spawned chinchilla Julian");
                    SzyszaCheat();
                    break;

                case "flyhigh":
                    Debug.Log("Cheat activated: platforms won't fall");
                    FlyhighCheat();
                    break;

                case "runboyrun":
                    Debug.Log("Cheat activated: max speed");
                    RunboyrunCheat();
                    break;

                case "ilovebunny":
                    Debug.Log("Cheat activated: higher jumps");
                    IlovebunnyCheat();
                    break;

                case "gohome":
                    Debug.Log("Cheat activated: day ended");
                    GohomeCheat();
                    break;

                case "wuhan":
                    Debug.Log("To nawet nie jest smieszne...");
                    break;
            }


            void GrandmaCheat()
            {
                //skrypt do dodania pieniedzy
            }

            void HolidaysCheat()
            {
                //skrypt do dodania czasu
            }

            void SzyszaCheat()
            {
                //skrypt do szyszy Julianka
            }

            void FlyhighCheat()
            {
                //skrypt do platform
                FallingPlatformSKS.jumpToFallPlatformStatic = 10000;
            }

            void RunboyrunCheat()
            {
                //skrypt do biegu
            }

            void IlovebunnyCheat()
            {
                //skrypt do wyzszego skoku
            }

            void GohomeCheat()
            {
                //skrypt do konca dnia
            }

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            search.SetActive(!search.activeSelf);
            CheatField();
        }
            
    }
}
        
    

