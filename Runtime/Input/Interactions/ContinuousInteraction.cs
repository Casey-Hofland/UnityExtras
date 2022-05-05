﻿#nullable enable
using UnityEngine;
using UnityEngine.InputSystem;

namespace UnityExtras.Input
{
#if UNITY_EDITOR
    [UnityEditor.InitializeOnLoad]
#endif
    public class ContinuousInteraction : IInputInteraction
    {
        public float activationTime { get; private set; }
        private float lastDeltaTime;

        static ContinuousInteraction()
        {
            InputSystem.RegisterInteraction<ContinuousInteraction>();
        }

        public void Process(ref InputInteractionContext context)
        {
            if (!context.ControlIsActuated())
            {
                lastDeltaTime = activationTime = 0f;
                context.Canceled();
            }
            else if (context.phase.IsInProgress())
            {
                activationTime += lastDeltaTime;
                context.PerformedAndStayPerformed();
                context.SetTimeout(0.000001f);
                lastDeltaTime = Time.deltaTime;
            }
            else
            {
                context.Started();
                Process(ref context);
            }
        }

        public void Reset()
        {
        }
    }
}
