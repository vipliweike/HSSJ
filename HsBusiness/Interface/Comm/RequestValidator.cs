﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace HsBusiness.Interface.Comm
{
    public static class RequestValidator
    {
        private const int ERR_CODE_PARAM_MISSING = 40;
        private const int ERR_CODE_PARAM_INVALID = 41;
        private const string ERR_MSG_PARAM_MISSING = "client-error:Missing required arguments : {0}";
        private const string ERR_MSG_PARAM_INVALID = "client-error:Invalid arguments : {0}";

        public static void ValidateRegex(this NameValueCollection parameters, string name, string pattern)
        {
            if (!Regex.IsMatch(parameters[name], pattern))
            {
                throw new TopException(ERR_CODE_PARAM_INVALID, string.Format(ERR_MSG_PARAM_INVALID, name));
            }
        }

        /// <summary>
        /// 验证参数是否为空。
        /// </summary>
        public static string ValidateRequired(this NameValueCollection parameters, string name, bool isNull = false)
        {
            if (!parameters.AllKeys.Any(k => k == name))
            {
                throw new TopException(ERR_CODE_PARAM_MISSING, string.Format(ERR_MSG_PARAM_MISSING, name));
            }
            else
            {
                string value = parameters[name];
                ValidateRequired(name, value, isNull);
            }
            return parameters[name];
        }

        /// <summary>
        /// 验证数字参数的范围。
        /// </summary>
        public static int ValidateRangeValue(this NameValueCollection parameters, string name, int minValue, int maxValue, int def = 0)
        {
            int value = 0;
            if (parameters[name] != null)
            {
                value = Convert.ToInt32(parameters[name]);
            }
            if (value > maxValue || value < minValue)
            {
                throw new TopException(ERR_CODE_PARAM_INVALID, string.Format(ERR_MSG_PARAM_INVALID, name));
            }
            return value;
        }

        /// <summary>
        /// 验证参数是否为空。
        /// </summary>
        /// <param name="name">参数名</param>
        /// <param name="value">参数值</param>
        /// <param name="isNull">参数值是否可空</param>
        public static void ValidateRequired(string name, object value, bool isNull)
        {
            if (!isNull && value == null)
            {
                throw new TopException(ERR_CODE_PARAM_MISSING, string.Format(ERR_MSG_PARAM_MISSING, name));
            }
            else
            {
                if (value.GetType() == typeof(string))
                {
                    string strValue = value as string;
                    if (string.IsNullOrEmpty(strValue) && !isNull)
                    {
                        throw new TopException(ERR_CODE_PARAM_MISSING, string.Format(ERR_MSG_PARAM_MISSING, name));
                    }
                }
            }
        }

        /// <summary>
        /// 验证字符串参数的最大长度。
        /// </summary>
        /// <param name="name">参数名</param>
        /// <param name="value">参数值</param>
        /// <param name="maxLength">最大长度</param>
        public static void ValidateMaxLength(string name, string value, int maxLength)
        {
            if (value != null && value.Length > maxLength)
            {
                throw new TopException(ERR_CODE_PARAM_INVALID, string.Format(ERR_MSG_PARAM_INVALID, name));
            }
        }

        /// <summary>
        /// 验证以逗号分隔的字符串参数的最大列表长度。
        /// </summary>
        /// <param name="name">参数名</param>
        /// <param name="value">参数值</param>
        /// <param name="maxSize">最大列表长度</param>
        public static void ValidateMaxListSize(string name, string value, int maxSize)
        {
            if (value != null)
            {
                string[] data = value.Split(',');
                if (data != null && data.Length > maxSize)
                {
                    throw new TopException(ERR_CODE_PARAM_INVALID, string.Format(ERR_MSG_PARAM_INVALID, name));
                }
            }
        }

        /// <summary>
        /// 验证字符串参数的最小长度。
        /// </summary>
        /// <param name="name">参数名</param>
        /// <param name="value">参数值</param>
        /// <param name="minLength">最小长度</param>
        public static void ValidateMinLength(string name, string value, int minLength)
        {
            if (value != null && value.Length < minLength)
            {
                throw new TopException(ERR_CODE_PARAM_INVALID, string.Format(ERR_MSG_PARAM_INVALID, name));
            }
        }

        /// <summary>
        /// 验证数字参数的最大值。
        /// </summary>
        /// <param name="name">参数名</param>
        /// <param name="value">参数值</param>
        /// <param name="maxValue">最大值</param>
        public static void ValidateMaxValue(string name, Nullable<long> value, long maxValue)
        {
            if (value != null && value > maxValue)
            {
                throw new TopException(ERR_CODE_PARAM_INVALID, string.Format(ERR_MSG_PARAM_INVALID, name));
            }
        }

        /// <summary>
        /// 验证数字参数的最小值。
        /// </summary>
        /// <param name="name">参数名</param>
        /// <param name="value">参数值</param>
        /// <param name="minValue">最小值</param>
        public static void ValidateMinValue(string name, Nullable<long> value, long minValue)
        {
            if (value != null && value < minValue)
            {
                throw new TopException(ERR_CODE_PARAM_INVALID, string.Format(ERR_MSG_PARAM_INVALID, name));
            }
        }
    }
}