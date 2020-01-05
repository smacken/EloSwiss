using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace EloSwissCli
{
    public class IgnorePropertiesResolver : DefaultContractResolver
    {
        private readonly IEnumerable<string> _propsToIgnore;
        public IgnorePropertiesResolver(IEnumerable<string> propNamesToIgnore)
        {
            _propsToIgnore = propNamesToIgnore;
        }
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty property = base.CreateProperty(member, memberSerialization);
            property.ShouldSerialize = (x) => !_propsToIgnore.Contains(property.PropertyName);
            return property;
        }
    }
}