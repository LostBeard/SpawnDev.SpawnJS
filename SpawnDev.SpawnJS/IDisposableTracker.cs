using System.Text.Json.Serialization;

namespace SpawnDev.SpawnJS
{
    /// <summary>
    /// Copntains tracking information for JSObjects created with the same stack trace. This is used for diagnostics purposes to track where JSObjects are being created and disposed, and can be used to identify potential leaks. This will likely be removed in future releases.
    /// </summary>
    public class IDisposableTracker
    {
        /// <summary>
        /// True when either verbose mode is on, so callers can skip the tracking work entirely when it is off
        /// </summary>
        public static bool Enabled => CreatedHandleVerboseMode || UndisposedHandleVerboseMode;
        /// <summary>
        /// The type name of the tracked disposable
        /// </summary>
        public required string Type { get; set; }
        /// <summary>
        /// The stack trace these instances were all created from. This is the key the tracker groups by.
        /// </summary>
        public required string Trace { get; set; }
        /// <summary>
        /// How many instances have been created from this stack trace
        /// </summary>
        public ulong Created { get; set; }
        /// <summary>
        /// How many were disposed explicitly. Created minus DisposedProper minus DisposedFinalizer is the live count.
        /// </summary>
        public ulong DisposedProper { get; set; }
        /// <summary>
        /// How many were reclaimed by a finalizer rather than disposed. A non zero count here is the leak signal.
        /// </summary>
        public ulong DisposedFinalizer { get; set; }
        /// <summary>
        /// How many instances from this stack trace are still alive
        /// </summary>
        public int AliveCount => Alive.Count;
        /// <summary>
        /// Ids of the instances from this stack trace that are still alive
        /// </summary>
        [JsonIgnore]
        public HashSet<ulong> Alive { get; } = new HashSet<ulong>();
        /// <summary>
        /// Contains tracking information for JSObjects created with the same stack trace. This is used for diagnostics purposes to track where JSObjects are being created and disposed, and can be used to identify potential leaks. This will likely be removed in future releases.
        /// </summary>
        public static Dictionary<string, IDisposableTracker> JSObjectTraces { get; } = new Dictionary<string, IDisposableTracker>();
        /// <summary>
        /// Records that a tracked disposable was released, either explicitly or by its finalizer
        /// </summary>
        public static void DisposableDisposed((IDisposableTracker? disposableTracker, ulong disposableId) tracker, bool disposing)
        {
            if (tracker.disposableTracker != null && tracker.disposableTracker.Alive.Contains(tracker.disposableId))
            {
                tracker.disposableTracker.Alive.Remove(tracker.disposableId);
                if (disposing)
                {
                    tracker.disposableTracker.DisposedProper++;
                }
                else
                {
                    tracker.disposableTracker.DisposedFinalizer++;
                    if (tracker.disposableTracker.DisposedFinalizer == 1 && UndisposedHandleVerboseMode)
                    {
                        Console.WriteLine($"DEBUG WARNING: IDisposable disposed in finalizer: {tracker.disposableTracker.Type}\n{tracker.disposableTracker.Trace}");
                    }
                }
            }
        }
        /// <summary>
        /// When true, every creation is traced. Diagnostics only - it costs a stack trace per instance.
        /// </summary>
        public static bool CreatedHandleVerboseMode
        {
            get => _CreatedHandleVerboseMode;
            set
            {
                if (_CreatedHandleVerboseMode == value) return;
                _CreatedHandleVerboseMode = value;
                if (value) Init();
            }
        }
        private static bool _CreatedHandleVerboseMode { get; set; }
        /// <summary>
        /// When true, instances reclaimed by a finalizer rather than disposed are reported
        /// </summary>
        public static bool UndisposedHandleVerboseMode
        {
            get => _UndisposedHandleVerboseMode;
            set
            {
                if (_UndisposedHandleVerboseMode == value) return;
                _UndisposedHandleVerboseMode = value;
                if (value) Init();
            }
        }
        private static bool _UndisposedHandleVerboseMode { get; set; }
        static bool _beenInit = false;
        static void Init()
        {
            if (_beenInit) return;
            _beenInit = true;
            (SpawnJSRuntime.Instance ?? throw new InvalidOperationException("SpawnJSRuntime has not been created.")).Set(nameof(IDisposableTracker), new
            {
                created = Callback.Create(() => IDisposableTracker.JSObjectTraces.Values.OrderByDescending(o => o.Created).ToList()),
                disposedProper = Callback.Create(() => IDisposableTracker.JSObjectTraces.Values.OrderByDescending(o => o.DisposedProper).Where(o => o.DisposedProper > 0).ToList()),
                disposedFinalizer = Callback.Create(() => IDisposableTracker.JSObjectTraces.Values.OrderByDescending(o => o.DisposedFinalizer).Where(o => o.DisposedFinalizer > 0).ToList()),
                aliveCount = Callback.Create(() => IDisposableTracker.JSObjectTraces.Values.OrderByDescending(o => o.AliveCount).Where(o => o.AliveCount > 0).ToList()),
                reset = Callback.Create(() => IDisposableTracker.JSObjectTraces.Clear()),
            });
        }
        static ulong _totalCreated = 0;
        /// <summary>
        /// Records the creation of a tracked disposable and returns the handle used to report its release
        /// </summary>
        public static (IDisposableTracker? disposableTracker, ulong disposableId) DisposableCreated(IDisposable disposable)
        {
            if (!Enabled) return (null, 0);
            var index = _totalCreated++;
            IDisposableTracker? _LifeTrack = null;
            var creationStackTrace = Environment.StackTrace;
            if (!JSObjectTraces.TryGetValue(creationStackTrace, out _LifeTrack))
            {
                _LifeTrack = new IDisposableTracker
                {
                    Trace = creationStackTrace,
                    Type = disposable.GetType().FullName!
                };
                JSObjectTraces.Add(creationStackTrace, _LifeTrack);
                if (CreatedHandleVerboseMode)
                {
                    Console.WriteLine($"NOTICE: IDisposable created: {_LifeTrack.Type}\n{_LifeTrack.Trace}");
                }
            }
            _LifeTrack.Alive.Add(index);
            _LifeTrack.Created++;
            return (_LifeTrack, index);
        }
    }
}