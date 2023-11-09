using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerIntro : MonoBehaviour
{
    public Sprite standSpr;
    public Player player;
    public GameObject hud;

    bool arive = false;
    SpriteRenderer sprite;
    private void Awake()
    {
        GameManager.instance.joystick.localScale = Vector3.zero;
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        Vector2 myPos = transform.position;
        if (myPos.y > 0) { myPos.y -= 10 * Time.unscaledDeltaTime; transform.position = myPos; }
        else if (!arive){ StartCoroutine(GameStartRoutin()); arive = true; }
    }

    IEnumerator GameStartRoutin()
    {
        sprite.sprite = standSpr;
        AudioManager.instance.PlaySfx(AudioManager.SFX.Land);
        yield return new WaitForSecondsRealtime(1);
        sprite.flipX = true;
        yield return new WaitForSecondsRealtime(0.5f);
        sprite.flipX = false;
        yield return new WaitForSecondsRealtime(0.5f);
        sprite.flipX = true;
        yield return new WaitForSecondsRealtime(0.5f);
        sprite.flipX = false;
        yield return new WaitForSecondsRealtime(0.5f);
        GameManager.instance.joystick.localScale = Vector3.one * 2.5f;
        player.gameObject.SetActive(true);
        player.InitializedWeapons();
        hud.SetActive(true);
        AudioManager.instance.PlayBgm(true);
        gameObject.SetActive(false);
    }
}
