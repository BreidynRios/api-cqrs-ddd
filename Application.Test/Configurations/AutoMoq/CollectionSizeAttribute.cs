using AutoFixture;
using AutoFixture.Kernel;
using AutoFixture.Xunit2;
using System.Reflection;

namespace Application.Test.Configurations.AutoMoq
{
    public class CollectionSizeAttribute : CustomizeAttribute
    {
        private readonly int _size;

        public CollectionSizeAttribute(int size)
        {
            _size = size;
        }

        public override ICustomization GetCustomization(ParameterInfo parameter)
        {
            if (parameter == null) throw new ArgumentNullException(nameof(parameter));

            var objectType = GetElementType(parameter.ParameterType);
            if (objectType == null) throw new ArgumentNullException(nameof(objectType));

            var listType = typeof(List<>).MakeGenericType(objectType);
            var arrayType = objectType.MakeArrayType();

            var isTypeCompatible = listType.IsAssignableFrom(parameter.ParameterType) || arrayType.IsAssignableFrom(parameter.ParameterType);

            if (!isTypeCompatible)
            {
                throw new InvalidOperationException($"{nameof(CollectionSizeAttribute)} specified for type incompatible with List or Array: {parameter.ParameterType} {parameter.Name}");
            }

            var customizationType = typeof(CollectionSizeCustomization<>).MakeGenericType(objectType);
            return  (ICustomization)Activator.CreateInstance(customizationType, parameter, _size);
        }

        private Type? GetElementType(Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
                return type.GetGenericArguments()[0];

            if (type.IsArray) return type.GetElementType();

            return null;
        }

        public class CollectionSizeCustomization<T> : ICustomization
        {
            private readonly ParameterInfo _parameter;
            private readonly int _repeatCount;

            public CollectionSizeCustomization(ParameterInfo parameter, int repeatCount)
            {
                _parameter = parameter;
                _repeatCount = repeatCount;
            }

            public void Customize(IFixture fixture)
            {
                fixture.Customizations.Add(new FilteringSpecimenBuilder(
                    new FixedBuilder(fixture.CreateMany<T>(_repeatCount).ToList()),
                    new EqualRequestSpecification(_parameter)
                ));
            }
        }
    }
}
