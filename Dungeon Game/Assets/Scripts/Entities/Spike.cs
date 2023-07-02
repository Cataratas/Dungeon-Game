using Entities.Characters;
using UnityEngine;

namespace Entities {
    public class Spike : StateMachineBehaviour {
        [SerializeField] public float damageAmount = 0.125f;
        private Transform spikeTransform;
        private bool isDamaging;
        private float damageTimer;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            spikeTransform = animator.transform;
            isDamaging = true;
            damageTimer = 0f;
        }
        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            if (!isDamaging) return;
            
            damageTimer += Time.deltaTime;
            if (!(damageTimer >= 0.0625f)) return;
            damageTimer -= 0.0625f;
            
            var colliders = Physics2D.OverlapBoxAll(spikeTransform.position, Vector2.one, 0f);
            foreach (var collider in colliders) {
                if (!collider.CompareTag("Player") && !collider.CompareTag("Enemy")) continue;
                
                var entity = collider.GetComponent<Character>();
                if (entity == null) continue;
                
                entity.Damage(damageAmount);
            }
        }
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            isDamaging = false;
            damageTimer = 0f;
        }
    }
}
