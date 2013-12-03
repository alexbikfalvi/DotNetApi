/* 
 * Copyright (C) Ury Jamshy / ujamshy@yahoo.com
 *
 * THIS WORK IS PROVIDED "AS IS", "WHERE IS" AND "AS AVAILABLE", WITHOUT ANY EXPRESS OR
 * IMPLIED WARRANTIES OR CONDITIONS OR GUARANTEES. YOU, THE USER, ASSUME ALL RISK IN ITS
 * USE, INCLUDING COPYRIGHT INFRINGEMENT, PATENT INFRINGEMENT, SUITABILITY, ETC. AUTHOR
 * EXPRESSLY DISCLAIMS ALL EXPRESS, IMPLIED OR STATUTORY WARRANTIES OR CONDITIONS, INCLUDING
 * WITHOUT LIMITATION, WARRANTIES OR CONDITIONS OF MERCHANTABILITY, MERCHANTABLE QUALITY
 * OR FITNESS FOR A PARTICULAR PURPOSE, OR ANY WARRANTY OF TITLE OR NON-INFRINGEMENT, OR
 * THAT THE WORK (OR ANY PORTION THEREOF) IS CORRECT, USEFUL, BUG-FREE OR FREE OF VIRUSES.
 * 
 */


using System;
using System.Collections;
using System.Collections.Generic;

namespace DotNetApi.Collections.Generic
{
    internal interface IGenericEnumeratorProvider<T>
    {
        T Current { get; }
        void Reset();
        bool MoveNext();
    }

	/// <summary>
	/// A helper class providing a generic enumerator.
	/// </summary>
	/// <typeparam name="T"></typeparam>
    internal class GenericEnumerator<T> : IEnumerator<T>, IEnumerator, IDisposable
    {
        IGenericEnumeratorProvider<T> m_provider;

        public GenericEnumerator(IGenericEnumeratorProvider<T> provider)
        {
            m_provider = provider;
            m_provider.Reset();
        }

        protected T GetCurrentItem()
        {
            if (m_provider.Current == null)
                throw new InvalidOperationException("No current object");
            return m_provider.Current;
        }

        T IEnumerator<T>.Current
        {
            get
            {
                return GetCurrentItem();
            }
        }

        object IEnumerator.Current
        {
            get
            {
                return GetCurrentItem();
            }
        }

        public bool MoveNext()
        {
            return m_provider.MoveNext();
        }

        public void Reset()
        {
            m_provider.Reset();
        }

        public void Dispose()
        {
        }
    }

    internal class GenericEnumerable<T> : IEnumerable<T>, IEnumerable
    {
        private IEnumerator<T> m_enumerator;
        public GenericEnumerable(IEnumerator<T> enumerator)
        {
            m_enumerator = enumerator;
        }
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return m_enumerator;
        }
        public IEnumerator GetEnumerator()
        {
            return m_enumerator;
        }
    }
}
