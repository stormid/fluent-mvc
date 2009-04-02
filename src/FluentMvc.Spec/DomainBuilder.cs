using System;
using System.Collections.Generic;

public class DomainBuilder<T> where T : new()
{
    private readonly Queue<Action<T>> propertyCache = new Queue<Action<T>>();

    public DomainBuilder<T> WithProperty(Action<T> expression)
    {
        propertyCache.Enqueue(expression);

        return this;
    }

    public T Build()
    {
        T instance = CreateInstance();

        BindProperties(instance);

        return instance;
    }

    private void BindProperties(T instance)
    {
        foreach (Action<T> action in propertyCache)
        {
            action(instance);
        }
    }

    private T CreateInstance()
    {
        return new T();
    }
}