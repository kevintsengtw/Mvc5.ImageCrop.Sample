using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace ImageCrop
{
	public class ClientScriptHelper
	{
		#region -- CleanMessage --

		/// <summary>
		/// Cleans the message.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <returns></returns>
		private static string CleanMessage(string message)
		{
			if (string.IsNullOrWhiteSpace(message))
			{
				return string.Empty;
			}

			string cleanString = message;

			//Replace \n
			cleanString = cleanString.Replace("\n", "\\n");
			//Replace \r
			cleanString = cleanString.Replace("\r", "\\r");
			//Replace "
			cleanString = cleanString.Replace("\"", "\\\"");
			//Replace '
			cleanString = cleanString.Replace("\'", "\\\'");

			return cleanString;
		}

		#endregion

		#region -- ShowMessage --

		/// <summary>
		/// Shows the message.
		/// </summary>
		/// <param name="page">The page.</param>
		/// <param name="message">The message.</param>
		public static void ShowMessage(Page page, string message)
		{
			ShowMessage(page, message, RegisterScriptType.Block);
		}

		/// <summary>
		/// Shows the message.
		/// Start: 在Page物件的 form 元素的結束標記之前發出alert(msg),
		/// Block: 在Page物件的 form 元素的開始標記後立即發出alert(msg)
		/// </summary>
		/// <param name="page">The page.</param>
		/// <param name="message">The message.</param>
		/// <param name="registerType">RegisterScriptType.</param>
		/// <param name="isCleanMessage">if set to <c>true</c> [is clean message].</param>
		public static void ShowMessage(
			Page page,
			string message,
			RegisterScriptType registerType)
		{
			ShowMessage(page, message, registerType, true);
		}

		public static void ShowMessage(
			Page page,
			string message,
			RegisterScriptType registerType,
			bool isCleanScript)
		{
			string clientScript = string.Format(
				"alert('{0}');",
				isCleanScript ? CleanMessage(message) : message
			);

			ExecuteClientScript(page, FormatClientScript(clientScript), registerType);
		}

		/// <summary>
		/// Shows the message and Redirect.
		/// Start: 在Page物件的 form 元素的結束標記之前發出alert(msg),
		/// Block: 在Page物件的 form 元素的開始標記後立即發出alert(msg)
		/// </summary>
		/// <param name="page">The page.</param>
		/// <param name="message">The message.</param>
		/// <param name="redirectURI">The redirect URI (Reolad: redirectURI = window.location.href).</param>
		/// <param name="registerType">RegisterScriptType.</param>
		/// Reolad: redirectURI = window.location.href
		public static void ShowMessage(
			Page page,
			string message,
			string redirectURI,
			RegisterScriptType registerType)
		{
			ShowMessage(page, message, redirectURI, null, registerType);
		}

		/// <summary>
		/// Shows the message and Redirect. 
		/// Start: 在Page物件的 form 元素的結束標記之前發出alert(msg), 
		/// Block: 在Page物件的 form 元素的開始標記後立即發出alert(msg)
		/// </summary>
		/// <param name="page">The page.</param>
		/// <param name="message">The message.</param>
		/// <param name="redirectURI">The redirect URI (Reolad: redirectURI = window.location.href).</param>
		/// <param name="target">The target (top or this).</param>
		/// <param name="registerType">RegisterScriptType.</param>
		public static void ShowMessage(
			Page page,
			string message,
			string redirectURI,
			string target,
			RegisterScriptType registerType)
		{
			string script = !string.IsNullOrWhiteSpace(target)
				? string.Format(@"window.alert('{0}');{1}.location.href='{2}';", message, target, redirectURI)
				: string.Format(@"window.alert('{0}');location.href='{1}';", message, redirectURI);

			ExecuteClientScript(page, FormatClientScript(script), registerType);
		}

		#endregion

		#region -- ExecuteClientScript --

		/// <summary>
		/// Executes the client script.
		/// Start: 在Page物件的 form runat= server 元素的結束標記之前發出該腳本, 
		/// Block: 在Page物件的 form runat= server 元素的開始標記後立即發出該腳本
		/// </summary>
		/// <param name="page">The page.</param>
		/// <param name="script">The script.</param>
		/// <param name="registerType">Type of the register.</param>
		public static void ExecuteClientScript(
			Page page,
			string script,
			RegisterScriptType registerType)
		{
			ExecuteClientScript(page, script, false, registerType);
		}

		/// <summary>
		/// Executes the client script.
		/// Start: 在Page物件的 form runat= server 元素的結束標記之前發出該腳本,
		/// Block: 在Page物件的 form runat= server 元素的開始標記後立即發出該腳本
		/// </summary>
		/// <param name="page">The page.</param>
		/// <param name="script">The client script.</param>
		/// <param name="isFormatClientScript">if set to <c>true</c> [is format client script].</param>
		/// <param name="registerType">RegisterScriptType.</param>
		public static void ExecuteClientScript(
			Page page,
			string script,
			bool isFormatClientScript,
			RegisterScriptType registerType)
		{
			string scriptString = isFormatClientScript 
				? FormatClientScript(script) 
				: script;

			string strKey = (Guid.NewGuid().ToString()).Replace("-", "").ToUpper();

			switch (registerType)
			{
				case RegisterScriptType.Start:
					page.ClientScript.RegisterStartupScript(page.GetType(), strKey, scriptString);
					break;

				case RegisterScriptType.Block:
					page.ClientScript.RegisterClientScriptBlock(page.GetType(), strKey, scriptString);
					break;
			}
		}

		#endregion

		#region -- FormatClientScript --
		/// <summary>
		/// Formats a client script block along 
		/// </summary>
		/// <param name="ScriptStatementBlock"></param>
		/// <returns></returns>
		public static string FormatClientScript(string scriptStatementBlock)
		{
			return string.Format(
				System.Globalization.CultureInfo.CurrentCulture,
				"\n<script language=\"javascript\" type=\"text/javascript\">\n<!-- \n {0} \n // -->\n</script>\n",
				scriptStatementBlock
			);
		}

		#endregion
	}
}

public enum RegisterScriptType
{
	Start,
	Block
}
