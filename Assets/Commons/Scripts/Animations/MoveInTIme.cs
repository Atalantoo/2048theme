using UnityEngine;
using System.Collections;
namespace Commons.Animations
{
    public class MoveInTIme : MonoBehaviour
    {

        public Vector2 move = new Vector2(0, 100);
        public float time_for_move = 0.7f;

        Vector2 begin;
        Vector2 end;
        float timer;

        void Start()
        {
            begin = transform.position;
            end = new Vector2(
                begin.x + move.x,
                begin.y + move.y);
            transform.position = begin;
            timer = time_for_move;
        }

        void Update()
        {
            timer -= Time.deltaTime;
            if (timer > 0)
            {
                Vector2 distance = end - begin;
                float degree_of_movement = (time_for_move - timer) / time_for_move;
                transform.position = new Vector2(
                    begin.x + (distance.x * degree_of_movement),
                    begin.y + (distance.y * degree_of_movement));
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}