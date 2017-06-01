using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;

namespace TwinFinder.Base.Utils
{
    /// <summary>
    /// Liefert die Typen aller PresentationObjects
    /// </summary>
    public static class KnownTypesProvider
    {
        /// <summary>
        /// the list with the known types
        /// </summary>
        private static List<Type> knownTypes = new List<Type>();

        public static IEnumerable<Type> GetKnownTypes(ICustomAttributeProvider provider)
        {
            //var serviceKnownTypeProviderAttributes
            //    =
            //    provider.GetCustomAttributes(true).Where(
            //        predicate => predicate.GetType() == typeof(ServiceKnownTypeProviderAttribute));

            //foreach (ServiceKnownTypeProviderAttribute serviceKnownTypeProviderAttribute in serviceKnownTypeProviderAttributes)
            //{
            //    GetKnownTypes(serviceKnownTypeProviderAttribute.ModelDomainNamespace);
            //}

            return knownTypes;
        }

        public static IEnumerable<Type> GetKnownTypes()
        {
            return GetKnownTypes(string.Empty, null);
        }

        public static IEnumerable<Type> GetKnownTypes(string domainNamespace, IList<String> excludeNameSpacePrefixes = null)
        {
            if (knownTypes != null && knownTypes.Count > 0)
            {
                return knownTypes;
            }

            // get all assemblies
            List<Assembly> allAssemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();

            // get all relevant assemblies
            List<Assembly> assemblies;

            if (!string.IsNullOrEmpty(domainNamespace))
            {
                // get all relevant assemblies
                assemblies = allAssemblies.Where(assembly => assembly.FullName.ToLowerInvariant().StartsWith(domainNamespace))
                                          .ToList();
            }
            else
            {
                assemblies = allAssemblies;
            }

            // get all public types
            var publicTypes = assemblies.Where(x => !x.IsDynamic)
                                        .SelectMany(assembly => assembly.GetExportedTypes());

            // get all data contracts
            //var dataContracts = publicTypes.Where(type => type.GetCustomAttributes(typeof(DataContractAttribute), true).Length > 0).ToList();

            var relevantTypes = publicTypes.Where(t => (!t.IsGenericType) && (!t.IsInterface) && (!t.Namespace.StartsWith("System")) && (!t.Namespace.StartsWith("Microsoft"))).ToList();

            // get all serializable
            var serializable = relevantTypes.Where(type => type.GetCustomAttributes(typeof(SerializableAttribute), true).Length > 0).ToList();

            // Register the types
            RegisterTypes(serializable);

            // Register all list types
            RegisterListTypes(serializable);

            // register all the derived types
            RegisterDerivedTypesOf((IEnumerable<Type>)serializable, relevantTypes);

            // Register custom types
            RegisterCustomTypes();

            // Exclude Namespaces
            if (excludeNameSpacePrefixes != null)
            {
                foreach (string excludeNampeSpace in excludeNameSpacePrefixes)
                {
                    knownTypes.RemoveAll(x => x.FullName.StartsWith(excludeNampeSpace) || x.FullName.Contains("[" + excludeNampeSpace));
                }
            }

            return knownTypes;
        }

        private static void RegisterCustomTypes()
        {
            //knownTypes.Add(typeof(SerializableDictionary<string, object>));
            //knownTypes.Add(typeof(List<SerializableDictionary<string, object>>));
        }

        private static void RegisterTypes(IEnumerable<Type> types)
        {
            knownTypes = knownTypes.Union(types).ToList();
        }

        private static void RegisterListTypes(IEnumerable<Type> types)
        {
            var listTypes = from exportedType in types
                            let listTypeName = string.Format(
                                (string)"System.Collections.Generic.List`1[[{0}]]",
                                (object)exportedType.AssemblyQualifiedName)
                            select Type.GetType(listTypeName);

            knownTypes = knownTypes.Union(listTypes).ToList();
        }

        public static void RegisterDerivedTypesOf(IEnumerable<Type> basetypes, IEnumerable<Type> types)
        {
            foreach (Type basetype in basetypes)
            {
                RegisterDerivedTypesOf(basetype, types);
            }
        }

        public static void RegisterDerivedTypesOf(Type basetype, IEnumerable<Type> types)
        {
            List<Type> derivedTypes = GetDerivedTypesOf(basetype, types);
            knownTypes = knownTypes.Union(derivedTypes).ToList();
        }

        public static void RegisterDerivedTypesOf<T>(IEnumerable<Type> types)
        {
            RegisterDerivedTypesOf(typeof(T), types);
        }

        private static List<Type> GetDerivedTypesOf(Type baseType, IEnumerable<Type> types)
        {
            return types.Where(t => !t.IsAbstract && t.IsSubclassOf(baseType)).ToList();
        }

        public static void Clear()
        {
            knownTypes = new List<Type>();
        }

        public static void Register<T>()
        {
            Register(typeof(T));
        }

        public static void Register(Type type)
        {
            knownTypes.Add(type);
        }

        public static bool IsSerializable(this object obj)
        {
            Type t = obj.GetType();

            if (t.IsSerializable)
            {
                return true;
            }

            if (obj is IXmlSerializable)
            {
                return true;
            }

            //if (Attribute.IsDefined(t, typeof(DataContractAttribute)))
            //{
            //    return true;
            //}

            return false;
        }
    }
}