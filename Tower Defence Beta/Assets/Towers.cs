using UnityEngine;

public class Towers : MonoBehaviour
{
    public GameObject towerToClone;

    private GameObject currentTower;
    private TowerPlacementValidator validator;
    private bool isPlacing = false;

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space) && !isPlacing)
        {
            StartPlacingTower();
        }

        if (isPlacing && currentTower != null)
        {
            FollowMouse();
            UpdateRadiusColor();
        }

        if (isPlacing && Input.GetMouseButtonDown(0))
        {
            TryPlaceTower();
        }
    }

    void StartPlacingTower()
    {
        currentTower = Instantiate(towerToClone);
        validator = currentTower.GetComponentInChildren<TowerPlacementValidator>();
        isPlacing = true;
    }

    void FollowMouse()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;
        currentTower.transform.position = mouseWorldPos;
    }

    void TryPlaceTower()
    {
        if (validator != null && !validator.CanPlace)
            return;

       
        Transform radius = currentTower.transform.Find("radius");
        if (radius != null)
        {
            SpriteRenderer sr = radius.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                Color c = sr.color;
                c.a = 0f; // fully transparent
                sr.color = c;
            }

            // If the radius has child sprites, make them transparent too
            foreach (SpriteRenderer childSR in radius.GetComponentsInChildren<SpriteRenderer>())
            {
                Color c = childSR.color;
                c.a = 0f;
                childSR.color = c;
            }
            //tower placement scriptet yk yk yk yk
            validator = currentTower.GetComponentInChildren<TowerPlacementValidator>();

            // Set targeting script to active
            TowerTargeting targeting = currentTower.GetComponentInChildren<TowerTargeting>();
            if (targeting != null)
            {
                targeting.isPlaced = true;
            }

        }


        isPlacing = false;
        currentTower = null;
        validator = null;
    }
    void UpdateRadiusColor()
    {
        if (validator == null) return;

        Transform radius = currentTower.transform.Find("radius");
        if (radius == null) return;

        Color targetColor = validator.CanPlace ? Color.green : Color.red;

        // Change the radius sprite color
        SpriteRenderer sr = radius.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            Color c = sr.color;
            c.r = targetColor.r;
            c.g = targetColor.g;
            c.b = targetColor.b;
            sr.color = c;
        }

        // Change child sprites if any
        foreach (SpriteRenderer childSR in radius.GetComponentsInChildren<SpriteRenderer>())
        {
            Color c = childSR.color;
            c.r = targetColor.r;
            c.g = targetColor.g;
            c.b = targetColor.b;
            childSR.color = c;
        }
    }

}
