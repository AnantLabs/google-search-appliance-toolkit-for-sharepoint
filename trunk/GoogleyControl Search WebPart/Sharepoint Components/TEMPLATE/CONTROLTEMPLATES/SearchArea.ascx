<%@ Control Language="C#" Inherits="Microsoft.SharePoint.WebControls.SearchArea,Microsoft.SharePoint,Version=12.0.0.0,Culture=neutral,PublicKeyToken=71e9bce111e9429c"    compilationMode="Always" %>
<%@ Register Tagprefix="wssawc" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> <%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> <%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> <%@ Import Namespace="Microsoft.SharePoint" %>
<%
	string strScopeWeb = null;
	string strScopeList = null;
	string strWebSelected = null;
	SPWeb web = SPControl.GetContextWeb(Context);
	string strEncodedUrl
		= SPHttpUtility.EcmaScriptStringLiteralEncode(
			SPHttpUtility.UrlPathEncode(web.Url + "/_layouts/searchresults.aspx", false, false)
			);
	strEncodedUrl = "'" + strEncodedUrl + "'";
	strScopeWeb = "'" + SPHttpUtility.HtmlEncode( web.Url ) + "'";
	SPList list = SPContext.Current.List;
	if ( list != null &&
			 ((list.BaseTemplate != SPListTemplateType.DocumentLibrary && list.BaseTemplate != SPListTemplateType.WebPageLibrary) ||
			  (SPContext.Current.ListItem == null) ||
			  (SPContext.Current.ListItem.ParentList == null) ||
			  (SPContext.Current.ListItem.ParentList != list))
	   )
	{
		strScopeList = list.ID.ToString();
	}
	else
	{
		strWebSelected = "SELECTED";
	}
%>
<table border=0 cellpadding="0" cellspacing="0" class='ms-searchform'><tr>
<td>
<SELECT id='idSearchScope' name='SearchScope' class='ms-searchbox' title=<%SPHttpUtility.AddQuote(SPHttpUtility.HtmlEncode(SearchScopeToolTip),Response.Output);%>>
<OPTION value=<%=strScopeWeb%> <%=strWebSelected%>> <SharePoint:EncodedLiteral runat="server" text="<%$Resources:wss,search_Scope_Site%>" EncodeMethod='HtmlEncode' Id='idSearchScopeSite'/> </OPTION>
<%
if (strScopeList != null)
{
%>
	<OPTION value=<%=strScopeList%> SELECTED> <SharePoint:EncodedLiteral runat="server" text="<%$Resources:wss,search_Scope_List%>" EncodeMethod='HtmlEncode' Id='idSearchScopeList'/> </OPTION>
<%
}
%>
</SELECT>
</td>
<td>

<DIV style="BACKGROUND-IMAGE: url(/_layouts/images/google_custom_search_watermark.gif); BACKGROUND-REPEAT: no-repeat; BACKGROUND-POSITION:10px">
<INPUT Type=TEXT id='idSearchString' size=25 name='SearchString' display='inline' maxlength=255 ACCESSKEY=S class='ms-searchbox' style="background-color: transparent"  id="xencontrol"  onfocus="javascript: var f = document.getElementById('idSearchString'); f.style.background = '#ffffff';" onblur="javascript: var f = document.getElementById('idSearchString'); f.style.background = 'background-color: transparent';" onKeyDown="return SearchKeyDown(event, <%=strEncodedUrl%>);" title=<%SPHttpUtility.AddQuote(SPHttpUtility.HtmlEncode(SearchTextToolTip),Response.Output);%>>
</DIV>

</td>
<td>
<div class="ms-searchimage"><a target='_self' href='javascript:' onClick="javascript:SubmitSearchRedirect(<%=strEncodedUrl%>);javascript:return false;" title=<%SPHttpUtility.AddQuote(SPHttpUtility.HtmlEncode(SearchImageToolTip),Response.Output);%> ID=onetIDGoSearch><img border='0' src="/_layouts/images/gosearch.gif" alt=<%SPHttpUtility.AddQuote(SPHttpUtility.HtmlEncode(SearchImageToolTip),Response.Output);%>></a></div>
</td>
</tr></table>
