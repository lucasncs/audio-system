using System;
using System.Collections.Generic;
using UnityEngine;

namespace Seven.AudioSystem.SubSystems.ObjectPool
{
	public class ObjectPool<T> : IDisposable, IObjectPool<T> where T : class
	{
		private readonly Stack<T> _stack;
		private readonly Func<T> _createFunc;
		private readonly Action<T> _actionOnGet;
		private readonly Action<T> _actionOnRelease;
		private readonly Action<T> _actionOnDestroy;
		private readonly int _maxSize;
		private readonly bool _collectionCheck;

		public int CountAll { get; private set; }
		public int CountActive => CountAll - CountInactive;
		public int CountInactive => _stack.Count;

		public ObjectPool(Func<T> createFunc, 
			Action<T> actionOnGet = null, 
			Action<T> actionOnRelease = null, 
			Action<T> actionOnDestroy = null, 
			bool collectionCheck = true, 
			int initialSize = 10, 
			int warmUpElements = 0, 
			int maxSize = 10000)
		{
			if (maxSize <= 0)
			{
				throw new ArgumentException("Max Size must be greater than 0", nameof(maxSize));
			}

			_stack = new Stack<T>(initialSize > warmUpElements ? initialSize : warmUpElements);
			_createFunc = createFunc ?? throw new ArgumentNullException(nameof(createFunc));
			_maxSize = maxSize;
			_actionOnGet = actionOnGet;
			_actionOnRelease = actionOnRelease;
			_actionOnDestroy = actionOnDestroy;
			_collectionCheck = collectionCheck;

			if (warmUpElements > 0)
			{
				for (var i = 0; i < warmUpElements; i++)
				{
					_stack.Push(CreateElement());
				}
			}
		}

		private T CreateElement()
		{
			CountAll++;
			return _createFunc();
		}

		public T Get()
		{
			T element = _stack.Count == 0 ? CreateElement() : _stack.Pop();

			_actionOnGet?.Invoke(element);

			return element;
		}

		public void Release(T element)
		{
			if (_collectionCheck && _stack.Count > 0 && _stack.Contains(element))
			{
				Debug.LogWarning("Trying to release an object that has already been released to the pool.");
				return;
			}

			_actionOnRelease?.Invoke(element);

			if (CountInactive < _maxSize)
			{
				_stack.Push(element);
			}
			else
			{
				_actionOnDestroy?.Invoke(element);
			}
		}

		public void Clear()
		{
			if (_actionOnDestroy != null)
			{
				foreach (T item in _stack)
				{
					_actionOnDestroy(item);
				}
			}

			_stack.Clear();
			CountAll = 0;
		}

		public void Dispose()
		{
			Clear();
		}
	}
}