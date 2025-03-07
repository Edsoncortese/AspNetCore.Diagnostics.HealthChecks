using k8s;

namespace HealthChecks.Kubernetes
{
    public class KubernetesResourceCheck
    {
        private readonly Func<IKubernetesObject, bool> _condition;

        public Type ResourceType { get; }
        public string Name { get; }
        public string Namespace { get; }

        private KubernetesResourceCheck(Type type, string name, string @namespace, Func<IKubernetesObject, bool> condition)
        {
            _condition = condition;
            Name = Guard.ThrowIfNull(name);
            Namespace = Guard.ThrowIfNull(@namespace);
            ResourceType = Guard.ThrowIfNull(type);
        }

        public static KubernetesResourceCheck Create<T>(string name, string @namespace, Func<IKubernetesObject, bool> condition)
            where T : IKubernetesObject
        {
            return new KubernetesResourceCheck(typeof(T), name, @namespace, condition);
        }

        public bool Check(IKubernetesObject kubernetesObject)
        {
            return _condition?.Invoke(kubernetesObject) ?? true;
        }
    }
}
