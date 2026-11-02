using System;
using System.Collections.Generic;
using NotoriousPIG.Framework.Events;

namespace TheRoyalExecutioner.Tests {
    public class RecordingEvents : IEvents {
        public List<Type> triggered { get; } = new();
        public ChoiceResult lastResolvedResult { get; private set; }

        public void StartListening<TEvent>(Action<TEvent> handler) { }

        public void StopListening<TEvent>(Action<TEvent> handler) { }

        public void Trigger<TEvent>(TEvent eventReference) {
            triggered.Add(typeof(TEvent));
            if (eventReference is ChoiceResolved resolved) {
                lastResolvedResult = resolved.result;
            }
        }
    }
}
