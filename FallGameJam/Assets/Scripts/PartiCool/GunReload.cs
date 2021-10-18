using System.Collections;
using UnityEngine;

public class GunReload : MonoBehaviour
{
    public int MAX_MAG_SIZE;
    public int curr_Mag;
    public bool isReloading = false;
    [SerializeField] private Player player;
    [SerializeField] Transform crosshairInside;
    [SerializeField] float reloadTime;

    public void reloadAnimation() {
        StartCoroutine(reloadCoroutine());
    }
    public IEnumerator reloadCoroutine() {
        float time = reloadTime;
        Quaternion target = crosshairInside.rotation;
        isReloading = true;

        while (time >= 0) {
            time -= Time.deltaTime;

            crosshairInside.rotation = Quaternion.Euler(0, 0, (1 - time/reloadTime) * 900 + 45);

            player.SHOB_HEAD_RADIUS = Mathf.Abs(Mathf.Sin(time / reloadTime * Mathf.PI - Mathf.PI/2)) * 0.8f;

            yield return null;
        }

        crosshairInside.rotation = target;
        player.SHOB_HEAD_RADIUS = 0.8f;
        isReloading = false;
        curr_Mag = MAX_MAG_SIZE;
    }
}
