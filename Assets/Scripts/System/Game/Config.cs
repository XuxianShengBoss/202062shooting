using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace NetEngine{
	public class Config
	{

        //外网
        /*
    	public static string loginURL = "http://192.168.1.100:8080/";
    	public static string gameURL = "http://106.75.45.217:8080/ophyer-love-game/facade";
    	public static string downLoadURL = "http://file.igame123.com";
    	*/
        //外网
     //   public static string loginURL = "http://runningplupon.igame123.com/";
        //内网
        //public static string loginURL = "http://106.75.51.35:28080/";
          public static string loginURL = "http://runningplupon.igame123.com/";
        //public static string downLoadURL = "http://file.igame123.com";

        //版本
        public static string CN = "cn";//简体
		public static string ZH = "zh";
		public static string EN = "en";//英文
		public static string TW = "tw";//繁体

		//版本号
		public static string VER = "1.0.0";

        //talkingdata  appid
        public static string TGGAAPPID = "D012192EA3AE4381B2977A962F0B3F00";
        //talkingdata  channelid
        public static string TGGACHANNELID = "TapTap";
    }
}
