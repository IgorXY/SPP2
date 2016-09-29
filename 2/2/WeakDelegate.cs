using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace _2
{
    class WeakDelegate
    {
        WeakReference weakReference;
        MethodInfo method;
        DelegateCreator delegateCreator;

        public Delegate Weak { get { return delegateCreator.SetDelegate(method, weakReference); } }
        public bool IsAlive { get { return weakReference.IsAlive; } }

        public WeakDelegate(Delegate del)
        {
            weakReference = new WeakReference(del.Target);
            method = del.Method;
            delegateCreator = new DelegateCreator();
        }

        private class DelegateCreator
        {
            private MethodInfo methodInfo;
            private WeakReference weakReference;
 
            public Delegate SetDelegate(MethodInfo mInfo, WeakReference wRef)
            {
                this.methodInfo = mInfo;
                this.weakReference = wRef;
                return GenericAction();
            }
            private ParameterExpression[] GetParams() => methodInfo.GetParameters().
                    Select(param => Expression.Parameter(param.ParameterType)).ToArray();
            private Type GetActionType()
            {
                var args = new List<Type>(methodInfo.GetParameters()
                .Select(param => param.ParameterType));
                args.Add(methodInfo.ReturnType);

                return Expression.GetDelegateType(args.ToArray());
            }
            //Create delegate from lambda
            private Delegate GenericAction()
            {
                var _params = GetParams();
                // .Compile returns delegate
                return Expression.Lambda(GetActionType(), BlockAction(_params), _params).Compile();
            }

            //Call delegate method
            private Expression[] CallAction(ParameterExpression[] _params) =>
                    new Expression[] { CallDelegate(_params) };

            private MemberExpression GetTarget() => Expression.Property(Expression.Convert(
                    Expression.Constant(weakReference), typeof(WeakReference)), "Target");

            private MethodCallExpression CallDelegate(ParameterExpression[] _params) => 
                Expression.Call(
                    instance: Expression.Convert(GetTarget(), methodInfo.DeclaringType),
                    method: methodInfo,
                    arguments: _params);

            //if GetIsAlive exists then Call Action
            private ConditionalExpression BlockAction(ParameterExpression[] _params) => Expression.IfThen(
                    Expression.IsTrue(GetIsAlive()), Expression.Block(GetVariablesList(_params), 
                        CallAction(_params)));

            //Check weakReference property IsAlive
            private MemberExpression GetIsAlive() => Expression.Property(Expression.Convert(
                    Expression.Constant(weakReference), typeof(WeakReference)), "IsAlive");

            private List<ParameterExpression> GetVariablesList(ParameterExpression[] 
                argumentsType) => new List<ParameterExpression>(argumentsType.Select(argument =>
                       Expression.Variable(argument.Type)));
        }

        public static implicit operator Delegate(WeakDelegate v)
        {
            return v.Weak;
        }
    }
}
