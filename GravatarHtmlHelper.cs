﻿namespace System.Web.Helpers
 {
	 using System.Collections.Generic;
	 using System.Security.Cryptography;
	 using System.Text;
	 using System.Web;
	 using System.Web.Mvc;
	 using System.Web.Routing;

	 public static class GravatarHtmlHelper
	 {
		 public static HtmlString Gravatar(this HtmlHelper html, string email)
		 {
			 return GetImage(GetGravatar(email));
		 }

		 public static HtmlString Gravatar(this HtmlHelper html, string email, object gravatarAttributes)
		 {
			 return GetImage(GetGravatar(email, gravatarAttributes));
		 }

		 public static HtmlString Gravatar(this HtmlHelper html, string email, object gravatarAttributes, object htmlAttributes)
		 {
			 return GetImage(GetGravatar(email, gravatarAttributes), htmlAttributes);
		 }

		 private static HtmlString GetImage(string source)
		 {
			 return GetImage(source, null);
		 }

		 private static HtmlString GetImage(string source, object htmlAttributes)
		 {
			 IDictionary<string, object> attributes =
				 (htmlAttributes == null
					 ? new RouteValueDictionary()
					 : new RouteValueDictionary(htmlAttributes));

			 var tagBuilder = new TagBuilder("img");
			 tagBuilder.MergeAttribute("src", source);
			 tagBuilder.MergeAttributes(attributes);

			 return new HtmlString(tagBuilder.ToString(TagRenderMode.SelfClosing));
		 }

		 private static string GetGravatar(string email)
		 {
			 return string.Format("http://www.gravatar.com/avatar/{0}", GetHash(email));
		 }

		 private static string GetGravatar(string email, object gravatarAttributes)
		 {
			 IDictionary<string, object> attributes =
				 (gravatarAttributes == null
					 ? new RouteValueDictionary()
					 : new RouteValueDictionary(gravatarAttributes));

			 var uriBuilder = new StringBuilder(GetGravatar(email));
			 bool first = true;

			 foreach (string key in attributes.Keys)
			 {
				 if (first)
				 {
					 uriBuilder.AppendFormat("?{0}={1}", key, attributes[key].ToString());
					 first = false;
					 continue;
				 }

				 uriBuilder.AppendFormat("&{0}={1}", key, attributes[key].ToString());
			 }

			 return uriBuilder.ToString();
		 }

		 private static string GetHash(string email)
		 {
			 var cryptoProvider = new MD5CryptoServiceProvider();

			 byte[] bytes = Encoding.ASCII.GetBytes(email);
			 bytes = cryptoProvider.ComputeHash(bytes);

			 var hashBuilder = new StringBuilder();
			 foreach (var value in bytes)
				 hashBuilder.Append(value.ToString("x2").ToLower());

			 return hashBuilder.ToString();
		 }
	 }
 }