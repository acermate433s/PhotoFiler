using System;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

namespace PhotoFiler.Helpers
{
    public class InterfaceMetadataProvider : System.Web.Mvc.EmptyModelMetadataProvider
    {
        public override ModelMetadata GetMetadataForProperty(
            Func<object> modelAccessor, 
            Type containerType, 
            string propertyName
        )
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

            var result = 
                base.GetMetadataForProperty(
                    modelAccessor, 
                    containerType,
                    property
                );

            // retrieve the DisplayNameAttribute from the model defined
            // in the view and not from the instantiated class
            result.DisplayName = property.DisplayName;

            return result;
        }
    }
}