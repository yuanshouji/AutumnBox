﻿using AutumnBox.Support.CstmDebug;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AutumnBox.Basic.Device
{
#pragma warning disable CS0659 // 类型重写 Object.Equals(object o)，但不重写 Object.GetHashCode()
#pragma warning disable CS0661 // 类型定义运算符 == 或运算符 !=，但不重写 Object.GetHashCode()
    public class Serial : IEquatable<Serial>
#pragma warning restore CS0661 // 类型定义运算符 == 或运算符 !=，但不重写 Object.GetHashCode()
#pragma warning restore CS0659 // 类型重写 Object.Equals(object o)，但不重写 Object.GetHashCode()
    {
        public bool IsIpAdress
        {
            get
            {
                return _ip != null;
            }
        }
        private IPEndPoint _ip = null;
        private string _serialNum = null;
        public override string ToString()
        {
            return _serialNum ?? _ip.ToString();
        }
        public string ToFullSerial() => $"-s {_serialNum ?? _ip.ToString()}";
        public Serial(string serialStr)
        {
            var strs = serialStr.Split(':');
            if (strs.Length > 1)
            {
                var ip = IPAddress.Parse(strs[0]);
                var port = Convert.ToInt32(strs[1]);
                _ip = new IPEndPoint(ip, port);
            }
            else
            {
                _serialNum = serialStr;
            }
        }
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
        public static bool operator ==(Serial left, Serial right)
        {
            return left?.ToString() == right?.ToString();
        }
        public static bool operator !=(Serial left, Serial right)
        {
            return left?.ToString() != right?.ToString();
        }
        public bool Equals(Serial other)
        {
            return this.ToString() == other.ToString();
        }
    }
}