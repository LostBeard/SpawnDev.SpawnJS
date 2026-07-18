//using System.Collections.Concurrent;
//using System.Reflection;
//using System.Text.Json.Serialization;

//namespace SpawnDev.SpawnJS.Marshallers
//{
//    /// <summary>
//    /// Property metadata cache for Json attributes used durign serialization
//    /// </summary>
//    public static class PropertyMetadataCache
//    {
//        private static readonly ConcurrentDictionary<PropertyInfo, PropertyMetadata> Cache = new();

//        /// <summary>
//        /// Property Metadata
//        /// </summary>
//        public class PropertyMetadata
//        {
//            /// <summary>
//            /// The .Net property name in its JS form
//            /// </summary>
//            public string JSPropertyName { get; init; }
//            public bool ShouldIgnore { get; init; }
//        }

//        public static PropertyMetadata GetMetadata(PropertyInfo prop)
//        {
//            return Cache.GetOrAdd(prop, p =>
//            {
//                var ignoreAttr = p.GetCustomAttribute<JsonIgnoreAttribute>();
//                var nameAttr = p.GetCustomAttribute<JsonPropertyNameAttribute>();

//                return new PropertyMetadata
//                {
//                    ShouldIgnore = ignoreAttr != null,
//                    JSPropertyName = nameAttr?.Name ?? p.Name
//                };
//            });
//        }
//    }
//}
