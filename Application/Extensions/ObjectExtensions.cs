using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Extensions
{
    public static class ObjectExtensions
    {
        public static void Set(this object obj, string propertyName, object value)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));

            // Get the property info via Reflection
            var propertyInfo = obj.GetType().GetProperty(propertyName);

            if (propertyInfo == null)
            {
                throw new ArgumentException($"Property '{propertyName}' not found on object of type '{obj.GetType().Name}'.");
            }
            
            if (!propertyInfo.CanWrite)
            {
                throw new InvalidOperationException($"Property '{propertyName}' is read-only.");
            }

            // Convert the type if necessary (e.g., string to int)
            var targetType = Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType;
            var convertedValue = Convert.ChangeType(value, targetType);

            // Assign the value to the object
            propertyInfo.SetValue(obj, convertedValue, null);
        }
    }
}
