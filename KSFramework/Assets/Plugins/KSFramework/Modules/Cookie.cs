﻿using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace KSFramework
{
    /// <summary>
    /// Cookie for game developement,  such as a cache
    /// </summary>
    public class Cookie
    {
        public delegate object CookieGetter(object srcValue);
        public delegate void CookieSetter(string key, object setValue);
        public delegate object IfNullGetter();

        /// <summary>
        /// Store values
        /// </summary>
        protected static Dictionary<string, object> _hashtable = new Dictionary<string, object>();
        /// <summary>
        /// Store Getters, of key
        /// </summary>
        protected static Dictionary<string, CookieGetter> _getters = new Dictionary<string, CookieGetter>(); 

        /// <summary>
        /// Store setters
        /// </summary>
        protected static Dictionary<string, CookieSetter> _setters = new Dictionary<string, CookieSetter>();


        /// <summary>
        /// Default Setter of cookie `Set`
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        static void DefaultSetter(string key, object setValue)
        {
            _hashtable[key] = setValue;
        }

        /// <summary>
        /// Default getter of cookie `Get`
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        static object DefaultGetter(object srcValue)
        {
            return srcValue;
        }

        /// <summary>
        /// Define a key's getter and setter; Only one chance to define
        /// </summary>
        /// <param name="key"></param>
        /// <param name="getter"></param>
        /// <param name="setter"></param>
        public static void Define(string key, CookieGetter getter, CookieSetter setter)
        {
            if (_getters.ContainsKey(key))
                throw new Exception("Duplicated getter of : " + key);
            if (_setters.ContainsKey(key))
                throw new Exception("Duplicated setter of : " + key);

            _getters[key] = getter;
            _setters[key] = setter;
        }

        /// <summary>
        /// Set value for cookie
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void Set(string key, object value)
        {
            CookieSetter setter;
            if (!_setters.TryGetValue(key, out setter) || setter == null)
            {
                setter = DefaultSetter;
            }
            setter(key, value);
        }

        /// <summary>
        /// Get value from cookie, if null, do the action get default value!
        /// </summary>
        /// <param name="key"></param>
        /// <param name="ifNullSetDefault"></param>
        /// <returns></returns>
        public static object Get(string key, IfNullGetter ifNullSetDefault)
        {
            if (!_hashtable.ContainsKey(key))
            {
                return ifNullSetDefault();
            }
            return Get(key);
        }

        /// <summary>
        /// Get value from cookie
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static object Get(string key)
        {
            CookieGetter getter;
            if (!_getters.TryGetValue(key, out getter) || getter == null)
            {
                getter = DefaultGetter;
            }
            object value;
            if (_hashtable.ContainsKey(key))
                value =_hashtable[key];
            value = null;
            return getter(value);
        }

    }
}
