using Newtonsoft.Json;
using System.Reflection;

namespace ImageStoreBase.Common.Helper
{
    public static class ClassHelper
    {
        private static readonly HashSet<Type> _simpleTypes = new ()
        {
            typeof(Enum),
            typeof(string),
            typeof(decimal),
            typeof(DateTime),
            typeof(DateTimeOffset),
            typeof(TimeSpan),
            typeof(Guid)
        };

        public static bool IsSimpleType(this Type type)
        {
            return type.IsPrimitive || _simpleTypes.Contains(type) || Convert.GetTypeCode(type) != TypeCode.Object;
        }

        public static bool HasProperty(object fromObject, string propertyName)
        {
            return fromObject?.GetType().GetProperty(propertyName) != null;
        }

        public static PropertyInfo? GetPropertyInfo(object fromObject, string propertyName)
        {
            return fromObject?.GetType().GetProperty(propertyName);
        }

        public static string GetPropertyDataType(object fromObject, string propertyName)
        {
            return fromObject?.GetType().GetProperty(propertyName)?.PropertyType.Name ?? string.Empty;
        }

        public static T? GetPropertyValue<T>(object fromObject, string propertyName)
        {
            var prop = fromObject?.GetType().GetProperty(propertyName);
            return prop != null ? JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(prop.GetValue(fromObject))) : default;
        }
       
        public static List<KeyValuePair<string, Type>> GetPropertyList(object fromObject)
        {
            return fromObject?.GetType().GetProperties()
                .Where(p => p.PropertyType.IsSimpleType())
                .Select(p => new KeyValuePair<string, Type>(p.Name, p.PropertyType))
                .ToList() ?? new List<KeyValuePair<string, Type>>();
        }

        public static TResult? GetPropertyData<TResult>(object fromObject, string propertyName)
        {
            var propertyValue = fromObject?.GetType().GetProperty(propertyName)?.GetValue(fromObject);
            return propertyValue != null ? (TResult)propertyValue : default;
        }

        public static bool SetProperty(object fromObject, string propertyName, string propertyValue)
        {
            var prop = fromObject?.GetType().GetProperty(propertyName);
            if (prop == null) return false;
            prop.SetValue(fromObject, propertyValue);
            return true;
        }

        public static bool ComparePropertyValues(object firstObject, object secondObject, string propertyName)
        {
            if (firstObject == null || secondObject == null || string.IsNullOrEmpty(propertyName))
                return false;

            var firstProperty = firstObject.GetType().GetProperty(propertyName);
            var secondProperty = secondObject.GetType().GetProperty(propertyName);

            // Kiểm tra thuộc tính có tồn tại trên cả hai đối tượng
            if (firstProperty == null || secondProperty == null)
                return false;

            // Lấy giá trị của thuộc tính
            var firstValue = firstProperty.GetValue(firstObject);
            var secondValue = secondProperty.GetValue(secondObject);

            // So sánh giá trị (dùng Equals để xử lý trường hợp null)
            return Equals(firstValue, secondValue);
        }
    }
}
