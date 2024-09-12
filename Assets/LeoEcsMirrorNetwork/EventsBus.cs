using System;
using System.Collections.Generic;
using Leopotam.EcsLite;

namespace SevenBoldPencil.EasyEvents
{
    public class EventsBus
    {
        private readonly EcsWorld eventsWorld;
        private readonly Dictionary<Type, int> singletonEntities;
        private readonly Dictionary<Type, EcsFilter> cachedFilters;

        public EventsBus(int capacityEvents = 8, int capacityEventsSingleton = 8)
        {
            eventsWorld = new EcsWorld();
            singletonEntities = new Dictionary<Type, int>(capacityEventsSingleton);
            cachedFilters = new Dictionary<Type, EcsFilter>(capacityEvents);
        }
        
        #region Events

        public ref T NewEvent<T>() where T : struct
        {
            var newEntity = eventsWorld.NewEntity();
            return ref eventsWorld.GetPool<T>().Add(newEntity);
        }

        private EcsFilter GetFilter<T>() where T : struct
        {
            var type = typeof(T);
            if (!cachedFilters.TryGetValue(type, out var filter)) {
                filter = eventsWorld.Filter<T>().End();
                cachedFilters.Add(type, filter);
            }

            return filter;
        }

        public EcsFilter GetEventBodies<T>(out EcsPool<T> pool) where T : struct
        {
            pool = eventsWorld.GetPool<T>();
            return GetFilter<T>();
        }

        public bool HasEvents<T>() where T : struct
        {
            var filter = GetFilter<T>();
            return filter.GetEntitiesCount() != 0;
        }

        public void DestroyEvents<T>() where T : struct
        {
            foreach (var eventEntity in GetFilter<T>()) {
                eventsWorld.DelEntity(eventEntity);
            }
        }

        #endregion

        #region DestroyEventsSystem

        public DestroyEventsSystem GetDestroyEventsSystem(int capacity = 16)
        {
            return new DestroyEventsSystem(this, capacity);
        }

        public class DestroyEventsSystem : IEcsRunSystem
        {
            private readonly EventsBus eventsBus;
            private readonly List<Action> destructionActions;

            public DestroyEventsSystem(EventsBus eventsBus, int capacity)
            {
                this.eventsBus = eventsBus;
                destructionActions = new List<Action>(capacity);
            }

            public void Run(IEcsSystems systems)
            {
                foreach (var action in destructionActions) {
                    action();
                }
            }

            public DestroyEventsSystem IncReplicant<R>() where R : struct {
                destructionActions.Add(() => eventsBus.DestroyEvents<R>());
                return this;
            }
        }

        #endregion

        /// <summary>
        /// External modification of events world can lead to Unforeseen Consequences
        /// </summary>
        /// <returns></returns>
        public EcsWorld GetEventsWorld()
        {
            return eventsWorld;
        }

        public void Destroy()
        {
            singletonEntities.Clear();
            cachedFilters.Clear();
            eventsWorld.Destroy();
        }
    }
}
