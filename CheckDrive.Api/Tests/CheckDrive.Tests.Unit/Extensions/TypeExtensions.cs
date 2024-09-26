namespace CheckDrive.Tests.Unit.Extensions;

internal static class TypeExtensions
{
    public static bool HasGenericInterface(this Type type, Type interfaceType)
    {
        var interfaces = type.GetInterfaces().ToList();
        return interfaces.Exists(r => r.IsGenericType
            && r.GetGenericTypeDefinition() == interfaceType);
    }

    public static bool IsSubclassOfRawGeneric(this Type derivedType, Type baseType)
    {
        while (derivedType != null && derivedType != typeof(object))
        {
            var currentType = derivedType.IsGenericType ? derivedType.GetGenericTypeDefinition() : derivedType;
            if (baseType == currentType)
            {
                return true;
            }

            derivedType = derivedType.BaseType!;
        }
        return false;
    }
}