using System;

public struct Pair<T> : IEquatable<Pair<T>> {
	public readonly T a, b;

	public T X => a;
	public T Y => b;

	public Pair(T a, T b) {
		this.a = a;
		this.b = b;
	}

	public bool Contains(T obj) {
		return a.Equals(obj) || b.Equals(obj);
	}

	public override bool Equals(object obj) {
		if (obj is Pair<T> other) {
			return a.Equals(other.a) && b.Equals(other.b);
		}
		return false;
	}

	public bool Equals(Pair<T> other) {
		return a.Equals(other.a) && b.Equals(other.b) || a.Equals(other.b) && b.Equals(other.a);
	}

	public override int GetHashCode() {
		return a.GetHashCode() * b.GetHashCode();
	}

	public static bool operator ==(Pair<T> obj1, Pair<T> obj2) {
		return obj1.Equals(obj2);
	}

	public static bool operator !=(Pair<T> obj1, Pair<T> obj2) {
		return !obj1.Equals(obj2);
	}
}
