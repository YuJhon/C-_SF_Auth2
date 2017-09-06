using System;  
using System.Collections.Generic;  
using System.Linq;  
using System.Text;  
using System.Net.Security;  
using System.Security.Cryptography.X509Certificates;  
using System.Net;  
using System.IO;  
using System.IO.Compression;  
using System.Text.RegularExpressions;

class Program
{
    	private static readonly string DefaultUserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
    
    	private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)  
	{  
		return true; //总是接受     
  	}  
        
    	public static HttpWebResponse CreatePostHttpResponse(string url, IDictionary<string, string> parameters,Encoding charset)  
    	{  
		HttpWebRequest request = null;  
		//HTTPSQ请求  
		ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult); 

		//如果需要POST数据     
		if (!(parameters == null || parameters.Count == 0))  
		{  
			StringBuilder buffer = new StringBuilder();  
			int i = 0;  
			foreach (string key in parameters.Keys)  
			{  
				if (i > 0)  
				{  
					buffer.AppendFormat("&{0}={1}", key, parameters[key]);  
				}  
				else  
				{  
					buffer.AppendFormat("?{0}={1}", key, parameters[key]);  
				}  
				i++;  
			 }
			 url += buffer.ToString();
		}  


		request = WebRequest.Create(url) as HttpWebRequest;  
		request.ProtocolVersion = HttpVersion.Version10;  
		request.Method = "POST";  
		request.ContentType = "application/x-www-form-urlencoded";  
		request.Accept = "application/json"; 
		request.Proxy = null;
		request.UserAgent = DefaultUserAgent;

		return request.GetResponse() as HttpWebResponse;  
	}     
    
    
	static void Main()
	{
		string sfdcConsumerKey = "xxxxx";
		string sfdcConsumerSecret = "xxxxx";

		string sfdcUserName = "xxxxx";
		string sfdcPassword = "xxxxx";

		string url = "https://login.salesforce.com/services/oauth2/token";  
		Encoding encoding = Encoding.GetEncoding("utf-8");  
		IDictionary<string, string> parameters = new Dictionary<string, string>();  
		parameters.Add("grant_type", "password");  
		parameters.Add("client_id",sfdcConsumerKey);  
		parameters.Add("client_secret",sfdcConsumerSecret);  
		parameters.Add("username", sfdcUserName);
		parameters.Add("password", sfdcPassword); 

		HttpWebResponse response = Program.CreatePostHttpResponse(url,parameters,encoding);  
		//打印返回值  
		Stream stream = response.GetResponseStream();   //获取响应的字符串流  
		StreamReader sr = new StreamReader(stream); //创建一个stream读取流  
		string html = sr.ReadToEnd();   //从头读到尾，放到字符串html  
		Console.WriteLine(html);   
	}
}
