using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public class TCContext
    {
        #region 变量

        const string _cookie_UserNickname = "User_Nickname";
        const string _context_ItemKey = "TCContext";

        HttpContext _context;
        int _accountID = -1;
        string _nickName = "";
        bool _isLogin = false;


        #endregion

        #region 构造函数
        /// <summary>
        /// 创建自定义 http 会话上下文的新实例
        /// </summary>
        public TCContext()
        {
            _context = HttpContext.Current;
            if (_context == null) return;

            if (_context.Request.IsAuthenticated)
            {
                _isLogin = true;
                _nickName = OnlineUserNickName;
                _accountID = int.Parse(_context.User.Identity.Name);
            }
            else
            {
                _nickName = "匿名用户";
                _accountID = -1;
            }
        }

        #endregion

        #region 属性

        /// <summary>
        /// 获取上下文实例
        /// </summary>
        public HttpContext Context
        {
            get { return (_context != null) ? _context : new HttpContext(null); }
        }
        /// <summary>
        /// 获取或设置用户登录状态
        /// </summary>
        public bool IsLogin
        {
            get { return _isLogin; }
            set { _isLogin = value; }
        }
        /// <summary>
        /// 获取或设置用户账户 ID
        /// </summary>
        public int AccountID
        {
            get { return _accountID; }
            set { _accountID = value; }
        }
        /// <summary>
        /// 获取或设置用户昵称
        /// </summary>
        public string NickName
        {
            get { return _nickName; }
            set { _nickName = value; }
        }


        #endregion

        #region 静态属性

        /// <summary>
        /// 静态 获取当前用户的自定义上下文实例
        /// </summary>
        public static TCContext Current
        {
            get
            {
                if (HttpContext.Current != null && HttpContext.Current.Items.Contains(_context_ItemKey))
                    return (TCContext)HttpContext.Current.Items[_context_ItemKey];
                else
                {
                    TCContext tc = new TCContext();
                    HttpContext.Current.Items.Add(_context_ItemKey, tc);
                    return tc;

                }
            }
        }


        /// <summary>
        /// 静态 读取或设置用户昵称
        /// </summary>
        public string OnlineUserNickName
        {
            get
            {
                HttpCookie cookie = _context.Request.Cookies[_cookie_UserNickname];
                return (cookie != null) ? HttpUtility.UrlDecode(cookie.Value) : string.Empty;
            }
            set
            {

                HttpCookie cookie = new HttpCookie(_cookie_UserNickname, HttpUtility.UrlEncode(value));
                cookie.HttpOnly = true;
                //cookie.Expires = DateTime.Now.AddYears(1);
                _context.Response.Cookies.Add(cookie);
            }
        }


        /// <summary>
        /// 读取或设置用户真实名称
        /// </summary>
        public string OnlineRealName
        {
            get
            {
                HttpCookie cookie = _context.Request.Cookies["RealName"];
                return (cookie != null) ? HttpUtility.UrlDecode(cookie.Value) : string.Empty;
            }
            set
            {

                HttpCookie cookie = new HttpCookie("RealName", HttpUtility.UrlEncode(value));
                cookie.HttpOnly = true;
                //cookie.Expires = DateTime.Now.AddYears(1);
                _context.Response.Cookies.Add(cookie);
            }
        }

        /// <summary>
        /// 读取或设置用户ID
        /// </summary>
        public string OnlineUserID
        {
            get
            {
                HttpCookie cookie = _context.Request.Cookies["UserID"];
                return (cookie != null) ? HttpUtility.UrlDecode(cookie.Value) : string.Empty;
            }
            set
            {

                HttpCookie cookie = new HttpCookie("UserID", HttpUtility.UrlEncode(value));
                cookie.HttpOnly = true;
                //cookie.Expires = DateTime.Now.AddYears(1);
                _context.Response.Cookies.Add(cookie);
            }
        }

        /// <summary>
        /// 读取或设置用户类型
        /// </summary>
        public string OnlineUserType
        {
            get
            {
                HttpCookie cookie = _context.Request.Cookies["UserType"];
                return (cookie != null) ? HttpUtility.UrlDecode(cookie.Value) : string.Empty;
            }
            set
            {

                HttpCookie cookie = new HttpCookie("UserType", HttpUtility.UrlEncode(value));
                cookie.HttpOnly = true;
                //cookie.Expires = DateTime.Now.AddYears(1);
                _context.Response.Cookies.Add(cookie);
            }
        }

        /// <summary>
        /// 读取或设置所属区县
        /// </summary>
        public string OnlineArea
        {
            get
            {
                HttpCookie cookie = _context.Request.Cookies["Area"];
                return (cookie != null) ? HttpUtility.UrlDecode(cookie.Value) : string.Empty;
            }
            set
            {

                HttpCookie cookie = new HttpCookie("Area", HttpUtility.UrlEncode(value));
                cookie.HttpOnly = true;
                //cookie.Expires = DateTime.Now.AddYears(1);
                _context.Response.Cookies.Add(cookie);
            }
        }

        #endregion
    }
}
