using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AopCodeGeneration.Behaviour;
using AopCodeGeneration.Metadata;

namespace AopCodeGeneration.Samples
{
    public interface ITestInterface : IDisposable
    {
        void DoSomething(string s, int x);

        void DoSomething(List<IEnumerable<string>> list);

        void DoSomething(string[] array);

        Task<List<int>> DoSomething(int parameter);

        void SetDictionary(Dictionary<string, string> dict);

        string Prop1 { get; }

        string Prop2 { set; }

        string Prop3 { get; set; }

        event EventHandler<EventArgs> Changed;

        event EventHandler OtherEvent;

        List<T> GetGenericList<T>();

        void SetGenericValue<T>(T value);
    }

    public sealed class TestInterfaceAopProxy : ITestInterface
    {
        private readonly InterceptionHandler _interceptionHandler;
        private readonly ITestInterface _realInstance;

        public TestInterfaceAopProxy(
            ITestInterface realInstance,
            InterceptionHandler interceptionHandler)
        {
            _realInstance = realInstance;
            _interceptionHandler = interceptionHandler;
        }

        //TODO do we need a link to IDisposable interface?
        public void Dispose()
        {
            var invocationInfo = InvocationInfo.Create(
                typeof(TestInterfaceAopProxy),
                nameof(Dispose),
                Dispose);
            _interceptionHandler.Invoke(invocationInfo);
        }

        private InvocationInfo Dispose(InvocationInfo invocationInfo)
        {
            _realInstance.Dispose();
            return invocationInfo;
        }

        public void DoSomething(string s, int x)
        {
            var invocationInfo = InvocationInfo.Create(
                typeof(TestInterfaceAopProxy),
                nameof(DoSomething),
                new[]
                {
                    ArgumentInfo.Create(typeof (string), nameof(s), s),
                    ArgumentInfo.Create(typeof (int), nameof(x), x)
                },
                DoSomethingStringInt32);

            _interceptionHandler.Invoke(invocationInfo);
        }

        //TODO do we need to specify signature always or in ambiguilty?
        private InvocationInfo DoSomethingStringInt32(InvocationInfo invocationInfo)
        {
            const int sIndex = 0;
            const int xIndex = 1;
            _realInstance.DoSomething(
                (string)invocationInfo.Arguments[sIndex].Value,
                (int)invocationInfo.Arguments[xIndex].Value);

            return invocationInfo;
        }

        public void DoSomething(List<IEnumerable<string>> list)
        {
            var invocationInfo = InvocationInfo.Create(
                typeof(TestInterfaceAopProxy),
                nameof(DoSomething),
                new[]
                {
                    ArgumentInfo.Create(typeof (List<IEnumerable<string>>), nameof(list), list)
                },
                DoSomethingListOfIEnumerableOfString);

            _interceptionHandler.Invoke(invocationInfo);
        }

        private InvocationInfo DoSomethingListOfIEnumerableOfString(InvocationInfo invocationInfo)
        {
            const int listIndex = 0;
            _realInstance.DoSomething((List<IEnumerable<string>>) invocationInfo.Arguments[listIndex].Value);

            return invocationInfo;
        }

        public void DoSomething(string[] array)
        {
            var invocationInfo = InvocationInfo.Create(
                typeof(TestInterfaceAopProxy),
                nameof(DoSomething),
                new[]
                {
                    ArgumentInfo.Create(typeof (string[]), nameof(array), array)
                },
                DoSomethingArrayOfString);

            _interceptionHandler.Invoke(invocationInfo);
        }

        private InvocationInfo DoSomethingArrayOfString(InvocationInfo invocationInfo)
        {
            const int arrayIndex = 0;
            _realInstance.DoSomething((string[])invocationInfo.Arguments[arrayIndex].Value);

            return invocationInfo;
        }

        public Task<List<int>> DoSomething(int parameter)
        {
            var invocationInfo = InvocationInfo.Create(
                typeof (TestInterfaceAopProxy),
                nameof(DoSomething),
                new[]
                {
                    ArgumentInfo.Create(typeof (int), nameof(parameter), parameter)
                },
                ResultInfo.Create(typeof (Task<List<int>>)),
                DoSomethingInt32);

            var result = _interceptionHandler.Invoke(invocationInfo);

            return (Task<List<int>>)result.Result.Value;
        }

        private InvocationInfo DoSomethingInt32(InvocationInfo invocationInfo)
        {
            const int parameterIndex = 0;
            _realInstance.DoSomething((int)invocationInfo.Arguments[parameterIndex].Value);

            return invocationInfo;
        }

        public void SetDictionary(Dictionary<string, string> dict)
        {
            var invocationInfo = InvocationInfo.Create(
                typeof(TestInterfaceAopProxy),
                nameof(DoSomething),
                new[]
                {
                    ArgumentInfo.Create(typeof (Dictionary<string, string>), nameof(dict), dict)
                },
                SetDictionary);

            _interceptionHandler.Invoke(invocationInfo);
        }

        private InvocationInfo SetDictionary(InvocationInfo invocationInfo)
        {
            const int dictIndex = 0;
            _realInstance.SetDictionary((Dictionary<string, string>)invocationInfo.Arguments[dictIndex].Value);

            return invocationInfo;
        }

        public string Prop1
        {
            get
            {
                var invocationInfo = InvocationInfo.Create(
                    typeof (TestInterfaceAopProxy),
                    nameof(Prop1),
                    ResultInfo.Create(typeof (string)),
                    Prop1Get,
                    InvocationSpecialType.PropertyGet);

                var result = _interceptionHandler.Invoke(invocationInfo);

                return (string) result.Result.Value;
            }
        }

        private InvocationInfo Prop1Get(InvocationInfo invocationInfo)
        {
            invocationInfo.Result.Value = _realInstance.Prop1;
            return invocationInfo;
        }

        public string Prop2
        {
            set
            {
                var invocationInfo = InvocationInfo.Create(
                    typeof(TestInterfaceAopProxy),
                    nameof(Prop2),
                    new[]
                    {
                        ArgumentInfo.Create(typeof (string), nameof(value), value)
                    },
                    Prop2Set,
                    InvocationSpecialType.PropertySet);

                _interceptionHandler.Invoke(invocationInfo);
            }
        }

        private InvocationInfo Prop2Set(InvocationInfo invocationInfo)
        {
            const int valueIndex = 0;
            _realInstance.Prop2 = (string)invocationInfo.Arguments[valueIndex].Value;
            return invocationInfo;
        }

        public string Prop3
        {
            get
            {
                var invocationInfo = InvocationInfo.Create(
                    typeof(TestInterfaceAopProxy),
                    nameof(Prop3),
                    ResultInfo.Create(typeof(string)),
                    Prop3Get,
                    InvocationSpecialType.PropertyGet);

                var result = _interceptionHandler.Invoke(invocationInfo);

                return (string)result.Result.Value;
            }
            set
            {
                var invocationInfo = InvocationInfo.Create(
                    typeof(TestInterfaceAopProxy),
                    nameof(Prop3),
                    new[]
                    {
                        ArgumentInfo.Create(typeof (string), nameof(value), value)
                    },
                    Prop3Set,
                    InvocationSpecialType.PropertySet);

                _interceptionHandler.Invoke(invocationInfo);
            }
        }

        private InvocationInfo Prop3Get(InvocationInfo invocationInfo)
        {
            invocationInfo.Result.Value = _realInstance.Prop3;
            return invocationInfo;
        }

        private InvocationInfo Prop3Set(InvocationInfo invocationInfo)
        {
            const int valueIndex = 0;
            _realInstance.Prop3 = (string)invocationInfo.Arguments[valueIndex].Value;
            return invocationInfo;
        }

        public event EventHandler<EventArgs> Changed
        {
            add
            {
                var invocationInfo = InvocationInfo.Create(
                    typeof(TestInterfaceAopProxy),
                    nameof(Changed),
                    new[]
                    {
                        ArgumentInfo.Create(typeof (EventHandler<EventArgs>), nameof(value), value)
                    },
                    ChangedAdd,
                    InvocationSpecialType.EventAdd);

                _interceptionHandler.Invoke(invocationInfo);
            }
            remove
            {
                var invocationInfo = InvocationInfo.Create(
                    typeof(TestInterfaceAopProxy),
                    nameof(Changed),
                    new[]
                    {
                        ArgumentInfo.Create(typeof (EventHandler<EventArgs>), nameof(value), value)
                    },
                    ChangedRemove,
                    InvocationSpecialType.EventRemove);

                _interceptionHandler.Invoke(invocationInfo);
            }
        }

        private InvocationInfo ChangedAdd(InvocationInfo invocationInfo)
        {
            const int valueIndex = 0;
            _realInstance.Changed += (EventHandler<EventArgs>)invocationInfo.Arguments[valueIndex].Value;
            return invocationInfo;
        }

        private InvocationInfo ChangedRemove(InvocationInfo invocationInfo)
        {
            const int valueIndex = 0;
            _realInstance.Changed -= (EventHandler<EventArgs>)invocationInfo.Arguments[valueIndex].Value;
            return invocationInfo;
        }

        public event EventHandler OtherEvent
        {
            add
            {
                var invocationInfo = InvocationInfo.Create(
                    typeof(TestInterfaceAopProxy),
                    nameof(OtherEvent),
                    new[]
                    {
                        ArgumentInfo.Create(typeof (EventHandler), nameof(value), value)
                    },
                    OtherEventAdd,
                    InvocationSpecialType.EventAdd);

                _interceptionHandler.Invoke(invocationInfo);
            }
            remove
            {
                var invocationInfo = InvocationInfo.Create(
                    typeof(TestInterfaceAopProxy),
                    nameof(OtherEvent),
                    new[]
                    {
                        ArgumentInfo.Create(typeof (EventHandler), nameof(value), value)
                    },
                    OtherEventRemove,
                    InvocationSpecialType.EventRemove);

                _interceptionHandler.Invoke(invocationInfo);
            }
        }

        private InvocationInfo OtherEventAdd(InvocationInfo invocationInfo)
        {
            const int valueIndex = 0;
            _realInstance.OtherEvent += (EventHandler)invocationInfo.Arguments[valueIndex].Value;
            return invocationInfo;
        }

        private InvocationInfo OtherEventRemove(InvocationInfo invocationInfo)
        {
            const int valueIndex = 0;
            _realInstance.OtherEvent -= (EventHandler)invocationInfo.Arguments[valueIndex].Value;
            return invocationInfo;
        }

        public List<T> GetGenericList<T>()
        {
            var genericArgumentT = GenericArgumentInfo.Create(typeof (T), nameof(T));
            var invocationInfo = InvocationInfo.Create(
                typeof (ContainerAopProxy),
                nameof(GetGenericList),
                new[]
                {
                    genericArgumentT
                },
                ResultInfo.Create(typeof (List<T>), genericArgumentT),
                GetGenericList<T>);

            var result = _interceptionHandler.Invoke(invocationInfo);

            return (List<T>) result.Result.Value;
        }

        private InvocationInfo GetGenericList<T>(InvocationInfo invocationInfo)
        {
            invocationInfo.Result.Value = _realInstance.GetGenericList<T>();

            return invocationInfo;
        }

        public void SetGenericValue<T>(T value)
        {
            var genericArgumentT = GenericArgumentInfo.Create(typeof (T), nameof(T));
            var invocationInfo = InvocationInfo.Create(
                typeof (ContainerAopProxy),
                nameof(GetGenericList),
                new[]
                {
                    ArgumentInfo.Create(typeof (T), nameof(value), value, genericArgumentT),
                },
                new[]
                {
                    genericArgumentT
                },
                SetGenericValue<T>);

            _interceptionHandler.Invoke(invocationInfo);
        }

        private InvocationInfo SetGenericValue<T>(InvocationInfo invocationInfo)
        {
            const int valueIndex = 0;
            _realInstance.SetGenericValue((T)invocationInfo.Arguments[valueIndex].Value);
            
            return invocationInfo;
        }
    }
}