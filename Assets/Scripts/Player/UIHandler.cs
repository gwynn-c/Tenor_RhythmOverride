using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    
    [SerializeField] private TextMeshProUGUI Title;
    [SerializeField] private TextMeshProUGUI Description;
    [SerializeField] private Image panelImage;

    [SerializeField] private TextMeshProUGUI Force; 
    [SerializeField] private TextMeshProUGUI Speed;
    [SerializeField] private TextMeshProUGUI Damage;

    
    public void InitializePanel(WeaponSO weaponInfo)
    {
        Title.SetText(weaponInfo._name);
        Description.SetText(weaponInfo._description);
        panelImage.sprite = weaponInfo._icon;
        
        
        Force.SetText("Force: " + weaponInfo.shootForce.ToString());
        Speed.SetText("Speed: " + weaponInfo.upwardForce.ToString());
        Damage.SetText("Damage: " + weaponInfo.bulletPrefab.GetComponent<BulletController>().damage.ToString());

    }
}