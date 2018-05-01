using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TopDownScroller.Agents
{

    public class MoverAgent : Agent
    {
        /// <summary>
		/// If flying agents are blocked, they should still move through obstacles
		/// </summary>
		protected override void OnPartialPathUpdate()
        {

        }

        protected override void PathUpdate()
        {
            switch (state)
            {
                case State.OnCompletePath:
                    OnCompletePathUpdate();
                    break;
                case State.OnPartialPath:
                    OnPartialPathUpdate();
                    break;
            }
        }
    }

}