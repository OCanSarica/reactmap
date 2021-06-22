using System;
using System.Linq.Expressions;
using System.Reflection;

namespace core.Attributes.FilterAttributes
{


    public class Converter<TSource, TResult> : AConverter
    {
        private Expression<Func<TSource, TResult>> predicate = null;

        public Converter(Expression<Func<TSource, TResult>> predicate)
        {
            this.predicate = predicate;
        }

        public override PropertyInfo Convert()
        {
            return Convert(predicate);
        }
    }

    public abstract class AConverter
    {
        public abstract PropertyInfo Convert();

        protected PropertyInfo Convert<TSource, TResult>(Expression<Func<TSource, TResult>> predicate)
        {
            var memberExpression = (MemberExpression)predicate.Body;
            var property = (PropertyInfo)memberExpression.Member;

            return property;
        }
    }
}