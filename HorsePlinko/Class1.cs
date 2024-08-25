using BepInEx;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Plinko
{
    [BepInPlugin("biotest.plinko", "Plinko", "0.0.1")]
    public class Class1 : BaseUnityPlugin
    {
        int coins;
        float multiplier;
        float balls;
        List<PlinkoBall> plinkoBalls = new();
        Shader uberShader = Shader.Find("GorillaTag/UberShader");
        float delay = 0.5f;

        void Start()
        {
            coins = PlayerPrefs.GetInt("PlinkoCoins", 0); // theres a way to make this a million, i know but who cares
            multiplier = PlayerPrefs.GetFloat("PlinkoMultiplier", 1); // same with this
            balls = PlayerPrefs.GetInt("PlinkoBalls", 1); // again, same with this

            GorillaTagger.OnPlayerSpawned(Init);
        }

        void Init()
        {
            uberShader = Shader.Find("Universal Render Pipeline/Lit");

            StartCoroutine(SpawnBalls());
        }

        IEnumerator SpawnBalls()
        {
            for (int i = 0; i < balls; i++)
            {
                SpawnBall(Vector3.zero);
                yield return new WaitForSeconds(delay);
            }
        }

        void SpawnBall(Vector3 pos, float radius = 0.25f)
        {
            var ball = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            ball.transform.position = pos;
            ball.transform.localScale = Vector3.one * radius;

            Renderer ballRend = ball.GetComponent<Renderer>();

            if (ballRend != null)
                ballRend.material = new Material(uberShader);
            else
                ball.AddComponent<Renderer>().material = new Material(uberShader);

            ball.AddComponent<Rigidbody>();

            plinkoBalls.Add(new PlinkoBall(ball));
        }
    }
}
