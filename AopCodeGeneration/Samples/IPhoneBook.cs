using System;
using AopCodeGeneration.Behaviour;
using AopCodeGeneration.Metadata;

namespace AopCodeGeneration.Samples
{
    public interface IPhoneBook
    {
        long GetContactPhoneNumber(string firstName, string lastName);

        long MyNumber { get; set; }

        int ContactsCount { get; }

        event EventHandler<long> PhoneNumberChanged;
    }

    public sealed class PhoneBookAopProxy : IPhoneBook
    {
        private readonly InterceptionHandler _interceptionHandler;
        private readonly IPhoneBook _realInstance;

        public PhoneBookAopProxy(
            IPhoneBook realInstance,
            InterceptionHandler interceptionHandler)
        {
            _realInstance = realInstance;
            _interceptionHandler = interceptionHandler;
        }

        public long GetContactPhoneNumber(string firstName, string lastName)
        {
            var invocationInfo = InvocationInfo.Create(
                typeof(PhoneBookAopProxy),
                nameof(GetContactPhoneNumber),
                new[]
                {
                    ArgumentInfo.Create(typeof (string), nameof(firstName), firstName),
                    ArgumentInfo.Create(typeof (string), nameof(lastName), lastName)
                },
                ResultInfo.Create(typeof(long)),
                GetContactPhoneNumber);

            var result = _interceptionHandler.Invoke(invocationInfo);

            return (long)result.Result.Value;
        }

        private InvocationInfo GetContactPhoneNumber(InvocationInfo invocationInfo)
        {
            const int firstNameIndex = 0;
            const int lastNameIndex = 1;
            var result = _realInstance.GetContactPhoneNumber(
                (string)invocationInfo.Arguments[firstNameIndex].Value,
                (string)invocationInfo.Arguments[lastNameIndex].Value);

            invocationInfo.Result.Value = result;

            return invocationInfo;
        }

        public long MyNumber
        {
            get
            {
                var invocationInfo = InvocationInfo.Create(
                typeof(PhoneBookAopProxy),
                nameof(MyNumber),
                ResultInfo.Create(typeof(long)),
                MyNumberGet,
                InvocationSpecialType.PropertyGet);

                var result = _interceptionHandler.Invoke(invocationInfo);

                return (long)result.Result.Value;
            }
            set
            {
                var invocationInfo = InvocationInfo.Create(
                    typeof(PhoneBookAopProxy),
                    nameof(MyNumber),
                    new[]
                    {
                        ArgumentInfo.Create(typeof (long), nameof(value), value)
                    },
                    MyNumberSet,
                    InvocationSpecialType.PropertySet);

                _interceptionHandler.Invoke(invocationInfo);
            }
        }

        private InvocationInfo MyNumberGet(InvocationInfo invocationInfo)
        {
            invocationInfo.Result.Value = _realInstance.MyNumber;
            return invocationInfo;
        }

        private InvocationInfo MyNumberSet(InvocationInfo invocationInfo)
        {
            const int valueIndex = 0;
            _realInstance.MyNumber = (long)invocationInfo.Arguments[valueIndex].Value;
            return invocationInfo;
        }

        public int ContactsCount
        {
            get
            {
                var invocationInfo = InvocationInfo.Create(
                    typeof(PhoneBookAopProxy),
                    nameof(ContactsCount),
                    ResultInfo.Create(typeof(int)),
                    ContactsCountGet,
                    InvocationSpecialType.PropertyGet);

                var result = _interceptionHandler.Invoke(invocationInfo);

                return (int)result.Result.Value;
            }
        }

        private InvocationInfo ContactsCountGet(InvocationInfo invocationInfo)
        {
            invocationInfo.Result.Value = _realInstance.ContactsCount;
            return invocationInfo;
        }
        
        public event EventHandler<long> PhoneNumberChanged
        {
            add
            {
                var invocationInfo = InvocationInfo.Create(
                    typeof(PhoneBookAopProxy),
                    nameof(PhoneNumberChanged),
                    new[]
                    {
                        ArgumentInfo.Create(typeof (EventHandler<long>), nameof(value), value)
                    },
                    PhoneNumberChangedAdd,
                    InvocationSpecialType.EventAdd);

                _interceptionHandler.Invoke(invocationInfo);
            }
            remove
            {
                var invocationInfo = InvocationInfo.Create(
                    typeof(PhoneBookAopProxy),
                    nameof(PhoneNumberChanged),
                    new[]
                    {
                        ArgumentInfo.Create(typeof (EventHandler<long>), nameof(value), value)
                    },
                    PhoneNumberChangedRemove,
                    InvocationSpecialType.EventRemove);

                _interceptionHandler.Invoke(invocationInfo);
            }
        }

        private InvocationInfo PhoneNumberChangedAdd(InvocationInfo invocationInfo)
        {
            const int valueIndex = 0;
            _realInstance.PhoneNumberChanged += (EventHandler<long>) invocationInfo.Arguments[valueIndex].Value;
            return invocationInfo;
        }

        private InvocationInfo PhoneNumberChangedRemove(InvocationInfo invocationInfo)
        {
            const int valueIndex = 0;
            _realInstance.PhoneNumberChanged -= (EventHandler<long>)invocationInfo.Arguments[valueIndex].Value;
            return invocationInfo;
        }
    }
}