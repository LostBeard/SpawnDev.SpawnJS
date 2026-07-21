using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SpawnDev.SpawnJS.Marshallers
{
    /// <summary>
    /// Determines information used for Json serialization for a specified Propert or Field
    /// </summary>
    public class ClassMemberJsonInfo
    {
        /// <summary>
        /// Type default value
        /// </summary>
        public object? DefaultValue { get; private set; }
        /// <summary>
        /// Field or Property name
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// JsonIgnoeAttribute
        /// </summary>
        public JsonIgnoreAttribute? JsonIgnoreAttribute { get; set; }
        /// <summary>
        /// JsonPropertyNameAttribute
        /// </summary>
        public JsonPropertyNameAttribute? JsonPropertyNameAttribute { get; set; }
        /// <summary>
        /// ProeprtyInfo is a property
        /// </summary>
        public PropertyInfo? PropertyInfo { get; private set; }
        /// <summary>
        /// FieldInfo if a field
        /// </summary>
        public FieldInfo? FieldInfo { get; private set; }
        /// <summary>
        /// New instance usign FieldInfo
        /// </summary>
        /// <param name="fieldInfo"></param>
        public ClassMemberJsonInfo(FieldInfo fieldInfo)
        {
            FieldInfo = fieldInfo;
            Name = FieldInfo.Name;
            DefaultValue = FieldInfo.FieldType.GetDefaultValue();
            MemberType = UnwrapNullable(FieldInfo.FieldType);
            RuntimeTypeIsKnown = IsRuntimeTypeKnown(MemberType);
        }
        /// <summary>
        /// New instance usign PropertyInfo
        /// </summary>
        /// <param name="propertyInfo"></param>
        public ClassMemberJsonInfo(PropertyInfo propertyInfo)
        {
            PropertyInfo = propertyInfo;
            Name = PropertyInfo.Name;
            DefaultValue = PropertyInfo.PropertyType.GetDefaultValue();
            MemberType = UnwrapNullable(PropertyInfo.PropertyType);
            RuntimeTypeIsKnown = IsRuntimeTypeKnown(MemberType);
        }
        /// <summary>
        /// The member's declared type, with Nullable&lt;&gt; unwrapped. A boxed Nullable&lt;int&gt; reports
        /// its runtime type as int, so the unwrapped type is the one a marshaller is selected for.
        /// </summary>
        public Type MemberType { get; private set; }
        /// <summary>
        /// True when a non null value of this member is guaranteed to have <see cref="MemberType"/> as its
        /// exact runtime type - that is, the type is a value type or is sealed, so nothing can derive from
        /// it and be stored here. When true the marshaller for this member can be resolved once and reused;
        /// when false the value must be asked for its own type on every marshal, because a member declared
        /// as a base class may hold any subclass and the two can marshal differently.
        /// </summary>
        public bool RuntimeTypeIsKnown { get; private set; }
        /// <summary>
        /// Marshaller for <see cref="MemberType"/>, resolved on first use and only when
        /// <see cref="RuntimeTypeIsKnown"/>. Resolution is a dictionary lookup keyed on a Type, which is
        /// cheap in isolation but ran once per member per marshal and measured 2.86us of the 20.2us cost of
        /// marshalling a five member descriptor - more than the reflection reads in the same walk.
        /// </summary>
        internal JSMarshaller? CachedMarshaller;
        /// <summary>
        /// The Javascript slot holding this member's property NAME, interned on first use.<br/>
        /// A property name is a fixed literal for its type, so it crosses the boundary once per process
        /// and is a plain number - a slot id - every time after. That is what lets a whole descriptor be
        /// built in one crossing: names and values are all just numbers in the frame.
        /// </summary>
        internal double NameSlot;

        static Type UnwrapNullable(Type type) => Nullable.GetUnderlyingType(type) ?? type;

        static bool IsRuntimeTypeKnown(Type type) => type.IsValueType || type.IsSealed;
        /// <summary>
        /// Returns if the Property/Field should be written when serialized
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool GetShouldWrite(object? value)
        {
            if (JsonIgnoreAttribute == null) return true;
            if (JsonIgnoreAttribute.Condition == JsonIgnoreCondition.Always) return false;
            if (JsonIgnoreAttribute.Condition == JsonIgnoreCondition.WhenWritingNull && value is null) return false;
            if (JsonIgnoreAttribute.Condition == JsonIgnoreCondition.WhenWritingDefault && Equals(value, DefaultValue)) return false;
            return true;
        }
        /// <summary>
        /// Get's the Json version of the Property/Field name
        /// </summary>
        public string GetJsonName(JsonNamingPolicy jsonNamingPolicy)
        {
            if (JsonPropertyNameAttribute != null) return JsonPropertyNameAttribute.Name;
            return jsonNamingPolicy == null ? Name : jsonNamingPolicy.ConvertName(Name);
        }
        /// <summary>
        /// Get's the Json version of the Property/Field name
        /// </summary>
        public string GetJsonName() => _jsonName ??=
            JsonPropertyNameAttribute?.Name ?? JsonNamingPolicy.CamelCase.ConvertName(Name);
        // A member's Javascript name cannot change, but this ran on every property of every marshal and
        // ConvertName allocates a fresh string each time. Types carrying an explicit JsonPropertyName hid
        // the cost; most wrapper types do not have one, so they paid it on every single object marshalled.
        string? _jsonName;
    }
    
}
