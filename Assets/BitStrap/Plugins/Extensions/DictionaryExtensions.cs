﻿using System.Collections.Generic;
using System.Text;

namespace BitStrap {

    /// <summary>
    /// Bunch of utility extension methods to the generic Dictionary class.
    /// These methods are intended to be System.Ling substitues as they do not generate garbage.
    /// </summary>
    public static class DictionaryExtensions {

        public struct GCFreeEnumerator<K, V> {
            private Dictionary<K, V>.Enumerator enumerator;

            public KeyValuePair<K, V> Current {
                get { return enumerator.Current; }
            }

            public GCFreeEnumerator(Dictionary<K, V> collection) {
                enumerator = collection.GetEnumerator();
            }

            public GCFreeEnumerator<K, V> GetEnumerator() {
                return this;
            }

            public bool MoveNext() {
                return enumerator.MoveNext();
            }
        }

        /// <summary>
        /// Use this method to iterate a Dictionary in a foreach loop but with no garbage
        /// </summary>
        /// <example>
        /// foreach( var pair in myDictionary.Each() )
        /// {
        ///     // code goes here...
        /// }
        /// </example>
        /// <typeparam name="K"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static GCFreeEnumerator<K, V> Each<K, V>(this Dictionary<K, V> collection) {
            return new GCFreeEnumerator<K, V>(collection);
        }

        /// <summary>
        /// Behaves like System.Linq.Count however it does not generate garbage.
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="collection"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static int Count<K, V>(this Dictionary<K, V> collection, System.Predicate<KeyValuePair<K, V>> predicate) {
            if (predicate == null)
                return 0;

            int count = 0;
            for (var enumerator = collection.GetEnumerator(); enumerator.MoveNext();) {
                if (predicate(enumerator.Current))
                    count++;
            }

            return count;
        }

        /// <summary>
        /// Behaves like System.Linq.All however it does not generate garbage.
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="collection"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static bool All<K, V>(this Dictionary<K, V> collection, System.Predicate<KeyValuePair<K, V>> predicate) {
            if (predicate == null)
                return false;

            for (var enumerator = collection.GetEnumerator(); enumerator.MoveNext();) {
                if (!predicate(enumerator.Current))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Behaves like System.Linq.Any however it does not generate garbage.
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="collection"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static bool Any<K, V>(this Dictionary<K, V> collection, System.Predicate<KeyValuePair<K, V>> predicate) {
            if (predicate == null)
                return false;

            for (var enumerator = collection.GetEnumerator(); enumerator.MoveNext();) {
                if (predicate(enumerator.Current))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Behaves like System.Linq.FirstOrDefault however it does not generate garbage.
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static KeyValuePair<K, V> FirstOrDefault<K, V>(this Dictionary<K, V> collection) {
            var enumerator = collection.GetEnumerator();
            if (enumerator.MoveNext())
                return enumerator.Current;

            return default(KeyValuePair<K, V>);
        }

        /// <summary>
        /// Behaves like System.Linq.FirstOrDefault however it does not generate garbage.
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="collection"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static KeyValuePair<K, V> FirstOrDefault<K, V>(this Dictionary<K, V> collection, System.Predicate<KeyValuePair<K, V>> predicate) {
            for (var enumerator = collection.GetEnumerator(); enumerator.MoveNext();) {
                if (predicate(enumerator.Current))
                    return enumerator.Current;
            }

            return default(KeyValuePair<K, V>);
        }

        /// <summary>
        /// Pretty format an dictionary as "{ k1=e1, k2=e2, k3=e3, ..., kn=en }".
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static string ToStringFull<K, V>(this Dictionary<K, V> collection) {
            if (collection == null)
                return "null";
            if (collection.Count <= 0)
                return "{}";

            StringBuilder sb = new StringBuilder();

            sb.Append("{ ");

            for (var enumerator = collection.GetEnumerator(); enumerator.MoveNext();) {
                sb.Append(enumerator.Current.Key.ToString());
                sb.Append("=");
                sb.Append(enumerator.Current.Value.ToString());
                sb.Append(", ");
            }

            sb.Remove(sb.Length - 2, 2);
            sb.Append(" }");

            return sb.ToString();
        }
    }
}