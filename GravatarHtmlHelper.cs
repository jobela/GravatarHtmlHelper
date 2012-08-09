namespace System.Web.Helpers
{
	using System.Collections.Generic;
	using System.Security.Cryptography;
	using System.Text;
	using System.Web;
	using System.Web.Mvc;
	using System.Web.Routing;

	/// <summary>
	/// Globally Recognised Avatar - Gravatar (http://gravatar.com)
	/// </summary>
	/// <remarks>
	/// Written by Joar Langaard (https://github.com/jobela/GravatarHtmlHelper)
	/// </remarks>
	public static class GravatarHtmlHelper
	{
		/// <summary>
		/// Returns a gravatar as an &lt;img /&gt;
		/// </summary>
		/// <param name="email">Email for the Gravatar</param>
		public static HtmlString Gravatar(this HtmlHelper html, string email)
		{
			return GetImage(GetGravatar(html, email));
		}

		/// <summary>
		/// Returns a gravatar as an &lt;img /&gt;
		/// </summary>
		/// <param name="email">Email for the Gravatar</param>
		/// <param name="gravatarAttributes">An object that contains the Gravatar attributes for the element. (http://gravatar.com/site/implement/images/) </param>
		public static HtmlString Gravatar(this HtmlHelper html, string email, object gravatarAttributes)
		{
			return GetImage(GetGravatar(html, email, gravatarAttributes));
		}

		/// <summary>
		/// Returns a gravatar as an &lt;img /&gt;
		/// </summary>
		/// <param name="email">Email for the Gravatar</param>
		/// <param name="gravatarAttributes">An object that contains the Gravatar attributes for the element. (http://gravatar.com/site/implement/images/) </param>
		/// <param name="htmlAttributes">An object that contains the HTML attributes for the element.</param>
		public static HtmlString Gravatar(this HtmlHelper html, string email, object gravatarAttributes, object htmlAttributes)
		{
			return GetImage(GetGravatar(html, email, gravatarAttributes), htmlAttributes);
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
			tagBuilder.MergeAttribute("alt", "Gravatar");
			tagBuilder.MergeAttribute("class", "gravatar");
			tagBuilder.MergeAttributes(attributes);

			return new HtmlString(tagBuilder.ToString(TagRenderMode.SelfClosing));
		}

		private static string GetGravatar(HtmlHelper html, string email)
		{
			/// Standard format http://www.gravatar.com/avatar/{0}
			/// Secure format https://secure.gravatar.com/avatar/{0}
			return string.Format("{0}://{1}.gravatar.com/avatar/{2}",
				html.ViewContext.HttpContext.Request.IsSecureConnection ? "https" : "http",
				html.ViewContext.HttpContext.Request.IsSecureConnection ? "secure" : "www",
				GetHash(email));
		}

		private static string GetGravatar(HtmlHelper html, string email, object gravatarAttributes)
		{
			IDictionary<string, object> attributes =
				(gravatarAttributes == null
					? new RouteValueDictionary()
					: new RouteValueDictionary(gravatarAttributes));

			var uriBuilder = new StringBuilder(GetGravatar(html, email));
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

		/// <summary>
		/// Generates an MD5 hash of the string
		/// </summary>
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