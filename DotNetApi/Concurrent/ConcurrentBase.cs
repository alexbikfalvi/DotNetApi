using System;
using System.Threading;

namespace DotNetApi.Concurrent
{
	public abstract class ConcurrentBase
	{
		/// <summary>
		/// A structure representing the lock info.
		/// </summary>
		public struct LockInfo
		{
			/// <summary>
			/// Creates a new lock info with the <code>Locked</code> field set the specified value and the <code>Cookie</code>
			/// field to <b>false</b>.
			/// </summary>
			/// <param name="locked">The locked value.</param>
			public LockInfo(bool locked)
			{
				this.Locked = locked;
				this.Cookie = null;
			}

			/// <summary>
			/// Creates a new lock info with the <code>Locked</code> field set to <b>true</b> and the <code>Cookie</code>
			/// field set to the specified nullable value.
			/// </summary>
			/// <param name="cookie">The lock cookie.</param>
			public LockInfo(LockCookie? cookie)
			{
				this.Locked = true;
				this.Cookie = cookie;
			}

			/// <summary>
			/// Indicates whether the last operation acquired a lock.
			/// </summary>
			public readonly bool Locked;
			/// <summary>
			/// The lock cookie for the last operation, or <b>null</b> if the last operation has not returned a cookie.
			/// </summary>
			public readonly LockCookie? Cookie;
		}

		private readonly ReaderWriterLock sync = new ReaderWriterLock();

		// Public methods.

		/// <summary>
		/// Acquires a reader lock for the current list.
		/// </summary>
		public void Lock()
		{
			this.sync.AcquireReaderLock(-1);
		}

		/// <summary>
		/// Tries to acquire a reader lock for the current list.
		/// </summary>
		/// <returns><b>True</b> if the lock has been acquired, <b>false</b> otherwise.</returns>
		public bool TryLock()
		{
			try
			{
				this.sync.AcquireReaderLock(0);
				return true;
			}
			catch (ApplicationException)
			{
				return false;
			}
		}

		/// <summary>
		/// Releases the previous reader lock or the reader lock that has been upgraded to a writer lock.
		/// </summary>
		public void Unlock()
		{
			this.sync.ReleaseReaderLock();
		}

		// Protected methods.

		/// <summary>
		/// Checks whether the current thread has a reader or writer lock.
		/// </summary>
		/// <returns><b>True</b> if the current thread has a reader or writer lock, <b>false</b> otherwise.</returns>
		protected bool HasLock()
		{
			return this.sync.IsReaderLockHeld || this.sync.IsWriterLockHeld;
		}

		/// <summary>
		/// Acquires a reader lock on the list for the current thread.
		/// </summary>
		/// <returns>The lock info.</returns>
		protected LockInfo AcquireReaderLock()
		{
			//  If there is a reader lock held.
			if (this.sync.IsReaderLockHeld)
			{
				// Do not acquire a new lock.
				return new LockInfo(false);
			}
			else
			{
				// Acquire a new reader lock.
				this.sync.AcquireReaderLock(-1);
				return new LockInfo(true);
			}
		}

		/// <summary>
		/// Acquires a write lock on the list for the current thread.
		/// </summary>
		/// <returns>The lock info.</returns>
		protected LockInfo AcquireWriterLock()
		{
			// If there is a writer lock held.
			if (this.sync.IsWriterLockHeld)
			{
				// Do not acquire a new lock.
				return new LockInfo(false);
			}
			// Else, if there is a reader lock held.
			else if (this.sync.IsReaderLockHeld)
			{
				// Upgrade the reader lock to a writer lock.
				LockCookie cookie = this.sync.UpgradeToWriterLock(-1);
				return new LockInfo(cookie);
			}
			else
			{
				// Acquire a new writer lock/
				this.sync.AcquireWriterLock(-1);
				return new LockInfo(true);
			}
		}

		/// <summary>
		/// Releases the read lock on the list for the current thread.
		/// </summary>
		/// <param name="info">The lock info.</param>
		protected void ReleaseReaderLock(LockInfo info)
		{
			// If the a new lock was acquired.
			if (info.Locked)
			{
				// Release the lock.
				this.sync.ReleaseReaderLock();
			}
		}

		/// <summary>
		/// Releases the write lock on the list for the current thread.
		/// </summary>
		/// <param name="info">The lock info.</param>
		protected void ReleaseWriterLock(LockInfo info)
		{
			// If the writer lock was upgrader from a reader lock.
			if (info.Cookie.HasValue)
			{
				// Downgrade the writer lock to a reader lock.
				LockCookie cookie = info.Cookie.Value;
				this.sync.DowngradeFromWriterLock(ref cookie);
			}
			// Else, if a new lock was acquired.
			else if (info.Locked)
			{
				// Release the writer lock.
				this.sync.ReleaseWriterLock();
			}
		}
	}
}
