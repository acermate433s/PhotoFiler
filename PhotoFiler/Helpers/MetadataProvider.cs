using System;
using System.Globalization;
using System.Linq;

namespace PhotoFiler.Helpers
{
    public class InterfaceMetadataProvider : System.Web.Mvc.EmptyModelMetadataProvider
    {
        public override System.Web.Mvc.ModelMetadata GetMetadataForProperty(
            Func<object> modelAccessor, Type containerType, string propertyName)
        {
            if (containerType == null)
            {
                throw new ArgumentNullException("containerType");
            }
            if (String.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentException(
                    "The property &apos;{0}&apos; cannot be null or empty", "propertyName"
                );
            }

            var property =
                GetTypeDescriptor(containerType)
                    .GetProperties()
                    .Find(propertyName, true);

            if (property == null && containerType.IsInterface)
            {
                property =
                    (
                        from t in containerType.GetInterfaces()
                        let p =
                            GetTypeDescriptor(t)
                                .GetProperties()
                                .Find(propertyName, true)
                        where p != null
                        select p
                    )
                    .FirstOrDefault();
            }

            if (property == null)
            {
                throw new ArgumentException(
                    String.Format(
                        CultureInfo.CurrentCulture,
                        "The property {0}.{1} could not be found",
                        containerType.FullName,
                        propertyName
                    )
                );
            }

            return GetMetadataForProperty(modelAccessor, containerType, property);
        }
    }
}