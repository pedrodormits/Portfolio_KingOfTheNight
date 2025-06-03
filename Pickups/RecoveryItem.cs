using UnityEngine;

public enum ResourceType
{
    Health,
    Mana
}

public class RecoveryItem : MonoBehaviour
{
    public ResourceType ResourceType;
    public RecoveryItemData RecoveryItemData;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Use(other.gameObject);
        }
    }

    public void Use(GameObject target)
    {
        var health = target.GetComponent<Health>();
        //var mana = target.GetComponent<Mana>();

        switch (ResourceType)
        {
            case ResourceType.Health:
                if (health != null)
                    health.Heal(RecoveryItemData.HealAmount);
                break;

                //case ResourceType.Mana:
                //    if (mana != null)
                //        mana.RestoreMana(medKitData.manaRestoreAmount);
                //    break;

                //case ResourceType.Both:
                //    if (health != null)
                //        health.Heal(medKitData.healAmount);
                //    if (mana != null)
                //        mana.RestoreMana(medKitData.manaRestoreAmount);
                //    break;
        }

        Destroy(gameObject);
    }
}