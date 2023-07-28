using System;
using UnityEngine;

public static class ModifierLibrary
{

    public static class OneTime
    {
        /// <summary>
        /// Applies one time damage modifier to target of type.
        /// </summary>
        /// <param name="target">The target to attach the modifier to</param>
        /// <param name="DamageAmount">The amount of damage to do</param>
        /// <param name="DamageType">The damage type</param>
        public static void ApplyDamageModifier(GameObject target, float DamageAmount, DamageTypeEnum DamageType)
        {
            if (!target.gameObject.TryGetComponent<DealDamageModifier>(out DealDamageModifier existingModifier))
            {
                DealDamageModifier DamageEffect = target.gameObject.AddComponent<DealDamageModifier>();
                DamageEffect.DamageAmount = DamageAmount;
                DamageEffect.DamageType = DamageType;
                DamageEffect.StartCoroutine(DamageEffect.Evaluate());
            }

        }

        /// <summary>
        /// Tentale whap modifer occurs when a tentacle hits an enemy. It does damage and applies force.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="tentacle"></param>
        /// <param name="DamageAmount"></param>
        /// <param name="direction"></param>
        /// <param name="acceleration"></param>
        public static void ApplyTentacleWhapModifer(GameObject target, GameObject tentacle, float DamageAmount, Vector3 direction, float acceleration)
        {
            if (!target.gameObject.TryGetComponent<TentacleWhapModifer>(out TentacleWhapModifer existingModifier))
            {
                TentacleWhapModifer TentacleWhap = target.gameObject.AddComponent<TentacleWhapModifer>();
                TentacleWhap.DamageAmount = DamageAmount;
                TentacleWhap.direction = direction;
                TentacleWhap.acceleration = acceleration;
                TentacleWhap.StartCoroutine(TentacleWhap.Evaluate());
            }

        }
    }

    public static class Digestion
    {

        /// <summary>
        /// Applies one time Digestion modifier to target of type.
        /// </summary>
        /// <param name="target">The target to attach the modifier to</param>
        /// <param name="DamageAmount">The amount of damage to do</param>
        /// <param name="DamageType">The damage type</param>
        public static void ApplyDigestionModifier(GameObject target, float DamageAmount, GameObject Digester)
        {


            if (!target.gameObject.TryGetComponent<DigestNutritionModifier>(out DigestNutritionModifier existingModifier))
            {
                DigestNutritionModifier DamageEffect = target.gameObject.AddComponent<DigestNutritionModifier>();
                DamageEffect.DigestAmount = DamageAmount;
                DamageEffect.Digester = Digester;
                DamageEffect.StartCoroutine(DamageEffect.Evaluate());
            }
        }

        /// <summary>
        /// Applies modifier to target. Continues until the specified game object drops off the specified list.
        /// </summary>
        /// <param name="target">The target to attach the modifier to</param>
        /// <param name="comparer1">The game object which should be on a list</param>
        /// <param name="comparer2">The list of game objects that the game object should be on</param>
        /// <param name="DragInsideStomach"></param>
        /// <param name="AngularDragInsideStomach"></param>
        /// <param name="Inverse">Whether to invert the conditional logic</param>
        public static void ApplyInContactWithBlobModifier(GameObject target, GameObject comparer1,
            GameObjectRuntimeSet comparer2, FloatVariable DragInsideStomach, FloatVariable AngularDragInsideStomach, bool Inverse = false)
        {
            InContactWithBlobModifier existingModifier = target.gameObject.GetComponent<InContactWithBlobModifier>();
            if (existingModifier == null)
            {
                InContactWithBlobModifier InContactEffect = target.gameObject.AddComponent<InContactWithBlobModifier>();
                InContactEffect.DragInsideStomach = DragInsideStomach;
                InContactEffect.AngularDragInsideStomach = AngularDragInsideStomach;
                InContactEffect.Comparer1 = comparer1;
                InContactEffect.Comparer2 = comparer2;
                InContactEffect.Inverse = Inverse;
                try
                {
                    InContactEffect.StartCoroutine(InContactEffect.Evaluate());
                }
                catch (Exception ex)
                {
                    Debug.LogError(ex.ToString());
                }

            }
        }

        /// <summary>
        /// Applies modifier to target. Continues until the specified game object drops off the specified list.
        /// </summary>
        /// <param name="target">The target to attach the modifier to</param>
        /// <param name="comparer1">The game object which should be on a list</param>
        /// <param name="comparer2">The list of game objects that the game object should be on</param>
        /// <param name="CubeWidth">The CubeWidth scriptable object</param>
        /// <param name="Inverse">Whether to invert the conditional logic</param>
        public static void ApplyInBlobsStomachModifier(GameObject target, GameObject comparer1,
            GameObjectRuntimeSet comparer2, GameObject Stomach, bool Inverse = false)
        {
            InBlobsStomachModifier existingModifier = target.gameObject.GetComponent<InBlobsStomachModifier>();
            if (existingModifier == null)
            {
                InBlobsStomachModifier InStomachEffect = target.gameObject.AddComponent<InBlobsStomachModifier>();
                InStomachEffect.Comparer1 = comparer1;
                InStomachEffect.Comparer2 = comparer2;
                InStomachEffect.Stomach = Stomach;
                InStomachEffect.Inverse = Inverse;
                InStomachEffect.StartCoroutine(InStomachEffect.Evaluate());
            }
        }


    }


}
